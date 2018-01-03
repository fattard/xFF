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
                    int m_frameSequencerTimer;
                    int m_lengthTimer;
                    byte[] m_outputWave;
                    int m_outputWaveIdx;
                    int[] m_regs = new int[0x30];


                    int[] m_samples = new int[2048];

                    int m_timeToGenerateSample;

                    bool m_masterSoundEnabled;

                    SoundChannel3 m_channel3 = new SoundChannel3();
                    

                    public EmuGB.PlayAudioFunc PlayAudio = (aAPU) => { };


                    public bool MasterSoundEnabled
                    {
                        get { return m_masterSoundEnabled; }
                        set
                        {
                            if (!m_masterSoundEnabled && value)
                            {
                                //m_frameSequencerTimer = 0;

                                for (int i = RegsIO.WAVE00; i <= RegsIO.WAVE15; ++i)
                                {
                                    this[i] = 0;
                                }
                            }
                            m_masterSoundEnabled = value;

                            if (!value)
                            {
                                for (int i = RegsIO.NR10; i < RegsIO.NR51; ++i)
                                {
                                    this[i] = 0;
                                }
                            }
                        }
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
                            // While APU is disbled, all reads to range 0xFF10-0xFF2F
                            // are ignored, except NR52
                            if (!MasterSoundEnabled && aAddress != RegsIO.NR52)
                            {
                                return 0xFF;
                            }


                            switch (aAddress)
                            {
                                case RegsIO.NR30:
                                    return 0x7F | (m_channel3.ChannelEnabled ? (1 << 7) : 0);


                                case RegsIO.NR31:
                                    return m_channel3.SoundLengthData;


                                case RegsIO.NR32:
                                    return 0x9F | (m_channel3.OutputVolumeLevel << 5);


                                case RegsIO.NR33: // This Reg is write-only
                                    return 0xFF;


                                case RegsIO.NR34:
                                    return 0xBF | (!m_channel3.IsContinuous ? (1 << 6) : 0);


                                case RegsIO.NR50:
                                    return    (OutputVolumeRight << 0)
                                            | (OutputVolumeLeft << 4)
                                            | (ExternalInputRightEnabled ? (1 << 3) : 0)
                                            | (ExternalInputLeftEnabled ? (1 << 7) : 0);


                                case RegsIO.NR51:
                                    
                                    return    (m_channel3.RightOutputEnabled ? (1 << 2) : 0)
                                            | (m_channel3.LeftOutputEnabled ? (1 << 6) : 0)
                                            //TODO: split to each channel
                                            | (0xBB & m_regs[aAddress - RegsIO.NR10]);


                                case RegsIO.NR52:
                                    return 0x70 | (MasterSoundEnabled ? (1 << 7) : 0)
                                                | (m_channel3.IsSoundOn ? (1 << 2) : 0)
                                                // TODO: get real flags
                                                | (0x0B & m_regs[aAddress - RegsIO.NR10]);


                                default:
                                    if (aAddress >= RegsIO.WAVE00 && aAddress <= RegsIO.WAVE15)
                                    {
                                        int index = (aAddress - RegsIO.WAVE00);
                                        return (m_channel3.WaveForm[index * 2] << 4) | m_channel3.WaveForm[(index * 2) + 1];
                                    }

                                    // Unused
                                    else if (aAddress >= 0xFF27 && aAddress <= 0xFF2F || aAddress == 0xFF15 || aAddress == 0xFF1F)
                                    {
                                        return 0xFF;
                                    }

                                    return m_regs[aAddress - RegsIO.NR10];
                            }
                        }

                        set
                        {
                            // Waveform RAM
                            if (aAddress >= RegsIO.WAVE00 && aAddress <= RegsIO.WAVE15)
                            {
                                int index = (aAddress - RegsIO.WAVE00);
                                m_channel3.WaveForm[index * 2] = (byte)((0xF0 & value) >> 4);
                                m_channel3.WaveForm[(index * 2) + 1] = (byte)(0x0F & value);
                                return;
                            }

                            // While APU is disbled, all writes to range 0xFF10-0xFF2F
                            // are ignored, except NR52
                            else if (!MasterSoundEnabled && aAddress != RegsIO.NR52)
                            {
                                return;
                            }

                            switch (aAddress)
                            {
                                case RegsIO.NR30:
                                    {
                                        m_channel3.ChannelEnabled = ((0x80 & value) > 0);
                                    }
                                    break;


                                case RegsIO.NR31:
                                    {
                                        m_channel3.SoundLengthData = (0xFF & value);
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

                                        // Check trigger flag
                                        if((0x80 & value) > 0)
                                        {
                                            m_channel3.TriggerInit();
                                        }
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
                                        //SetReg_TMP(aAddress, value);

                                        m_channel3.RightOutputEnabled = ((1 << 2) & value) != 0;
                                        m_channel3.LeftOutputEnabled = ((1 << 6) & value) != 0;
                                    }
                                    break;


                                case RegsIO.NR52:
                                    {
                                        MasterSoundEnabled = ((0x80 & value) > 0);

                                        m_regs[aAddress - RegsIO.NR10] = (0xF0 & value);
                                        
                                    }
                                    break;


                                default:
                                    {
                                        m_regs[aAddress - RegsIO.NR10] = (0xFF & value);
                                        //SetReg_TMP(aAddress, value);
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
                        while (aElapsedCycles > 0/* && MasterSoundEnabled*/)
                        {
                            m_frameSequencerTimer += 4;
                            m_timeToGenerateSample += 4;

                            if (m_frameSequencerTimer >= 8192) // 512 Hz
                            {
                                m_lengthTimer++;

                                if (m_lengthTimer == 2) // 256 Hz (16384 clocks: (8192 * 2))
                                {
                                    m_channel3.LengthStep();

                                    m_lengthTimer = 0;
                                }

                                m_frameSequencerTimer -= 8192;
                            }

                            m_channel3.PeriodStep();

                            if (m_timeToGenerateSample > kTimeToUpdate)
                            {
                                m_samples[m_outputWaveIdx * 2] = 0;
                                m_samples[m_outputWaveIdx * 2 + 1] = 0;

                                int ch3 = m_channel3.GenerateSample();

                                if (m_channel3.LeftOutputEnabled)
                                {
                                    m_samples[m_outputWaveIdx * 2] += ch3;
                                }
                                if (m_channel3.RightOutputEnabled)
                                {
                                    m_samples[m_outputWaveIdx * 2 + 1] += ch3;
                                }

                                m_outputWaveIdx = (m_outputWaveIdx + 2) % (m_samples.Length / 2);

                                m_timeToGenerateSample -= kTimeToUpdate;
                            }

                            aElapsedCycles -= 4;
                        }
                    }



                    // TEMP

                    int m_samplesAvailable;
                    int m_sampleRate = 44100;

                    int kTimeToUpdate = 4194304 / (44100 / 2);

                    public void SetSamplesAvailable(int aSamples)
                    {
                        m_samplesAvailable = aSamples;
                    }


                    
                    bool channel1Enable = true;
                    bool channel2Enable = true;
                    bool channel3Enable = true;
                    bool channel4Enable = true;



                    public void SetSampleRate(int sr)
                    {
                        m_sampleRate = sr;


                        m_channel3.SetSampleRate(sr);

                        //SetSampleRate_TMP(sr);
                    }

                    public void OutputSound(ref byte[] b)
                    {
                        

                        int numChannels = 2; // Always stereo for Game Boy
                        int numSamples = m_samplesAvailable;

                        /*byte[]*/
                        //b = new byte[numChannels * numSamples];
                        for (int i = 0; i < b.Length; ++i)
                        {
                            b[i] = 0;
                            b[i] = (byte)m_samples[i];
                        }

                        //UnityEngine.Debug.Log(m_outputWaveIdx);
                        m_outputWaveIdx = 0;

                        //OutputSound_TMP(ref b);

                        if (m_channel3.IsSoundOn && m_channel3.ChannelEnabled)
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
