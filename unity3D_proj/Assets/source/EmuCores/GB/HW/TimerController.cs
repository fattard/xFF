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

using xFF.EmuCores.GB.Defs;

namespace xFF
{
    namespace EmuCores
    {
        namespace GB
        {
            namespace HW
            {


                /// <summary>
                /// Class to control Timer module and its internal Regs and counters.
                /// 
                /// There are several obscure behaviours that were documented by AntonioND,
                /// author of emulator GiiBiiAdvance, in the "The Cycle-Accurate Game Boy Docs",
                /// that can be found on "https://github.com/AntonioND/giibiiadvance/raw/master/docs/TCAGBD.pdf"
                /// 
                /// Please, read that document before trying to read the sourcecode.
                /// 
                /// </summary>
                public class TimerController
                {
                    CPU.RequestIRQFunc RequestIRQ;

                    int m_internalCounter;
                    int m_timerModulo;

                    int m_prevCheckBit;

                    bool m_shouldResetInternalCounter;

                    int m_targetCounterMask;

                    int m_timerCounter;
                    int m_timerCounter_reloadingCycles;

                    int m_controllerData;

                    int m_timerEnabledMask;


                    /// <summary>
                    /// Accessor for the Reg DIV (0xFF04)
                    /// Reading from it returns the high-byte
                    /// of the 16 bits internal counter.
                    /// Writing any value to it will reset
                    /// the internal counter, affecting all
                    /// the Timer operations.
                    /// </summary>
                    public int Divider
                    {
                        get
                        {
                            // The high-byte
                            return (0xFF & (m_internalCounter >> 8));

                            //TODO: check if the low-byte is exposed to address 0xFF03
                        }
                        set
                        {
                            // Mark a flag to reset counter in the next cycles-step
                            m_shouldResetInternalCounter = true;
                        }
                    }


                    /// <summary>
                    /// Accessor for the Reg TIMA (0xFF05)
                    /// This register holds the Timer Counter,
                    /// which will be incremented at chosen
                    /// frequency, until it overflows.
                    /// Then, the value of register TMA will
                    /// be loaded into TIMA and the Timer
                    /// IRQ will be requested, if enabled.
                    /// </summary>
                    public int TimerCounter
                    {
                        get
                        {
                            // Note: During reload cycles, this register should reads as 0x00

                            return m_timerCounter;
                        }
                        set
                        {
                            // During reload cycles, any write to this register is ignored
                            if (m_timerCounter_reloadingCycles > 0)
                            {
                                return;
                            }

                            m_timerCounter = (0xFF & value);
                        }
                    }


                    /// <summary>
                    /// Accessor for the Reg TMA (0xFF06)
                    /// This register holds the value that
                    /// is loaded into TIMA when it overflows.
                    /// </summary>
                    public int TimerModulo
                    {
                        get { return m_timerModulo; }
                        set
                        {
                            m_timerModulo = (0xFF & value);
                        }
                    }


                    /// <summary>
                    /// Accessor for the Reg TAC (0xFF07)
                    /// </summary>
                    public int TimerControllerData
                    {
                        get
                        {
                            return (0xF8 | m_controllerData);
                        }

                        set
                        {
                            m_controllerData = (0x07 & value);

                            int newSelector = (RegsIO_Bits.TAC_CLK & value);
                            if (newSelector != InputClockSelector)
                            {
                                InputClockSelector = newSelector;
                                SetTimerFreqMaskCheck();
                            }

                            IsTimerEnabled = ((RegsIO_Bits.TAC_EN & value) > 0);
                        }
                    }


                    public int InputClockSelector
                    {
                        get;
                        set;
                    }


                    public bool IsTimerEnabled
                    {
                        get { return m_timerEnabledMask != 0; }
                        set { m_timerEnabledMask = (value) ? 0xFFFF : 0; }
                    }


                    public TimerController()
                    {
                        m_targetCounterMask = 0;
                        TimerControllerData = 0;
                        SetTimerFreqMaskCheck();

                        // Temp binding
                        RequestIRQ = (aIRQ_flag) => { };
                        
                    }


                    void SetTimerFreqMaskCheck()
                    {
                        switch (InputClockSelector)
                        {
                            case 0: // 4096 Hz (1024 clocks)
                                m_targetCounterMask = (1 << 9); // Checks for bit 9 overflowing to bit 10
                                break;

                            case 1: // 262144 Hz (16 clocks)
                                m_targetCounterMask = (1 << 3); // Checks for bit 3 overflowing to bit 4
                                break;

                            case 2: // 65536 Hz (64 clocks)
                                m_targetCounterMask = (1 << 5); // Checks for bit 5 overflowing to bit 6
                                break;

                            case 3: // 16386 Hz (256 clocks)
                                m_targetCounterMask = (1 << 7); // Checks for bit 7 overflowing to bit 8
                                break;
                        }
                    }


                    public void BindRequestIRQ(CPU.RequestIRQFunc aRequestIRQFunc)
                    {
                        RequestIRQ = aRequestIRQFunc;
                    }


                    public void CyclesStep(int aElapsedCycles)
                    {
                        // Sync cycles
                        while (aElapsedCycles > 0)
                        {
                            // Consume reloading cycles
                            if (m_timerCounter_reloadingCycles > 0)
                            {
                                m_timerCounter_reloadingCycles -= 4;

                                // If reloading finished, feed from TMA and do IRQ request
                                if (m_timerCounter_reloadingCycles <= 0)
                                {
                                    m_timerCounter = m_timerModulo;
                                    RequestIRQ(RegsIO_Bits.IF_TIMER);
                                    SetTimerFreqMaskCheck();
                                }
                            }


                            // Reset internal counter
                            if (m_shouldResetInternalCounter)
                            {
                                m_internalCounter = 0;
                                m_shouldResetInternalCounter = false;
                            }

                            // Increase internal counter (16 bits)
                            m_internalCounter = (0xFFFF & (m_internalCounter + 4));

                            int curBit = (m_internalCounter & m_targetCounterMask);

                            // Performs AND operation between freq bit and TAC Enable flag
                            curBit &= m_timerEnabledMask;

                            // Note1: even if the internal counter was reset but prevBit was set, falling edge should trigger!
                            // Note2: even if Timer was disabled but prevBit was set, falling edge should trigger!

                            // Falling edge detector - checks if bit from freq check mask changes from 1 to 0 (had overflow)
                            if ((m_prevCheckBit != 0) && (curBit == 0))
                            {
                                // Increase TimerCounter
                                m_timerCounter = (0xFF & (m_timerCounter + 1));


                                // Checks if overflows and init reloading operation
                                if (m_timerCounter == 0)
                                {
                                    m_timerCounter_reloadingCycles = 4;
                                }
                            }

                            // Store curBit for next cycle check
                            m_prevCheckBit = curBit;

                            // Consume elapsed cycles
                            aElapsedCycles -= 4;
                        }
                    }

                    
                }


            }
            // namespace HW
        }
        // namespace GB
    }
    // namespace EmuCores
}
// namespace xFF
