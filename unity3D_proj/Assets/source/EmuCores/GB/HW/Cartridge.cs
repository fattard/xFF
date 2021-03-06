﻿/*
*   This file is part of xFF
*   Copyright (C) 2017 Fabio Attard
*
*   This program is free software: you can redistribute it and/or modify
*   it under the terms of the GNU General Public License as published by
*   the Free Software Foundation, either version 3 of the License, or
*   (at your option) any later version.
*
*   This program is distributed in the hope that it will be useful,
*   but WITHOUT ANY WARRANTY; without even the implied warranty of
*   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*   GNU General Public License for more details.
*
*   You should have received a copy of the GNU General Public License
*   along with this program.  If not, see <http://www.gnu.org/licenses/>.
*
*   Additional Terms 7.b and 7.c of GPLv3 apply to this file:
*       * Requiring preservation of specified reasonable legal notices or
*         author attributions in that material or in the Appropriate Legal
*         Notices displayed by works containing it.
*       * Prohibiting misrepresentation of the origin of that material,
*         or requiring that modified versions of such material be marked in
*         reasonable ways as different from the original version.
*/

using xFF.EmuCores.GB.Defs;
using xFF.EmuCores.GB.HW.MBC;
using System.Collections.Generic;

namespace xFF
{
    namespace EmuCores
    {
        namespace GB
        {
            namespace HW
            {
                public class CartridgeHeader
                {
                    string m_title;
                    string m_manufacturer;
                    int m_cgbFlag;
                    string m_newLicenseCode;
                    int m_sgbFlag;
                    int m_cartType;
                    int m_romSize;
                    int m_ramSize;
                    int m_destinationCode;
                    int m_oldLicenseCode;
                    int m_maskRomVersion;
                    int m_headerChecksum;
                    int m_globalChecksum;

                    public CartridgeHeader(byte[] aData)
                    {
                        if (aData.Length < 0x150)
                        {
                            throw new System.ArgumentException("Invalid data size");
                        }


                        char[] aTitle = new char[16];
                        for (int i = 0; i < aTitle.Length; ++i)
                        {
                            aTitle[i] = (char)aData[0x134 + i];
                        }
                        m_title = new string(aTitle);

                        char[] aManufacturer = new char[4];
                        for (int i = 0; i < aManufacturer.Length; ++i)
                        {
                            aManufacturer[i] = (char)aData[0x13F + i];
                        }
                        m_manufacturer = new string(aManufacturer);

                        m_cgbFlag = aData[0x143];

                        char[] aNewLicenseCode = new char[2];
                        for (int i = 0; i < aNewLicenseCode.Length; ++i)
                        {
                            aNewLicenseCode[i] = (char)aData[0x144 + i];
                        }
                        m_newLicenseCode = new string(aNewLicenseCode);

                        m_sgbFlag = aData[0x146];

                        m_cartType = aData[0x147];

                        m_romSize = aData[0x148];

                        m_ramSize = aData[0x149];

                        m_destinationCode = aData[0x14A];

                        m_oldLicenseCode = aData[0x14B];

                        m_maskRomVersion = aData[0x14C];

                        m_headerChecksum = aData[0x14D];

                        m_globalChecksum = (aData[0x14E] << 8) + (aData[0x14F]);
                    }


                    public string Title
                    {
                        get { return m_title; }
                    }


                    public int CartType
                    {
                        get { return m_cartType; }
                    }


                    public int ROMSize
                    {
                        get { return m_romSize; }
                    }


                    public int RAMSize
                    {
                        get { return m_ramSize; }
                    }
                }



                public abstract class Cartridge
                {
                    protected CartridgeHeader m_cartHeader;
                    protected List<byte[]> m_ramBanks;
                    protected List<byte[]> m_romBanks;

                    public Cartridge( )
                    {
                        m_romBanks = new List<byte[]>(128);
                        m_ramBanks = new List<byte[]>(4);
                    }

                    public abstract int this[int aOffset]
                    {
                        get;
                        set;
                    }

