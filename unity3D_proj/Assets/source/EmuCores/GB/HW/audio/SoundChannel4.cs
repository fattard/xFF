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

using System.Collections.Generic;
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
                    /// Channel 4 is a White-Noise generator based on LFSR RNG
                    /// with configurable timer and volume envelope to help
                    /// fading notes and effects.
                    /// </summary>
                    public class SoundChannel4
                    {
                        int m_linearShiftReg = 1;

                        int m_envelopeSteps;
                        int m_defaultEnvelopeVolume;
                        int m_curVolume;
                        int m_envelopeCounter;
                        int m_envelopeMode;
                        
                        int m_lengthCounter;
                        bool m_lengthCounterEnabled;
                        bool m_channelStatusOn;

                        bool m_dacEnabled;

                        int m_polySteps;
                        int m_stepsMode;
                        int m_shiftFreq;
                        int m_divRatio;

                        int m_timer;


                        CircularBuffer<int> m_samplesToFilterR;
                        CircularBuffer<int> m_samplesToFilterL;


                        public SoundChannel4( )
                        {
                            const int bufferSize = (87 / 2);

                            m_samplesToFilterR = new CircularBuffer<int>(bufferSize);
                            m_samplesToFilterL = new CircularBuffer<int>(bufferSize);
                            for (int i = 0; i < bufferSize; ++i)
                            {
                                m_samplesToFilterL.Enqueue(0);
                                m_samplesToFilterR.Enqueue(0);
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
                        /// Flag indicated at NR52 (0xFF26) bit 3
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
                        /// Accessor for top 5 bits of Reg NR42 (0xFF21)
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
                        /// Accessor for Reg NR41 (0xFF20)
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
                        /// at Reg NR44
                        /// </summary>
                        public bool LengthCounterEnabled
                        {
                            get { return m_lengthCounterEnabled; }
                            set
                            {
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
                        /// of NR42 (0xFF21)
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
                        /// of NR42 (0xFF21)
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

                                    if (m_envelopeMode > 0 && m_curVolume < 15)
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




                        #region Configurable Timer Related

                        /// <summary>
                        /// Accessor for the polynomial counter
                        /// steps mode part of NR43 (0xFF22)
                        /// 1 - 7 steps
                        /// 0 - 15 steps
                        /// </summary>
                        public int StepsMode
                        {
                            get { return m_stepsMode; }
                            set
                            {
                                m_stepsMode = (0x1 & value);
                                m_polySteps = (m_stepsMode == 0) ? 15 : 7;
                            }
                        }

                        /// <summary>
                        /// Accessor for the shift clock frequency
                        /// part of NR43 (0xFF22)
                        /// Frequency = 524288 Hz / r / 2^(s+1) ;For r=0 use r=0.5 instead
                        /// </summary>
                        public int ShiftFreq
                        {
                            get { return m_shiftFreq; }
                            set
                            {
                                m_shiftFreq = (0x0F & value);
                            }
                        }


                        /// <summary>
                        /// Accessor for the divisor ratio part
                        /// of NR43 (0xFF22)
                        /// Frequency = 524288 Hz / r / 2^(s+1) ;For r=0 use r=0.5 instead
                        /// </summary>
                        public int DivRatio
                        {
                            get { return m_divRatio; }
                            set
                            {
                                m_divRatio = (0x07 & value);
                            }
                        }


                        int CalcFrequency( )
                        {
                            int freq = 8;

                            switch (m_divRatio)
                            {
                                case 0:
                                    freq = 8 << m_shiftFreq;
                                    break;

                                case 1:
                                    freq = 16 << m_shiftFreq;
                                    break;

                                case 2:
                                    freq = 32 << m_shiftFreq;
                                    break;

                                case 3:
                                    freq = 48 << m_shiftFreq;
                                    break;

                                case 4:
                                    freq = 64 << m_shiftFreq;
                                    break;

                                case 5:
                                    freq = 80 << m_shiftFreq;
                                    break;

                                case 6:
                                    freq = 96 << m_shiftFreq;
                                    break;

                                case 7:
                                    freq = 112 << m_shiftFreq;
                                    break;
                            }

                            return freq;
                        }


                        /// <summary>
                        /// Called from Frame Sequencer clocks
                        /// </summary>
                        public void FreqTimerStep( )
                        {
                            m_timer -= 4;

                            if (m_timer <= 0)
                            {
                                int result = (m_linearShiftReg & 0x1) ^ ((m_linearShiftReg >> 1) & 0x1);
                                m_linearShiftReg >>= 1;
                                m_linearShiftReg |= result << 14;
                                if (m_stepsMode == 1)
                                {
                                    m_linearShiftReg &= ~0x40; // clear bit 6
                                    m_linearShiftReg |= result << 6;
                                }

                                // Reload Frequency
                                m_timer += CalcFrequency();
                            }


                            // Update filter buffer
                            m_samplesToFilterL.Dequeue();
                            m_samplesToFilterL.Enqueue(SampleL());

                            m_samplesToFilterR.Dequeue();
                            m_samplesToFilterR.Enqueue(SampleR());
                        }

                        #endregion Configurable Timer Related




                        /// <summary>
                        /// Gets the sample for Left DAC
                        /// </summary>
                        public int SampleL( )
                        {
                            if (!IsSoundOn || !ChannelEnabled || !LeftOutputEnabled || !UserEnabled)
                            {
                                return 0;
                            }

                            // Output is bit 0 inverted

                            if ((m_linearShiftReg & 0x1) == 0)
                            {
                                return 15 & m_curVolume;
                            }

                            return 0;
                        }


                        /// <summary>
                        /// Gets the sample for Right DAC
                        /// </summary>
                        public int SampleR( )
                        {
                            if (!IsSoundOn || !ChannelEnabled || !RightOutputEnabled || !UserEnabled)
                            {
                                return 0;
                            }

                            // Output is bit 0 inverted

                            if ((m_linearShiftReg & 0x1) == 0)
                            {
                                return 15 & m_curVolume;
                            }

                            return 0;
                        }


                        /// <summary>
                        /// Gets the sample for Left DAC after a low-pass filter
                        /// </summary>
                        /// <returns></returns>
                        public int FilteredSampleL( )
                        {
                            return ApplyBoxFilter(m_samplesToFilterL);
                        }


                        /// <summary>
                        /// Gets the sample for Right DAC after a low-pass filter
                        /// </summary>
                        /// <returns></returns>
                        public int FilteredSampleR( )
                        {
                            return ApplyBoxFilter(m_samplesToFilterR);
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
                            m_timer = CalcFrequency();

                            // Reloads evelope counter
                            m_envelopeCounter = m_envelopeSteps;
                            /*if (m_envelopeCounter == 0)
                            {
                                m_envelopeCounter = 8;
                            }*/

                            // Reload volume
                            m_curVolume = m_defaultEnvelopeVolume;

                            // Resets LFSR
                            m_linearShiftReg = 0x7FFF;

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

                            // Related NR41
                            SoundLengthData = 0;

                            // Related NR42
                            EnvelopeSteps = 0;
                            EnvelopeMode = 0;
                            DefaultEnvelope = 0;

                            // Related NR43
                            DivRatio = 0;
                            ShiftFreq = 0;
                            StepsMode = 0;

                            // Related NR44
                            LengthCounterEnabled = false;

                            // Related NR42 (top 5 bits)
                            ChannelEnabled = false;
                        }


                        /// <summary>
                        /// Routine when the APU NR52 is powered on
                        /// </summary>
                        public void OnPowerOn()
                        {
                            
                        }


                        /// <summary>
                        /// Passes a very simple low-pass box-filter based
                        /// on the average of samples range
                        /// </summary>
                        int ApplyBoxFilter(CircularBuffer<int> aSamplesToFilter)
                        {
                            int total = 0;

                            for (int i = 0; i < aSamplesToFilter.Count; ++i)
                            {
                                total += aSamplesToFilter[i];
                            }

                            return total / aSamplesToFilter.Count;
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
