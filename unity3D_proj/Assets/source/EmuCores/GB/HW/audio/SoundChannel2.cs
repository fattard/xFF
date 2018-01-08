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


                    /// <summary>
                    /// Channel 2 is a Square Wave generator with adjustable duty and
                    /// volume envelope control to help with fading notes and effects.
                    /// </summary>
                    public class SoundChannel2
                    {
                        int[][] m_dutyWaveForm = new int[][]
                        {
                            new int[] { 0,0,0,0,0,0,0,1 }, // 12.5%
                            new int[] { 1,0,0,0,0,0,0,1 }, // 25%
                            new int[] { 1,0,0,0,0,1,1,1 }, // 50%
                            new int[] { 0,1,1,1,1,1,1,0 }, // 75%
                        };

                        int m_frequencyData;
                        int m_freqTimer;

                        int m_envelopeSteps;
                        int m_defaultEnvelopeVolume;
                        int m_curVolume;
                        int m_envelopeCounter;
                        int m_envelopeMode;
                        
                        int m_lengthCounter;
                        bool m_lengthCounterEnabled;
                        bool m_channelStatusOn;

                        bool m_dacEnabled;

                        int m_waveSamplePos;
                        int m_dutyCycleIdx;



                        /// <summary>
                        /// Accessor for Duty Cycle part of
                        /// NR21 (0xFF16)
                        /// </summary>
                        public int DutyCycle
                        {
                            get { return m_dutyCycleIdx; }
                            set
                            {
                                m_dutyCycleIdx = (0x3 & value);
                            }
                        }




                        #region Enabled/Disabled Controls

                        /// <summary>
                        /// Handles UI configs for this channel
                        /// </summary>
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
                            get { return (m_dacEnabled && m_channelStatusOn); }
                        }


                        /// <summary>
                        /// Enables/Disables DAC output Left at NR51 (0xFF25)
                        /// </summary>
                        public bool LeftOutputEnabled
                        {
                            get;
                            set;
                        }


                        /// <summary>
                        /// Enables/Disables DAC output Right at NR51 (0xFF25)
                        /// </summary>
                        public bool RightOutputEnabled
                        {
                            get;
                            set;
                        }


                        /// <summary>
                        /// Accessor for top 5 bits of Reg NR22 (0xFF17)
                        /// Enables/Disables sound generation
                        /// from this channel DAC
                        /// </summary>
                        public bool ChannelEnabled
                        {
                            get { return m_dacEnabled; }
                            set
                            {
                                m_dacEnabled = value;

                                // Note: Disabling DAC should disable channel immediately
                                // Note: Enabling DAC shouldn't re-enable channel
                                m_channelStatusOn &= value;
                            }
                        }

                        #endregion Enabled/Disabled Controls




                        #region Length Control Related

                        /// <summary>
                        /// Accessor for Reg NR21 (0xFF16)
                        /// Sound length data t1, where
                        /// total length = 64 - t1
                        /// Length in sec: = (64 - t1) * (1/256)
                        /// </summary>
                        public int SoundLengthData
                        {
                            get { return m_lengthCounter; }
                            set
                            {
                                // Length can be reloaded at any time
                                // Attempting to load length with 0 should load with maximum
                                // Reloading shouldn't re-enable channel
                                m_lengthCounter = (64 - (0x3F & value));
                            }
                        }


                        /// <summary>
                        /// Accessor for Length Counter Enabled flag
                        /// at Reg NR24
                        /// </summary>
                        public bool LengthCounterEnabled
                        {
                            get { return m_lengthCounterEnabled; }
                            set
                            {
                                // Disabling length shouldn't re-enable channel
                                m_lengthCounterEnabled = value;
                            }
                        }


                        /// <summary>
                        /// Called when Frame Sequencer clocks the Length Control
                        /// </summary>
                        public void LengthStep( )
                        {
                            // Disabled channel should still clock length (ignore m_channelStatusOn)
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

                        #endregion Length Control




                        #region Volume/Envelope Related

                        /// <summary>
                        /// Accessor for the default volume part
                        /// of NR22 (0xFF17)
                        /// </summary>
                        public int DefaultEnvelope
                        {
                            get { return m_defaultEnvelopeVolume; }
                            set
                            {
                                m_defaultEnvelopeVolume = (0x0F & value);
                            }
                        }


                        /// <summary>
                        /// Accessor for the number of steps part
                        /// of NR22 (0xFF17)
                        /// </summary>
                        public int EnvelopeSteps
                        {
                            get { return m_envelopeSteps; }
                            set
                            {
                                m_envelopeSteps = (0x07 & value);
                            }
                        }

                        
                        /// <summary>
                        /// Accessor for the direction mode part
                        /// of NR22 (0xFF17)
                        /// 1 - up
                        /// 0 - down
                        /// </summary>
                        public int EnvelopeMode
                        {
                            get { return m_envelopeMode; }
                            set { m_envelopeMode = (0x01 & value); }
                        }


                        /// <summary>
                        /// Called when the Frame Sequencer clocks the Envelope unit
                        /// </summary>
                        public void VolumeEnvelopeStep( )
                        {
                            if (m_envelopeCounter > 0)
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
                        }

                        #endregion Volume/Envelope Related




                        #region Frequency/Timer Related

                        /// <summary>
                        /// Accessor for combined Reg NR23 (0xFF18)
                        /// and NR24 (0xFF19) parts of the
                        /// Frequency data (11 bits)
                        /// 
                        /// </summary>
                        public int FrequencyData
                        {
                            get { return m_frequencyData; }
                            set
                            {
                                m_frequencyData = value;
                                m_freqTimer = CalcFrequency();
                            }
                        }


                        /// <summary>
                        /// Calcs the period
                        /// </summary>
                        int CalcFrequency()
                        {
                            //TODO: why multiply by 4 ??
                            return (2048 - m_frequencyData) * 4;
                        }


                        /// <summary>
                        /// Called from Frame Sequencer clocks
                        /// </summary>
                        public void FreqTimerStep( )
                        {
                            m_freqTimer -= 4;

                            if (m_freqTimer <= 0)
                            {
                                // Advances position
                                m_waveSamplePos = (m_waveSamplePos + 1) % 8;

                                // Reload Frequency
                                m_freqTimer += CalcFrequency();
                            }
                        }

                        #endregion Frequency/Timer Related



                        /// <summary>
                        /// Gets the sample for Left DAC
                        /// </summary>
                        public int SampleL()
                        {
                            if (!IsSoundOn || !ChannelEnabled || !LeftOutputEnabled || !UserEnabled)
                            {
                                return 0;
                            }

                            return (m_dutyWaveForm[m_dutyCycleIdx][m_waveSamplePos] * 15) & m_curVolume;
                        }


                        /// <summary>
                        /// Gets the sample for Right DAC
                        /// </summary>
                        public int SampleR()
                        {
                            if (!IsSoundOn || !ChannelEnabled || !RightOutputEnabled || !UserEnabled)
                            {
                                return 0;
                            }

                            return (m_dutyWaveForm[m_dutyCycleIdx][m_waveSamplePos] * 15) & m_curVolume;
                        }


                        /// <summary>
                        /// Called when setting Trigger bit of NR24
                        /// </summary>
                        public void TriggerInit( )
                        {
                            m_channelStatusOn = true;

                            // Note: Trigger shouldn't affect length
                            // Note: Trigger should treat 0 length as maximum
                            // regardless of Length Counter Enabled flag
                            if (m_lengthCounter == 0)
                            {
                                m_lengthCounter = 64;
                            }

                            // Reload frequency timer
                            m_freqTimer = CalcFrequency();

                            // Reloads evelope counter
                            m_envelopeCounter = m_envelopeSteps;
                            /*if (m_envelopeCounter == 0)
                            {
                                m_envelopeCounter = 8;
                            }*/

                            // Reloads volume
                            m_curVolume = m_defaultEnvelopeVolume;

                            // Disabled DAC should prevent enable at trigger
                            m_channelStatusOn &= m_dacEnabled;
                        }


                        /// <summary>
                        /// Routine when the APU NR52 is powered off
                        /// All related registers should be reset
                        /// </summary>
                        public void OnPowerOff()
                        {
                            // Related NR51
                            LeftOutputEnabled = false;
                            RightOutputEnabled = false;

                            // Related NR21
                            SoundLengthData = 0;
                            DutyCycle = 0;

                            // Related NR22
                            EnvelopeSteps = 0;
                            EnvelopeMode = 0;
                            DefaultEnvelope = 0;

                            // Related NR23/NR34
                            FrequencyData = 0;
                            LengthCounterEnabled = false;

                            // Related NR22 (top 5 bits)
                            ChannelEnabled = false;
                        }


                        /// <summary>
                        /// Routine when the APU NR52 is powered on
                        /// </summary>
                        public void OnPowerOn()
                        {
                            // Reset buffer pos
                            m_waveSamplePos = 0;
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
