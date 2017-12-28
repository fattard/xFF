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


                void FillExtendedInstructionHandler( )
                {
                    // Placeholder for invalid Opcode
                    InstructionHandler invalidOpcode = () =>
                    {
                        if (LogError != null)
                        {
                            LogError("Invalid extended instruction 0x" + m_fetchedInstruction.ToString("X2") + " at address 0x" + m_regs.PC.ToString("X4"));
                        }

                        m_trappedState = true;
                    };

                    for (int i = 0; i < m_extendedInstructionHandler.Length; ++i)
                    {
                        m_extendedInstructionHandler[i] = invalidOpcode;
                    }




                    #region Rotate & Shift Instructions


                    /*
                     * rlc r
                     * =====
                     * 
                     *     _____________
                     *     |           |
                     * CY <- r << r  <---
                     * 
                     * Desc: Rotates the contents of register r to the left. That is, the contents of bit 0 are copied to bit 1
                     * and the previous content of bit 1 (the contents before the copy operation) are copied to bit 2.
                     * The same operation is repeated in sequence for the rest of the register.
                     * The contents of bit 7 are placed in both carry and bit 0 of register r.
                     * 
                     * Flags: Z N H C
                     *        * 0 0 *
                     *        
                     *        Z: Set if result is zero
                     *        N: Reset
                     *        H: Reset
                     *        C: Old bit 7 data
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region rlc r
                    {
                        // rlc A
                        m_extendedInstructionHandler[0x07] = () =>
                        {
                            int r = (0x07 & m_fetchedInstruction);

                            m_regs.F.C = (m_regs[r] >> 7);
                            m_regs[r] = (m_regs[r] << 1) | (m_regs.F.C << 0);
                            m_regs.F.Z = IsZero(m_regs[r]);
                            m_regs.F.N = 0;
                            m_regs.F.H = 0;

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                        // rlc B
                        m_extendedInstructionHandler[0x00] = m_extendedInstructionHandler[0x07];
                        // rlc C
                        m_extendedInstructionHandler[0x01] = m_extendedInstructionHandler[0x07];
                        // rlc D
                        m_extendedInstructionHandler[0x02] = m_extendedInstructionHandler[0x07];
                        // rlc E
                        m_extendedInstructionHandler[0x03] = m_extendedInstructionHandler[0x07];
                        // rlc H
                        m_extendedInstructionHandler[0x04] = m_extendedInstructionHandler[0x07];
                        // rlc L
                        m_extendedInstructionHandler[0x05] = m_extendedInstructionHandler[0x07];
                    }
                    #endregion rlc r




                    /*
                     * rlc [HL]
                     * ========
                     * 
                     *     ___________________
                     *     |                 |
                     * CY <- [HL] << [HL]  <---
                     * 
                     * Desc: Rotates the contents memory pointed by register pair HL to the left. That is, the contents of bit 0 are copied to bit 1
                     * and the previous content of bit 1 (the contents before the copy operation) are copied to bit 2.
                     * The same operation is repeated in sequence for the rest of the contents pointed by register pair HL.
                     * The contents of bit 7 are placed in both carry and bit 0 of memory contents pointed by register pair HL.
                     * 
                     * Flags: Z N H C
                     *        * 0 0 *
                     *        
                     *        Z: Set if result is zero
                     *        N: Reset
                     *        H: Reset
                     *        C: Old bit 7 data
                     * 
                     * Clock Cycles:   16
                     * Machine Cycles:  4
                     * 
                     */
                    #region rlc [HL]
                    {
                        // rlc [HL]
                        m_extendedInstructionHandler[0x06] = () =>
                        {
                            int v = Read8(m_regs.HL);
                            m_regs.F.C = (v >> 7);
                            v = (v << 1) | (m_regs.F.C << 0);
                            m_regs.F.Z = IsZero(v);
                            m_regs.F.N = 0;
                            m_regs.F.H = 0;
                            Write8(m_regs.HL, v);

                            //TODO: increase accuracy
                            CyclesStep(16);
                        };
                    }
                    #endregion rlc [HL]




                    /*
                     * rl r
                     * ====
                     * 
                     *  ________________
                     *  |              |
                     * CY <- r << r <---
                     * 
                     * Desc: Rotates the contents of register r to the left.
                     * 
                     * Flags: Z N H C
                     *        * 0 0 *
                     *        
                     *        Z: Set if result is zero
                     *        N: Reset
                     *        H: Reset
                     *        C: Old bit 7 data
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region rl r
                    {
                        // rl A
                        m_extendedInstructionHandler[0x17] = () =>
                        {
                            int r = (0x07 & m_fetchedInstruction);

                            int bit7 = (m_regs[r] >> 7);
                            m_regs[r] = (m_regs[r] << 1) | (m_regs.F.C << 0);
                            m_regs.F.C = bit7;
                            m_regs.F.Z = IsZero(m_regs[r]);
                            m_regs.F.N = 0;
                            m_regs.F.H = 0;

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                        // rl B
                        m_extendedInstructionHandler[0x10] = m_extendedInstructionHandler[0x17];
                        // rl C
                        m_extendedInstructionHandler[0x11] = m_extendedInstructionHandler[0x17];
                        // rl D
                        m_extendedInstructionHandler[0x12] = m_extendedInstructionHandler[0x17];
                        // rl E
                        m_extendedInstructionHandler[0x13] = m_extendedInstructionHandler[0x17];
                        // rl H
                        m_extendedInstructionHandler[0x14] = m_extendedInstructionHandler[0x17];
                        // rl L
                        m_extendedInstructionHandler[0x15] = m_extendedInstructionHandler[0x17];
                    }
                    #endregion rl r




                    /*
                     * rl [HL]
                     * =======
                     * 
                     *  ______________________
                     *  |                    |
                     * CY <- [HL] << [HL] <---
                     * 
                     * Desc: Rotates the contents of memory pointed by register pair HL to the left.
                     * 
                     * Flags: Z N H C
                     *        * 0 0 *
                     *        
                     *        Z: Set if result is zero
                     *        N: Reset
                     *        H: Reset
                     *        C: Old bit 7 data
                     * 
                     * Clock Cycles:   16
                     * Machine Cycles:  4
                     * 
                     */
                    #region rl [HL]
                    {
                        // rl [HL]
                        m_extendedInstructionHandler[0x16] = () =>
                        {
                            int v = Read8(m_regs.HL);
                            int bit7 = (v >> 7);
                            v = (v << 1) | (m_regs.F.C << 0);
                            m_regs.F.C = bit7;
                            m_regs.F.Z = IsZero(v);
                            m_regs.F.N = 0;
                            m_regs.F.H = 0;
                            Write8(m_regs.HL, v);

                            //TODO: increase accuracy
                            CyclesStep(16);
                        };
                    }
                    #endregion rl [HL]




                    /*
                     * rrc r
                     * =====
                     * 
                     *     ____________
                     *     |          |
                     *     --> r >> r -> CY
                     * 
                     * Desc: Rotates the contents of register r to the right. That is, the contents of bit 7 are copied to bit 6
                     * and the previous content of bit 6 (the contents before the copy operation) are copied to bit 5.
                     * The same operation is repeated in sequence for the rest of the register.
                     * The contents of bit 0 are placed in both carry and bit 7 of register r.
                     * 
                     * Flags: Z N H C
                     *        * 0 0 *
                     *        
                     *        Z: Set if result is zero
                     *        N: Reset
                     *        H: Reset
                     *        C: Old bit 0 data
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region rrc r
                    {
                        // rrc A
                        m_extendedInstructionHandler[0x0F] = () =>
                        {
                            int r = (0x07 & m_fetchedInstruction);

                            m_regs.F.C = (m_regs[r] & 0x01);
                            m_regs[r] = (m_regs[r] >> 1) | (m_regs.F.C << 7);
                            m_regs.F.Z = IsZero(m_regs[r]);
                            m_regs.F.N = 0;
                            m_regs.F.H = 0;

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                        // rrc B
                        m_extendedInstructionHandler[0x08] = m_extendedInstructionHandler[0x0F];
                        // rrc C
                        m_extendedInstructionHandler[0x09] = m_extendedInstructionHandler[0x0F];
                        // rrc D
                        m_extendedInstructionHandler[0x0A] = m_extendedInstructionHandler[0x0F];
                        // rrc E
                        m_extendedInstructionHandler[0x0B] = m_extendedInstructionHandler[0x0F];
                        // rrc H
                        m_extendedInstructionHandler[0x0C] = m_extendedInstructionHandler[0x0F];
                        // rrc L
                        m_extendedInstructionHandler[0x0D] = m_extendedInstructionHandler[0x0F];
                    }
                    #endregion rrc r




                    /*
                     * rrc [HL]
                     * ========
                     * 
                     *     __________________
                     *     |                |
                     *     --> [HL] >> [HL] -> CY
                     * 
                     * Desc: Rotates the contents of memory pointed by register pair HL to the right. That is, the contents of bit 7 are copied to bit 6
                     * and the previous content of bit 6 (the contents before the copy operation) are copied to bit 5.
                     * The same operation is repeated in sequence for the rest of the memory contents pointed by register pair HL.
                     * The contents of bit 0 are placed in both carry and bit 7 of the contents of memory porinted by HL.
                     * 
                     * Flags: Z N H C
                     *        * 0 0 *
                     *        
                     *        Z: Set if result is zero
                     *        N: Reset
                     *        H: Reset
                     *        C: Old bit 0 data
                     * 
                     * Clock Cycles:   16
                     * Machine Cycles:  4
                     * 
                     */
                    #region rrc [HL]
                    {
                        // rrc [HL]
                        m_extendedInstructionHandler[0x0E] = () =>
                        {
                            int v = Read8(m_regs.HL);
                            m_regs.F.C = (v & 0x01);
                            v = (v >> 1) | (m_regs.F.C << 7);
                            m_regs.F.Z = IsZero(v);
                            m_regs.F.N = 0;
                            m_regs.F.H = 0;
                            Write8(m_regs.HL, v);

                            //TODO: increase accuracy
                            CyclesStep(16);
                        };
                    }
                    #endregion rrc [HL]




                    /*
                     * rr r
                     * ====
                     * 
                     *  ________________
                     *  |              |
                     *  ---> r >> r -> CY
                     * 
                     * Desc: Rotates the contents of register r to the right.
                     * 
                     * Flags: Z N H C
                     *        * 0 0 *
                     *        
                     *        Z: Set if result is zero
                     *        N: Reset
                     *        H: Reset
                     *        C: Old bit 0 data
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region rr r
                    {
                        // rr A
                        m_extendedInstructionHandler[0x1F] = () =>
                        {
                            int r = (0x07 & m_fetchedInstruction);

                            int bit0 = (m_regs[r] & 0x01);
                            m_regs[r] = (m_regs[r] >> 1) | (m_regs.F.C << 7);
                            m_regs.F.C = bit0;
                            m_regs.F.Z = IsZero(m_regs[r]);
                            m_regs.F.N = 0;
                            m_regs.F.H = 0;

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                        // rr B
                        m_extendedInstructionHandler[0x18] = m_extendedInstructionHandler[0x1F];
                        // rr C
                        m_extendedInstructionHandler[0x19] = m_extendedInstructionHandler[0x1F];
                        // rr D
                        m_extendedInstructionHandler[0x1A] = m_extendedInstructionHandler[0x1F];
                        // rr E
                        m_extendedInstructionHandler[0x1B] = m_extendedInstructionHandler[0x1F];
                        // rr H
                        m_extendedInstructionHandler[0x1C] = m_extendedInstructionHandler[0x1F];
                        // rr L
                        m_extendedInstructionHandler[0x1D] = m_extendedInstructionHandler[0x1F];
                    }
                    #endregion rr r




                    /*
                     * rr [HL]
                     * =======
                     * 
                     *  ______________________
                     *  |                    |
                     *  ---> [HL] >> [HL] -> CY
                     * 
                     * Desc: Rotates the contents of memory pointed by resgiter pair HL to the right.
                     * 
                     * Flags: Z N H C
                     *        * 0 0 *
                     *        
                     *        Z: Set if result is zero
                     *        N: Reset
                     *        H: Reset
                     *        C: Old bit 0 data
                     * 
                     * Clock Cycles:   16
                     * Machine Cycles:  4
                     * 
                     */
                    #region rr [HL]
                    {
                        // rr [HL]
                        m_extendedInstructionHandler[0x1E] = () =>
                        {
                            int v = Read8(m_regs.HL);
                            int bit0 = (v & 0x01);
                            v = (v >> 1) | (m_regs.F.C << 7);
                            m_regs.F.C = bit0;
                            m_regs.F.Z = IsZero(v);
                            m_regs.F.N = 0;
                            m_regs.F.H = 0;
                            Write8(m_regs.HL, v);

                            //TODO: increase accuracy
                            CyclesStep(16);
                        };
                    }
                    #endregion rr [HL]




                    /*
                     * sla r
                     * =====
                     * 
                     * CY <- r << r <- 0
                     * 
                     * Desc: Shift the contents of register r to the left. That is, the contents of bit 0 are copied to bit 1
                     * and the previous content of bit 1 (the contents before the copy operation) are copied to bit 2.
                     * The same operation is repeated in sequence for the rest of the register.
                     * The contents of bit 7 is copied to carry and bit 0 is reset.
                     * 
                     * Flags: Z N H C
                     *        * 0 0 *
                     *        
                     *        Z: Set if result is zero
                     *        N: Reset
                     *        H: Reset
                     *        C: Old bit 7 data
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region sla r
                    {
                        // sla A
                        m_extendedInstructionHandler[0x27] = () =>
                        {
                            int r = (0x07 & m_fetchedInstruction);

                            int bit7 = (m_regs[r] >> 7);
                            m_regs[r] = (m_regs[r] << 1);
                            m_regs.F.C = bit7;
                            m_regs.F.Z = IsZero(m_regs[r]);
                            m_regs.F.N = 0;
                            m_regs.F.H = 0;

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                        // sla B
                        m_extendedInstructionHandler[0x20] = m_extendedInstructionHandler[0x27];
                        // sla C
                        m_extendedInstructionHandler[0x21] = m_extendedInstructionHandler[0x27];
                        // sla D
                        m_extendedInstructionHandler[0x22] = m_extendedInstructionHandler[0x27];
                        // sla E
                        m_extendedInstructionHandler[0x23] = m_extendedInstructionHandler[0x27];
                        // sla H
                        m_extendedInstructionHandler[0x24] = m_extendedInstructionHandler[0x27];
                        // sla L
                        m_extendedInstructionHandler[0x25] = m_extendedInstructionHandler[0x27];
                    }
                    #endregion sla r




                    /*
                     * sla [HL]
                     * ========
                     * 
                     * CY <- [HL] << [HL] <- 0
                     * 
                     * Desc: Shift the contents of register r to the left. That is, the contents of bit 0 are copied to bit 1
                     * and the previous content of bit 1 (the contents before the copy operation) are copied to bit 2.
                     * The same operation is repeated in sequence for the rest of the register.
                     * The contents of bit 7 is copied to carry and bit 0 is reset.
                     * 
                     * Flags: Z N H C
                     *        * 0 0 *
                     *        
                     *        Z: Set if result is zero
                     *        N: Reset
                     *        H: Reset
                     *        C: Old bit 7 data
                     * 
                     * Clock Cycles:   16
                     * Machine Cycles:  4
                     * 
                     */
                    #region sla [HL]
                    {
                        // sla [HL]
                        m_extendedInstructionHandler[0x26] = () =>
                        {
                            int v = Read8(m_regs.HL);
                            int bit7 = (v >> 7);
                            v = (v << 1);
                            m_regs.F.C = bit7;
                            m_regs.F.Z = IsZero(v);
                            m_regs.F.N = 0;
                            m_regs.F.H = 0;
                            Write8(m_regs.HL, v);

                            //TODO: increase accuracy
                            CyclesStep(16);
                        };
                    }
                    #endregion sla [HL]




                    /*
                     * sra r
                     * =====
                     * 
                     *  ____
                     *  |   |
                     * bit7 r >> r -> CY
                     * 
                     * Desc: Shift the contents of register r to the right. That is, the contents of bit 7 are copied to bit 6
                     * and the previous content of bit 6 (the contents before the copy operation) are copied to bit 5.
                     * The same operation is repeated in sequence for the rest of the register.
                     * The contents of bit 0 is copied to carry and bit 7 is unchanged.
                     * 
                     * Flags: Z N H C
                     *        * 0 0 *
                     *        
                     *        Z: Set if result is zero
                     *        N: Reset
                     *        H: Reset
                     *        C: Old bit 0 data
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region sra r
                    {
                        // sra A
                        m_extendedInstructionHandler[0x2F] = () =>
                        {
                            int r = (0x07 & m_fetchedInstruction);

                            int bit7 = (m_regs[r] >> 7);
                            int bit0 = (m_regs[r] & 0x01);
                            m_regs[r] = (m_regs[r] >> 1) | (bit7 << 7);
                            m_regs.F.C = bit0;
                            m_regs.F.Z = IsZero(m_regs[r]);
                            m_regs.F.N = 0;
                            m_regs.F.H = 0;

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                        // sra B
                        m_extendedInstructionHandler[0x28] = m_extendedInstructionHandler[0x2F];
                        // sra C
                        m_extendedInstructionHandler[0x29] = m_extendedInstructionHandler[0x2F];
                        // sra D
                        m_extendedInstructionHandler[0x2A] = m_extendedInstructionHandler[0x2F];
                        // sra E
                        m_extendedInstructionHandler[0x2B] = m_extendedInstructionHandler[0x2F];
                        // sra H
                        m_extendedInstructionHandler[0x2C] = m_extendedInstructionHandler[0x2F];
                        // sra L
                        m_extendedInstructionHandler[0x2D] = m_extendedInstructionHandler[0x2F];
                    }
                    #endregion sra r




                    /*
                     * sra [HL]
                     * ========
                     * 
                     *  ______
                     *  |     |
                     * bit7 [HL] >> [HL] -> CY
                     * 
                     * Desc: Shift the contents of memory pointed by register pair HL to the right. That is, the contents of bit 7 are copied to bit 6
                     * and the previous content of bit 6 (the contents before the copy operation) are copied to bit 5.
                     * The same operation is repeated in sequence for the rest of the content of memory pointed by register HL.
                     * The contents of bit 0 is copied to carry and bit 7 is unchanged.
                     * 
                     * Flags: Z N H C
                     *        * 0 0 *
                     *        
                     *        Z: Set if result is zero
                     *        N: Reset
                     *        H: Reset
                     *        C: Old bit 0 data
                     * 
                     * Clock Cycles:   16
                     * Machine Cycles:  4
                     * 
                     */
                    #region sra [HL]
                    {
                        // sra [HL]
                        m_extendedInstructionHandler[0x2E] = () =>
                        {
                            int v = Read8(m_regs.HL);
                            int bit7 = (v >> 7);
                            int bit0 = (v & 0x01);
                            v = (v >> 1) | (bit7 << 7);
                            m_regs.F.C = bit0;
                            m_regs.F.Z = IsZero(v);
                            m_regs.F.N = 0;
                            m_regs.F.H = 0;
                            Write8(m_regs.HL, v);

                            //TODO: increase accuracy
                            CyclesStep(16);
                        };
                    }
                    #endregion sra [HL]




                    /*
                     * srl r
                     * =====
                     * 
                     * 0 -> r >> r -> CY
                     * 
                     * Desc: Shift the contents of register r to the right. That is, the contents of bit 7 are copied to bit 6
                     * and the previous content of bit 6 (the contents before the copy operation) are copied to bit 5.
                     * The same operation is repeated in sequence for the rest of the register.
                     * The contents of bit 0 is copied to carry and bit 7 is reset.
                     * 
                     * Flags: Z N H C
                     *        * 0 0 *
                     *        
                     *        Z: Set if result is zero
                     *        N: Reset
                     *        H: Reset
                     *        C: Old bit 0 data
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region srl r
                    {
                        // srl A
                        m_extendedInstructionHandler[0x3F] = () =>
                        {
                            int r = (0x07 & m_fetchedInstruction);

                            int bit0 = (m_regs[r] & 0x01);
                            m_regs[r] = ((m_regs[r] >> 1) & (0x7F));
                            m_regs.F.C = bit0;
                            m_regs.F.Z = IsZero(m_regs[r]);
                            m_regs.F.N = 0;
                            m_regs.F.H = 0;

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                        // srl B
                        m_extendedInstructionHandler[0x38] = m_extendedInstructionHandler[0x3F];
                        // srl C
                        m_extendedInstructionHandler[0x39] = m_extendedInstructionHandler[0x3F];
                        // srl D
                        m_extendedInstructionHandler[0x3A] = m_extendedInstructionHandler[0x3F];
                        // srl E
                        m_extendedInstructionHandler[0x3B] = m_extendedInstructionHandler[0x3F];
                        // srl H
                        m_extendedInstructionHandler[0x3C] = m_extendedInstructionHandler[0x3F];
                        // srl L
                        m_extendedInstructionHandler[0x3D] = m_extendedInstructionHandler[0x3F];
                    }
                    #endregion srl r




                    /*
                    * srl [HL]
                    * ========
                    * 
                    * 0 -> r >> r -> CY
                    * 
                    * Desc: Shift the contents of register r to the right. That is, the contents of bit 7 are copied to bit 6
                    * and the previous content of bit 6 (the contents before the copy operation) are copied to bit 5.
                    * The same operation is repeated in sequence for the rest of the register.
                    * The contents of bit 0 is copied to carry and bit 7 is reset.
                    * 
                    * Flags: Z N H C
                    *        * 0 0 *
                    *        
                    *        Z: Set if result is zero
                    *        N: Reset
                    *        H: Reset
                    *        C: Old bit 0 data
                    * 
                    * Clock Cycles:   16
                    * Machine Cycles:  4
                    * 
                    */
                    #region srl [HL]
                    {
                        // srl [HL]
                        m_extendedInstructionHandler[0x3E] = () =>
                        {
                            int v = Read8(m_regs.HL);
                            int bit0 = (v & 0x01);
                            v = ((v >> 1) & (0x7F));
                            m_regs.F.C = bit0;
                            m_regs.F.Z = IsZero(v);
                            m_regs.F.N = 0;
                            m_regs.F.H = 0;
                            Write8(m_regs.HL, v);

                            //TODO: increase accuracy
                            CyclesStep(16);
                        };
                    }
                    #endregion srl [HL]




                    /*
                     * swap r
                     * ======
                     * 
                     *    _______
                     *    |     |
                     *    |     |
                     *    v     |
                     *  7..4   3..0
                     *    |     ^
                     *    |     |
                     *    |_____|
                     * 
                     * 
                     * Desc: Shifts the contents of the lower-order 4 bits (0..3) of register r unmodified to the higher-order bis (4..7)
                     * of that register and shifts the contents of the higher-order 4 bits to the lower order 4 bits.
                     * 
                     * Flags: Z N H C
                     *        * 0 0 0
                     *        
                     *        Z: Set if result is zero
                     *        N: Reset
                     *        H: Reset
                     *        C: Reset
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region swap r
                    {
                        // swap A
                        m_extendedInstructionHandler[0x37] = () =>
                        {
                            int r = (0x07 & m_fetchedInstruction);

                            m_regs[r] = ((m_regs[r] >> 4) & 0xF) | ((m_regs[r] & 0xF) << 4);
                            m_regs.F.Z = IsZero(m_regs[r]);
                            m_regs.F.C = 0;
                            m_regs.F.H = 0;
                            m_regs.F.N = 0;

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                        // swap B
                        m_extendedInstructionHandler[0x30] = m_extendedInstructionHandler[0x37];
                        // swap C
                        m_extendedInstructionHandler[0x31] = m_extendedInstructionHandler[0x37];
                        // swap D
                        m_extendedInstructionHandler[0x32] = m_extendedInstructionHandler[0x37];
                        // swap E
                        m_extendedInstructionHandler[0x33] = m_extendedInstructionHandler[0x37];
                        // swap H
                        m_extendedInstructionHandler[0x34] = m_extendedInstructionHandler[0x37];
                        // swap L
                        m_extendedInstructionHandler[0x35] = m_extendedInstructionHandler[0x37];
                    }
                    #endregion swap r




                    /*
                     * swap [HL]
                     * =========
                     * 
                     *    _______
                     *    |     |
                     *    |     |
                     *    v     |
                     *  7..4   3..0
                     *    |     ^
                     *    |     |
                     *    |_____|
                     * 
                     * 
                     * Desc: Shifts the contents of the lower-order 4 bits (0..3) of memory contents pointed by register pair HL unmodified
                     * to the higher-order bis (4..7) of that content pointed by register pair HL and shifts the contents of the higher-order 4 bits
                     * to the lower order 4 bits.
                     * 
                     * Flags: Z N H C
                     *        * 0 0 0
                     *        
                     *        Z: Set if result is zero
                     *        N: Reset
                     *        H: Reset
                     *        C: Reset
                     * 
                     * Clock Cycles:   16
                     * Machine Cycles:  4
                     * 
                     */
                    #region swap [HL]
                    {
                        // swap [HL]
                        m_extendedInstructionHandler[0x36] = () =>
                        {
                            int v = Read8(m_regs.HL);
                            v = ((v >> 4) & 0xF) | ((v & 0xF) << 4);
                            m_regs.F.Z = IsZero(v);
                            m_regs.F.C = 0;
                            m_regs.F.H = 0;
                            m_regs.F.N = 0;
                            Write8(m_regs.HL, v);

                            //TODO: increase accuracy
                            CyclesStep(16);
                        };
                    }
                    #endregion swap [HL]


                    #endregion Rotate & Shift Instructions




                    #region Bit Operations


                    /*
                     * bit b, r
                     * ========
                     * 
                     * Z <- (bit_x == 0)
                     * 
                     * 
                     * Desc: Copies the complement of the contents of the specified bit in register r to the Z flag of the program status word (PSW)
                     * 
                     * Flags: Z N H C
                     *        * 0 1 -
                     *        
                     *        Z: Set if specified bit is zero
                     *        N: Reset
                     *        H: Set
                     *        C: Not affected
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region bit b, r
                    {
                        // bit 0, A
                        m_extendedInstructionHandler[0x47] = () =>
                        {
                            // Extract operands
                            int b = (0x07 & (m_fetchedInstruction >> 3));
                            int r = (0x07 & m_fetchedInstruction);

                            m_regs.F.Z = IsZero((m_regs[r] & (1 << b)));
                            m_regs.F.N = 0;
                            m_regs.F.H = 1;

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                        // bit 1, A
                        m_extendedInstructionHandler[0x4F] = m_extendedInstructionHandler[0x47];
                        // bit 2, A
                        m_extendedInstructionHandler[0x57] = m_extendedInstructionHandler[0x47];
                        // bit 3, A
                        m_extendedInstructionHandler[0x5F] = m_extendedInstructionHandler[0x47];
                        // bit 4, A
                        m_extendedInstructionHandler[0x67] = m_extendedInstructionHandler[0x47];
                        // bit 5, A
                        m_extendedInstructionHandler[0x6F] = m_extendedInstructionHandler[0x47];
                        // bit 6, A
                        m_extendedInstructionHandler[0x77] = m_extendedInstructionHandler[0x47];
                        // bit 7, A
                        m_extendedInstructionHandler[0x7F] = m_extendedInstructionHandler[0x47];
                        // bit 0, B
                        m_extendedInstructionHandler[0x40] = m_extendedInstructionHandler[0x47];
                        // bit 1, B
                        m_extendedInstructionHandler[0x48] = m_extendedInstructionHandler[0x47];
                        // bit 2, B
                        m_extendedInstructionHandler[0x50] = m_extendedInstructionHandler[0x47];
                        // bit 3, B
                        m_extendedInstructionHandler[0x58] = m_extendedInstructionHandler[0x47];
                        // bit 4, B
                        m_extendedInstructionHandler[0x60] = m_extendedInstructionHandler[0x47];
                        // bit 5, B
                        m_extendedInstructionHandler[0x68] = m_extendedInstructionHandler[0x47];
                        // bit 6, B
                        m_extendedInstructionHandler[0x70] = m_extendedInstructionHandler[0x47];
                        // bit 7, B
                        m_extendedInstructionHandler[0x78] = m_extendedInstructionHandler[0x47];
                        // bit 0, C
                        m_extendedInstructionHandler[0x41] = m_extendedInstructionHandler[0x47];
                        // bit 1, C
                        m_extendedInstructionHandler[0x49] = m_extendedInstructionHandler[0x47];
                        // bit 2, C
                        m_extendedInstructionHandler[0x51] = m_extendedInstructionHandler[0x47];
                        // bit 3, C
                        m_extendedInstructionHandler[0x59] = m_extendedInstructionHandler[0x47];
                        // bit 4, C
                        m_extendedInstructionHandler[0x61] = m_extendedInstructionHandler[0x47];
                        // bit 5, C
                        m_extendedInstructionHandler[0x69] = m_extendedInstructionHandler[0x47];
                        // bit 6, C
                        m_extendedInstructionHandler[0x71] = m_extendedInstructionHandler[0x47];
                        // bit 7, C
                        m_extendedInstructionHandler[0x79] = m_extendedInstructionHandler[0x47];
                        // bit 0, D
                        m_extendedInstructionHandler[0x42] = m_extendedInstructionHandler[0x47];
                        // bit 1, D
                        m_extendedInstructionHandler[0x4A] = m_extendedInstructionHandler[0x47];
                        // bit 2, D
                        m_extendedInstructionHandler[0x52] = m_extendedInstructionHandler[0x47];
                        // bit 3, D
                        m_extendedInstructionHandler[0x5A] = m_extendedInstructionHandler[0x47];
                        // bit 4, D
                        m_extendedInstructionHandler[0x62] = m_extendedInstructionHandler[0x47];
                        // bit 5, D
                        m_extendedInstructionHandler[0x6A] = m_extendedInstructionHandler[0x47];
                        // bit 6, D
                        m_extendedInstructionHandler[0x72] = m_extendedInstructionHandler[0x47];
                        // bit 7, D
                        m_extendedInstructionHandler[0x7A] = m_extendedInstructionHandler[0x47];
                        // bit 0, E
                        m_extendedInstructionHandler[0x43] = m_extendedInstructionHandler[0x47];
                        // bit 1, E
                        m_extendedInstructionHandler[0x4B] = m_extendedInstructionHandler[0x47];
                        // bit 2, E
                        m_extendedInstructionHandler[0x53] = m_extendedInstructionHandler[0x47];
                        // bit 3, E
                        m_extendedInstructionHandler[0x5B] = m_extendedInstructionHandler[0x47];
                        // bit 4, E
                        m_extendedInstructionHandler[0x63] = m_extendedInstructionHandler[0x47];
                        // bit 5, E
                        m_extendedInstructionHandler[0x6B] = m_extendedInstructionHandler[0x47];
                        // bit 6, E
                        m_extendedInstructionHandler[0x73] = m_extendedInstructionHandler[0x47];
                        // bit 7, E
                        m_extendedInstructionHandler[0x7B] = m_extendedInstructionHandler[0x47];
                        // bit 0, H
                        m_extendedInstructionHandler[0x44] = m_extendedInstructionHandler[0x47];
                        // bit 1, H
                        m_extendedInstructionHandler[0x4C] = m_extendedInstructionHandler[0x47];
                        // bit 2, H
                        m_extendedInstructionHandler[0x54] = m_extendedInstructionHandler[0x47];
                        // bit 3, H
                        m_extendedInstructionHandler[0x5C] = m_extendedInstructionHandler[0x47];
                        // bit 4, H
                        m_extendedInstructionHandler[0x64] = m_extendedInstructionHandler[0x47];
                        // bit 5, H
                        m_extendedInstructionHandler[0x6C] = m_extendedInstructionHandler[0x47];
                        // bit 6, H
                        m_extendedInstructionHandler[0x74] = m_extendedInstructionHandler[0x47];
                        // bit 7, H
                        m_extendedInstructionHandler[0x7C] = m_extendedInstructionHandler[0x47];
                        // bit 0, L
                        m_extendedInstructionHandler[0x45] = m_extendedInstructionHandler[0x47];
                        // bit 1, L
                        m_extendedInstructionHandler[0x4D] = m_extendedInstructionHandler[0x47];
                        // bit 2, L
                        m_extendedInstructionHandler[0x55] = m_extendedInstructionHandler[0x47];
                        // bit 3, L
                        m_extendedInstructionHandler[0x5D] = m_extendedInstructionHandler[0x47];
                        // bit 4, L
                        m_extendedInstructionHandler[0x65] = m_extendedInstructionHandler[0x47];
                        // bit 5, L
                        m_extendedInstructionHandler[0x6D] = m_extendedInstructionHandler[0x47];
                        // bit 6, L
                        m_extendedInstructionHandler[0x75] = m_extendedInstructionHandler[0x47];
                        // bit 7, L
                        m_extendedInstructionHandler[0x7D] = m_extendedInstructionHandler[0x47];
                    }
                    #endregion bit b, r




                    /*
                     * bit b, [HL]
                     * ===========
                     * 
                     * Z <- (bit_x == 0)
                     * 
                     * 
                     * Desc: Copies the complement of the contents of the memory contents pointed by register pair HL to the Z flag of the program status word (PSW)
                     * 
                     * Flags: Z N H C
                     *        * 0 1 -
                     *        
                     *        Z: Set if specified bit is zero
                     *        N: Reset
                     *        H: Set
                     *        C: Not affected
                     * 
                     * Clock Cycles:   12
                     * Machine Cycles:  3
                     * 
                     */
                    #region bit b, [HL]
                    {
                        // bit 0, [HL]
                        m_extendedInstructionHandler[0x46] = () =>
                        {
                            // Extract operand
                            int b = (0x07 & (m_fetchedInstruction >> 3));

                            int v = Read8(m_regs.HL);

                            m_regs.F.Z = IsZero((v & (1 << b)));
                            m_regs.F.N = 0;
                            m_regs.F.H = 1;

                            //TODO: increase accuracy
                            CyclesStep(12);
                        };
                        // bit 1, [HL]
                        m_extendedInstructionHandler[0x4E] = m_extendedInstructionHandler[0x46];
                        // bit 2, [HL]
                        m_extendedInstructionHandler[0x56] = m_extendedInstructionHandler[0x46];
                        // bit 3, [HL]
                        m_extendedInstructionHandler[0x5E] = m_extendedInstructionHandler[0x46];
                        // bit 4, [HL]
                        m_extendedInstructionHandler[0x66] = m_extendedInstructionHandler[0x46];
                        // bit 5, [HL]
                        m_extendedInstructionHandler[0x6E] = m_extendedInstructionHandler[0x46];
                        // bit 6, [HL]
                        m_extendedInstructionHandler[0x76] = m_extendedInstructionHandler[0x46];
                        // bit 7, [HL]
                        m_extendedInstructionHandler[0x7E] = m_extendedInstructionHandler[0x46];
                    }
                    #endregion bit b, [HL]




                    /*
                     * set b, r
                     * ========
                     * 
                     * bit_x <- 1
                     * 
                     * 
                     * Desc: Sets to 1 the specified bit in the specified register r
                     * 
                     * Flags: Z N H C
                     *        - - - -
                     * 
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region set b, r
                    {
                        // set 0, A
                        m_extendedInstructionHandler[0xC7] = () =>
                        {
                            // Extract operands
                            int b = (0x07 & (m_fetchedInstruction >> 3));
                            int r = (0x07 & m_fetchedInstruction);

                            m_regs[r] = (m_regs[r] | (1 << b));

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                        // set 1, A
                        m_extendedInstructionHandler[0xCF] = m_extendedInstructionHandler[0xC7];
                        // set 2, A
                        m_extendedInstructionHandler[0xD7] = m_extendedInstructionHandler[0xC7];
                        // set 3, A
                        m_extendedInstructionHandler[0xDF] = m_extendedInstructionHandler[0xC7];
                        // set 4, A
                        m_extendedInstructionHandler[0xE7] = m_extendedInstructionHandler[0xC7];
                        // set 5, A
                        m_extendedInstructionHandler[0xEF] = m_extendedInstructionHandler[0xC7];
                        // set 6, A
                        m_extendedInstructionHandler[0xF7] = m_extendedInstructionHandler[0xC7];
                        // set 7, A
                        m_extendedInstructionHandler[0xFF] = m_extendedInstructionHandler[0xC7];
                        // set 0, B
                        m_extendedInstructionHandler[0xC0] = m_extendedInstructionHandler[0xC7];
                        // set 1, B
                        m_extendedInstructionHandler[0xC8] = m_extendedInstructionHandler[0xC7];
                        // set 2, B
                        m_extendedInstructionHandler[0xD0] = m_extendedInstructionHandler[0xC7];
                        // set 3, B
                        m_extendedInstructionHandler[0xD8] = m_extendedInstructionHandler[0xC7];
                        // set 4, B
                        m_extendedInstructionHandler[0xE0] = m_extendedInstructionHandler[0xC7];
                        // set 5, B
                        m_extendedInstructionHandler[0xE8] = m_extendedInstructionHandler[0xC7];
                        // set 6, B
                        m_extendedInstructionHandler[0xF0] = m_extendedInstructionHandler[0xC7];
                        // set 7, B
                        m_extendedInstructionHandler[0xF8] = m_extendedInstructionHandler[0xC7];
                        // set 0, C
                        m_extendedInstructionHandler[0xC1] = m_extendedInstructionHandler[0xC7];
                        // set 1, C
                        m_extendedInstructionHandler[0xC9] = m_extendedInstructionHandler[0xC7];
                        // set 2, C
                        m_extendedInstructionHandler[0xD1] = m_extendedInstructionHandler[0xC7];
                        // set 3, C
                        m_extendedInstructionHandler[0xD9] = m_extendedInstructionHandler[0xC7];
                        // set 4, C
                        m_extendedInstructionHandler[0xE1] = m_extendedInstructionHandler[0xC7];
                        // set 5, C
                        m_extendedInstructionHandler[0xE9] = m_extendedInstructionHandler[0xC7];
                        // set 6, C
                        m_extendedInstructionHandler[0xF1] = m_extendedInstructionHandler[0xC7];
                        // set 7, C
                        m_extendedInstructionHandler[0xF9] = m_extendedInstructionHandler[0xC7];
                        // set 0, D
                        m_extendedInstructionHandler[0xC2] = m_extendedInstructionHandler[0xC7];
                        // set 1, D
                        m_extendedInstructionHandler[0xCA] = m_extendedInstructionHandler[0xC7];
                        // set 2, D
                        m_extendedInstructionHandler[0xD2] = m_extendedInstructionHandler[0xC7];
                        // set 3, D
                        m_extendedInstructionHandler[0xDA] = m_extendedInstructionHandler[0xC7];
                        // set 4, D
                        m_extendedInstructionHandler[0xE2] = m_extendedInstructionHandler[0xC7];
                        // set 5, D
                        m_extendedInstructionHandler[0xEA] = m_extendedInstructionHandler[0xC7];
                        // set 6, D
                        m_extendedInstructionHandler[0xF2] = m_extendedInstructionHandler[0xC7];
                        // set 7, D
                        m_extendedInstructionHandler[0xFA] = m_extendedInstructionHandler[0xC7];
                        // set 0, E
                        m_extendedInstructionHandler[0xC3] = m_extendedInstructionHandler[0xC7];
                        // set 1, E
                        m_extendedInstructionHandler[0xCB] = m_extendedInstructionHandler[0xC7];
                        // set 2, E
                        m_extendedInstructionHandler[0xD3] = m_extendedInstructionHandler[0xC7];
                        // set 3, E
                        m_extendedInstructionHandler[0xDB] = m_extendedInstructionHandler[0xC7];
                        // set 4, E
                        m_extendedInstructionHandler[0xE3] = m_extendedInstructionHandler[0xC7];
                        // set 5, E
                        m_extendedInstructionHandler[0xEB] = m_extendedInstructionHandler[0xC7];
                        // set 6, E
                        m_extendedInstructionHandler[0xF3] = m_extendedInstructionHandler[0xC7];
                        // set 7, E
                        m_extendedInstructionHandler[0xFB] = m_extendedInstructionHandler[0xC7];
                        // set 0, H
                        m_extendedInstructionHandler[0xC4] = m_extendedInstructionHandler[0xC7];
                        // set 1, H
                        m_extendedInstructionHandler[0xCC] = m_extendedInstructionHandler[0xC7];
                        // set 2, H
                        m_extendedInstructionHandler[0xD4] = m_extendedInstructionHandler[0xC7];
                        // set 3, H
                        m_extendedInstructionHandler[0xDC] = m_extendedInstructionHandler[0xC7];
                        // set 4, H
                        m_extendedInstructionHandler[0xE4] = m_extendedInstructionHandler[0xC7];
                        // set 5, H
                        m_extendedInstructionHandler[0xEC] = m_extendedInstructionHandler[0xC7];
                        // set 6, H
                        m_extendedInstructionHandler[0xF4] = m_extendedInstructionHandler[0xC7];
                        // set 7, H
                        m_extendedInstructionHandler[0xFC] = m_extendedInstructionHandler[0xC7];
                        // set 0, L
                        m_extendedInstructionHandler[0xC5] = m_extendedInstructionHandler[0xC7];
                        // set 1, L
                        m_extendedInstructionHandler[0xCD] = m_extendedInstructionHandler[0xC7];
                        // set 2, L
                        m_extendedInstructionHandler[0xD5] = m_extendedInstructionHandler[0xC7];
                        // set 3, L
                        m_extendedInstructionHandler[0xDD] = m_extendedInstructionHandler[0xC7];
                        // set 4, L
                        m_extendedInstructionHandler[0xE5] = m_extendedInstructionHandler[0xC7];
                        // set 5, L
                        m_extendedInstructionHandler[0xED] = m_extendedInstructionHandler[0xC7];
                        // set 6, L
                        m_extendedInstructionHandler[0xF5] = m_extendedInstructionHandler[0xC7];
                        // set 7, L
                        m_extendedInstructionHandler[0xFD] = m_extendedInstructionHandler[0xC7];
                    }
                    #endregion set b, r




                    /*
                     * set b, [HL]
                     * ===========
                     * 
                     * bit_x <- 1
                     * 
                     * 
                     * Desc: Sets to 1 the specified bit in the memory contents specified by register pair HL
                     * 
                     * Flags: Z N H C
                     *        - - - -
                     * 
                     * 
                     * Clock Cycles:   16
                     * Machine Cycles:  4
                     * 
                     */
                    #region set b, [HL]
                    {
                        // set 0, [HL]
                        m_extendedInstructionHandler[0xC6] = () =>
                        {
                            // Extract operand
                            int b = (0x07 & (m_fetchedInstruction >> 3));

                            int v = Read8(m_regs.HL);
                            v = (v | (1 << b));
                            Write8(m_regs.HL, v);

                            //TODO: increase accuracy
                            CyclesStep(16);
                        };
                        // set 1, [HL]
                        m_extendedInstructionHandler[0xCE] = m_extendedInstructionHandler[0xC6];
                        // set 2, [HL]
                        m_extendedInstructionHandler[0xD6] = m_extendedInstructionHandler[0xC6];
                        // set 3, [HL]
                        m_extendedInstructionHandler[0xDE] = m_extendedInstructionHandler[0xC6];
                        // set 4, [HL]
                        m_extendedInstructionHandler[0xE6] = m_extendedInstructionHandler[0xC6];
                        // set 5, [HL]
                        m_extendedInstructionHandler[0xEE] = m_extendedInstructionHandler[0xC6];
                        // set 6, [HL]
                        m_extendedInstructionHandler[0xF6] = m_extendedInstructionHandler[0xC6];
                        // set 7, [HL]
                        m_extendedInstructionHandler[0xFE] = m_extendedInstructionHandler[0xC6];
                    }
                    #endregion set b, [HL]




                    /*
                     * res b, r
                     * ========
                     * 
                     * bit_x <- 0
                     * 
                     * 
                     * Desc: Resets to 0 the specified bit in the specified register r
                     * 
                     * Flags: Z N H C
                     *        - - - -
                     * 
                     * 
                     * Clock Cycles:   8
                     * Machine Cycles: 2
                     * 
                     */
                    #region res b, r
                    {
                        // res 0, A
                        m_extendedInstructionHandler[0x87] = () =>
                        {
                            // Extract operands
                            int b = (0x07 & (m_fetchedInstruction >> 3));
                            int r = (0x07 & m_fetchedInstruction);

                            m_regs[r] = (m_regs[r] & (~(1 << b)));

                            //TODO: increase accuracy
                            CyclesStep(8);
                        };
                        // res 1, A
                        m_extendedInstructionHandler[0x8F] = m_extendedInstructionHandler[0x87];
                        // res 2, A
                        m_extendedInstructionHandler[0x97] = m_extendedInstructionHandler[0x87];
                        // res 3, A
                        m_extendedInstructionHandler[0x9F] = m_extendedInstructionHandler[0x87];
                        // res 4, A
                        m_extendedInstructionHandler[0xA7] = m_extendedInstructionHandler[0x87];
                        // res 5, A
                        m_extendedInstructionHandler[0xAF] = m_extendedInstructionHandler[0x87];
                        // res 6, A
                        m_extendedInstructionHandler[0xB7] = m_extendedInstructionHandler[0x87];
                        // res 7, A
                        m_extendedInstructionHandler[0xBF] = m_extendedInstructionHandler[0x87];
                        // res 0, B
                        m_extendedInstructionHandler[0x80] = m_extendedInstructionHandler[0x87];
                        // res 1, B
                        m_extendedInstructionHandler[0x88] = m_extendedInstructionHandler[0x87];
                        // res 2, B
                        m_extendedInstructionHandler[0x90] = m_extendedInstructionHandler[0x87];
                        // res 3, B
                        m_extendedInstructionHandler[0x98] = m_extendedInstructionHandler[0x87];
                        // res 4, B
                        m_extendedInstructionHandler[0xA0] = m_extendedInstructionHandler[0x87];
                        // res 5, B
                        m_extendedInstructionHandler[0xA8] = m_extendedInstructionHandler[0x87];
                        // res 6, B
                        m_extendedInstructionHandler[0xB0] = m_extendedInstructionHandler[0x87];
                        // res 7, B
                        m_extendedInstructionHandler[0xB8] = m_extendedInstructionHandler[0x87];
                        // res 0, C
                        m_extendedInstructionHandler[0x81] = m_extendedInstructionHandler[0x87];
                        // res 1, C
                        m_extendedInstructionHandler[0x89] = m_extendedInstructionHandler[0x87];
                        // res 2, C
                        m_extendedInstructionHandler[0x91] = m_extendedInstructionHandler[0x87];
                        // res 3, C
                        m_extendedInstructionHandler[0x99] = m_extendedInstructionHandler[0x87];
                        // res 4, C
                        m_extendedInstructionHandler[0xA1] = m_extendedInstructionHandler[0x87];
                        // res 5, C
                        m_extendedInstructionHandler[0xA9] = m_extendedInstructionHandler[0x87];
                        // res 6, C
                        m_extendedInstructionHandler[0xB1] = m_extendedInstructionHandler[0x87];
                        // res 7, C
                        m_extendedInstructionHandler[0xB9] = m_extendedInstructionHandler[0x87];
                        // res 0, D
                        m_extendedInstructionHandler[0x82] = m_extendedInstructionHandler[0x87];
                        // res 1, D
                        m_extendedInstructionHandler[0x8A] = m_extendedInstructionHandler[0x87];
                        // res 2, D
                        m_extendedInstructionHandler[0x92] = m_extendedInstructionHandler[0x87];
                        // res 3, D
                        m_extendedInstructionHandler[0x9A] = m_extendedInstructionHandler[0x87];
                        // res 4, D
                        m_extendedInstructionHandler[0xA2] = m_extendedInstructionHandler[0x87];
                        // res 5, D
                        m_extendedInstructionHandler[0xAA] = m_extendedInstructionHandler[0x87];
                        // res 6, D
                        m_extendedInstructionHandler[0xB2] = m_extendedInstructionHandler[0x87];
                        // res 7, D
                        m_extendedInstructionHandler[0xBA] = m_extendedInstructionHandler[0x87];
                        // res 0, E
                        m_extendedInstructionHandler[0x83] = m_extendedInstructionHandler[0x87];
                        // res 1, E
                        m_extendedInstructionHandler[0x8B] = m_extendedInstructionHandler[0x87];
                        // res 2, E
                        m_extendedInstructionHandler[0x93] = m_extendedInstructionHandler[0x87];
                        // res 3, E
                        m_extendedInstructionHandler[0x9B] = m_extendedInstructionHandler[0x87];
                        // res 4, E
                        m_extendedInstructionHandler[0xA3] = m_extendedInstructionHandler[0x87];
                        // res 5, E
                        m_extendedInstructionHandler[0xAB] = m_extendedInstructionHandler[0x87];
                        // res 6, E
                        m_extendedInstructionHandler[0xB3] = m_extendedInstructionHandler[0x87];
                        // res 7, E
                        m_extendedInstructionHandler[0xBB] = m_extendedInstructionHandler[0x87];
                        // res 0, H
                        m_extendedInstructionHandler[0x84] = m_extendedInstructionHandler[0x87];
                        // res 1, H
                        m_extendedInstructionHandler[0x8C] = m_extendedInstructionHandler[0x87];
                        // res 2, H
                        m_extendedInstructionHandler[0x94] = m_extendedInstructionHandler[0x87];
                        // res 3, H
                        m_extendedInstructionHandler[0x9C] = m_extendedInstructionHandler[0x87];
                        // res 4, H
                        m_extendedInstructionHandler[0xA4] = m_extendedInstructionHandler[0x87];
                        // res 5, H
                        m_extendedInstructionHandler[0xAC] = m_extendedInstructionHandler[0x87];
                        // res 6, H
                        m_extendedInstructionHandler[0xB4] = m_extendedInstructionHandler[0x87];
                        // res 7, H
                        m_extendedInstructionHandler[0xBC] = m_extendedInstructionHandler[0x87];
                        // res 0, L
                        m_extendedInstructionHandler[0x85] = m_extendedInstructionHandler[0x87];
                        // res 1, L
                        m_extendedInstructionHandler[0x8D] = m_extendedInstructionHandler[0x87];
                        // res 2, L
                        m_extendedInstructionHandler[0x95] = m_extendedInstructionHandler[0x87];
                        // res 3, L
                        m_extendedInstructionHandler[0x9D] = m_extendedInstructionHandler[0x87];
                        // res 4, L
                        m_extendedInstructionHandler[0xA5] = m_extendedInstructionHandler[0x87];
                        // res 5, L
                        m_extendedInstructionHandler[0xAD] = m_extendedInstructionHandler[0x87];
                        // res 6, L
                        m_extendedInstructionHandler[0xB5] = m_extendedInstructionHandler[0x87];
                        // res 7, L
                        m_extendedInstructionHandler[0xBD] = m_extendedInstructionHandler[0x87];
                    }
                    #endregion res b, r




                    /*
                     * res b, [HL]
                     * ===========
                     * 
                     * bit_x <- 0
                     * 
                     * 
                     * Desc: Resets to 0 the specified bit in the memory contents specified by register pair HL
                     * 
                     * Flags: Z N H C
                     *        - - - -
                     * 
                     * 
                     * Clock Cycles:   16
                     * Machine Cycles:  4
                     * 
                     */
                    #region res b, [HL]
                    {
                        // res 0, [HL]
                        m_extendedInstructionHandler[0x86] = () =>
                        {
                            // Extract operand
                            int b = (0x07 & (m_fetchedInstruction >> 3));

                            int v = Read8(m_regs.HL);
                            v = (v & (~(1 << b)));
                            Write8(m_regs.HL, v);

                            //TODO: increase accuracy
                            CyclesStep(16);
                        };
                        // res 1, [HL]
                        m_extendedInstructionHandler[0x8E] = m_extendedInstructionHandler[0x86];
                        // res 2, [HL]
                        m_extendedInstructionHandler[0x96] = m_extendedInstructionHandler[0x86];
                        // res 3, [HL]
                        m_extendedInstructionHandler[0x9E] = m_extendedInstructionHandler[0x86];
                        // res 4, [HL]
                        m_extendedInstructionHandler[0xA6] = m_extendedInstructionHandler[0x86];
                        // res 5, [HL]
                        m_extendedInstructionHandler[0xAE] = m_extendedInstructionHandler[0x86];
                        // res 6, [HL]
                        m_extendedInstructionHandler[0xB6] = m_extendedInstructionHandler[0x86];
                        // res 7, [HL]
                        m_extendedInstructionHandler[0xBE] = m_extendedInstructionHandler[0x86];
                    }
                    #endregion res b, [HL]


                    #endregion Bit Operations
                }


            }


        }
        // namespace Sharp
    }
    // namespace Processors
}
// namespace xFF


