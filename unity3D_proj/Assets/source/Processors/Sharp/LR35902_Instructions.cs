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
                            m_regs.HL = v;

                            m_regs.F.Z = 0;
                            m_regs.F.N = 0;
                            m_regs.F.H = HasHalfCarry16(v, m_regs.SP);
                            m_regs.F.C = HasCarry16(v, m_regs.SP);

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
                            CyclesStep(4);

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


