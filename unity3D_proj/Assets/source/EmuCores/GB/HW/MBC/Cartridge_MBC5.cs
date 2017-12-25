/*
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
using System.Collections.Generic;

namespace xFF
{
    namespace EmuCores
    {
        namespace GB
        {
            namespace HW
            {
                namespace MBC
                {


                    public class Cartridge_MBC5 : Cartridge
                    {
                        int m_curBank_lowBits;
                        int m_curBank_highBits;
                        int m_curBank_sram;
                        bool m_isRAMEnabled;
                        int m_mask;


                        public Cartridge_MBC5(CartridgeHeader aHeader)
                        {
                            m_cartHeader = aHeader;

                            int totalROMBanks = 0;

                            switch (m_cartHeader.ROMSize)
                            {
                                case 0x01: // 64KB (4 banks)
                                    totalROMBanks = 4;
                                    break;

                                case 0x02: // 128KB (8 banks)
                                    totalROMBanks = 8;
                                    break;

                                case 0x03: // 256KB (16 banks)
                                    totalROMBanks = 16;
                                    break;

                                case 0x04: // 512KB (32 banks)
                                    totalROMBanks = 32;
                                    break;

                                case 0x05: // 1MB (64 banks)
                                    totalROMBanks = 64;
                                    break;

                                case 0x06: // 2MB (128 banks)
                                    totalROMBanks = 128;
                                    break;

                                case 0x07: // 4MB (256 banks)
                                    totalROMBanks = 256;
                                    break;

                                case 0x08: // 8MB (512 banks)
                                    totalROMBanks = 512;
                                    break;

                                default:
                                    totalROMBanks = 2; // 32KB (2 banks)
                                    break;
                            }

                            m_mask = (totalROMBanks - 1);

                            while (m_romBanks.Count < totalROMBanks)
                            {
                                m_romBanks.Add(new byte[0x4000]);
                            }


                            int totalRAMBanks = 0;

                            switch (m_cartHeader.RAMSize)
                            {
                                case 0:
                                    totalRAMBanks = 0;
                                    break;

                                case 1:
                                    totalRAMBanks = 1;
                                    m_ramBanks.Add(new byte[2 * 1024]);
                                    break;

                                case 2:
                                    totalRAMBanks = 1;
                                    break;

                                case 3:
                                    totalRAMBanks = 4;
                                    break;

                                case 4:
                                    totalRAMBanks = 16;
                                    break;

                                case 5:
                                    totalRAMBanks = 8;
                                    break;

                                default:
                                    totalRAMBanks = 16;
                                    break;
                            }

                            while (m_ramBanks.Count < totalRAMBanks)
                            {
                                m_ramBanks.Add(new byte[0x2000]);

                                // Inits to 0xFF
                                for (int i = 0; i < m_ramBanks[m_ramBanks.Count - 1].Length; ++i)
                                {
                                    m_ramBanks[m_ramBanks.Count - 1][i] = 0xFF;
                                }
                            }
                        }


                        public override int this[int aOffset]
                        {
                            get
                            {
                                if (aOffset < 0x4000)
                                {
                                    return m_romBanks[0][aOffset];
                                }

                                else if (aOffset < 0x8000)
                                {
                                    return m_romBanks[CurSelectedBank][aOffset - 0x4000];
                                }

                                // SRAM
                                else if (aOffset >= 0xA000 && aOffset <= 0xBFFF)
                                {
                                    if (!m_isRAMEnabled || m_ramBanks.Count == 0)
                                    {
                                        return 0xFF;
                                    }

                                    else
                                    {
                                        return m_ramBanks[m_curBank_sram][aOffset - 0xA000];
                                    }
                                }

                                // Invalid offset
                                return 0xFF;
                            }

                            set
                            {
                                if (aOffset <= 0x1FFF)
                                {
                                    m_isRAMEnabled = (0x0F & value) == 0x0A ? true : false;
                                }

                                else if (aOffset >= 0x2000 && aOffset <= 0x2FFF)
                                {
                                    m_curBank_lowBits = (0xFF & value);
                                }

                                else if (aOffset >= 0x3000 && aOffset <= 0x3FFF)
                                {
                                    m_curBank_highBits = (0x1 & value);
                                }

                                else if (aOffset >= 0x4000 && aOffset <= 0x5FFF)
                                {
                                    m_curBank_sram = (0x0F & value);
                                }

                                // SRAM
                                else if (aOffset >= 0xA000 && aOffset <= 0xBFFF)
                                {
                                    if (!m_isRAMEnabled || m_ramBanks.Count == 0)
                                    {
                                        // Ignore writes
                                        return;
                                    }

                                    m_ramBanks[m_curBank_sram][aOffset - 0xA000] = (byte)(0xFF & value);
                                }
                            }
                        }


                        int CurSelectedBank
                        {
                            get
                            {
                                return (m_mask & (m_curBank_lowBits | (m_curBank_highBits << 8)));
                            }
                        }


                        public override void SaveRAM(string filePath)
                        {
                            if (m_ramBanks.Count == 0)
                            {
                                return;
                            }

                            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(32 * 1024))
                            {
                                for (int i = 0; i < m_ramBanks.Count; ++i)
                                {
                                    ms.Write(m_ramBanks[i], 0, m_ramBanks[i].Length);
                                }

                                System.IO.File.WriteAllBytes(filePath, ms.ToArray());
                            }
                        }


                        public override void LoadRAM(string filePath)
                        {
                            if (!System.IO.File.Exists(filePath))
                            {
                                return;
                            }

                            byte[] sav = System.IO.File.ReadAllBytes(filePath);

                            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(sav))
                            {
                                for (int i = 0; i < m_ramBanks.Count; ++i)
                                {
                                    ms.Read(m_ramBanks[i], 0, m_ramBanks[i].Length);
                                }
                            }
                        }


                        public static bool Validate(CartridgeHeader aHeader)
                        {
                            if (aHeader.CartType != 0x19 && aHeader.CartType != 0x1A && aHeader.CartType != 0x1B &&
                                aHeader.CartType != 0x1C && aHeader.CartType != 0x1D && aHeader.CartType != 0x1E)
                            {
                                return false;
                            }
                            
                            if (aHeader.ROMSize > 0x08) // Limited to 512 banks of 32KB
                            {
                                return false;
                            }
                            
                            if (aHeader.RAMSize > 0x05) // Limited to 16 banks of 8KB
                            {
                                return false;
                            }

                            return true;
                        }
                    }


                }
                // namespace MBC
            }
            // namespace HW
        }
        // namespace GB
    }
    // namespace EmuCores
}
// namespace xFF
