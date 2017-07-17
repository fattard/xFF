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
    namespace Processors
    {
        namespace Sharp
        {


            public partial class LR35902
            {


                void FillInstructionHandler( )
                {
                    // Placeholder for invalid Opcode
                    InstructionHandler invalidOpcode = () =>
                    {
                        if (LogError != null)
                        {
                            LogError("Invalid instruction 0x" + m_fetchedInstruction.ToString("X2") + " at address 0x" + m_regs.PC.ToString("X4"));
                        }

                        m_trappedState = true;
                        CyclesStep(4);
                    };

                    for (int i = 0; i < m_instructionHandler.Length; ++i)
                    {
                        m_instructionHandler[i] = invalidOpcode;
                    }




                    #region 8-bit Transfers


                    /*
                     * ld r1, r2
                     * =========
                     * 
                     * r1 <- r2
                     * 
                     * Desc: Loads the contents of r2 into register r1
                     * 
                     * Flags: Z N H C
                     *        - - - -
                     * 
                     * Clock Cycles:   4
                     * Machine Cycles: 1
                     * 
                     */
                    #region ld r1, r2
                    {
                        // ld A, A
                        m_instructionHandler[0x7F] = () =>
                        {
                            int r1 = (0x07 & (m_fetchedInstruction >> 3));
                            int r2 = (0x07 & m_fetchedInstruction);

                            m_regs[r1] = m_regs[r2];

                            //TODO: increase accuracy
                            CyclesStep(4);
                        };
                        // ld A, B
                        m_instructionHandler[0x78] = m_instructionHandler[0x7F];
                        // ld A, C
                        m_instructionHandler[0x79] = m_instructionHandler[0x7F];
                        // ld A, D
                        m_instructionHandler[0x7A] = m_instructionHandler[0x7F];
                        // ld A, E
                        m_instructionHandler[0x7B] = m_instructionHandler[0x7F];
                        // ld A, H
                        m_instructionHandler[0x7C] = m_instructionHandler[0x7F];
                        // ld A, L
                        m_instructionHandler[0x7D] = m_instructionHandler[0x7F];
                        // ld B, A
                        m_instructionHandler[0x47] = m_instructionHandler[0x7F];
                        // ld B, B
                        m_instructionHandler[0x40] = m_instructionHandler[0x7F];
                        // ld B, C
                        m_instructionHandler[0x41] = m_instructionHandler[0x7F];
                        // ld B, D
                        m_instructionHandler[0x42] = m_instructionHandler[0x7F];
                        // ld B, E
                        m_instructionHandler[0x43] = m_instructionHandler[0x7F];
                        // ld B, H
                        m_instructionHandler[0x44] = m_instructionHandler[0x7F];
                        // ld B, L
                        m_instructionHandler[0x45] = m_instructionHandler[0x7F];
                        // ld C, A
                        m_instructionHandler[0x4F] = m_instructionHandler[0x7F];
                        // ld C, B
                        m_instructionHandler[0x48] = m_instructionHandler[0x7F];
                        // ld C, C
                        m_instructionHandler[0x49] = m_instructionHandler[0x7F];
                        // ld C, D
                        m_instructionHandler[0x4A] = m_instructionHandler[0x7F];
                        // ld C, E
                        m_instructionHandler[0x4B] = m_instructionHandler[0x7F];
                        // ld C, H
                        m_instructionHandler[0x4C] = m_instructionHandler[0x7F];
                        // ld C, L
                        m_instructionHandler[0x4D] = m_instructionHandler[0x7F];
                        // ld D, A
                        m_instructionHandler[0x57] = m_instructionHandler[0x7F];
                        // ld D, B
                        m_instructionHandler[0x50] = m_instructionHandler[0x7F];
                        // ld D, C
                        m_instructionHandler[0x51] = m_instructionHandler[0x7F];
                        // ld D, D
                        m_instructionHandler[0x52] = m_instructionHandler[0x7F];
                        // ld D, E
                        m_instructionHandler[0x53] = m_instructionHandler[0x7F];
                        // ld D, H
                        m_instructionHandler[0x54] = m_instructionHandler[0x7F];
                        // ld D, L
                        m_instructionHandler[0x55] = m_instructionHandler[0x7F];
                        // ld E, A
                        m_instructionHandler[0x5F] = m_instructionHandler[0x7F];
                        // ld E, B
                        m_instructionHandler[0x58] = m_instructionHandler[0x7F];
                        // ld E, C
                        m_instructionHandler[0x59] = m_instructionHandler[0x7F];
                        // ld E, D
                        m_instructionHandler[0x5A] = m_instructionHandler[0x7F];
                        // ld E, E
                        m_instructionHandler[0x5B] = m_instructionHandler[0x7F];
                        // ld E, H
                        m_instructionHandler[0x5C] = m_instructionHandler[0x7F];
                        // ld E, L
                        m_instructionHandler[0x5D] = m_instructionHandler[0x7F];
                        // ld H, A
                        m_instructionHandler[0x67] = m_instructionHandler[0x7F];
                        // ld H, B
                        m_instructionHandler[0x60] = m_instructionHandler[0x7F];
                        // ld H, C
                        m_instructionHandler[0x61] = m_instructionHandler[0x7F];
                        // ld H, D
                        m_instructionHandler[0x62] = m_instructionHandler[0x7F];
                        // ld H, E
                        m_instructionHandler[0x63] = m_instructionHandler[0x7F];
                        // ld H, H
                        m_instructionHandler[0x64] = m_instructionHandler[0x7F];
                        // ld H, L
                        m_instructionHandler[0x65] = m_instructionHandler[0x7F];
                        // ld L, A
                        m_instructionHandler[0x6F] = m_instructionHandler[0x7F];
                        // ld L, B
                        m_instructionHandler[0x68] = m_instructionHandler[0x7F];
                        // ld L, C
                        m_instructionHandler[0x69] = m_instructionHandler[0x7F];
                        // ld L, D
                        m_instructionHandler[0x6A] = m_instructionHandler[0x7F];
                        // ld L, E
                        m_instructionHandler[0x6B] = m_instructionHandler[0x7F];
                        // ld L, H
                        m_instructionHandler[0x6C] = m_instructionHandler[0x7F];
                        // ld L, L
                        m_instructionHandler[0x6D] = m_instructionHandler[0x7F];
                    }
                    #endregion ld r1, r2




                    /*
                     * ld r, n
                     * =======
                     * 
                     * r <- n
                     * 
                     * Desc: Loads 8-bit immediate data n into register r
                     * 
                     * Flags: Z N H C
                     *        - - - -
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region ld r, n
                    {
                        // ld A, n
                        m_instructionHandler[0x3E] = () =>
                        {
                            int r = (0x07 & (m_fetchedInstruction >> 3));
                            m_regs[r] = Read8(m_regs.PC++);

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                        // ld B, n
                        m_instructionHandler[0x06] = m_instructionHandler[0x3E];
                        // ld C, n
                        m_instructionHandler[0x0E] = m_instructionHandler[0x3E];
                        // ld D, n
                        m_instructionHandler[0x16] = m_instructionHandler[0x3E];
                        // ld E, n
                        m_instructionHandler[0x1E] = m_instructionHandler[0x3E];
                        // ld H, n
                        m_instructionHandler[0x26] = m_instructionHandler[0x3E];
                        // ld L, n
                        m_instructionHandler[0x2E] = m_instructionHandler[0x3E];
                    }
                    #endregion ld r, n




                    /*
                     * ld r, [HL]
                     * ==========
                     * 
                     * r <- [HL]
                     * 
                     * Desc: Loads the contents of memory (8 bits) specified by register pair HL into register r
                     * 
                     * Flags: Z N H C
                     *        - - - -
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region ld r, [HL]
                    {
                        // ld A, [HL]
                        m_instructionHandler[0x7E] = () =>
                        {
                            int r = (0x07 & (m_fetchedInstruction >> 3));
                            m_regs[r] = Read8(m_regs.HL);

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                        // ld B, [HL]
                        m_instructionHandler[0x46] = m_instructionHandler[0x7E];
                        // ld C, [HL]
                        m_instructionHandler[0x4E] = m_instructionHandler[0x7E];
                        // ld D, [HL]
                        m_instructionHandler[0x56] = m_instructionHandler[0x7E];
                        // ld E, [HL]
                        m_instructionHandler[0x5E] = m_instructionHandler[0x7E];
                        // ld H, [HL]
                        m_instructionHandler[0x66] = m_instructionHandler[0x7E];
                        // ld L, [HL]
                        m_instructionHandler[0x6E] = m_instructionHandler[0x7E];
                    }
                    #endregion ld r, [HL]




                    /*
                     * ld [HL], r
                     * ==========
                     * 
                     * [HL] <- r
                     * 
                     * Desc: Stores the contents of register r in memory specified by register pair HL
                     * 
                     * Flags: Z N H C
                     *        - - - -
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region ld [HL], r
                    {
                        // ld [HL], A
                        m_instructionHandler[0x77] = () =>
                        {
                            int r = (0x07 & m_fetchedInstruction);
                            Write8(m_regs.HL, m_regs[r]);

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                        // ld [HL], B
                        m_instructionHandler[0x70] = m_instructionHandler[0x77];
                        // ld [HL], C
                        m_instructionHandler[0x71] = m_instructionHandler[0x77];
                        // ld [HL], D
                        m_instructionHandler[0x72] = m_instructionHandler[0x77];
                        // ld [HL], E
                        m_instructionHandler[0x73] = m_instructionHandler[0x77];
                        // ld [HL], H
                        m_instructionHandler[0x74] = m_instructionHandler[0x77];
                        // ld [HL], L
                        m_instructionHandler[0x75] = m_instructionHandler[0x77];
                    }
                    #endregion ld [HL], r




                    /*
                     * ld [HL], n
                     * ==========
                     * 
                     * [HL] <- n
                     * 
                     * Desc: Loads 8-bit immediate data n into memory specified by register pair HL
                     * 
                     * Flags: Z N H C
                     *        - - - -
                     * 
                     * Clock Cycles:   12
                     * Machine Cycles:  3
                     * 
                     */
                    #region ld [HL], n
                    {
                        // ld [HL], n
                        m_instructionHandler[0x36] = () =>
                        {
                            int n = Read8(m_regs.PC++);
                            Write8(m_regs.HL, n);

                            //TODO: increase accuracy
                            CyclesStep(12);
                        };
                    }
                    #endregion ld [HL], n




                    /*
                     * ld A, [BC]
                     * ==========
                     * 
                     * A <- [BC]
                     * 
                     * Desc: Loads the contents specified by the contents of register pair BC into register A
                     * 
                     * Flags: Z N H C
                     *        - - - -
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region ld A, [BC]
                    {
                        // ld A, [BC]
                        m_instructionHandler[0x0A] = () =>
                        {
                            m_regs.A = Read8(m_regs.BC);

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                    }
                    #endregion ld A, [BC]




                    /*
                     * ld A, [DE]
                     * ==========
                     * 
                     * A <- [DE]
                     * 
                     * Desc: Loads the contents specified by the contents of register pair DE into register A
                     * 
                     * Flags: Z N H C
                     *        - - - -
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region ld A, [DE]
                    {
                        // ld A, [DE]
                        m_instructionHandler[0x1A] = () =>
                        {
                            m_regs.A = Read8(m_regs.DE);

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                    }
                    #endregion ld A, [DE]




                    /*
                     * ld A, [C]
                     * ld A, [$FF00+C]
                     * ===============
                     * 
                     * A <- [$FF00+C]
                     * 
                     * Desc: Loads into register A the contents of the internal RAM, port register, or mode register at the address
                     * in the range 0xFF00-0xFFFF specified by register C
                     * 
                     * Flags: Z N H C
                     *        - - - -
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region ld A, [$FF00+C]
                    {
                        // ld A, [C]
                        // ld A, [$FF00+C]
                        m_instructionHandler[0xF2] = () =>
                        {
                            m_regs.A = Read8((0xFF00 + m_regs.C));

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                    }
                    #endregion ld A, [$FF00+C]




                    /*
                     * ld [C], A
                     * ld [$FF00+C], A
                     * ===============
                     * 
                     * [$FF00+C] <- A
                     * 
                     * Desc: Loads the contents of register A in the internal RAM, port register, or mode register at the address
                     * in the range 0xFF00-0xFFFF specified by register C
                     * 
                     * Flags: Z N H C
                     *        - - - -
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region ld [$FF00+C], A
                    {
                        // ld [C], A
                        // ld [$FF00+C], A
                        m_instructionHandler[0xE2] = () =>
                        {
                            Write8(0xFF00 + m_regs.C, m_regs.A);

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                    }
                    #endregion ld [$FF00+C], A




                    /*
                     * ld A, [n]
                     * ldh A, [n]
                     * ==========
                     * 
                     * A <- [$FF00+n]
                     * 
                     * Desc: Loads into register A the contents of the internal RAM, port register, or mode register at the address
                     * in the range 0xFF00-0xFFFF specified by 8-bit immediate operand n
                     * 
                     * Flags: Z N H C
                     *        - - - -
                     * 
                     * Clock Cycles:   12
                     * Machine Cycles:  3
                     * 
                     */
                    #region ldh A, [n]
                    {
                        // ldh A, [n]
                        m_instructionHandler[0xF0] = () =>
                        {
                            int n = Read8(m_regs.PC++);
                            m_regs.A = Read8(0xFF00 + n);

                            //TODO: increase accuracy
                            CyclesStep(12);
                        };
                    }
                    #endregion ldh A, [n]




                    /*
                     * ld [n], A
                     * ldh [n], A
                     * ==========
                     * 
                     * [$FF00+n] <- A
                     * 
                     * Desc: Loads the contents of register A to the internal RAM, port register, or mode register at the address
                     * in the range 0xFF00-0xFFFF specified by the 8-bit immediate operand n
                     * 
                     * Flags: Z N H C
                     *        - - - -
                     * 
                     * Clock Cycles:   12
                     * Machine Cycles:  3
                     * 
                     */
                    #region ldh [n], A
                    {
                        // ldh [n], A
                        m_instructionHandler[0xE0] = () =>
                        {
                            int n = Read8(m_regs.PC++);
                            Write8(0xFF00 + n, m_regs.A);

                            //TODO: increase accuracy
                            CyclesStep(12);
                        };
                    }
                    #endregion ldh [n], A




                    /*
                     * ld A, [nn]
                     * ==========
                     * 
                     * A <- [nn]
                     * 
                     * Desc: Loads into register A the contents of the internal RAM or register specified by 16-bit immediate operand nn
                     * 
                     * Flags: Z N H C
                     *        - - - -
                     * 
                     * Clock Cycles:   16
                     * Machine Cycles:  4
                     * 
                     */
                    #region ld A, [nn]
                    {
                        // ld A, [nn]
                        m_instructionHandler[0xFA] = () =>
                        {
                            int nn = Read8(m_regs.PC++);
                            nn |= (Read8(m_regs.PC++) << 8);
                            m_regs.A = Read8(nn);

                            //TODO: increase accuracy
                            CyclesStep(16);
                        };
                    }
                    #endregion ld A, [nn]




                    /*
                    * ld [nn], A
                    * ==========
                    * 
                    * [nn] <- A
                    * 
                    * Desc: Loads the contents of register A to the internal RAM or register specified by 16-bit immediate operand nn
                    * 
                    * Flags: Z N H C
                    *        - - - -
                    * 
                    * Clock Cycles:   16
                    * Machine Cycles:  4
                    * 
                    */
                    #region ld [nn], A
                    {
                        // ld [nn], A
                        m_instructionHandler[0xEA] = () =>
                        {
                            int nn = Read8(m_regs.PC++);
                            nn |= (Read8(m_regs.PC++) << 8);
                            Write8(nn, m_regs.A);

                            //TODO: increase accuracy
                            CyclesStep(16);
                        };
                    }
                    #endregion ld [nn], A




                    /*
                     * ld A, [HLI]
                     * ld A, [HL+]
                     * ldi A, [HL]
                     * ===========
                     * 
                     * A <- [HL]
                     * HL <- HL + 1
                     * 
                     * Desc: Loads in register A the contents of memory specified by the contents of register pair HL
                     * and simultaneously increments the contents of HL
                     * 
                     * Flags: Z N H C
                     *        - - - -
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region ld A, [HL+]
                    {
                        // ld A, [HLI]
                        // ld A, [HL+]
                        // ldi A, [HL]
                        m_instructionHandler[0x2A] = () =>
                        {
                            m_regs.A = Read8(m_regs.HL++);

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                    }
                    #endregion ld A, [HL+]




                    /*
                     * ld A, [HLD]
                     * ld A, [HL-]
                     * ldd A, [HL]
                     * ===========
                     * 
                     * A <- [HL]
                     * HL <- HL - 1
                     * 
                     * Desc: Loads in register A the contents of memory specified by the contents of register pair HL
                     * and simultaneously decrements the contents of HL
                     * 
                     * Flags: Z N H C
                     *        - - - -
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region ld A, [HL-]
                    {
                        // ld A, [HLD]
                        // ld A, [HL-]
                        // ldd A, [HL]
                        m_instructionHandler[0x3A] = () =>
                        {
                            m_regs.A = Read8(m_regs.HL--);

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                    }
                    #endregion ld A, [HL-]




                    /*
                     * ld [BC], A
                     * ==========
                     * 
                     * [BC] <- A
                     * 
                     * Desc: Stores the contents of register A in the memory specified by register pair BC
                     * 
                     * Flags: Z N H C
                     *        - - - -
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region ld [BC], A
                    {
                        // ld [BC], A
                        m_instructionHandler[0x02] = () =>
                        {
                            Write8(m_regs.BC, m_regs.A);

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                    }
                    #endregion ld [BC], A




                    /*
                    * ld [DE], A
                    * ==========
                    * 
                    * [DE] <- A
                    * 
                    * Desc: Stores the contents of register A in the memory specified by register pair DE
                    * 
                    * Flags: Z N H C
                    *        - - - -
                    * 
                    * Clock Cycles:   8
                    * Machine Cycles: 2
                    * 
                    */
                    #region ld [DE], A
                    {
                        // ld [DE], A
                        m_instructionHandler[0x12] = () =>
                        {
                            Write8(m_regs.DE, m_regs.A);

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                    }
                    #endregion ld [DE], A




                    /*
                     * ld [HLI], A
                     * ld [HL+], A
                     * ldi [HL], A
                     * ===========
                     * 
                     * [HL] <- A
                     * HL <- HL + 1
                     * 
                     * Desc: Stores the content of register A in the memory specified by register pair HL
                     * and simultaneously increments the contents of HL
                     * 
                     * Flags: Z N H C
                     *        - - - -
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region ld [HL+], A
                    {
                        // ld [HLI], A
                        // ld [HL+], A
                        // ldi [HL], A
                        m_instructionHandler[0x22] = () =>
                        {
                            Write8(m_regs.HL++, m_regs.A);

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                    }
                    #endregion ld [HL+], A




                    /*
                     * ld [HLD], A
                     * ld [HL-], A
                     * ldd [HL], A
                     * ===========
                     * 
                     * [HL] <- A
                     * HL <- HL - 1
                     * 
                     * Desc: Stores the content of register A in the memory specified by register pair HL
                     * and simultaneously decrements the contents of HL
                     * 
                     * Flags: Z N H C
                     *        - - - -
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region ld [HL-], A
                    {
                        // ld [HLD], A
                        // ld [HL-], A
                        // ldd [HL], A
                        m_instructionHandler[0x32] = () =>
                        {
                            Write8(m_regs.HL--, m_regs.A);

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                    }
                    #endregion ld [HL-], A


                    #endregion 8-bit Transfers



                    #region 16-bit Transfers


                    /*
                     * ld dd, nn
                     * =========
                     * 
                     * dd <- nn
                     * 
                     * Desc: Loads 2 bytes immediate data to register pair dd
                     * 
                     * Flags: Z N H C
                     *        - - - -
                     * 
                     * Clock Cycles:   12
                     * Machine Cycles:  3
                     * 
                     */
                    #region ld dd, nn
                    {
                        // ld BC, nn
                        m_instructionHandler[0x01] = () =>
                        {
                            int rL = 0;
                            int rH = 0;

                            switch (0x3 & (m_fetchedInstruction >> 4))
                            {
                                case 0x00: // BC
                                    rH = 0x00;
                                    rL = 0x01;
                                    break;

                                case 0x01: // DE
                                    rH = 0x02;
                                    rL = 0x03;
                                    break;

                                case 0x02: // HL
                                    rH = 0x04;
                                    rL = 0x05;
                                    break;
                            }

                            m_regs[rH] = Read8(m_regs.PC++);
                            m_regs[rL] = Read8(m_regs.PC++);

                            //TODO: increase accuracy
                            CyclesStep(12);
                        };
                        // ld DE, nn
                        m_instructionHandler[0x11] = m_instructionHandler[0x01];
                        // ld HL, nn
                        m_instructionHandler[0x21] = m_instructionHandler[0x01];
                        // ld SP, nn
                        m_instructionHandler[0x31] = () =>
                        {
                            m_regs.SP = Read8(m_regs.PC++);
                            m_regs.SP |= (Read8(m_regs.PC++) << 8);

                            //TODO: increase accuracy
                            CyclesStep(12);
                        };
                    }
                    #endregion ld dd, nn




                    /*
                     * ld SP, HL
                     * =========
                     * 
                     * SP <- HL
                     * 
                     * Desc: Loads the contents of register pair HL in stack pointer SP
                     * 
                     * Flags: Z N H C
                     *        - - - -
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region ld SP, HL
                    {
                        // ld SP, HL
                        m_instructionHandler[0xF9] = () =>
                        {
                            m_regs.SP = m_regs.L;
                            m_regs.SP |= (m_regs.H << 8);

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                    }
                    #endregion ld SP, HL




                    /*
                     * push qq
                     * =======
                     * 
                     * [SP - 1] <- qqH
                     * [SP - 2] <- qqL
                     * SP <- SP - 2
                     * 
                     * Desc: Pushes the contents of register pair qq onto the memory stack.
                     * First 1 is subtracted from SP and the contents of the higher portion of qq are placed on the stack.
                     * The contents of the lower portion of qq are then placed on the stack. The contents of SP are automatically decremented by 2
                     * 
                     * Flags: Z N H C
                     *        - - - -
                     * 
                     * Clock Cycles:   16
                     * Machine Cycles:  4
                     * 
                     */
                    #region push qq
                    {
                        // push BC
                        m_instructionHandler[0xC5] = () =>
                        {
                            int qqH = 0;
                            int qqL = 0;

                            switch (0x03 & (m_fetchedInstruction >> 4))
                            {
                                case 0x00: // BC
                                    qqH = 0x00;
                                    qqL = 0x01;
                                    break;

                                case 0x01: // DE
                                    qqH = 0x02;
                                    qqL = 0x03;
                                    break;

                                case 0x02: // HL
                                    qqH = 0x04;
                                    qqL = 0x05;
                                    break;
                            }

                            Write8(--m_regs.SP, m_regs[qqH]);
                            Write8(--m_regs.SP, m_regs[qqL]);

                            //TODO: increase accuracy
                            CyclesStep(16);
                        };
                        // push DE
                        m_instructionHandler[0xD5] = m_instructionHandler[0xC5];
                        // push HL
                        m_instructionHandler[0xE5] = m_instructionHandler[0xC5];
                        // push AF
                        m_instructionHandler[0xF5] = () =>
                        {
                            Write8(--m_regs.SP, m_regs.A);
                            Write8(--m_regs.SP, m_regs.F.Packed);

                            //TODO: increase accuracy
                            CyclesStep(16);
                        };
                    }
                    #endregion push qq




                    /*
                    * pop qq
                    * ======
                    * 
                    * qqL <- [SP]
                    * qqH <- [SP + 1]
                    * SP <- SP + 2
                    * 
                    * Desc: Pops contents from the memory stack and into register pair qq.
                    * First the contents of memory specified by the contents of SP are loaded in the lower portion of qq.
                    * Next, the contents of SP are incremented by 1 and the contents of the memory they specify are loaded
                    * in the upper portion of qq. The contents of SP are automatically incremented by 2.
                    * 
                    * Flags: Z N H C
                    *        - - - -
                    * 
                    * Clock Cycles:   12
                    * Machine Cycles:  3
                    * 
                    */
                    #region pop qq
                    {
                        // pop BC
                        m_instructionHandler[0xC1] = () =>
                        {
                            int qqH = 0;
                            int qqL = 0;

                            switch (0x03 & (m_fetchedInstruction >> 4))
                            {
                                case 0x00: // BC
                                    qqH = 0x00;
                                    qqL = 0x01;
                                    break;

                                case 0x01: // DE
                                    qqH = 0x02;
                                    qqL = 0x03;
                                    break;

                                case 0x02: // HL
                                    qqH = 0x04;
                                    qqL = 0x05;
                                    break;
                            }

                            m_regs[qqL] = Read8(m_regs.SP++);
                            m_regs[qqH] = Read8(m_regs.SP++);

                            //TODO: increase accuracy
                            CyclesStep(12);
                        };
                        // pop DE
                        m_instructionHandler[0xD1] = m_instructionHandler[0xC1];
                        // pop HL
                        m_instructionHandler[0xE1] = m_instructionHandler[0xC1];
                        // pop AF
                        m_instructionHandler[0xF1] = () =>
                        {
                            m_regs.F.Packed = Read8(m_regs.SP++);
                            m_regs.A = Read8(m_regs.SP++);

                            //TODO: increase accuracy
                            CyclesStep(12);
                        };
                    }
                    #endregion pop qq




                    /*
                     * ldHL SP, e
                     * ld HL, SP+e
                     * ===========
                     * 
                     * HL <- SP + e
                     * 
                     * Desc: The 8 bit operand e is added to SP and the result is stored in HL
                     * 
                     * Flags: Z N H C
                     *        0 0 * *
                     *        
                     *        Z: Reset
                     *        N: Reset
                     *        H: Set if there is a carry from bit 11; otherwise reset
                     *        C: Set if there is a carry from bit 15; otherwise reset
                     * 
                     * Clock Cycles:   12
                     * Machine Cycles:  3
                     * 
                     */
                    #region ld HL, SP+e
                    {
                        // ldHL SP, e
                        // ld HL, SP+e
                        m_instructionHandler[0xF8] = () =>
                        {
                            sbyte e = (sbyte)Read8(m_regs.PC++);
                            int v = m_regs.SP + e;

                            m_regs.F.Z = 0;
                            m_regs.F.N = 0;
                            m_regs.F.H = HasHalfCarry16(v, m_regs.SP);
                            m_regs.F.C = HasCarry16(v, m_regs.SP);

                            m_regs.HL = v;

                            //TODO: increase accuracy
                            CyclesStep(12);
                        };
                    }
                    #endregion ld HL, SP+e




                    /*
                     * ld [nn], SP
                     * ===========
                     * 
                     * [nn] <- SPl
                     * [nn+1] <- SPh
                     * 
                     * Desc: Stores the lower bytes of SP at address nn specified by the 16-bit immediate operand nn
                     * and the upper byte of SP at address nn + 1
                     * 
                     * Flags: Z N H C
                     *        - - - -
                     * 
                     * Clock Cycles:   20
                     * Machine Cycles:  5
                     * 
                     */
                    #region ld [nn], SP
                    {
                        // ld BC, nn
                        m_instructionHandler[0x08] = () =>
                        {
                            int nn = Read8(m_regs.PC++);
                            nn |= (Read8(m_regs.PC++) << 8);
                            Write8(nn, m_regs.SP);
                            Write8(nn + 1, (m_regs.SP >> 8));

                            //TODO: increase accuracy
                            CyclesStep(20);
                        };
                    }
                    #endregion ld [nn], SP


                    #endregion 16-bit Transfers



                    #region 8-bit ALU


                    /*
                     * add A, r
                     * ========
                     * 
                     * A <- A + r
                     * 
                     * Desc: Adds the content of register r to those of register A and stores the results in register A
                     * 
                     * Flags: Z N H C
                     *        * 0 * *
                     *        
                     *        Z: Set if result is 0; otherwise reset
                     *        N: Reset
                     *        H: Set if there is a carry from bit 3; otherwise reset
                     *        C: Set if there is a carry from bit 7; otherwise reset
                     * 
                     * Clock Cycles:   4
                     * Machine Cycles: 1
                     * 
                     */
                    #region add A, r
                    {
                        // add A, A
                        m_instructionHandler[0x87] = () =>
                        {
                            int r = (0x7 & m_fetchedInstruction);

                            int v = (m_regs.A + m_regs[r]);

                            m_regs.F.Z = IsZero(v);
                            m_regs.F.N = 0;
                            m_regs.F.H = HasHalfCarry8(v, m_regs.A);
                            m_regs.F.C = HasCarry8(v, m_regs.A);

                            m_regs.A = v;

                            //TODO: increase accuracy
                            CyclesStep(4);
                        };
                        // add A, B
                        m_instructionHandler[0x80] = m_instructionHandler[0x87];
                        // add A, C
                        m_instructionHandler[0x81] = m_instructionHandler[0x87];
                        // add A, D
                        m_instructionHandler[0x82] = m_instructionHandler[0x87];
                        // add A, E
                        m_instructionHandler[0x83] = m_instructionHandler[0x87];
                        // add A, H
                        m_instructionHandler[0x84] = m_instructionHandler[0x87];
                        // add A, L
                        m_instructionHandler[0x85] = m_instructionHandler[0x87];
                    }
                    #endregion add A, r




                    /*
                     * add A, n
                     * ========
                     * 
                     * A <- A + n
                     * 
                     * Desc: Adds 8-bit immediate operand n to the contents of register A and stores the results in register A
                     * 
                     * Flags: Z N H C
                     *        * 0 * *
                     *        
                     *        Z: Set if result is 0; otherwise reset
                     *        N: Reset
                     *        H: Set if there is a carry from bit 3; otherwise reset
                     *        C: Set if there is a carry from bit 7; otherwise reset
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region add A, n
                    {
                        // add A, n
                        m_instructionHandler[0xC6] = () =>
                        {
                            int n = Read8(m_regs.PC++);
                            int v = (m_regs.A + n);

                            m_regs.F.Z = IsZero(v);
                            m_regs.F.N = 0;
                            m_regs.F.H = HasHalfCarry8(v, m_regs.A);
                            m_regs.F.C = HasCarry8(v, m_regs.A);

                            m_regs.A = v;

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                    }
                    #endregion add A, n




                    /*
                     * add A, [HL]
                     * ===========
                     * 
                     * A <- A + [HL]
                     * 
                     * Desc: Adds the contents of the memory specified by the contents of register pair HL to the contents of register A and stores the results in register A
                     * 
                     * Flags: Z N H C
                     *        * 0 * *
                     *        
                     *        Z: Set if result is 0; otherwise reset
                     *        N: Reset
                     *        H: Set if there is a carry from bit 3; otherwise reset
                     *        C: Set if there is a carry from bit 7; otherwise reset
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region add A, [HL]
                    {
                        // add A, [HL]
                        m_instructionHandler[0x86] = () =>
                        {
                            int n = Read8(m_regs.HL);
                            int v = (m_regs.A + n);

                            m_regs.F.Z = IsZero(v);
                            m_regs.F.N = 0;
                            m_regs.F.H = HasHalfCarry8(v, m_regs.A);
                            m_regs.F.C = HasCarry8(v, m_regs.A);

                            m_regs.A = v;

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                    }
                    #endregion add A, [HL]




                    /*
                     * adc A, r
                     * ========
                     * 
                     * A <- A + r + carry
                     * 
                     * Desc: Adds the content of register r and carry to the contents of register A and stores the results in register A
                     * 
                     * Flags: Z N H C
                     *        * 0 * *
                     *        
                     *        Z: Set if result is 0; otherwise reset
                     *        N: Reset
                     *        H: Set if there is a carry from bit 3; otherwise reset
                     *        C: Set if there is a carry from bit 7; otherwise reset
                     * 
                     * Clock Cycles:   4
                     * Machine Cycles: 1
                     * 
                     */
                    #region adc A, r
                    {
                        // adc A, A
                        m_instructionHandler[0x8F] = () =>
                        {
                            int r = (0x07 & m_fetchedInstruction);

                            int v = (m_regs.A + m_regs[r] + m_regs.F.C);

                            m_regs.F.Z = IsZero(v);
                            m_regs.F.N = 0;
                            m_regs.F.H = HasHalfCarry8(v, m_regs.A);
                            m_regs.F.C = HasCarry8(v, m_regs.A);

                            m_regs.A = v;

                            //TODO: increase accuracy
                            CyclesStep(4);
                        };
                        // adc A, B
                        m_instructionHandler[0x88] = m_instructionHandler[0x8F];
                        // adc A, C
                        m_instructionHandler[0x89] = m_instructionHandler[0x8F];
                        // adc A, D
                        m_instructionHandler[0x8A] = m_instructionHandler[0x8F];
                        // adc A, E
                        m_instructionHandler[0x8B] = m_instructionHandler[0x8F];
                        // adc A, H
                        m_instructionHandler[0x8C] = m_instructionHandler[0x8F];
                        // adc A, L
                        m_instructionHandler[0x8D] = m_instructionHandler[0x8F];

                    }
                    #endregion adc A, r




                    /*
                     * adc A, n
                     * ========
                     * 
                     * A <- A + n + carry
                     * 
                     * Desc: Adds 8-bit immediate operand n and carry to the contents of register A and stores the results in register A
                     * 
                     * Flags: Z N H C
                     *        * 0 * *
                     *        
                     *        Z: Set if result is 0; otherwise reset
                     *        N: Reset
                     *        H: Set if there is a carry from bit 3; otherwise reset
                     *        C: Set if there is a carry from bit 7; otherwise reset
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region adc A, n
                    {
                        // adc A, n
                        m_instructionHandler[0xCE] = () =>
                        {
                            int n = Read8(m_regs.PC++);
                            int v = (m_regs.A + n + m_regs.F.C);

                            m_regs.F.Z = IsZero(v);
                            m_regs.F.N = 0;
                            m_regs.F.H = HasHalfCarry8(v, m_regs.A);
                            m_regs.F.C = HasCarry8(v, m_regs.A);

                            m_regs.A = v;

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                    }
                    #endregion adc A, n




                    /*
                     * adc A, [HL]
                     * ===========
                     * 
                     * A <- A + [HL] + carry
                     * 
                     * Desc: Adds the contents of the memory specified by the contents of register pair HL and carry to the contents of register A and stores the results in register A
                     * 
                     * Flags: Z N H C
                     *        * 0 * *
                     *        
                     *        Z: Set if result is 0; otherwise reset
                     *        N: Reset
                     *        H: Set if there is a carry from bit 3; otherwise reset
                     *        C: Set if there is a carry from bit 7; otherwise reset
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region adc A, [HL]
                    {
                        // adc A, [HL]
                        m_instructionHandler[0x8E] = () =>
                        {
                            int n = Read8(m_regs.HL);
                            int v = (m_regs.A + n + m_regs.F.C);

                            m_regs.F.Z = IsZero(v);
                            m_regs.F.N = 0;
                            m_regs.F.H = HasHalfCarry8(v, m_regs.A);
                            m_regs.F.C = HasCarry8(v, m_regs.A);

                            m_regs.A = v;

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                    }
                    #endregion adc A, [HL]




                    /*
                     * sub r
                     * =====
                     * 
                     * A <- A - r
                     * 
                     * Desc: Subtracts the content of register r from the contents of register A and stores the results in register A
                     * 
                     * Flags: Z N H C
                     *        * 1 * *
                     *        
                     *        Z: Set if result is 0; otherwise reset
                     *        N: Set
                     *        H: Set if there is a borrow from bit 4; otherwise reset
                     *        C: Set if there is a borrow; otherwise reset
                     * 
                     * Clock Cycles:   4
                     * Machine Cycles: 1
                     * 
                     */
                    #region sub r
                    {
                        // sub A
                        m_instructionHandler[0x97] = () =>
                        {
                            int r = (0x07 & m_fetchedInstruction);

                            int v = (m_regs.A - m_regs[r]);

                            m_regs.F.Z = IsZero(v);
                            m_regs.F.N = 1;
                            m_regs.F.C = (v < 0) ? 1 : 0;
                            m_regs.F.H = HasHalfBorrow8(v, m_regs.A);

                            m_regs.A = v;

                            //TODO: increase accuracy
                            CyclesStep(4);
                        };
                        // sub B
                        m_instructionHandler[0x90] = m_instructionHandler[0x97];
                        // sub C
                        m_instructionHandler[0x91] = m_instructionHandler[0x97];
                        // sub D
                        m_instructionHandler[0x92] = m_instructionHandler[0x97];
                        // sub E
                        m_instructionHandler[0x93] = m_instructionHandler[0x97];
                        // sub H
                        m_instructionHandler[0x94] = m_instructionHandler[0x97];
                        // sub L
                        m_instructionHandler[0x95] = m_instructionHandler[0x97];
                    }
                    #endregion sub r




                    /*
                     * sub n
                     * =====
                     * 
                     * A <- A - n
                     * 
                     * Desc: Subtracts 8-bit immediate operand n from the contents of register A and stores the results in register A
                     * 
                     * Flags: Z N H C
                     *        * 1 * *
                     *        
                     *        Z: Set if result is 0; otherwise reset
                     *        N: Set
                     *        H: Set if there is a borrow from bit 4; otherwise reset
                     *        C: Set if there is a borrow; otherwise reset
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region sub n
                    {
                        // sub n
                        m_instructionHandler[0xD6] = () =>
                        {
                            int n = Read8(m_regs.PC++);
                            int v = (m_regs.A - n);

                            m_regs.F.Z = IsZero(v);
                            m_regs.F.N = 1;
                            m_regs.F.C = (v < 0) ? 1 : 0;
                            m_regs.F.H = HasHalfBorrow8(v, m_regs.A);

                            m_regs.A = v;

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                    }
                    #endregion sub n




                    /*
                     * sub [HL]
                     * ========
                     * 
                     * A <- A - [HL]
                     * 
                     * Desc: Subtracts the contents of the memory specified by the contents of register pair HL from the contents of register A and stores the results in register A
                     * 
                     * Flags: Z N H C
                     *        * 1 * *
                     *        
                     *        Z: Set if result is 0; otherwise reset
                     *        N: Set
                     *        H: Set if there is a borrow from bit 4; otherwise reset
                     *        C: Set if there is a borrow; otherwise reset
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region sub [HL]
                    {
                        // sub [HL]
                        m_instructionHandler[0x96] = () =>
                        {
                            int n = Read8(m_regs.HL);
                            int v = (m_regs.A - n);

                            m_regs.F.Z = IsZero(v);
                            m_regs.F.N = 1;
                            m_regs.F.C = (v < 0) ? 1 : 0;
                            m_regs.F.H = HasHalfBorrow8(v, m_regs.A);

                            m_regs.A = v;

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                    }
                    #endregion sub [HL]




                    /*
                     * sbc r
                     * =====
                     * 
                     * A <- A - r - carry
                     * 
                     * Desc: Subtracts the content of register r and carry from the contents of register A and stores the results in register A
                     * 
                     * Flags: Z N H C
                     *        * 1 * *
                     *        
                     *        Z: Set if result is 0; otherwise reset
                     *        N: Set
                     *        H: Set if there is a borrow from bit 4; otherwise reset
                     *        C: Set if there is a borrow; otherwise reset
                     * 
                     * Clock Cycles:   4
                     * Machine Cycles: 1
                     * 
                     */
                    #region sbc r
                    {
                        // sbc A
                        m_instructionHandler[0x9F] = () =>
                        {
                            int r = (0x07 & m_fetchedInstruction);

                            int v = (m_regs.A - m_regs[r] - m_regs.F.C);

                            m_regs.F.Z = IsZero(v);
                            m_regs.F.N = 1;
                            m_regs.F.C = (v < 0) ? 1 : 0;
                            m_regs.F.H = HasHalfBorrow8(v, m_regs.A);

                            m_regs.A = v;

                            //TODO: increase accuracy
                            CyclesStep(4);
                        };
                        // sbc B
                        m_instructionHandler[0x98] = m_instructionHandler[0x9F];
                        // sbc C
                        m_instructionHandler[0x99] = m_instructionHandler[0x9F];
                        // sbc D
                        m_instructionHandler[0x9A] = m_instructionHandler[0x9F];
                        // sbc E
                        m_instructionHandler[0x9B] = m_instructionHandler[0x9F];
                        // sbc H
                        m_instructionHandler[0x9C] = m_instructionHandler[0x9F];
                        // sbc L
                        m_instructionHandler[0x9D] = m_instructionHandler[0x9F];
                    }
                    #endregion sbc r




                    /*
                     * sbc n
                     * =====
                     * 
                     * A <- A - n - carry
                     * 
                     * Desc: Subtracts 8-bit immediate operand n and carry from the contents of register A and stores the results in register A
                     * 
                     * Flags: Z N H C
                     *        * 1 * *
                     *        
                     *        Z: Set if result is 0; otherwise reset
                     *        N: Set
                     *        H: Set if there is a borrow from bit 4; otherwise reset
                     *        C: Set if there is a borrow; otherwise reset
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region sbc n
                    {
                        // sbc n
                        m_instructionHandler[0xDE] = () =>
                        {
                            int n = Read8(m_regs.PC++);
                            int v = (m_regs.A - n - m_regs.F.C);

                            m_regs.F.Z = IsZero(v);
                            m_regs.F.N = 1;
                            m_regs.F.C = (v < 0) ? 1 : 0;
                            m_regs.F.H = HasHalfBorrow8(v, m_regs.A);

                            m_regs.A = v;

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                    }
                    #endregion sbc n




                    /*
                     * sbc [HL]
                     * ========
                     * 
                     * A <- A - [HL] - carry
                     * 
                     * Desc: Subtracts the contents of the memory specified by the contents of register pair HL and carry from the contents of register A and stores the results in register A
                     * 
                     * Flags: Z N H C
                     *        * 1 * *
                     *        
                     *        Z: Set if result is 0; otherwise reset
                     *        N: Set
                     *        H: Set if there is a borrow from bit 4; otherwise reset
                     *        C: Set if there is a borrow; otherwise reset
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region sbc [HL]
                    {
                        // sbc [HL]
                        m_instructionHandler[0x9E] = () =>
                        {
                            int n = Read8(m_regs.HL);
                            int v = (m_regs.A - n - m_regs.F.C);

                            m_regs.F.Z = IsZero(v);
                            m_regs.F.N = 1;
                            m_regs.F.C = (v < 0) ? 1 : 0;
                            m_regs.F.H = HasHalfBorrow8(v, m_regs.A);

                            m_regs.A = v;

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                    }
                    #endregion sbc [HL]




                    /*
                     * and r
                     * =====
                     * 
                     * A <- A & r
                     * 
                     * Desc: Takes the logical-AND for each bit of the contents of register r and register A and stores the results in register A
                     * 
                     * Flags: Z N H C
                     *        * 0 1 0
                     *        
                     *        Z: Set if result is 0; otherwise reset
                     *        N: Reset
                     *        H: Set
                     *        C: Reset
                     * 
                     * Clock Cycles:   4
                     * Machine Cycles: 1
                     * 
                     */
                    #region and r
                    {
                        // and A
                        m_instructionHandler[0xA7] = () =>
                        {
                            int r = (0x07 & m_fetchedInstruction);

                            int v = (m_regs.A & m_regs[r]);

                            m_regs.F.Z = IsZero(v);
                            m_regs.F.N = 0;
                            m_regs.F.H = 1;
                            m_regs.F.C = 0;

                            m_regs.A = v;

                            //TODO: increase accuracy
                            CyclesStep(4);
                        };
                        // and B
                        m_instructionHandler[0xA0] = m_instructionHandler[0xA7];
                        // and C
                        m_instructionHandler[0xA1] = m_instructionHandler[0xA7];
                        // and D
                        m_instructionHandler[0xA2] = m_instructionHandler[0xA7];
                        // and E
                        m_instructionHandler[0xA3] = m_instructionHandler[0xA7];
                        // and H
                        m_instructionHandler[0xA4] = m_instructionHandler[0xA7];
                        // and L
                        m_instructionHandler[0xA5] = m_instructionHandler[0xA7];
                    }
                    #endregion and r




                    /*
                     * and n
                     * =====
                     * 
                     * A <- A & n
                     * 
                     * Desc: Takes the logical-AND for each bit of the contents of the 8-bit immediate operand n and register A and stores the results in register A
                     * 
                     * Flags: Z N H C
                     *        * 0 1 0
                     *        
                     *        Z: Set if result is 0; otherwise reset
                     *        N: Reset
                     *        H: Set
                     *        C: Reset
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region and n
                    {
                        // and n
                        m_instructionHandler[0xE6] = () =>
                        {
                            int n = Read8(m_regs.PC++);
                            int v = (m_regs.A & n);

                            m_regs.F.Z = IsZero(v);
                            m_regs.F.N = 0;
                            m_regs.F.H = 1;
                            m_regs.F.C = 0;

                            m_regs.A = v;

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                    }
                    #endregion and n




                    /*
                     * and [HL]
                     * ========
                     * 
                     * A <- A & [HL]
                     * 
                     * Desc: Takes the logical-AND for each bit of the contents of the memory specified by the contents of register pair HL
                     * and register A and stores the results in register A
                     * 
                     * Flags: Z N H C
                     *        * 0 1 0
                     *        
                     *        Z: Set if result is 0; otherwise reset
                     *        N: Reset
                     *        H: Set
                     *        C: Reset
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region and [HL]
                    {
                        // and [HL]
                        m_instructionHandler[0xA6] = () =>
                        {
                            int n = Read8(m_regs.HL);
                            int v = (m_regs.A & n);

                            m_regs.F.Z = IsZero(v);
                            m_regs.F.N = 0;
                            m_regs.F.H = 1;
                            m_regs.F.C = 0;

                            m_regs.A = v;

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                    }
                    #endregion and [HL]




                    /*
                     * or r
                     * ====
                     * 
                     * A <- A | r
                     * 
                     * Desc: Takes the logical-OR for each bit of the contents of register r and register A and stores the results in register A
                     * 
                     * Flags: Z N H C
                     *        * 0 0 0
                     *        
                     *        Z: Set if result is 0; otherwise reset
                     *        N: Reset
                     *        H: Reset
                     *        C: Reset
                     * 
                     * Clock Cycles:   4
                     * Machine Cycles: 1
                     * 
                     */
                    #region or r
                    {
                        // or A
                        m_instructionHandler[0xB7] = () =>
                        {
                            int r = (0x07 & m_fetchedInstruction);

                            int v = (m_regs.A | m_regs[r]);

                            m_regs.F.Z = IsZero(v);
                            m_regs.F.N = 0;
                            m_regs.F.H = 0;
                            m_regs.F.C = 0;

                            m_regs.A = v;

                            //TODO: increase accuracy
                            CyclesStep(4);
                        };
                        // or B
                        m_instructionHandler[0xB0] = m_instructionHandler[0xB7];
                        // or C
                        m_instructionHandler[0xB1] = m_instructionHandler[0xB7];
                        // or D
                        m_instructionHandler[0xB2] = m_instructionHandler[0xB7];
                        // or E
                        m_instructionHandler[0xB3] = m_instructionHandler[0xB7];
                        // or H
                        m_instructionHandler[0xB4] = m_instructionHandler[0xB7];
                        // or L
                        m_instructionHandler[0xB5] = m_instructionHandler[0xB7];
                    }
                    #endregion or r




                    /*
                     * or n
                     * ====
                     * 
                     * A <- A | n
                     * 
                     * Desc: Takes the logical-OR for each bit of the contents of the 8-bit immediate operand n and register A and stores the results in register A
                     * 
                     * Flags: Z N H C
                     *        * 0 0 0
                     *        
                     *        Z: Set if result is 0; otherwise reset
                     *        N: Reset
                     *        H: Reset
                     *        C: Reset
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region or n
                    {
                        // or n
                        m_instructionHandler[0xF6] = () =>
                        {
                            int n = Read8(m_regs.PC++);
                            int v = (m_regs.A | n);

                            m_regs.F.Z = IsZero(v);
                            m_regs.F.N = 0;
                            m_regs.F.H = 0;
                            m_regs.F.C = 0;

                            m_regs.A = v;

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                    }
                    #endregion or n




                    /*
                     * or [HL]
                     * =======
                     * 
                     * A <- A | [HL]
                     * 
                     * Desc: Takes the logical-OR for each bit of the contents of the memory specified by the contents of register pair HL
                     * and register A and stores the results in register A
                     * 
                     * Flags: Z N H C
                     *        * 0 0 0
                     *        
                     *        Z: Set if result is 0; otherwise reset
                     *        N: Reset
                     *        H: Reset
                     *        C: Reset
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region or [HL]
                    {
                        // or [HL]
                        m_instructionHandler[0xB6] = () =>
                        {
                            int n = Read8(m_regs.HL);
                            int v = (m_regs.A | n);

                            m_regs.F.Z = IsZero(v);
                            m_regs.F.N = 0;
                            m_regs.F.H = 0;
                            m_regs.F.C = 0;

                            m_regs.A = v;

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                    }
                    #endregion or [HL]




                    /*
                     * xor r
                     * =====
                     * 
                     * A <- A ^ r
                     * 
                     * Desc: Takes the logical-XOR for each bit of the contents of register r and register A and stores the results in register A
                     * 
                     * Flags: Z N H C
                     *        * 0 0 0
                     *        
                     *        Z: Set if result is 0; otherwise reset
                     *        N: Reset
                     *        H: Reset
                     *        C: Reset
                     * 
                     * Clock Cycles:   4
                     * Machine Cycles: 1
                     * 
                     */
                    #region xor r
                    {
                        // xor A
                        m_instructionHandler[0xAF] = () =>
                        {
                            int r = (0x07 & m_fetchedInstruction);

                            int v = (m_regs.A ^ m_regs[r]);

                            m_regs.F.Z = IsZero(v);
                            m_regs.F.N = 0;
                            m_regs.F.H = 0;
                            m_regs.F.C = 0;

                            m_regs.A = v;

                            //TODO: increase accuracy
                            CyclesStep(4);
                        };
                        // xor B
                        m_instructionHandler[0xA8] = m_instructionHandler[0xAF];
                        // xor C
                        m_instructionHandler[0xA9] = m_instructionHandler[0xAF];
                        // xor D
                        m_instructionHandler[0xAA] = m_instructionHandler[0xAF];
                        // xor E
                        m_instructionHandler[0xAB] = m_instructionHandler[0xAF];
                        // xor H
                        m_instructionHandler[0xAC] = m_instructionHandler[0xAF];
                        // xor L
                        m_instructionHandler[0xAD] = m_instructionHandler[0xAF];
                    }
                    #endregion xor r




                    /*
                     * xor n
                     * =====
                     * 
                     * A <- A ^ n
                     * 
                     * Desc: Takes the logical-XOR for each bit of the contents of the 8-bit immediate operand n and register A and stores the results in register A
                     * 
                     * Flags: Z N H C
                     *        * 0 0 0
                     *        
                     *        Z: Set if result is 0; otherwise reset
                     *        N: Reset
                     *        H: Reset
                     *        C: Reset
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region xor n
                    {
                        // xor n
                        m_instructionHandler[0xEE] = () =>
                        {
                            int n = Read8(m_regs.PC++);
                            int v = (m_regs.A ^ n);

                            m_regs.F.Z = IsZero(v);
                            m_regs.F.N = 0;
                            m_regs.F.H = 0;
                            m_regs.F.C = 0;

                            m_regs.A = v;

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                    }
                    #endregion xor n




                    /*
                     * xor [HL]
                     * ========
                     * 
                     * A <- A ^ [HL]
                     * 
                     * Desc: Takes the logical-XOR for each bit of the contents of the memory specified by the contents of register pair HL
                     * and register A and stores the results in register A
                     * 
                     * Flags: Z N H C
                     *        * 0 0 0
                     *        
                     *        Z: Set if result is 0; otherwise reset
                     *        N: Reset
                     *        H: Reset
                     *        C: Reset
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region xor [HL]
                    {
                        // xor [HL]
                        m_instructionHandler[0xAE] = () =>
                        {
                            int n = Read8(m_regs.HL);
                            int v = (m_regs.A ^ n);

                            m_regs.F.Z = IsZero(v);
                            m_regs.F.N = 0;
                            m_regs.F.H = 0;
                            m_regs.F.C = 0;

                            m_regs.A = v;

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                    }
                    #endregion xor [HL]




                    /*
                     * cp r
                     * ====
                     * 
                     * A == r
                     * 
                     * Desc: Compares the content of register r and register A and sets the flags if they are equal
                     * 
                     * Flags: Z N H C
                     *        * 1 * *
                     *        
                     *        Z: Set if result is 0; otherwise reset
                     *        N: Set
                     *        H: Set if there is a borrow from bit 4; otherwise reset
                     *        C: Set if there is a borrow; otherwise reset
                     * 
                     * Clock Cycles:   4
                     * Machine Cycles: 1
                     * 
                     */
                    #region cp r
                    {
                        // cp A
                        m_instructionHandler[0xBF] = () =>
                        {
                            int r = (0x07 & m_fetchedInstruction);

                            int v = (m_regs.A - m_regs[r]);

                            m_regs.F.Z = IsZero(v);
                            m_regs.F.N = 1;
                            m_regs.F.C = (v < 0) ? 1 : 0;
                            m_regs.F.H = HasHalfBorrow8(v, m_regs.A);

                            //TODO: increase accuracy
                            CyclesStep(4);
                        };
                        // cp B
                        m_instructionHandler[0xB8] = m_instructionHandler[0xBF];
                        // cp C
                        m_instructionHandler[0xB9] = m_instructionHandler[0xBF];
                        // cp D
                        m_instructionHandler[0xBA] = m_instructionHandler[0xBF];
                        // cp E
                        m_instructionHandler[0xBB] = m_instructionHandler[0xBF];
                        // cp H
                        m_instructionHandler[0xBC] = m_instructionHandler[0xBF];
                        // cp L
                        m_instructionHandler[0xBD] = m_instructionHandler[0xBF];
                    }
                    #endregion cp r




                    /*
                     * cp n
                     * ====
                     * 
                     * A == n
                     * 
                     * Desc: Compares the 8-bit immediate operand n and register A and sets the flags if they are equal
                     * 
                     * Flags: Z N H C
                     *        * 1 * *
                     *        
                     *        Z: Set if result is 0; otherwise reset
                     *        N: Set
                     *        H: Set if there is a borrow from bit 4; otherwise reset
                     *        C: Set if there is a borrow; otherwise reset
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region cp n
                    {
                        // cp n
                        m_instructionHandler[0xFE] = () =>
                        {
                            int n = Read8(m_regs.PC++);
                            int v = (m_regs.A - n);

                            m_regs.F.Z = IsZero(v);
                            m_regs.F.N = 1;
                            m_regs.F.C = (v < 0) ? 1 : 0;
                            m_regs.F.H = HasHalfBorrow8(v, m_regs.A);

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                    }
                    #endregion cp n




                    /*
                     * cp [HL]
                     * ========
                     * 
                     * A == [HL]
                     * 
                     * Desc: Compares the contents of the memory specified by the contents of register pair HL and register A and sets the flags if they are equal
                     * 
                     * Flags: Z N H C
                     *        * 1 * *
                     *        
                     *        Z: Set if result is 0; otherwise reset
                     *        N: Set
                     *        H: Set if there is a borrow from bit 4; otherwise reset
                     *        C: Set if there is a borrow; otherwise reset
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region cp [HL]
                    {
                        // cp [HL]
                        m_instructionHandler[0xBE] = () =>
                        {
                            int n = Read8(m_regs.HL);
                            int v = (m_regs.A - n);

                            m_regs.F.Z = IsZero(v);
                            m_regs.F.N = 1;
                            m_regs.F.C = (v < 0) ? 1 : 0;
                            m_regs.F.H = HasHalfBorrow8(v, m_regs.A);

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                    }
                    #endregion cp [HL]




                    /*
                     * inc r
                     * =====
                     * 
                     * r <- r + 1
                     * 
                     * Desc: Increments the contents of register r by 1
                     * 
                     * Flags: Z N H C
                     *        * 0 * -
                     *        
                     *        Z: Set if result is 0; otherwise reset
                     *        N: Reset
                     *        H: Set if there is a carry from bit 3; otherwise reset
                     *        C: Not affected
                     * 
                     * Clock Cycles:   4
                     * Machine Cycles: 1
                     * 
                     */
                    #region inc r
                    {
                        // inc A
                        m_instructionHandler[0x3C] = () =>
                        {
                            int r = (0x07 & (m_fetchedInstruction >> 3));

                            int v = m_regs[r] + 1;

                            m_regs.F.Z = IsZero(v);
                            m_regs.F.N = 0;
                            m_regs.F.H = HasHalfCarry8(v, m_regs[r]);

                            m_regs[r] = v;

                            //TODO: increase accuracy
                            CyclesStep(4);
                        };
                        // inc B
                        m_instructionHandler[0x04] = m_instructionHandler[0x3C];
                        // inc C
                        m_instructionHandler[0x0C] = m_instructionHandler[0x3C];
                        // inc D
                        m_instructionHandler[0x14] = m_instructionHandler[0x3C];
                        // inc E
                        m_instructionHandler[0x1C] = m_instructionHandler[0x3C];
                        // inc H
                        m_instructionHandler[0x24] = m_instructionHandler[0x3C];
                        // inc L
                        m_instructionHandler[0x2C] = m_instructionHandler[0x3C];
                    }
                    #endregion inc r




                    /*
                     * inc [HL]
                     * ========
                     * 
                     * [HL] <- [HL] + 1
                     * 
                     * Desc: Increments by 1 the contents of memory specified register pair HL
                     * 
                     * Flags: Z N H C
                     *        * 0 * -
                     *        
                     *        Z: Set if result is 0; otherwise reset
                     *        N: Reset
                     *        H: Set if there is a carry from bit 3; otherwise reset
                     *        C: Not affected
                     * 
                     * Clock Cycles:   12
                     * Machine Cycles:  3
                     * 
                     */
                    #region inc [HL]
                    {
                        // inc [HL]
                        m_instructionHandler[0x34] = () =>
                        {
                            int n = Read8(m_regs.HL);
                            int v = n + 1;

                            m_regs.F.Z = IsZero(v);
                            m_regs.F.N = 0;
                            m_regs.F.H = HasHalfCarry8(v, n);

                            Write8(m_regs.HL, v);

                            //TODO: increase accuracy
                            CyclesStep(12);
                        };
                    }
                    #endregion inc [HL]




                    /*
                     * dec r
                     * =====
                     * 
                     * r <- r - 1
                     * 
                     * Desc: Subtracts 1 from the contents of register r
                     * 
                     * Flags: Z N H C
                     *        * 1 * -
                     *        
                     *        Z: Set if result is 0; otherwise reset
                     *        N: Set
                     *        H: Set if there is a borrow from bit 4; otherwise reset
                     *        C: Not affected
                     * 
                     * Clock Cycles:   4
                     * Machine Cycles: 1
                     * 
                     */
                    #region dec r
                    {
                        // dec A
                        m_instructionHandler[0x3D] = () =>
                        {
                            int r = (0x07 & (m_fetchedInstruction >> 3));

                            int v = (m_regs[r] - 1);

                            m_regs.F.Z = IsZero(v);
                            m_regs.F.N = 1;
                            m_regs.F.H = HasHalfBorrow8(v, m_regs[r]);

                            m_regs[r] = v;

                            //TODO: increase accuracy
                            CyclesStep(4);
                        };
                        // dec B
                        m_instructionHandler[0x05] = m_instructionHandler[0x3D];
                        // dec C
                        m_instructionHandler[0x0D] = m_instructionHandler[0x3D];
                        // dec D
                        m_instructionHandler[0x15] = m_instructionHandler[0x3D];
                        // dec E
                        m_instructionHandler[0x1D] = m_instructionHandler[0x3D];
                        // dec H
                        m_instructionHandler[0x25] = m_instructionHandler[0x3D];
                        // dec L
                        m_instructionHandler[0x2D] = m_instructionHandler[0x3D];
                    }
                    #endregion dec r




                    /*
                     * dec [HL]
                     * ========
                     * 
                     * [HL] <- [HL] - 1
                     * 
                     * Subtracts 1 from the contents of memory specified register pair HL
                     * 
                     * Flags: Z N H C
                     *        * 1 * -
                     *        
                     *        Z: Set if result is 0; otherwise reset
                     *        N: Set
                     *        H: Set if there is a borrow from bit 4; otherwise reset
                     *        C: Not affected
                     * 
                     * Clock Cycles:   12
                     * Machine Cycles:  3
                     * 
                     */
                    #region dec [HL]
                    {
                        // dec [HL]
                        m_instructionHandler[0x35] = () =>
                        {
                            int n = Read8(m_regs.HL);
                            int v = (n - 1);

                            m_regs.F.Z = IsZero(v);
                            m_regs.F.N = 1;
                            m_regs.F.H = HasHalfBorrow8(v, n);

                            Write8(m_regs.HL, v);

                            //TODO: increase accuracy
                            CyclesStep(12);
                        };
                    }
                    #endregion dec [HL]


                    #endregion 8-bit ALU



                    #region 16-bit Arithmetic


                    /*
                     * add HL, ss
                     * ==========
                     * 
                     * HL <- HL + ss
                     * 
                     * Desc: Adds the content of register pair ss to the contents of register pair HL and stores the results in HL
                     * 
                     * Flags: Z N H C
                     *        - 0 * *
                     *        
                     *        Z: Not affected
                     *        N: Reset
                     *        H: Set if there is a carry from bit 11; otherwise reset
                     *        C: Set if there is a carry from bit 15; otherwise reset
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region add HL, ss
                    {
                        // add HL, BC
                        m_instructionHandler[0x09] = () =>
                        {
                            int ss = 0;

                            switch (0x03 & (m_fetchedInstruction >> 4))
                            {
                                case 0x00: // BC
                                    ss = m_regs.BC;
                                    break;

                                case 0x01: // DE
                                    ss = m_regs.DE;
                                    break;

                                case 0x02: // HL
                                    ss = m_regs.HL;
                                    break;

                                case 0x03: // SP
                                    ss = m_regs.SP;
                                    break;
                            }

                            int v = (m_regs.HL + ss);

                            m_regs.F.N = 0;
                            m_regs.F.H = HasHalfCarry16(v, m_regs.HL);
                            m_regs.F.C = HasCarry16(v, m_regs.HL);

                            m_regs.HL = v;

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                        // add HL, DE
                        m_instructionHandler[0x19] = m_instructionHandler[0x09];
                        // add HL, HL
                        m_instructionHandler[0x29] = m_instructionHandler[0x09];
                        // add HL, SP
                        m_instructionHandler[0x39] = m_instructionHandler[0x09];
                    }
                    #endregion add HL, ss




                    /*
                     * add SP, e
                     * =========
                     * 
                     * SP <- SP + e
                     * 
                     * Desc: Adds the contents of the 8 bit immediate operand e and SP and stores the result in SP
                     * 
                     * Flags: Z N H C
                     *        0 0 * *
                     *        
                     *        Z: Reset
                     *        N: Reset
                     *        H: Set if there is a carry from bit 11; otherwise reset
                     *        C: Set if there is a carry from bit 15; otherwise reset
                     * 
                     * Clock Cycles:   16
                     * Machine Cycles:  4
                     * 
                     */
                    #region add SP, e
                    {
                        // add SP, e
                        m_instructionHandler[0xE8] = () =>
                        {
                            sbyte e = (sbyte)Read8(m_regs.PC++);
                            int v = m_regs.SP + e;

                            m_regs.F.Z = 0;
                            m_regs.F.N = 0;
                            m_regs.F.H = HasHalfCarry16(v, m_regs.SP);
                            m_regs.F.C = HasCarry16(v, m_regs.SP);

                            m_regs.SP = v;

                            //TODO: increase accuracy
                            CyclesStep(16);
                        };
                    }
                    #endregion add SP, e




                    /*
                     * inc ss
                     * ======
                     * 
                     * ss <- ss + 1
                     * 
                     * Desc: Increments the contents of register pair ss by 1
                     * 
                     * Flags: Z N H C
                     *        - - - -
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region inc ss
                    {
                        // inc BC
                        m_instructionHandler[0x03] = () =>
                        {
                            switch (0x03 & (m_fetchedInstruction >> 4))
                            {
                                case 0x00: // BC
                                    ++m_regs.BC;
                                    break;

                                case 0x01: // DE
                                    ++m_regs.DE;
                                    break;

                                case 0x02: // HL
                                    ++m_regs.HL;
                                    break;

                                case 0x03: // SP
                                    ++m_regs.SP;
                                    break;
                            }

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                        // inc DE
                        m_instructionHandler[0x13] = m_instructionHandler[0x03];
                        // inc HL
                        m_instructionHandler[0x23] = m_instructionHandler[0x03];
                        // inc SP
                        m_instructionHandler[0x33] = m_instructionHandler[0x03];
                    }
                    #endregion inc ss




                    /*
                     * dec ss
                     * ======
                     * 
                     * ss <- ss - 1
                     * 
                     * Desc: Decrements the contents of register pair ss by 1
                     * 
                     * Flags: Z N H C
                     *        - - - -
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region dec ss
                    {
                        // dec BC
                        m_instructionHandler[0x0B] = () =>
                        {
                            switch (0x03 & (m_fetchedInstruction >> 4))
                            {
                                case 0x00: // BC
                                    --m_regs.BC;
                                    break;

                                case 0x01: // DE
                                    --m_regs.DE;
                                    break;

                                case 0x02: // HL
                                    --m_regs.HL;
                                    break;

                                case 0x03: // SP
                                    --m_regs.SP;
                                    break;
                            }

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                        // dec DE
                        m_instructionHandler[0x1B] = m_instructionHandler[0x0B];
                        // dec HL
                        m_instructionHandler[0x2B] = m_instructionHandler[0x0B];
                        // dec SP
                        m_instructionHandler[0x3B] = m_instructionHandler[0x0B];
                    }
                    #endregion dec ss


                    #endregion 16- bit Arithmetic



                    #region Rotate & Shift Instructions


                    /*
                     * rlca
                     * ====
                     * 
                     *     _____________
                     *     |           |
                     * CY <- A << A <---
                     * 
                     * Desc: Rotates the contents of register A to the left. That is, the contents of bit 0 are copied to bit 1
                     * and the previous content of bit 1 (the contents before the copy operation) are copied to bit 2.
                     * The same operation is repeated in sequence for the rest of the register.
                     * The contents of bit 7 are placed in both carry and bit 0 of register A.
                     * 
                     * Flags: Z N H C
                     *        0 0 0 *
                     *        
                     *        Z: Reset
                     *        N: Reset
                     *        H: Reset
                     *        C: Old bit 7 data
                     * 
                     * Clock Cycles:   4
                     * Machine Cycles: 1
                     * 
                     */
                    #region rlca
                    {
                        // rlca
                        m_instructionHandler[0x07] = () =>
                        {
                            m_regs.F.C = (m_regs.A >> 7);
                            m_regs.A = (m_regs.A << 1) | (m_regs.F.C << 0);
                            m_regs.F.Z = 0;
                            m_regs.F.N = 0;
                            m_regs.F.H = 0;

                            //TODO: increase accuracy
                            CyclesStep(4);
                        };
                    }
                    #endregion rlca




                    /*
                     * rla
                     * ===
                     * 
                     *  ________________
                     *  |              |
                     * CY <- A << A <---
                     * 
                     * Desc: Rotates the contents of register A to the left.
                     * 
                     * Flags: Z N H C
                     *        0 0 0 *
                     *        
                     *        Z: Reset
                     *        N: Reset
                     *        H: Reset
                     *        C: Old bit 7 data
                     * 
                     * Clock Cycles:   4
                     * Machine Cycles: 1
                     * 
                     */
                    #region rla
                    {
                        // rla
                        m_instructionHandler[0x17] = () =>
                        {
                            int bit7 = (m_regs.A >> 7);
                            m_regs.A = (m_regs.A << 1) | (m_regs.F.C << 0);
                            m_regs.F.C = bit7;
                            m_regs.F.Z = 0;
                            m_regs.F.N = 0;
                            m_regs.F.H = 0;

                            //TODO: increase accuracy
                            CyclesStep(4);
                        };
                    }
                    #endregion rla




                    /*
                     * rrca
                     * ====
                     * 
                     *     ____________
                     *     |          |
                     *     --> A >> A -> CY
                     * 
                     * Desc: Rotates the contents of register A to the right. That is, the contents of bit 7 are copied to bit 6
                     * and the previous content of bit 6 (the contents before the copy operation) are copied to bit 5.
                     * The same operation is repeated in sequence for the rest of the register.
                     * The contents of bit 0 are placed in both carry and bit 7 of register A.
                     * 
                     * Flags: Z N H C
                     *        0 0 0 *
                     *        
                     *        Z: Reset
                     *        N: Reset
                     *        H: Reset
                     *        C: Old bit 0 data
                     * 
                     * Clock Cycles:   4
                     * Machine Cycles: 1
                     * 
                     */
                    #region rrca
                    {
                        // rrca
                        m_instructionHandler[0x0F] = () =>
                        {
                            m_regs.F.C = (m_regs.A & 0x01);
                            m_regs.A = (m_regs.A >> 1) | (m_regs.F.C << 7);
                            m_regs.F.Z = 0;
                            m_regs.F.N = 0;
                            m_regs.F.H = 0;

                            //TODO: increase accuracy
                            CyclesStep(4);
                        };
                    }
                    #endregion rrca




                    /*
                     * rra
                     * ===
                     * 
                     *  ________________
                     *  |              |
                     *  ---> A >> A -> CY
                     * 
                     * Desc: Rotates the contents of register A to the right.
                     * 
                     * Flags: Z N H C
                     *        0 0 0 *
                     *        
                     *        Z: Reset
                     *        N: Reset
                     *        H: Reset
                     *        C: Old bit 0 data
                     * 
                     * Clock Cycles:   4
                     * Machine Cycles: 1
                     * 
                     */
                    #region rra
                    {
                        // rra
                        m_instructionHandler[0x1F] = () =>
                        {
                            int bit0 = (m_regs.A & 0x01);
                            m_regs.A = (m_regs.A >> 1) | (m_regs.F.C << 7);
                            m_regs.F.C = bit0;
                            m_regs.F.Z = 0;
                            m_regs.F.N = 0;
                            m_regs.F.H = 0;

                            //TODO: increase accuracy
                            CyclesStep(4);
                        };
                    }
                    #endregion rra


                    #endregion Rotate & Shift Instructions



                    #region Jump Instructions


                    /*
                     * jp nn
                     * =====
                     * 
                     * PC <- nn
                     * 
                     * Desc: Loads the operand nn to the program counter (PC). The operand nn specifies the address of the subsequently executed instruction.
                     * The lower-order byte is placed in byte 2 of the object code and the higher-order byte is placed in byte 3.
                     * 
                     * Flags: Z N H C
                     *        - - - -
                     * 
                     * Clock Cycles:   16
                     * Machine Cycles:  4
                     * 
                     */
                    #region jp nn
                    {
                        // jp nn
                        m_instructionHandler[0xC3] = () =>
                        {
                            int j = Read8(m_regs.PC++);
                            j |= (Read8(m_regs.PC++) << 8);

                            m_regs.PC = j;

                            //TODO: increase accuracy
                            CyclesStep(16);
                        };
                    }
                    #endregion jp nn




                    /*
                     * jp cc, nn
                     * =========
                     * 
                     * if (cc == true)
                     *   PC <- nn
                     * 
                     * Desc: Loads the operand nn to the program counter (PC) if the condition cc and the flag status match.
                     * The subsequent instruction starts at address nn.
                     * If the condition of cc and the flag status do not match, the contents of PC are incremented, and the
                     * instruction following the current 'jp' instruction is executed.
                     * 
                     * Flags: Z N H C
                     *        - - - -
                     * 
                     * Clock Cycles:   12/16
                     * Machine Cycles:  3/4
                     * 
                     */
                    #region jp cc, nn
                    {
                        // jp NZ, nn
                        m_instructionHandler[0xC2] = () =>
                        {
                            bool shouldJump = false;

                            switch (0x03 & (m_fetchedInstruction >> 3))
                            {
                                case 0x00: // NZ
                                    shouldJump = (m_regs.F.Z == 0);
                                    break;

                                case 0x01: // Z
                                    shouldJump = (m_regs.F.Z == 1);
                                    break;

                                case 0x02: // NC
                                    shouldJump = (m_regs.F.C == 0);
                                    break;

                                case 0x03: // C
                                    shouldJump = (m_regs.F.C == 1);
                                    break;
                            }

                            int j = Read8(m_regs.PC++);
                            j |= (Read8(m_regs.PC++) << 8);

                            if (shouldJump)
                            {
                                m_regs.PC = j;

                                //TODO: increase accuracy
                                CyclesStep(16);
                            }

                            else
                            {
                                //TODO: increase accuracy
                                CyclesStep(12);
                            }
                        };
                        // jp Z, nn
                        m_instructionHandler[0xCA] = m_instructionHandler[0xC2];
                        // jp NC, nn
                        m_instructionHandler[0xD2] = m_instructionHandler[0xC2];
                        // jp C, nn
                        m_instructionHandler[0xDA] = m_instructionHandler[0xC2];
                    }
                    #endregion jp cc, nn




                    /*
                     * jr e
                     * ====
                     * 
                     * PC <- PC + e
                     * 
                     * Desc: Jumps -127 to +129 steps from current address
                     * 
                     * Flags: Z N H C
                     *        - - - -
                     * 
                     * Clock Cycles:   12
                     * Machine Cycles:  3
                     * 
                     */
                    #region jr e
                    {
                        // jr e
                        m_instructionHandler[0x18] = () =>
                        {
                            sbyte e = (sbyte)Read8(m_regs.PC++);
                            m_regs.PC = (m_regs.PC + e);

                            //TODO: increase accuracy
                            CyclesStep(12);
                        };
                    }
                    #endregion jr e




                    /*
                     * jr cc, e
                     * ========
                     * 
                     * if (cc == true)
                     *   PC <- PC + e
                     * 
                     * Desc: If condition cc and the flag status match, jumps -127 to +129 steps from current address.
                     * If cc and flag status do not match, the instruction following the current 'jp' instruction is executed.
                     * 
                     * Flags: Z N H C
                     *        - - - -
                     * 
                     * Clock Cycles:   8/12
                     * Machine Cycles: 2/3
                     * 
                     */
                    #region jr cc, e
                    {
                        // jr NZ, e
                        m_instructionHandler[0x20] = () =>
                        {
                            bool shouldJump = false;

                            switch (0x03 & (m_fetchedInstruction >> 3))
                            {
                                case 0x00: // NZ
                                    shouldJump = (m_regs.F.Z == 0);
                                    break;

                                case 0x01: // Z
                                    shouldJump = (m_regs.F.Z == 1);
                                    break;

                                case 0x02: // NC
                                    shouldJump = (m_regs.F.C == 0);
                                    break;

                                case 0x03: // C
                                    shouldJump = (m_regs.F.C == 1);
                                    break;
                            }

                            sbyte e = (sbyte)Read8(m_regs.PC++);

                            if (shouldJump)
                            {
                                m_regs.PC = (m_regs.PC + e);

                                //TODO: increase accuracy
                                CyclesStep(12);
                            }

                            else
                            {
                                //TODO: increase accuracy
                                CyclesStep(8);
                            }
                        };
                        // jr Z, e
                        m_instructionHandler[0x28] = m_instructionHandler[0x20];
                        // jr NC, e
                        m_instructionHandler[0x30] = m_instructionHandler[0x20];
                        // jr C, e
                        m_instructionHandler[0x38] = m_instructionHandler[0x20];

                    }
                    #endregion jr cc, e




                    /*
                     * jp [HL]
                     * jp HL
                     * =======
                     * 
                     * PC <- HL
                     * 
                     * Desc: Loads the contents of register pair HL in program counter PC.
                     * The next instruction if fetched from the location specified by the new value of PC.
                     * 
                     * Flags: Z N H C
                     *        - - - -
                     * 
                     * Clock Cycles:   4
                     * Machine Cycles: 1
                     * 
                     */
                    #region jp HL
                    {
                        // jp HL
                        m_instructionHandler[0xE9] = () =>
                        {
                            int j = m_regs.HL;
                            m_regs.PC = j;

                            //TODO: increase accuracy
                            CyclesStep(4);
                        };
                    }
                    #endregion jp HL


                    #endregion Jump Instructions



                    #region Call and Return Instructions


                    /*
                     * call nn
                     * =======
                     * 
                     * [SP - 1] <- PC_h
                     * [SP - 2] <- PC_l
                     * PC <- nn
                     * SP <- SP - 2
                     * 
                     * Desc: In memory, pushes the PC value corresponding to the instruction at the address following that of the
                     * 'Call' instruction to the 2 bytes following the byte specified by the current SP. Operand nn is then loaded in the PC.
                     * 
                     * Flags: Z N H C
                     *        - - - -
                     * 
                     * Clock Cycles:   24
                     * Machine Cycles:  6
                     * 
                     */
                    #region call nn
                    {
                        // call nn
                        m_instructionHandler[0xCD] = () =>
                        {
                            int j = Read8(m_regs.PC++);
                            j |= (Read8(m_regs.PC++) << 8);
                            Write8(--m_regs.SP, ((m_regs.PC >> 8) & 0xFF));
                            Write8(--m_regs.SP, (m_regs.PC & 0xFF));
                            m_regs.PC = j;

                            //TODO: increase accuracy
                            CyclesStep(24);
                        };
                    }
                    #endregion call nn




                    /*
                     * call cc, nn
                     * =======
                     * 
                     * if (cc == true)
                     *   [SP - 1] <- PC_h
                     *   [SP - 2] <- PC_l
                     *   PC <- nn
                     *   SP <- SP - 2
                     * 
                     * Desc: If condition cc matches the flag, the PC value corresponding to the instruction at the address following that of the
                     * 'Call' instruction to the 2 bytes following the byte specified by the current SP. Operand nn is then loaded in the PC.
                     * 
                     * Flags: Z N H C
                     *        - - - -
                     * 
                     * Clock Cycles:   24/12
                     * Machine Cycles:  6/3
                     * 
                     */
                    #region call cc, nn
                    {
                        // call NZ, nn
                        m_instructionHandler[0xC4] = () =>
                        {
                            bool shouldJump = false;

                            switch (0x03 & (m_fetchedInstruction >> 3))
                            {
                                case 0x00: // NZ
                                    shouldJump = (m_regs.F.Z == 0);
                                    break;

                                case 0x01: // Z
                                    shouldJump = (m_regs.F.Z == 1);
                                    break;

                                case 0x02: // NC
                                    shouldJump = (m_regs.F.C == 0);
                                    break;

                                case 0x03: // C
                                    shouldJump = (m_regs.F.C == 1);
                                    break;
                            }

                            int j = Read8(m_regs.PC++);
                            j |= (Read8(m_regs.PC++) << 8);

                            if (shouldJump)
                            {
                                Write8(--m_regs.SP, ((m_regs.PC >> 8) & 0xFF));
                                Write8(--m_regs.SP, (m_regs.PC & 0xFF));
                                m_regs.PC = j;

                                //TODO: increase accuracy
                                CyclesStep(24);
                            }

                            else
                            {
                                //TODO: increase accuracy
                                CyclesStep(12);
                            }
                        };
                        // call Z, nn
                        m_instructionHandler[0xCC] = m_instructionHandler[0xC4];
                        // call NC, nn
                        m_instructionHandler[0xD4] = m_instructionHandler[0xC4];
                        // call C, nn
                        m_instructionHandler[0xDC] = m_instructionHandler[0xC4];
                    }
                    #endregion call cc, nn




                    /*
                     * ret
                     * ===
                     * 
                     * PC_l <- [SP]
                     * PC_h <- [SP + 1]
                     * SP <- SP + 2
                     * 
                     * Desc: Pops from memory stack the PC value pushed when the subroutine was called, returning control to the source program.
                     * 
                     * Flags: Z N H C
                     *        - - - -
                     * 
                     * Clock Cycles:   16
                     * Machine Cycles:  4
                     * 
                     */
                    #region ret
                    {
                        // ret
                        m_instructionHandler[0xC9] = () =>
                        {
                            int j = Read8(m_regs.SP++);
                            j |= (Read8(m_regs.SP++) << 8);
                            m_regs.PC = j;

                            //TODO: increase accuracy
                            CyclesStep(16);
                        };
                    }
                    #endregion ret




                    /*
                     * reti
                     * ====
                     * 
                     * PC_l <- [SP]
                     * PC_h <- [SP + 1]
                     * SP <- SP + 2
                     * 
                     * Desc: Used when an interrupt-service routine finishes.
                     * The execution of this return is as follow.
                     * 
                     * The address for the return from the interrupt is loaded in program counter PC.
                     * The master interrupt enable flag is returned to its pre-interrupt status.
                     * 
                     * Flags: Z N H C
                     *        - - - -
                     * 
                     * Clock Cycles:   16
                     * Machine Cycles:  4
                     * 
                     */
                    #region reti
                    {
                        // reti
                        m_instructionHandler[0xD9] = () =>
                        {
                            int j = Read8(m_regs.SP++);
                            j |= (Read8(m_regs.SP++) << 8);
                            m_regs.PC = j;
                            m_interruptsMasterFlagEnabled = true;

                            //TODO: increase accuracy
                            CyclesStep(16);
                        };
                    }
                    #endregion reti




                    /*
                     * ret cc
                     * ======
                     * 
                     * if (cc == true)
                     *   PC_l <- [SP]
                     *   PC_h <- [SP + 1]
                     *   SP <- SP + 2
                     * 
                     * Desc: If condition cc and the flag match, control is returned to the source program by popping from the memory
                     * stack the PC value pushed to the stack when the subroutine was called.
                     * 
                     * Flags: Z N H C
                     *        - - - -
                     * 
                     * Clock Cycles:   20/8
                     * Machine Cycles:  5/2
                     * 
                     */
                    #region ret cc
                    {
                        // ret NZ
                        m_instructionHandler[0xC0] = () =>
                        {
                            bool shouldJump = false;

                            switch (0x03 & (m_fetchedInstruction >> 3))
                            {
                                case 0x00: // NZ
                                    shouldJump = (m_regs.F.Z == 0);
                                    break;

                                case 0x01: // Z
                                    shouldJump = (m_regs.F.Z == 1);
                                    break;

                                case 0x02: // NC
                                    shouldJump = (m_regs.F.C == 0);
                                    break;

                                case 0x03: // C
                                    shouldJump = (m_regs.F.C == 1);
                                    break;
                            }

                            if (shouldJump)
                            {
                                int j = Read8(m_regs.SP++);
                                j |= (Read8(m_regs.SP++) << 8);
                                m_regs.PC = j;

                                //TODO: increase accuracy
                                CyclesStep(20);
                            }

                            else
                            {
                                //TODO: increase accuracy
                                CyclesStep(8);
                            }
                        };
                        // ret Z
                        m_instructionHandler[0xC8] = m_instructionHandler[0xC0];
                        // ret NC
                        m_instructionHandler[0xD0] = m_instructionHandler[0xC0];
                        // ret C
                        m_instructionHandler[0xD8] = m_instructionHandler[0xC0];
                    }
                    #endregion ret cc




                    /*
                     * rst t
                     * =====
                     * 
                     * [SP - 1] <- PC_h
                     * [SP - 2] <- PC_l
                     * SP <- SP - 2
                     * PC_h <- 0  PC_l <- P
                     * 
                     * Desc: Pushes the current value of the PC to the memory stack and loads to the PC the page 0 memory
                     * addresses provided by operand t.
                     * 
                     * Flags: Z N H C
                     *        - - - -
                     * 
                     * Clock Cycles:   16
                     * Machine Cycles:  4
                     * 
                     */
                    #region rst t
                    {
                        // rst $00
                        m_instructionHandler[0xC7] = () =>
                        {
                            int j = 0;

                            switch (0x07 & (m_fetchedInstruction >> 3))
                            {
                                case 0x00: // $00
                                    j = 0x0000;
                                    break;

                                case 0x01: // $08
                                    j = 0x0008;
                                    break;

                                case 0x02: // $10
                                    j = 0x0010;
                                    break;

                                case 0x03: // $18
                                    j = 0x0018;
                                    break;

                                case 0x04: // $20
                                    j = 0x0020;
                                    break;

                                case 0x05: // $28
                                    j = 0x0028;
                                    break;

                                case 0x06: // $30
                                    j = 0x0030;
                                    break;

                                case 0x07: // $38
                                    j = 0x0038;
                                    break;
                            }

                            Write8(--m_regs.SP, ((m_regs.PC >> 8) & 0xFF));
                            Write8(--m_regs.SP, (m_regs.PC & 0xFF));
                            m_regs.PC = j;

                            //TODO: increase accuracy
                            CyclesStep(16);
                        };
                        // rst $08
                        m_instructionHandler[0xCF] = m_instructionHandler[0xC7];
                        // rst $10
                        m_instructionHandler[0xD7] = m_instructionHandler[0xC7];
                        // rst $18
                        m_instructionHandler[0xDF] = m_instructionHandler[0xC7];
                        // rst $20
                        m_instructionHandler[0xE7] = m_instructionHandler[0xC7];
                        // rst $28
                        m_instructionHandler[0xEF] = m_instructionHandler[0xC7];
                        // rst $30
                        m_instructionHandler[0xF7] = m_instructionHandler[0xC7];
                        // rst $38
                        m_instructionHandler[0xFF] = m_instructionHandler[0xC7];
                    }
                    #endregion rst t


                    #endregion Call and Return Instructions



                    #region Misc Instructions


                    /*
                    * nop
                    * ===
                    * 
                    * N/A
                    * 
                    * Desc: Only advances the program counter by 1; performs no other operations that have an effect
                    * 
                    * Flags: Z N H C
                    *        - - - -
                    *        
                    * Clock Cycles:   4
                    * Machine Cycles: 1
                    * 
                    */
                    #region nop
                    {
                        // nop
                        m_instructionHandler[0x00] = () =>
                        {
                            //TODO: increase accuracy
                            CyclesStep(4);
                        };
                    }
                    #endregion nop


                    #endregion Misc Instructions


                    // Extended opcode
                    #region Extended opcode 0xCB
                    {
                        m_instructionHandler[0xCB] = () =>
                        {
                            m_fetchedInstruction = Read8(m_regs.PC++);

                            // Decode Extended Instruction
                            m_extendedInstructionHandler[m_fetchedInstruction]();
                        };
                    }
                    #endregion Extended opcode 0xCB

                }


            }


        }
        // namespace Sharp
    }
    // namespace Processors
}
// namespace xFF


