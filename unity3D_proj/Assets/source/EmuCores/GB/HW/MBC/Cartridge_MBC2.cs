/*
*   This file is part of xFF
*   Copyright (C) 2017-2021 Fabio Attard
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


                    public class Cartridge_MBC2 : Cartridge
                    {
                        int m_curBank;
                        bool m_isRAMEnabled;
                        int m_mask;
                        

                        public Cartridge_MBC2(CartridgeHeader aHeader)
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

                                default:
                                    totalROMBanks = 2; // 32KB (2 banks)
                                    break;
                            }

                            m_mask = (totalROMBanks - 1);

                            while (m_romBanks.Count < totalROMBanks)
                            {
                                m_romBanks.Add(new byte[0x4000]);
                            }


                            m_ramBanks.Add(new byte[0x200]); // 512 x 4 bits

                            // Inits to 0xF0
                            for (int i = 0; i < m_ramBanks[m_ramBanks.Count - 1].Length; ++i)
                            {
                                m_ramBanks[m_ramBanks.Count - 1][i] = 0xF0;
                            }


                            m_curBank = 1;
                            m_isRAMEnabled = false;
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
                                    // Wrap around at 0xA1FF
                                    aOffset = 0xA000 | (0x1FF & aOffset);

                                    if (!m_isRAMEnabled || m_ramBanks.Count == 0)
                                    {
                                        return 0xFF;
                                    }


                                    return (0xF0 | m_ramBanks[0][aOffset - 0xA000]);
                                }

                                // Invalid offset
                                return 0xFF;
                            }

                            set
                            {
                                if (aOffset < 0x4000)
                                {
                                    // The LSB of the high-byte must be 0 to trigger
                                    if ((0x100 & aOffset) == 0)
                                    {
                                        m_isRAMEnabled = (0x0F & value) == 0x0A ? true : false;
                                    }

                                    // The LSB of the high-byte must be 1 to trigger
                                    else
                                    {
                                        m_curBank = (0x0F & value);
                                        if (m_curBank == 0)
                                        {
                                            m_curBank = 1;
                                        }
                                    }
                                }

                                // SRAM
                                else if (aOffset >= 0xA000 && aOffset <= 0xBFFF)
                                {
                                    // Wrap around at 0xA1FF
                                    aOffset = 0xA000 | (0x1FF & aOffset);

                                    if (!m_isRAMEnabled || m_ramBanks.Count == 0)
                                    {
                                        // Ignore writes
                                        return;
                                    }

                                    // Stores the lower 4 bits
                                    m_ramBanks[0][aOffset - 0xA000] = (byte)(0xF0 | (0x0F & value));
                                }
                            }
                        }


                        int CurSelectedBank
                        {
                            get
                            {
                                return (m_mask & m_curBank);
                            }
                        }


                        public override void SaveRAM(string filePath)
                        {
                            if (m_ramBanks.Count == 0)
                            {
                                return;
                            }

                            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(512))
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
                            if (aHeader.CartType != 0x05 && aHeader.CartType != 0x06)
                            {
                                return false;
                            }

                            if (aHeader.ROMSize > 0x03) // Limited to 16 banks of 32KB
                            {
                                return false;
                            }

                            if (aHeader.RAMSize > 0x00) // MBC2 must have this value as 0
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
