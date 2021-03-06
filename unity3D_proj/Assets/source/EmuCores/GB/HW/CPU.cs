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

using xFF.Processors.Sharp;
using xFF.EmuCores.GB.Defs;

namespace xFF
{
    namespace EmuCores
    {
        namespace GB
        {
            namespace HW
            {


                public class CPU
                {
                    public const uint kCPUClock = 4194304; // ~4.19 MHz
                    public const uint kCyclesPerFrame = kCPUClock / 60; // 69905 cycles per frame

                    public delegate void RequestIRQFunc(int aIRQ_flag);


                    LR35902 m_gb_core;
                    MEM m_mem;

                    uint m_cyclesElapsed;
                    uint m_userCyclesRate;

                    int m_interruptsRequests;
                    int m_interruptsEnables;

                    bool m_haltBugActivated;


                    public int InterruptsRequests
                    {
                        get { return (0xE0 | (0x1F & m_interruptsRequests)); }
                        set { m_interruptsRequests = (0x1F & value); }
                    }


                    public int InterruptsEnables
                    {
                        get { return m_interruptsEnables; }
                        set { m_interruptsEnables = (0xFF & value); }
                    }


                    public uint UserCyclesRate
                    {
                        get { return m_userCyclesRate; }
                        set { m_userCyclesRate = value; }
                    }


                    public LR35902 ProcessorState
                    {
                        get { return m_gb_core; }
                    }


                    public CPU( )
                    {
                        m_gb_core = new LR35902();

                        m_gb_core.BindCyclesStep(CyclesStep);

                        m_userCyclesRate = kCyclesPerFrame;
                        m_cyclesElapsed = 0;
                    }


                    public void BindMemBUS(MEM aMem)
                    {
                        m_mem = aMem;

                        m_gb_core.BindBUS(aMem.Read8, aMem.Write8);
                    }


                    public void Run( )
                    {
                        while (m_cyclesElapsed < m_userCyclesRate)
                        {
                            //UnityEngine.Debug.Log(m_gb_core.ToString());

                            // Check interrupts
                            if (m_gb_core.IsInterruptsMasterFlagEnabled || (m_gb_core.IsInHaltMode && !m_haltBugActivated))
                            {
                                CheckInterrupts();
                            }

                            if (m_gb_core.IsInHaltMode && !m_haltBugActivated)
                            {
                                m_gb_core.AdvanceCycles(4);
                                continue;
                            }
                            else if (m_haltBugActivated)
                            {
                                m_gb_core.CancelHalt();
                                m_haltBugActivated = false;

                                // Execute next instruction without incrementing PC
                                m_gb_core.ReFetch();
                                m_gb_core.DecodeAndExecute();
                            }

                            else
                            {
                                m_gb_core.Fetch();
                                m_gb_core.DecodeAndExecute();
                            }

                            // Check for halt bug
                            if (m_gb_core.IsInHaltMode && !m_gb_core.IsInterruptsMasterFlagEnabled)
                            {
                                int interruptRequests = m_mem.Read8(RegsIO.IF);
                                int interruptEnables = m_mem.Read8(RegsIO.IE);

                                // if interrupts are disabled and there is a pending interrupt,
                                // the next instruction executed does not increment PC as part of
                                // the instruction fetch
                                if ((interruptRequests & interruptEnables & 0x1F) > 0)
                                {
                                    m_haltBugActivated = true;
                                }
                            }
                        }


                        //UnityEngine.Debug.Log(m_cyclesElapsed + " - " + m_gb_core.ToString());

                        // Reset cycles counter 
                        m_cyclesElapsed -= m_userCyclesRate;
                    }


                    void CheckInterrupts( )
                    {
                        int interruptRequests = m_mem.Read8(RegsIO.IF);
                        int interruptEnables = m_mem.Read8(RegsIO.IE);

                        for (int irq = 0; irq < 5; ++irq)
                        {
                            if (((interruptRequests & (1 << irq)) > 0) && ((interruptEnables & (1 << irq)) > 0))
                            {
                                if (m_gb_core.IsInHaltMode)
                                {
                                    m_gb_core.CancelHalt();

                                    if (!m_gb_core.IsInterruptsMasterFlagEnabled)
                                    {
                                        break;
                                    }
                                }
                                // Disable request flag
                                m_mem.Write8(RegsIO.IF, interruptRequests & (~(1 << irq)));

                                m_gb_core.ServiceIRQ(irq);
                                break;
                            }
                        }
                    }


                    public void RequestIRQ(int aIRQ_flag)
                    {
                        int interruptRequests = (m_mem.Read8(RegsIO.IF) | aIRQ_flag);
                        m_mem.Write8(RegsIO.IF, (0x1F & interruptRequests));
                    }


                    void CyclesStep(int aElapsedCycles)
                    {
                        m_cyclesElapsed += (uint)aElapsedCycles;
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
