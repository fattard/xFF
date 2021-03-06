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
                    public const int P1   = 0xFF00;  // Joypad Ports

                    public const int SB   = 0xFF01;  // Serial Transfer
                    public const int SC   = 0xFF02;  // Serial Control

                    public const int DIV  = 0xFF04;  // Divider Read/Reset
                    public const int TIMA = 0xFF05;  // Timer Counter
                    public const int TMA  = 0xFF06;  // Timer Modulo
                    public const int TAC  = 0xFF07;  // Timer Controller


                    public const int IF   = 0xFF0F;  // Interrupt Request
                    public const int IE   = 0xFFFF;  // Interrupt Enable

                    public const int LCDC = 0xFF40;  // LCD Control
                    public const int STAT = 0xFF41;  // LCD Controller Status Flags

                    public const int SCY  = 0xFF42;  // Background ScrollY
                    public const int SCX  = 0xFF43;  // Background ScrollX

                    public const int WY   = 0xFF4A;  // Window position Y ( 0 <= WY <= 143)
                    public const int WX   = 0xFF4B;  // Window position X ( 7 <= WY <= 166)

                    public const int LY   = 0xFF44;  // LCDC Y-Cordinate - Cur Scanline
                    public const int LYC  = 0xFF45;  // Scanline Compare

                    public const int DMA  = 0xFF46;  // DMA transfer to OAM with starting address

                    public const int BGP  = 0xFF47;  // DMG Background Palette
                    public const int OBP0 = 0xFF48;  // Object Palette 0
                    public const int OBP1 = 0xFF49;  // Object Palette 1

                    public const int BOOT = 0xFF50;  // BootROM protection


                    public const int NR10 = 0xFF10;  // Sound Reg 10
                    public const int NR11 = 0xFF11;  // Sound Reg 11
                    public const int NR12 = 0xFF12;  // Sound Reg 12
                    public const int NR13 = 0xFF13;  // Sound Reg 13
                    public const int NR14 = 0xFF14;  // Sound Reg 14

                    public const int NR21 = 0xFF16;  // Sound Reg 21
                    public const int NR22 = 0xFF17;  // Sound Reg 22
                    public const int NR23 = 0xFF18;  // Sound Reg 23
                    public const int NR24 = 0xFF19;  // Sound Reg 24

                    public const int NR30 = 0xFF1A;  // Sound Reg 30
                    public const int NR31 = 0xFF1B;  // Sound Reg 31
                    public const int NR32 = 0xFF1C;  // Sound Reg 32
                    public const int NR33 = 0xFF1D;  // Sound Reg 33
                    public const int NR34 = 0xFF1E;  // Sound Reg 34

                    public const int NR41 = 0xFF20;  // Sound Reg 41
                    public const int NR42 = 0xFF21;  // Sound Reg 42
                    public const int NR43 = 0xFF22;  // Sound Reg 43
                    public const int NR44 = 0xFF23;  // Sound Reg 44

                    public const int NR50 = 0xFF24;  // Sound Reg 50
                    public const int NR51 = 0xFF25;  // Sound Reg 51
                    public const int NR52 = 0xFF26;  // Sound Reg 52

                    public const int WAVE00 = 0xFF30;  // Wave 00
                    public const int WAVE01 = 0xFF31;  // Wave 01
                    public const int WAVE02 = 0xFF32;  // Wave 02
                    public const int WAVE03 = 0xFF33;  // Wave 03
                    public const int WAVE04 = 0xFF34;  // Wave 04
                    public const int WAVE05 = 0xFF35;  // Wave 05
                    public const int WAVE06 = 0xFF36;  // Wave 06
                    public const int WAVE07 = 0xFF37;  // Wave 07
                    public const int WAVE08 = 0xFF38;  // Wave 08
                    public const int WAVE09 = 0xFF39;  // Wave 09
                    public const int WAVE10 = 0xFF3A;  // Wave 10
                    public const int WAVE11 = 0xFF3B;  // Wave 11
                    public const int WAVE12 = 0xFF3C;  // Wave 12
                    public const int WAVE13 = 0xFF3D;  // Wave 13
                    public const int WAVE14 = 0xFF3E;  // Wave 14
                    public const int WAVE15 = 0xFF3F;  // Wave 15
                }



                /// <summary>
                /// List of BitMasks for individual I/O Registers
                /// </summary>
                public static class RegsIO_Bits
                {
                    // P1
                    public const int P10 = (1 << 0); // Input Port P10
                    public const int P11 = (1 << 1); // Input Port P11
                    public const int P12 = (1 << 2); // Input Port P12
                    public const int P13 = (1 << 3); // Input Port P13
                    public const int P14 = (1 << 4); // Output Port P14
                    public const int P15 = (1 << 5); // Output Port P15


                    // SC
                    public const int SC_CLK = (1 << 0); // Shift clock: 0=External / 1=Internal
                    public const int SC_EN  = (1 << 7); // Serial Transfer Enable


                    // TAC
                    public const int TAC_CLK =     0x03; // 11b - Input Clock Select
                    public const int TAC_EN  = (1 << 2); // Timer Enable 


                    // IF
                    public const int IF_VBLANK = (1 << 0); // Vertical Blanking
                    public const int IF_STAT   = (1 << 1); // LCDC (STAT referenced)
                    public const int IF_TIMER  = (1 << 2); // Timer overflow
                    public const int IF_SERIAL = (1 << 3); // Serial I/O transfer completion
                    public const int IF_JOYPAD = (1 << 4); // P10-P13 terminal negative edge


                    // LCDC
                    public const int LCDC_BGEN    = (1 << 0); // BG Display: 1=On / 0=Off
                    public const int LCDC_OBJEN   = (1 << 1); // OBJ Display: 1=On / 0=Off
                    public const int LCDC_OBJSIZE = (1 << 2); // OBJ Block Size: 0=8x8 / 1=8x16
                    public const int LCDC_BGMAP   = (1 << 3); // BG Map: 0=0x9800-0x9BFF / 1=0x9C00-0x9FFF
                    public const int LCDC_TILE    = (1 << 4); // Tile data: 0=0x8800-0x97FF / 1=0x8000-0x8FFF
                    public const int LCDC_WEN     = (1 << 5); // Window Display: 1=On / 0=Off
                    public const int LCDC_WMAP    = (1 << 6); // Window Map: 0=0x9800-0x9BFF / 1=0x9C00-0x9FFF
                    public const int LCDC_EN      = (1 << 7); // LCD Operation: 1=On / 0=Off


                    // STAT
                    public const int STAT_MODE   =     0x03; // 11b - Mode Flag
                    public const int STAT_LYC    = (1 << 2); // Match Flag (LYC==LY)
                    public const int STAT_INTM0  = (1 << 3); // Interrupt Mode 0
                    public const int STAT_INTM1  = (1 << 4); // Interrupt Mode 1
                    public const int STAT_INTM2  = (1 << 5); // Interrupt Mode 2
                    public const int STAT_INTLYC = (1 << 6); // Interrupt Match LYC==LY


                    // IE
                    public const int IE_VBLANK = (1 << 0); // Vertical Blanking
                    public const int IE_STAT   = (1 << 1); // LCDC (STAT referenced)
                    public const int IE_TIMER  = (1 << 2); // Timer overflow
                    public const int IE_SERIAL = (1 << 3); // Serial I/O transfer completion
                    public const int IE_JOYPAD = (1 << 4); // P10-P13 terminal negative edge


                    // BOOT
                    public const int BOOT_OFF = (1 << 0);
                }


            }
            // namespace Defs
        }
        // namespace GB
    }
    // namespace EmuCores
}
// namespace xFF
