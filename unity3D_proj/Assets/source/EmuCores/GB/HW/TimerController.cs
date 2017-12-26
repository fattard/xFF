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

using xFF.EmuCores.GB.Defs;

namespace xFF
{
    namespace EmuCores
    {
        namespace GB
        {
            namespace HW
            {


                public class TimerController
                {
                    CPU.RequestIRQFunc RequestIRQ;

                    int m_dividerCounter;
                    int m_targetCounter;
                    int m_internalCounter;

                    int m_timerCounter;
                    int m_timerCounter_reloadingCycles;

                    int m_controllerData;

                    public int Divider
                    {
                        get { return m_dividerCounter; }
                        set
                        {
                            m_dividerCounter = 0;
                            m_internalCounter = 0;
                        }
                    }


                    public int TimerCounter
                    {
                        get
                        {
                            if (m_timerCounter_reloadingCycles > 0)
                            {
                                return 0;
                            }

                            return m_timerCounter;
                        }
                        set
                        {
                            if (m_timerCounter_reloadingCycles > 0)
                            {
                                return;
                            }
                            m_timerCounter = (0xFF & value);
                        }
                    }


                    public int TimerModulo
                    {
                        get;
                        set;
                    }


                    public int InputClockSelector
                    {
                        get;
                        set;
                    }


                    public bool IsTimerEnabled
                    {
                        get;
                        set;
                    }


                    public TimerController( )
                    {
                        m_dividerCounter = 0;
                        m_targetCounter = 0;
                        SetControllerData(0);
                        SetTimerFreq();

                        // Temp binding
                        RequestIRQ = (aIRQ_flag) => { };

                        m_timerCounter_reloadingCycles = 8;
                    }


                    void SetTimerFreq( )
                    {
                        switch (InputClockSelector)
                        {
                            case 0:
                                m_targetCounter = 1024;
                                break;

                            case 1:
                                m_targetCounter = 16;
                                break;

                            case 2:
                                m_targetCounter = 64;
                                break;

                            case 3:
                                m_targetCounter = 256;
                                break;
                        }
                    }


                    public void SetControllerData(int aData)
                    {
                        m_controllerData = aData;

                        int newSelector = (RegsIO_Bits.TAC_CLK & aData);
                        if (newSelector != InputClockSelector)
                        {
                            InputClockSelector = newSelector;
                            SetTimerFreq();
                        }
                        IsTimerEnabled = ((RegsIO_Bits.TAC_EN & aData) > 0);
                    }


                    public int GetControllerData( )
                    {
                        return m_controllerData;
                    }
                    


                    public void BindRequestIRQ(CPU.RequestIRQFunc aRequestIRQFunc)
                    {
                        RequestIRQ = aRequestIRQFunc;
                    }


                    public void CyclesStep(int aElapsedCycles)
                    {
                        //m_internalCounter += aElapsedCycles;
                        if (((m_internalCounter & 0xFF) + aElapsedCycles)  > 255)
                        {
                            m_dividerCounter = (0xFF & (m_dividerCounter + 1));
                        }


                        if (IsTimerEnabled)
                        {
                            //m_timerClockCounter += aElapsedCycles;

                            if ((m_internalCounter & (m_targetCounter - 1)) + aElapsedCycles >= m_targetCounter)
                            {
                                if (TimerCounter == 255)
                                {
                                    TimerCounter = TimerModulo;
                                    RequestIRQ(RegsIO_Bits.IF_TIMER);
                                    m_timerCounter_reloadingCycles = 8;
                                }
                                else
                                {
                                    TimerCounter = (0xFF & (TimerCounter + 1));
                                }

                                SetTimerFreq();
                            }
                        }

                        m_internalCounter += aElapsedCycles;

                        if (m_timerCounter_reloadingCycles > 0)
                        {
                            m_timerCounter_reloadingCycles -= aElapsedCycles;
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
