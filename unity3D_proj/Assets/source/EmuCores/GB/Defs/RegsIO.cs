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


                /// <summary>
                /// List of I/O Registers
                /// </summary>
                public static class RegsIO
                {
                    public const int P1   = 0xFF00;  // Joystick Port

                    public const int SCY  = 0xFF42;  // Background ScrollY
                    public const int SCX  = 0xFF43;  // Background ScrollX

                    public const int LY   = 0xFF44;  // LCDC Y-Cordinate - Cur Scanline

                    public const int BGP  = 0xFF47;  // DMG Background Palette

                    public const int BOOT = 0xFF50;  // BootROM protection

                }



                /// <summary>
                /// List of BitMasks for individual I/O Registers
                /// </summary>
                public static class RegsIO_Bits
                {
                    // P1
                    public const int P10 = (1 << 0);
                    public const int P11 = (1 << 1);
                    public const int P12 = (1 << 2);
                    public const int P13 = (1 << 3);
                    public const int P14 = (1 << 4);
                    public const int P15 = (1 << 5);

                    // BOOT
                    public const int BOOT_LOCK = (1 << 0);

                }


            }
            // namespace Defs
        }
        // namespace GB
    }
    // namespace EmuCores
}
// namespace xFF
