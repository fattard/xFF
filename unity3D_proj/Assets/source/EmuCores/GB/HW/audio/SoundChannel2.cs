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


                    public class SoundChannel2
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
                        /// Flag indicated at NR52 (0xFF26) bit 1
                        /// </summary>
                        public bool IsSoundOn
                        {
                            get { return m_dacEnabled && m_channelStatusOn; }
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

                                // Disabling DAC should disable channel immediately
                                // Enabling DAC shouldn't re-enable channel
                                m_channelStatusOn &= value;
                            }
                        }


                        /// <summary>
                        /// Accessor for Reg NR21 (0xFF16)
                        /// Sound length data t1, where
                        /// total length = 64 - t1
                        /// </summary>
                        public int SoundLengthData
                        {
                            get { return m_lengthCounter; }
                            set
                            {
                                m_lengthCounter = (64 - (0x3F & value));

                                // Reloading shouldn't re-enable channel
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
                                // Trigger should treat 0 length as maximum
                                // regardless of length counter enabled/disabled
                                if (m_lengthCounter == 0)
                                {
                                    m_lengthCounter = 64;
                                }
                                m_period = (2048 - m_frequencyData) * 4;
                                m_curVolume = m_defaultEnvelopeVolume;
                                m_envelopeCounter = m_envelopeSteps;

                                // Disabled DAC should prevent enable at trigger
                                m_channelStatusOn = ChannelEnabled;

                                /*if (m_envelopeCounter == 0)
                                {
                                    m_envelopeCounter = 8;
                                }*/
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
                        /// Accessor for combined Reg NR23 (0xFF18)
                        /// and NR24 (0xFF19) parts of the
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


                        public bool LengthCounterEnabled
                        {
                            get { return m_lengthCounterEnabled; }
                            set
                            {
                                m_lengthCounterEnabled = value;

                                // Disabling length shouldn't re-enable channel
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


                        public void LengthStep()
                        {
                            if (m_lengthCounter > 0 && m_lengthCounterEnabled)
                            {
                                m_lengthCounter--;

                                if (m_lengthCounter == 0)
                                {
                                    // Length becoming 0 should clear status
                                    m_channelStatusOn = false;
                                }
                            }
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
