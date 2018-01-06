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


                    public class SoundChannel3
                    {
                        int[] m_waveForm;
                        int m_volumeLevel_RAW;
                        int m_volumeShift;
                        int m_lengthCounter;
                        int m_frequencyData;
                        int m_period;
                        bool m_isContinuous;

                        int m_waveSamplePos;


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


                        /// <summary>
                        /// Accessor for Reg NR30 (0xFF1A)
                        /// Enables/Disables sound generation
                        /// from this channel.
                        /// </summary>
                        public bool ChannelEnabled
                        {
                            get;
                            set;
                        }


                        /// <summary>
                        /// Accessor for Reg NR31 (0xFF1B)
                        /// Sound length data t1, where
                        /// total length = 256 - t1
                        /// </summary>
                        public int SoundLengthData
                        {
                            get { return m_lengthCounter; }
                            set
                            {
                                m_lengthCounter = (0xFF & (256 - value));
                            }
                        }


                        /// <summary>
                        /// Accessor for Reg NR32 (0xFF1C)
                        /// Selection of the output volume level
                        /// </summary>
                        public int OutputVolumeLevel
                        {
                            get { return m_volumeLevel_RAW; }
                            set
                            {
                                m_volumeLevel_RAW = value;
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


                        /// <summary>
                        /// Accessor for combined Reg NR33 (0xFF1D)
                        /// and NR34 (0xFF1E) parts of the
                        /// Frequency data (11 bits)
                        /// 
                        /// </summary>
                        public int Frequency
                        {
                            get { return m_frequencyData; }
                            set
                            {
                                m_frequencyData = value;
                                m_period = (2048 - m_frequencyData) * 2; // needs to capture at 2 times the frequency we want to hear
                            }
                        }


                        public bool IsContinuous
                        {
                            get { return m_isContinuous; }
                            set
                            {
                                m_isContinuous = value;
                            }
                        }


                        public int[] WaveForm
                        {
                            get { return m_waveForm; }
                        }



                        public SoundChannel3( )
                        {
                            m_waveForm = new int[32];
                        }


                        public void Reset( )
                        {
                            
                        }


                        public void TriggerInit( )
                        {
                            //if (ChannelEnabled)
                            {
                                if (m_lengthCounter == 0)
                                {
                                    m_lengthCounter = 256;
                                }
                                m_period = (2048 - m_frequencyData) * 2;
                                m_waveSamplePos = 0;
                            }
                        }



                        public void PeriodStep( )
                        {
                            m_period -= 4;

                            if (m_period <= 0)
                            {
                                m_waveSamplePos = (m_waveSamplePos + 1) % 32;

                                m_period += (2048 - m_frequencyData) * 2;
                            }
                        }


                        public int GenerateSampleL( )
                        {
                            if (!IsSoundOn || !ChannelEnabled || !LeftOutputEnabled || !UserEnabled)
                            {
                                return 0;
                            }
                            
                            return m_waveForm[m_waveSamplePos] >> m_volumeShift;
                        }


                        public int GenerateSampleR()
                        {
                            if (!IsSoundOn || !ChannelEnabled || !RightOutputEnabled || !UserEnabled)
                            {
                                return 0;
                            }

                            return m_waveForm[m_waveSamplePos] >> m_volumeShift;
                        }



                        public void LengthStep( )
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
