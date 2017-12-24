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


                public class Cartridge
                {
                    List<byte[]> m_romBanks;
                    int m_curSelectedBank;
                    int m_opMode;


                    public int this[int aOffset]
                    {
                        get
                        {
                            if (aOffset < 0x4000)
                            {
                                return m_romBanks[0][aOffset];
                            }

                            else
                            {
                                return m_romBanks[m_curSelectedBank][aOffset - 0x4000];
                            }
                        }

                        set
                        {
                            // Ignore on simple ROMs
                            if (m_romBanks.Count <= 2)
                            {
                                return;
                            }


                            if (aOffset >= 0x2000 && aOffset <= 0x3FFF)
                            {
                                int lowBits = (0x1F & value);
                                if(lowBits == 0)
                                {
                                    lowBits = 1;
                                }

                                m_curSelectedBank = (0x60 & m_curSelectedBank) | lowBits;
                            }

                            else if (aOffset >= 0x4000 && aOffset <= 0x5FFF)
                            {
                                int highBits = (0x3 & value);

                                m_curSelectedBank = (0x1F & m_curSelectedBank) | (highBits << 5);
                            }
                        }
                    }


                    public Cartridge( )
                    {
                        m_romBanks = new List<byte[]>(128)
                        {
                            new byte[0x4000], // BANK0
                            new byte[0x4000]  // BANK1
                        };

                        m_curSelectedBank = 1;
                    }


                    public void SetROMBank(int aBankNum, byte[] aData)
                    {
                        while (aBankNum >= m_romBanks.Count)
                        {
                            m_romBanks.Add(new byte[0x4000]);
                        }

                        System.Buffer.BlockCopy(aData, 0, m_romBanks[aBankNum], 0, aData.Length);
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
