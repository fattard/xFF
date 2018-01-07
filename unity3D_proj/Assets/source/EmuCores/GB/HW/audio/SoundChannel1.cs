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

using xFF.EmuCores.GB;
using xFF.EmuCores.GB.Defs;
using xFF.EmuCores.GB.HW;


namespace xFF
{
    namespace EmuCores
    {
        namespace GB
        {
            namespace HW
            {
                namespace audio
                {


                    public class SoundChannel1
                    {
                        int m_dutyCycleIdx;
                        int m_envelopeSteps;
                        int m_defaultEnvelopeVolume;
                        int m_curVolume;
                        int m_envelopeCounter;
                        int m_envelopeMode;
                        int m_frequencyData;
                        int m_period;
                        

                        int m_lengthCounter;
                        bool m_lengthCounterEnabled;
                        bool m_channelStatusOn;

                        bool m_dacEnabled;

                        int m_sweepShift;
                        int m_sweepMode;
                        int m_sweepTime;
                        int m_sweepCounter;
                        int m_sweepShadowFreq;
                        bool m_sweepEnabled;

                        int m_waveSamplePos;

                        int[][] m_dutyWaveForm = new int[][]
                        {
                            new int[] { 0,0,0,0,0,0,0,1 }, // 12.5%
                            new int[] { 1,0,0,0,0,0,0,1 }, // 25%
                            new int[] { 1,0,0,0,0,1,1,1 }, // 50%
                            new int[] { 0,1,1,1,1,1,1,0 }, // 75%
                        };


                        public bool UserEnabled
                        {
                            get;
                            set;
                        }

                        /// <summary>
                        /// Flag indicated at NR52 (0xFF26) bit 0
                        /// </summary>
                        public bool IsSoundOn
                        {
                            get { return ChannelEnabled && m_channelStatusOn; }
                        }


                        public bool LeftOutputEnabled
                        {
                            get;
                            set;
                        }


                        public bool RightOutputEnabled
                        {
                            get;
                            set;
                        }


                        public bool ChannelEnabled
                        {
                            get { return m_dacEnabled; }
                            set
                            {
                                m_dacEnabled = value;
                                if (!m_dacEnabled)
                                {
                                    m_channelStatusOn = false;
                                }
                            }
                        }


                        /// <summary>
                        /// Accessor for Reg NR11 (0xFF11)
                        /// Sound length data t1, where
                        /// total length = 64 - t1
                        /// </summary>
                        public int SoundLengthData
                        {
                            get { return m_lengthCounter; }
                            set
                            {
                                m_lengthCounter = (64 - (0x3F & value));
                            }
                        }


                        public int DutyCycle
                        {
                            get { return m_dutyCycleIdx; }
                            set
                            {
                                m_dutyCycleIdx = (0x3 & value);
                            }
                        }


                        public void TriggerInit()
                        {
                            //if (ChannelEnabled)
                            {
                                if (m_lengthCounter == 0)
                                {
                                    m_lengthCounter = 64;
                                }
                                m_period = (2048 - m_frequencyData) * 4;
                                m_curVolume = m_defaultEnvelopeVolume;
                                m_envelopeCounter = m_envelopeSteps;
                                /*if (m_envelopeCounter == 0)
                                {
                                    m_envelopeCounter = 8;
                                }*/

                                m_channelStatusOn = ChannelEnabled;

                                m_sweepShadowFreq = m_frequencyData;
                                m_sweepCounter = m_sweepTime;
                                m_sweepEnabled = (m_sweepShift != 0) || (m_sweepCounter != 0);

                                if (m_sweepShift > 0)
                                {
                                    CalcSweepFreq();
                                }
                            }
                        }


                        public int EnvelopeSteps
                        {
                            get { return m_envelopeSteps; }
                            set
                            {
                                m_envelopeSteps = (0x07 & value);
                            }
                        }


                        public int DefaultEnvelope
                        {
                            get { return m_defaultEnvelopeVolume; }
                            set
                            {
                                m_defaultEnvelopeVolume = (0x0F & value);
                            }
                        }


                        public int EnvelopeMode
                        {
                            get { return m_envelopeMode; }
                            set { m_envelopeMode = (0x01 & value); }
                        }