                    public void SetROMBank(int aBankNum, byte[] aData)
                    {
                        /*while (aBankNum >= m_romBanks.Count)
                        {
                            m_romBanks.Add(new byte[0x4000]);
                        }*/

                        System.Buffer.BlockCopy(aData, 0, m_romBanks[aBankNum], 0, aData.Length);
                    }


                    public abstract void SaveRAM(string filePath);
                    public abstract void LoadRAM(string filePath);


                    public static Cartridge Mount(CartridgeHeader aCartridgeHeader, byte[] aRomData)
                    {
                        Cartridge cart = null;


                        // ROM Only 
                        if (Cartridge_Single.Validate(aCartridgeHeader))
                        {
                            if (aRomData == null || aRomData.Length > (128 * 0x4000))
                            {
                                throw new System.ArgumentException("Invalid ROM header info");
                            }

                            cart = new Cartridge_Single(aCartridgeHeader);

                            byte[] buffer = new byte[0x4000];

                            System.Buffer.BlockCopy(aRomData, 0, buffer, 0, 0x4000);
                            cart.SetROMBank(0, buffer);

                            System.Buffer.BlockCopy(aRomData, 0x4000, buffer, 0, 0x4000);
                            cart.SetROMBank(1, buffer);
                        }


                        // ROM with some type of MBC
                        else
                        {
                            // MBC1
                            if (Cartridge_MBC1.Validate(aCartridgeHeader))
                            {
                                if (aRomData == null || aRomData.Length > (128 * 0x4000))
                                {
                                    throw new System.ArgumentException("Invalid MBC1 ROM header info");
                                }

                                cart = new Cartridge_MBC1(aCartridgeHeader);
                            }

                    #if ENABLE_WIP_MBC
                            // MBC3
                            else if (Cartridge_MBC3.Validate(aCartridgeHeader))
                            {
                                if (aRomData == null || aRomData.Length > (128 * 0x4000))
                                {
                                    throw new System.ArgumentException("Invalid MBC3 ROM header info");
                                }

                                cart = new Cartridge_MBC3(aCartridgeHeader);
                            }

                            // MBC2
                            else if (Cartridge_MBC2.Validate(aCartridgeHeader))
                            {
                                if (aRomData == null || aRomData.Length > (16 * 0x4000))
                                {
                                    throw new System.ArgumentException("Invalid MBC2 ROM header info");
                                }

                                cart = new Cartridge_MBC2(aCartridgeHeader);
                            }


                            // MBC5
                            else if (Cartridge_MBC5.Validate(aCartridgeHeader))
                            {
                                if (aRomData == null || aRomData.Length > (512 * 0x4000))
                                {
                                    throw new System.ArgumentException("Invalid MBC5 ROM header info");
                                }

                                cart = new Cartridge_MBC5(aCartridgeHeader);
                            }
                    #endif


                            // Unsupported, yet
                            else
                            {
                                throw new System.ArgumentException("Some MBC mappers are not supported yet.\nPlease, be patient :)");
                            }


                            byte[] buffer = new byte[0x4000];

                            int totalBanks = aRomData.Length / 0x4000;

                            for (int i = 0; i < totalBanks; ++i)
                            {
                                System.Buffer.BlockCopy(aRomData, (i * 0x4000), buffer, 0, 0x4000);
                                cart.SetROMBank(i, buffer);
                            }

                            if (EmuEnvironment.RomFilePath.EndsWith(".gbc"))
                            {
                                cart.LoadRAM(EmuEnvironment.RomFilePath.Replace(".gbc", ".sav"));
                            }

                            else if (EmuEnvironment.RomFilePath.EndsWith(".gb"))
                            {
                                cart.LoadRAM(EmuEnvironment.RomFilePath.Replace(".gb", ".sav"));
                            }

                            // Just append .sav to whatever name is
                            else
                            {
                                cart.LoadRAM(EmuEnvironment.RomFilePath + ".sav");
                            }
                        }


                        return cart;
                    }
                }


            }
            // namespace HW
        }
        // namespace GB
    }
    // namespace EmuCores
}
// namespace xFF
