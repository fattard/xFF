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


