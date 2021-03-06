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


                    public class Cartridge_Single : Cartridge
                    {

                        public Cartridge_Single(CartridgeHeader aHeader)
                        {
                            m_cartHeader = aHeader;

                            m_romBanks.Add(new byte[0x4000]); // BANK0
                            m_romBanks.Add(new byte[0x4000]); // BANK1
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
                                    return m_romBanks[1][aOffset - 0x4000];
                                }

                                return 0xFF;
                            }

                            set { }
                        }


                        public override void SaveRAM(string filePath)
                        {
                            // Do nothing
                        }


                        public override void LoadRAM(string filePath)
                        {
                            // Do nothing
                        }


                        public static bool Validate(CartridgeHeader aHeader)
                        {
                            if (aHeader.CartType != 0)
                            {
                                return false;
                            }

                            if (aHeader.ROMSize != 0)
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
