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
                    /// Channel 3 is a Voluntary Wave generator with programmable wave
                    /// table with 32 4-bit entries and a limited manual volume control.
                    /// </summary>
                    public class SoundChannel3
                    {
                        int[] m_waveForm;
                        
                        int m_frequencyData;
                        int m_freqTimer;

                        int m_volumeLevelCode;
                        int m_volumeShift;

                        int m_lengthCounter;
                        bool m_lengthCounterEnabled;
                        bool m_channelStatusOn;

                        bool m_dacEnabled;

                        int m_waveSamplePos;
                        int m_sampleBuffer;



                        /// <summary>
                        /// Accessor for Wave table (0xFF30-0xFF3F)
                        /// </summary>
                        public int[] WaveForm
                        {
                            get { return m_waveForm; }
                        }
                        

                        public SoundChannel3( )
                        {
                            m_waveForm = new int[32];
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
                        /// Flag indicated at NR52 (0xFF26) bit 2
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
                        /// Accessor for Reg NR30 (0xFF1A)
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
                                m_channelStatusOn &= m_dacEnabled;
                            }
                        }

                        #endregion Enabled/Disabled Controls




                        #region Length Control Related

                        /// <summary>
                        /// Accessor for Reg NR31 (0xFF1B)
                        /// Sound length data t1, where
                        /// total length = 256 - t1
                        /// Length in sec: = (256 - t1) * (1/256)
                        /// </summary>
                        public int SoundLengthData
                        {
                            get { return m_lengthCounter; }
                            set
                            {
                                // Length can be reloaded at any time
                                // Attempting to load length with 0 should load with maximum
                                // Reloading shouldn't re-enable channel
                                m_lengthCounter = (256 - (0xFF & value));
                            }
                        }


                        /// <summary>
                        /// Accessor for Length Counter Enabled flag
                        /// at Reg NR34
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
                        /// Accessor for Reg NR32 (0xFF1C)
                        /// Selection of the output volume level
                        /// </summary>
                        public int OutputVolumeLevel
                        {
                            get { return m_volumeLevelCode; }
                            set
                            {
                                m_volumeLevelCode = value;
                                switch (value)
                                {
                                    case 0:
                                        m_volumeShift = 8; // Mute
                                        break;

                                    case 1:
                                        m_volumeShift = 0; // Unmodified
                                        break;

                                    case 2:
                                        m_volumeShift = 1; // 50%
                                        break;

                                    case 3:
                                        m_volumeShift = 2; // 25%
                                        break;
                                }
                            }
                        }

                        #endregion Volume/Envelope Related




                        #region Frequency/Timer Related

                        /// <summary>
                        /// Accessor for combined Reg NR33 (0xFF1D)
                        /// and NR34 (0xFF1E) parts of the
                        /// Frequency data (11 bits)
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
                        int CalcFrequency( )
                        {
                            // needs to capture at 2 times the frequency we want to hear
                            return (2048 - m_frequencyData) * 2;
                        }


                        /// <summary>
                        /// Called from Frame Sequencer clocks
                        /// </summary>
                        public void FreqTimerStep( )
                        {
                            m_freqTimer -= 4;

                            if (m_freqTimer <= 0)
                            {
                                // Advances position and generate sample from this new position
                                m_waveSamplePos = (m_waveSamplePos + 1) % 32;
                                m_sampleBuffer = m_waveForm[m_waveSamplePos];

                                // Reload Frequency
                                m_freqTimer += CalcFrequency();
                            }
                        }

                        #endregion Frequency/Timer Related




                        /// <summary>
                        /// Gets the sample for Left DAC
                        /// </summary>
                        public int SampleL( )
                        {
                            if (!IsSoundOn || !ChannelEnabled || !LeftOutputEnabled || !UserEnabled)
                            {
                                return 0;
                            }
                            
                            return m_sampleBuffer >> m_volumeShift;
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

                            return m_sampleBuffer >> m_volumeShift;
                        }

                        
                        /// <summary>
                        /// Called when setting Trigger bit of NR34
                        /// </summary>
                        public void TriggerInit( )
                        {
                            m_channelStatusOn = true;

                            // Note: Trigger shouldn't affect length
                            // Note: Trigger should treat 0 length as maximum
                            // regardless of Length Counter Enabled flag
                            if (m_lengthCounter == 0)
                            {
                                m_lengthCounter = 256;
                            }

                            // Reload frequency timer
                            m_freqTimer = CalcFrequency();

                            // Position is reset, but Sample buffer is not refilled
                            m_waveSamplePos = 0;

                            // Disabled DAC should prevent enable at trigger
                            m_channelStatusOn &= m_dacEnabled;
                        }


                        /// <summary>
                        /// Routine when the APU NR52 is powered off
                        /// All related registers should be reset
                        /// </summary>
                        public void OnPowerOff( )
                        {
                            // Related NR51
                            LeftOutputEnabled = false;
                            RightOutputEnabled = false;

                            // Related NR30
                            ChannelEnabled = false;

                            // Related NR31
                            SoundLengthData = 0;

                            // Related NR32
                            OutputVolumeLevel = 0;

                            // Related NR33/NR34
                            FrequencyData = 0;
                            LengthCounterEnabled = false;
                        }


                        /// <summary>
                        /// Routine when the APU NR52 is powered on
                        /// </summary>
                        public void OnPowerOn( )
                        {
                            // Reset buffer pos
                            m_waveSamplePos = 0;
                            m_sampleBuffer = 0;
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
