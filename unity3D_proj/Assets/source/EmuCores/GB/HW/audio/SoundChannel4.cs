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


                    public class SoundChannel4
                    {
                        int m_lengthCounter;
                        int m_envelopeSteps;
                        int m_defaultEnvelopeVolume;
                        int m_curVolume;
                        int m_envelopeCounter;
                        int m_envelopeMode;
                        bool m_isContinuous;

                        int m_polySteps;
                        int m_stepsMode;
                        int m_shiftFreq;
                        int m_divRatio;

                        int m_linearShiftReg = 1;

                        int m_timer;


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
                            get { return (m_lengthCounter > 0) || IsContinuous; }
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
                            get;
                            set;
                        }


                        /// <summary>
                        /// Accessor for Reg NR41 (0xFF20)
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

                        
                        public void TriggerInit()
                        {
                            //if (ChannelEnabled)
                            {
                                if (m_lengthCounter == 0)
                                {
                                    m_lengthCounter = 64;
                                }
                                //m_period = (2048 - m_frequencyData) * 4;
                                m_curVolume = m_defaultEnvelopeVolume;
                                m_envelopeCounter = m_envelopeSteps;
                                /*if (m_envelopeCounter == 0)
                                {
                                    m_envelopeCounter = 8;
                                }*/
                                m_linearShiftReg = 0xFF;
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

                        

                        public bool IsContinuous
                        {
                            get { return m_isContinuous; }
                            set
                            {
                                m_isContinuous = value;
                            }
                        }


                        public int StepsMode
                        {
                            get { return m_stepsMode; }
                            set
                            {
                                m_stepsMode = (0x1 & value);
                                m_polySteps = (m_stepsMode == 0) ? 15 : 7;
                            }
                        }


                        public int ShiftFreq
                        {
                            get { return m_shiftFreq; }
                            set
                            {
                                m_shiftFreq = (0x0F & value);
                            }
                        }


                        public int DivRatio
                        {
                            get { return m_divRatio; }
                            set
                            {
                                m_divRatio = (0x07 & value);
                            }
                        }


                        public void PeriodStep()
                        {
                            /*m_period -= 4;

                            if (m_period <= 0)
                            {
                                m_waveSamplePos = (m_waveSamplePos + 1) % 8;

                                m_period += (2048 - m_frequencyData) * 4;
                            }*/

                            m_timer -= 4;

                            if (m_timer <= 0)
                            {

                                switch (m_divRatio)
                                {
                                    case 0:
                                        m_timer += 8 << m_shiftFreq;
                                        break;

                                    case 1:
                                        m_timer += 16 << m_shiftFreq;
                                        break;

                                    case 2:
                                        m_timer += 32 << m_shiftFreq;
                                        break;

                                    case 3:
                                        m_timer += 48 << m_shiftFreq;
                                        break;

                                    case 4:
                                        m_timer += 64 << m_shiftFreq;
                                        break;

                                    case 5:
                                        m_timer += 80 << m_shiftFreq;
                                        break;

                                    case 6:
                                        m_timer += 96 << m_shiftFreq;
                                        break;

                                    case 7:
                                        m_timer += 112 << m_shiftFreq;
                                        break;
                                }

                                int result = (m_linearShiftReg & 0x1) ^ ((m_linearShiftReg >> 1) & 0x1);
                                m_linearShiftReg >>= 1;
                                m_linearShiftReg |= result << 14;
                                if (m_stepsMode == 1)
                                {
                                    m_linearShiftReg &= ~0x40;
                                    m_linearShiftReg |= result << 6;
                                }
                            }
                        }


                        public int GenerateSampleL()
                        {
                            if (!IsSoundOn || !ChannelEnabled || !LeftOutputEnabled || !UserEnabled)
                            {
                                return 0;
                            }

                            if ((m_linearShiftReg & 0x1) == 0)
                            {
                                return 15 & m_curVolume;
                            }

                            return 0;
                        }


                        public int GenerateSampleR()
                        {
                            if (!IsSoundOn || !ChannelEnabled || !RightOutputEnabled || !UserEnabled)
                            {
                                return 0;
                            }

                            if ((m_linearShiftReg & 0x1) == 0)
                            {
                                return 15 & m_curVolume;
                            }

                            return 0;
                        }


                        public void VolumeEnvelopeStep()
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
                            if (m_lengthCounter > 0 && !IsContinuous)
                            {
                                m_lengthCounter--;

                                if (m_lengthCounter == 0)
                                {
                                    // Disable channel
                                    //m_waveSamplePos = 0;
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
