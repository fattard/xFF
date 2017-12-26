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


                    public partial class SoundChannel3
                    {
                        int[] m_waveForm;
                        int m_volumeLevel;
                        int m_volumeShift;
                        int m_soundLength;
                        int m_lengthCounter;
                        int m_frequency;
                        int m_period;


                        int[] m_samples;
                        int m_queueHead;


                        public bool IsSoundOn
                        {
                            get;
                            set;
                        }


                        public int SoundLength
                        {
                            get { return m_soundLength; }
                            set
                            {
                                m_soundLength = value;
                                m_lengthCounter = (256 - value);

                                //SetLength(value);
                            }
                        }


                        public int OutputVolumeLevel
                        {
                            get { return m_volumeLevel; }
                            set
                            {
                                m_volumeLevel = value;
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


                        public int Frequency
                        {
                            get { return m_frequency; }
                            set
                            {
                                m_frequency = (65536 / (2048 - value));
                                m_period = 4194304 / m_frequency;

                                //SetFrequency(value);


                            }
                        }


                        public bool IsContinuous
                        {
                            get;
                            set;
                        }


                        public int[] WaveForm
                        {
                            get { return m_waveForm; }
                        }



                        public SoundChannel3( )
                        {
                            m_waveForm = new int[32];
                            m_samples = new int[8192];
                            m_queueHead = 0;
                        }


                        public void Reset( )
                        {

                        }


                        public void CyclesStep(int aElapsedCycles)
                        {
                            m_period -= aElapsedCycles;

                            if (m_period < 0)
                            {
                                if (m_lengthCounter > 0 || IsContinuous)
                                {
                                    --m_lengthCounter;
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
