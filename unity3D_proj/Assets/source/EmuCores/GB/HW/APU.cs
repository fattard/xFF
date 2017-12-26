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
using xFF.EmuCores.GB.HW.audio;


namespace xFF
{
    namespace EmuCores
    {
        namespace GB
        {
            namespace HW
            {


                public partial class APU
                {
                    
                    byte[] m_outputWave;
                    int[] m_regs = new int[0x30];

                    SoundChannel3 m_channel3 = new SoundChannel3();
                    

                    public EmuGB.PlayAudioFunc PlayAudio = (aAPU) => { };


                    public bool MasterSoundEnabled
                    {
                        get;
                        set;
                    }


                    public int OutputVolumeLeft
                    {
                        get;
                        set;
                    }


                    public int OutputVolumeRight
                    {
                        get;
                        set;
                    }


                    public bool ExternalInputLeftEnabled
                    {
                        get;
                        set;
                    }


                    public bool ExternalInputRightEnabled
                    {
                        get;
                        set;
                    }


                    public byte[] OutputWave
                    {
                        get { return m_outputWave; }
                        set { m_outputWave = value; }
                    }


                    public int this[int aAddress]
                    {
                        get
                        {
                            switch (aAddress)
                            {
                                case RegsIO.NR30:
                                    return 0x7F | (m_channel3.IsSoundOn ? (1 << 7) : 0);


                                case RegsIO.NR31:
                                    return m_channel3.SoundLength;


                                case RegsIO.NR32:
                                    return 0x9F | (m_channel3.OutputVolumeLevel << 5);


                                case RegsIO.NR33:
                                    return 0xFF;


                                case RegsIO.NR34:
                                    return 0xBF | (!m_channel3.IsContinuous ? (1 << 6) : 0);


                                case RegsIO.NR50:
                                    return  (OutputVolumeRight) | (OutputVolumeLeft << 4)
                                            | (ExternalInputRightEnabled ? (1 << 3) : 0)
                                            | (ExternalInputLeftEnabled ? (1 << 7) : 0);


                                case RegsIO.NR51:
                                    //TODO: split to each channel
                                    return m_regs[aAddress - RegsIO.NR10];


                                case RegsIO.NR52:
                                    return 0x70 | (MasterSoundEnabled ? (1 << 7) : 0)
                                            // TODO: get real flags
                                            | (0x0F & m_regs[aAddress - RegsIO.NR10]);


                                default:
                                    if (aAddress >= RegsIO.WAVE00 && aAddress <= RegsIO.WAVE15)
                                    {
                                        return m_channel3.WaveForm[aAddress - RegsIO.WAVE00];
                                    }

                                    return m_regs[aAddress - RegsIO.NR10];
                            }
                        }

                        set
                        {
                            switch (aAddress)
                            {
                                case RegsIO.NR30:
                                    {
                                        m_channel3.IsSoundOn = ((0x80 & value) > 0);
                                    }
                                    break;


                                case RegsIO.NR31:
                                    {
                                        m_channel3.SoundLength = (0xFF & value);
                                    }
                                    break;


                                case RegsIO.NR32:
                                    {
                                        m_channel3.OutputVolumeLevel = (0x03 & (value >> 5));
                                    }
                                    break;
                                    

                                case RegsIO.NR33:
                                    {
                                        int freq = (0xFF & value) | (0x700 & m_channel3.Frequency);

                                        m_channel3.Frequency = freq;
                                    }
                                    break;


                                case RegsIO.NR34:
                                    {
                                        int freq = (0xFF & m_channel3.Frequency) | ((0x07 & value) << 8);

                                        m_channel3.Frequency = freq;

                                        m_channel3.IsContinuous = ((0x40 & value) == 0);
                                    }
                                    break;
                                    


                                case RegsIO.NR50:
                                    {
                                        OutputVolumeRight = (0x07 & value);
                                        OutputVolumeLeft = (0x07 & (value >> 4));
                                        ExternalInputRightEnabled = ((0x08 & value) > 0);
                                        ExternalInputLeftEnabled = ((0x80 & value) > 0);
                                    }
                                    break;


                                case RegsIO.NR51:
                                    {
                                        //TODO: split to each channel
                                        m_regs[aAddress - RegsIO.NR10] = (0xFF & value);
                                        //SetReg(aAddress, value);
                                    }
                                    break;


                                case RegsIO.NR52:
                                    {
                                        MasterSoundEnabled = ((0x80 & value) > 0);
                                        
                                        //TODO: split channel On flags
                                        m_regs[aAddress - RegsIO.NR10] = (0xFF & value);
                                    }
                                    break;


                                default:
                                    // Waveform RAM
                                    if (aAddress >= RegsIO.WAVE00 && aAddress <= RegsIO.WAVE15)
                                    {
                                        int index = (aAddress - RegsIO.WAVE00);
                                        m_channel3.WaveForm[index * 2] = (byte)((0xF0 & value) >> 4);
                                        m_channel3.WaveForm[(index * 2) + 1] = (byte)(0x0F & value);
                                    }

                                    else
                                    {
                                        m_regs[aAddress - RegsIO.NR10] = (0xFF & value);
                                        //SetReg(aAddress, value);
                                    }
                                    break;
                            }
                        }
                    }



                    public void UpdateOutputWave( )
                    {
                        OutputSound(ref m_outputWave);
                    }



                    public void CyclesStep(int aElapsedCycles)
                    {

                    }



                    // TEMP

                    int m_samplesAvailable;
                    int m_sampleRate = 44100;

                    public void SetSamplesAvailable(int aSamples)
                    {
                        m_samplesAvailable = aSamples;
                    }


                    bool channel3Enable = true;



                    public void SetSampleRate(int sr)
                    {
                        m_sampleRate = sr;


                        m_channel3.SetSampleRate(sr);
                    }

                    public void OutputSound(ref byte[] b)
                    {
                        if (!MasterSoundEnabled)
                            return;

                        int numChannels = 2; // Always stereo for Game Boy
                        int numSamples = m_samplesAvailable;

                        /*byte[]*/
                        //b = new byte[numChannels * numSamples];
                        for (int i = 0; i < b.Length; ++i)
                        {
                            b[i] = 0;
                        }

                        if (m_channel3.IsSoundOn)
                        {
                            m_channel3.Play(b, numSamples, numChannels, m_channel3.WaveForm);
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
