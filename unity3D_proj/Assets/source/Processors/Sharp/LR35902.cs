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
                public delegate int BUSRead8Func(int aAddress);
                public delegate void BUSWrite8Func(int aAddress, int aValue);

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
                bool m_inHaltMode;
                bool m_inStopMode;
                bool m_interruptsMasterFlagEnabled;

                InstructionHandler[] m_instructionHandler;
                InstructionHandler[] m_extendedInstructionHandler;

                BUSRead8Func Read8;
                BUSWrite8Func Write8;
                CyclesStepFunc CyclesStep;


                /// <summary>
                /// Current Registers state
                /// </summary>
                public Registers Regs
                {
                    get { return m_regs; }
                }


                /// <summary>
                /// Current fetched instruction opcode that will be decoded and executed
                /// </summary>
                public int FetchedInstruction
                {
                    get { return m_fetchedInstruction; }
                }


                /// <summary>
                /// Returns true if the processor is currently in Halt operation mode
                /// </summary>
                public bool IsInHaltMode
                {
                    get { return m_inHaltMode; }
                }


                /// <summary>
                /// Returns true if the processor is currently in Stop operation mode
                /// </summary>
                public bool IsInStopMode
                {
                    get { return m_inStopMode; }
                }


                /// <summary>
                /// Returns the state of the flag Interrupts Master Enabled (IME)
                /// </summary>
                public bool IsInterruptsMasterFlagEnabled
                {
                    get { return m_interruptsMasterFlagEnabled; }
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
                    m_inHaltMode = false;
                    m_inStopMode = false;
                    m_interruptsMasterFlagEnabled = false;

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
                public void BindBUS(BUSRead8Func aRead, BUSWrite8Func aWrite)
                {
                    Read8 = aRead;
                    Write8 = aWrite;
                }


                /// <summary>
                /// Binds a delegate to handle cycles steps.
                /// </summary>
                /// <param name="aCyclesStep">A compatible delegate</param>
                public void BindCyclesStep(CyclesStepFunc aCyclesStep)
                {
                    CyclesStep = aCyclesStep;
                }


                /// <summary>
                /// Fetches next instruction opcode from current PC.
                /// </summary>
                public void Fetch( )
                {
                    m_fetchedInstruction = Read8(m_regs.PC++);
                }


                /// <summary>
                /// Decodes and Execute the fetched instruction.
                /// </summary>
                public void DecodeAndExecute( )
                {
                    m_instructionHandler[m_fetchedInstruction]();
                }



                #region Helpers
                /// <summary>
                /// Check for value zero
                /// </summary>
                /// <param name="aValue">The value</param>
                /// <returns>Returns 1 if the value is 0</returns>
                public static int IsZero(int aValue)
                {
                    return ((0xFF & aValue) == 0) ? 1 : 0;
                }


                /// <summary>
                /// Check for 16-bits carry
                /// </summary>
                /// <param name="newValue">The new value</param>
                /// <param name="oldValue">The old value</param>
                /// <returns>Returns 1 if there is a carry from bit 15</returns>
                public static int HasCarry16(int newValue, int oldValue)
                {
                    return (newValue & 0xFF0000) > (oldValue & 0xFF0000) ? 1 : 0;
                }


                /// <summary>
                /// Checks for 16-bits half-carry
                /// </summary>
                /// <param name="newValue">The new value</param>
                /// <param name="oldValue">The old value</param>
                /// <returns>Returns 1 if there is a carry from bit 11</returns>
                public static int HasHalfCarry16(int newValue, int oldValue)
                {
                    return (newValue & 0xF000) > (oldValue & 0xF000) ? 1 : 0;
                }


                /// <summary>
                /// Checks for 8-bits carry
                /// </summary>
                /// <param name="newValue">The new value</param>
                /// <param name="oldValue">The old value</param>
                /// <returns>Returns 1 if there is a carry from bit 7</returns>
                public static int HasCarry8(int newValue, int oldValue)
                {
                    return (newValue & 0xFF00) > (oldValue & 0xFF00) ? 1 : 0;
                }


                /// <summary>
                /// Checks for 8-bits half-carry
                /// </summary>
                /// <param name="newValue">The new value</param>
                /// <param name="oldValue">The old value</param>
                /// <returns>Returns 1 if there is a carry from bit 3</returns>
                public static int HasHalfCarry8(int newValue, int oldValue)
                {
                    return (newValue & 0xFFF0) > (oldValue & 0xF0) ? 1 : 0;
                }


                /// <summary>
                /// Checks for 8-bits half-borrow
                /// </summary>
                /// <param name="newValue">The new value</param>
                /// <param name="oldValue">The old value</param>
                /// <returns>Returns 1 if there is a borrow from bit 4</returns>
                public static int HasHalfBorrow8(int newValue, int oldValue)
                {
                    int p1 = newValue & 0xFFF0;
                    int p2 = oldValue & 0xFFF0;

                    return (newValue & 0xFFF0) < (oldValue & 0xFFF0) ? 1 : 0;
                }
                #endregion Helpers


                public override string ToString()
                {
                    return ("PC=" + m_regs.PC.ToString("X4") + " - Opcode: 0x" + Read8(m_regs.PC).ToString("X2") + string.Format("  AF={0}  BC={1}  DE={2}  HL={3}  SP={4}  [HL]={5}  [SP]={6}", m_regs.AF.ToString("X4"), m_regs.BC.ToString("X4"), m_regs.DE.ToString("X4"), m_regs.HL.ToString("X4"), m_regs.SP.ToString("X4"), Read8(m_regs.HL).ToString("X2"), Read8(m_regs.SP + 1).ToString("X2") + Read8(m_regs.SP).ToString("X2")) + string.Format("   Flags=[Z={0} N={1} H={2} C={3}]\n", m_regs.F.Z, m_regs.F.N, m_regs.F.H, m_regs.F.C));
                }
            }


        }
        // namespace Sharp
    }
    // namespace Processors
}
// namespace xFF


