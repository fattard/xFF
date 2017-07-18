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

namespace xFF
{
    namespace EmuCores
    {
        namespace GB
        {
            namespace Defs
            {


                public enum HardwareModel
                {
                    DMG,    // Game Boy
                    MGB,    // Game Boy Pocket
                    MGL,    // Game Boy Light
                    SGB,    // Super Game Boy (SuperNES)
                    SGB2,   // Super Game Boy 2 (SuperNES)
                    CGB,    // Game Boy Color
                }


                public enum LinkPortDevices
                {
                    None,
                    GameLink,
                    PocketPrinter,
                    MobileAdapterGB,
                    BarcodeReader,

                    _EmuDBG,
                }


                public enum CartridgeType
                {
                    None = -1,

                    ROM,
                    MBC1,
                    MBC2,
                    MBC3,
                    MBC5,
                    MBC5_RUMBLE,
                    MBC6,
                    MBC7,
                    HuC1,  // Hudson Custom 1
                    HuC3,  // Hudson Custom 3
                    MMM01,
                    TAMA5, // Bandai Tamagotchi
                    POCKET_CAMERA,
                }


            }
            // namespace Defs
        }
        // namespace GB
    }
    // namespace EmuCores
}
// namespace xFF