                        /// <summary>
                        /// Accessor for combined Reg NR13 (0xFF13)
                        /// and NR14 (0xFF14) parts of the
                        /// Frequency data (11 bits)
                        /// 
                        /// </summary>
                        public int Frequency
                        {
                            get { return m_frequencyData; }
                            set
                            {
                                m_frequencyData = value;
                                m_period = (2048 - m_frequencyData) * 4; // needs to capture at 2 times the frequency we want to hear
                            }
                        }


                        public int SweepShift
                        {
                            get { return m_sweepShift; }
                            set
                            {
                                m_sweepShift = (0x07 & value);
                                if (m_sweepShift == 0)
                                {
                                    m_sweepEnabled = false;
                                }
                            }
                        }


                        public int SweepMode
                        {
                            get { return m_sweepMode; }
                            set { m_sweepMode = (0x01 & value); }
                        }


                        public int SweepTime
                        {
                            get { return m_sweepTime; }
                            set
                            {
                                m_sweepTime = (0x07 & value);
                                if (m_sweepTime == 0)
                                {
                                    m_sweepEnabled = false;
                                }
                            }
                        }


                        public bool LengthCounterEnabled
                        {
                            get { return m_lengthCounterEnabled; }
                            set
                            {
                                m_lengthCounterEnabled = value;
                            }
                        }


                        public void PeriodStep()
                        {
                            m_period -= 4;

                            if (m_period <= 0)
                            {
                                m_waveSamplePos = (m_waveSamplePos + 1) % 8;

                                m_period += (2048 - m_frequencyData) * 4;
                            }
                        }


                        public int GenerateSampleL()
                        {
                            if (!IsSoundOn || !ChannelEnabled || !LeftOutputEnabled || !UserEnabled)
                            {
                                return 0;
                            }

                            return (m_dutyWaveForm[m_dutyCycleIdx][m_waveSamplePos] * 15) & m_curVolume;
                        }


                        public int GenerateSampleR()
                        {
                            if (!IsSoundOn || !ChannelEnabled || !RightOutputEnabled || !UserEnabled)
                            {
                                return 0;
                            }

                            return (m_dutyWaveForm[m_dutyCycleIdx][m_waveSamplePos] * 15) & m_curVolume;
                        }


                        public void VolumeEnvelopeStep( )
                        {
                            m_envelopeCounter--;

                            if (m_envelopeCounter == 0)
                            {
                                m_envelopeCounter = m_envelopeSteps;
                                /*if (m_envelopeCounter == 0)
                                {
                                    m_envelopeCounter = 8;
                                }*/

                                if (m_envelopeMode > 0 && m_curVolume < 16)
                                {
                                    m_curVolume++;
                                }

                                else if (m_envelopeMode == 0 && m_curVolume > 0)
                                {
                                    m_curVolume--;
                                }
                            }
                        }


                        public void SweepStep( )
                        {
                            m_sweepCounter--;

                            if (m_sweepCounter == 0)
                            {
                                m_sweepCounter = m_sweepTime;

                                if (m_sweepEnabled && m_sweepCounter > 0)
                                {
                                    int newFreq = CalcSweepFreq();
                                    if (newFreq <= 2047 && m_sweepShift > 0)
                                    {
                                        m_sweepShadowFreq = newFreq;
                                        m_frequencyData = newFreq;
                                        CalcSweepFreq();
                                    }
                                    CalcSweepFreq();
                                }
                            }
                        }


                        public void LengthStep( )
                        {
                            if (m_lengthCounter > 0 && m_lengthCounterEnabled)
                            {
                                m_lengthCounter--;

                                if (m_lengthCounter == 0)
                                {
                                    // Disable channel
                                    //m_waveSamplePos = 0;
                                    m_channelStatusOn = false;
                                }
                            }
                        }


                        int CalcSweepFreq( )
                        {
                            int freq = m_sweepShadowFreq >> m_sweepShift;
                            if (m_sweepMode > 0)
                            {
                                freq = -freq;
                            }

                            freq = m_sweepShadowFreq + freq;

                            if (freq > 2047)
                            {
                                m_sweepEnabled = false;
                            }

                            return freq;
                        }
                    }
                    

                }
                // namespace audio
            }
            // namespace HW
        }
        // namespace GB
    }
    // namespace EmuCores
}
// namespace xFF
