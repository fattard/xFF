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



                    #endregion Bit Operations
                }


            }


        }
        // namespace Sharp
    }
    // namespace Processors
}
// namespace xFF


