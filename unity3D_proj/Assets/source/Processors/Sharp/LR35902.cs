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
                #region Delegates
                public delegate int AddressBUSRead8Func(int aAddress);
                public delegate void AddressBUSWrite8Func(int aAddress, int aValue);

                public delegate void CyclesStepFunc(int aElapsedCycles);

                public delegate void MsgHandler(object aMsg);

                delegate void InstructionHandler();
                #endregion Delegates


                #region Logger
                public static event MsgHandler Log;
                public static event MsgHandler LogWarning;
                public static event MsgHandler LogError;
                #endregion Logger


                Registers m_regs;
                int m_fetchedInstruction;

                bool m_trappedState;

                InstructionHandler[] m_instructionHandler;
                InstructionHandler[] m_extendedInstructionHandler;

                AddressBUSRead8Func Read8;
                AddressBUSWrite8Func Write8;
                CyclesStepFunc CyclesStep;


                /// <summary>
                /// Current Registers state
                /// </summary>
                public Registers Regs
                {
                    get { return m_regs; }
                }


                public LR35902( )
                {
                    m_regs = new Registers();
                    m_instructionHandler = new InstructionHandler[0x100]; // 256 instructions
                    m_extendedInstructionHandler = new InstructionHandler[0x100]; // 256 extended instructions

                    FillInstructionHandler();
                    FillExtendedInstructionHandler();

                    // Temp binding
                    Read8 = (int aAddress) => { return 0xFF; };
                    Write8 = (int aAddress, int aAvalue) => { };
                    CyclesStep = (int aElapsedCycles) => { };

                    Reset();
                }


                /// <summary>
                /// Resets the processor state
                /// </summary>
                public void Reset( )
                {
                    // Registers reset
                    m_regs.AF = 0;
                    m_regs.BC = 0;
                    m_regs.DE = 0;
                    m_regs.HL = 0;
                    m_regs.SP = 0;
                    m_regs.PC = 0;

                    m_fetchedInstruction = 0;

                    m_trappedState = false;

                    if (Log != null)
                    {
                        Log("Processor state was reset.");
                    }
                }


                /// <summary>
                /// Binds delegates to handle memory access.
                /// </summary>
                /// <param name="aRead">A compatible delegate</param>
                /// <param name="aWrite">A compatible delegate</param>
                public void BindAddressBUS(AddressBUSRead8Func aRead, AddressBUSWrite8Func aWrite)
                {
                    Read8 = aRead;
                    Write8 = aWrite;
                }


                /// <summary>
                /// Binds a delegate to handle clocks steps.
                /// </summary>
                /// <param name="aClockStep">A compatible delegate</param>
                public void BindClockStep(CyclesStepFunc aClockStep)
                {
                    CyclesStep = aClockStep;
                }


                void Decode( )
                {
                    m_fetchedInstruction = Read8(m_regs.PC++);
                }
            }


        }
        // namespace Sharp
    }
    // namespace Processors
}
// namespace xFF


