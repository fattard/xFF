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
                    int m_envelopeTimer;
                    int m_sweepTimer;
                    byte[] m_outputWave;
                    int m_outputWaveIdx;
                    int[] m_regs = new int[0x30];


                    int[] m_samples = new int[2048];

                    int m_timeToGenerateSample;

                    bool m_masterSoundEnabled;

                    SoundChannel1 m_channel1 = new SoundChannel1();
                    SoundChannel2 m_channel2 = new SoundChannel2();
                    SoundChannel3 m_channel3 = new SoundChannel3();
                    SoundChannel4 m_channel4 = new SoundChannel4();
                    

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

                            if (!value)
                            {
                                // Force reset Regs
                                m_masterSoundEnabled = true;

                                for (int i = RegsIO.NR10; i <= RegsIO.NR51; ++i)
                                {
                                    this[i] = 0;
                                }
                            }

                            m_masterSoundEnabled = value;
                        }
                    }


                    public bool UserChannel1Enabled
                    {
                        get;
                        set;
                    }


                    public bool UserChannel2Enabled
                    {
                        get;
                        set;
                    }


                    public bool UserChannel3Enabled
                    {
                        get;
                        set;
                    }

                    public bool UserChannel4Enabled
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
                            // While APU is disbled, all reads to range 0xFF10-0xFF2F
                            // are ignored, except NR52
                            /*if (!MasterSoundEnabled && aAddress != RegsIO.NR52)
                            {
                                return 0xFF;
                            }*/


                            switch (aAddress)
                            {
                                case RegsIO.NR10:
                                    return 0x80 | (m_channel1.SweepShift)
                                                | (m_channel1.SweepMode << 3)
                                                | (m_channel1.SweepTime << 4);


                                case RegsIO.NR11:
                                    return 0x3F | (m_channel1.DutyCycle << 6);


                                case RegsIO.NR12:
                                    return m_channel1.EnvelopeSteps
                                            | (m_channel1.EnvelopeMode << 3)
                                            | (m_channel1.DefaultEnvelope << 4);


                                case RegsIO.NR13: // This Reg is write-only
                                    return 0xFF;


                                case RegsIO.NR14:
                                    return 0xBF | (!m_channel1.IsContinuous ? (1 << 6) : 0);

                                case RegsIO.NR21:
                                    return 0x3F | (m_channel2.DutyCycle << 6);


                                case RegsIO.NR22:
                                    return     m_channel2.EnvelopeSteps
                                            | (m_channel2.EnvelopeMode << 3)
                                            | (m_channel2.DefaultEnvelope << 4);


                                case RegsIO.NR23: // This Reg is write-only
                                    return 0xFF;


                                case RegsIO.NR24:
                                    return 0xBF | (!m_channel2.IsContinuous ? (1 << 6) : 0);


                                case RegsIO.NR30:
                                    return 0x7F | (m_channel3.ChannelEnabled ? (1 << 7) : 0);


                                case RegsIO.NR31: // This Reg is write-only
                                    return 0xFF;


                                case RegsIO.NR32:
                                    return 0x9F | (m_channel3.OutputVolumeLevel << 5);


                                case RegsIO.NR33: // This Reg is write-only
                                    return 0xFF;


                                case RegsIO.NR34:
                                    return 0xBF | (!m_channel3.IsContinuous ? (1 << 6) : 0);


                                case RegsIO.NR41:
                                    return 0xFF | m_channel4.SoundLengthData;


                                case RegsIO.NR42:
                                    return m_channel4.EnvelopeSteps
                                            | (m_channel4.EnvelopeMode << 3)
                                            | (m_channel4.DefaultEnvelope << 4);


                                case RegsIO.NR43:
                                    return m_channel4.DivRatio
                                            | (m_channel4.StepsMode << 3)
                                            | (m_channel4.ShiftFreq << 4);


                                case RegsIO.NR44:
                                    return 0xBF | (!m_channel4.IsContinuous ? (1 << 6) : 0);


                                case RegsIO.NR50:
                                    return    (OutputVolumeRight << 0)
                                            | (OutputVolumeLeft << 4)
                                            | (ExternalInputRightEnabled ? (1 << 3) : 0)
                                            | (ExternalInputLeftEnabled ? (1 << 7) : 0);


                                case RegsIO.NR51:
                                    return    (m_channel1.RightOutputEnabled ? (1 << 0) : 0)
                                            | (m_channel2.RightOutputEnabled ? (1 << 1) : 0)
                                            | (m_channel3.RightOutputEnabled ? (1 << 2) : 0)
                                            | (m_channel4.RightOutputEnabled ? (1 << 3) : 0)
                                            | (m_channel1.LeftOutputEnabled ? (1 << 4) : 0)
                                            | (m_channel2.LeftOutputEnabled ? (1 << 5) : 0)
                                            | (m_channel3.LeftOutputEnabled ? (1 << 6) : 0)
                                            | (m_channel4.LeftOutputEnabled ? (1 << 7) : 0);
                                            


                                case RegsIO.NR52:
                                    if (!MasterSoundEnabled)
                                    {
                                        return 0x70 | (MasterSoundEnabled ? (1 << 7) : 0)
                                                | m_regs[aAddress - RegsIO.NR10]; // unused bits
                                    }
                                    return 0x70 | (MasterSoundEnabled ? (1 << 7) : 0)
                                                | m_regs[aAddress - RegsIO.NR10] // unused bits
                                                | (m_channel1.IsSoundOn ? (1 << 0) : 0)
                                                | (m_channel2.IsSoundOn ? (1 << 1) : 0)
                                                | (m_channel3.IsSoundOn ? (1 << 2) : 0)
                                                | (m_channel4.IsSoundOn ? (1 << 3) : 0);


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

                                    //return m_regs[aAddress - RegsIO.NR10];
                                    return 0xFF;
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
                                case RegsIO.NR10:
                                    {
                                        m_channel1.SweepShift = value;
                                        m_channel1.SweepMode = (value >> 3);
                                        m_channel1.SweepTime = (value >> 4);
                                    }
                                    break;

                                case RegsIO.NR11:
                                    {
                                        m_channel1.SoundLengthData = value;
                                        m_channel1.DutyCycle = (value >> 6);
                                    }
                                    break;


                                case RegsIO.NR12:
                                    {
                                        m_channel1.EnvelopeSteps = value;
                                        m_channel1.EnvelopeMode = (value >> 3);
                                        m_channel1.DefaultEnvelope = (value >> 4);

                                        // Top 5 bits
                                        m_channel1.ChannelEnabled = ((0xF8 & value) != 0);
                                    }
                                    break;


                                case RegsIO.NR13:
                                    {
                                        int freq = (0xFF & value) | (0x700 & m_channel1.Frequency);

                                        m_channel1.Frequency = freq;
                                    }
                                    break;


                                case RegsIO.NR14:
                                    {
                                        int freq = (0xFF & m_channel1.Frequency) | ((0x07 & value) << 8);

                                        m_channel1.Frequency = freq;

                                        m_channel1.IsContinuous = ((0x40 & value) == 0);

                                        // Check trigger flag
                                        if ((0x80 & value) > 0)
                                        {
                                            m_channel1.TriggerInit();
                                        }
                                    }
                                    break;

                                case RegsIO.NR21:
                                    {
                                        m_channel2.SoundLengthData = value;
                                        m_channel2.DutyCycle = (value >> 6);
                                    }
                                    break;


                                case RegsIO.NR22:
                                    {
                                        m_channel2.EnvelopeSteps = value;
                                        m_channel2.EnvelopeMode = (value >> 3);
                                        m_channel2.DefaultEnvelope = (value >> 4);

                                        // Top 5 bits
                                        m_channel2.ChannelEnabled = ((0xF8 & value) != 0);
                                    }
                                    break;


                                case RegsIO.NR23:
                                    {
                                        int freq = (0xFF & value) | (0x700 & m_channel2.Frequency);

                                        m_channel2.Frequency = freq;
                                    }
                                    break;


                                case RegsIO.NR24:
                                    {
                                        int freq = (0xFF & m_channel2.Frequency) | ((0x07 & value) << 8);

                                        m_channel2.Frequency = freq;

                                        m_channel2.IsContinuous = ((0x40 & value) == 0);

                                        // Check trigger flag
                                        if ((0x80 & value) > 0)
                                        {
                                            m_channel2.TriggerInit();
                                        }
                                    }
                                    break;


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


                                case RegsIO.NR41:
                                    {
                                        m_channel4.SoundLengthData = value;
                                    }
                                    break;


                                case RegsIO.NR42:
                                    {
                                        m_channel4.EnvelopeSteps = value;
                                        m_channel4.EnvelopeMode = (value >> 3);
                                        m_channel4.DefaultEnvelope = (value >> 4);

                                        // Top 5 bits
                                        m_channel4.ChannelEnabled = ((0xF8 & value) != 0);
                                    }
                                    break;


                                case RegsIO.NR43:
                                    {
                                        m_channel4.DivRatio = value;
                                        m_channel4.StepsMode = (value >> 3);
                                        m_channel4.ShiftFreq = (value >> 4);
                                    }
                                    break;


                                case RegsIO.NR44:
                                    {
                                        m_channel4.IsContinuous = ((0x40 & value) == 0);

                                        // Check trigger flag
                                        if ((0x80 & value) > 0)
                                        {
                                            m_channel4.TriggerInit();
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
                                        //m_regs[aAddress - RegsIO.NR10] = (0xFF & value);
                                        //SetReg_TMP(aAddress, value);

                                        m_channel1.RightOutputEnabled = ((1 << 0) & value) != 0;
                                        m_channel2.RightOutputEnabled = ((1 << 1) & value) != 0;
                                        m_channel3.RightOutputEnabled = ((1 << 2) & value) != 0;
                                        m_channel4.RightOutputEnabled = ((1 << 3) & value) != 0;
                                        m_channel1.LeftOutputEnabled = ((1 << 4) & value) != 0;
                                        m_channel2.LeftOutputEnabled = ((1 << 5) & value) != 0;
                                        m_channel3.LeftOutputEnabled = ((1 << 6) & value) != 0;
                                        m_channel4.LeftOutputEnabled = ((1 << 7) & value) != 0;
                                    }
                                    break;


                                case RegsIO.NR52:
                                    {
                                        MasterSoundEnabled = ((0x80 & value) > 0);
                                        m_regs[aAddress - RegsIO.NR10] = (0x70 & value); // unused bits
                                        
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
                        while (aElapsedCycles > 0)
                        {
                            m_frameSequencerTimer += 4;
                            m_timeToGenerateSample += 4;

                            if (m_frameSequencerTimer >= 8192) // 512 Hz
                            {
                                m_lengthTimer++;
                                m_envelopeTimer++;
                                m_sweepTimer++;

                                if (m_lengthTimer == 2) // 256 Hz (16384 clocks: (8192 * 2))
                                {
                                    m_channel1.LengthStep();
                                    m_channel2.LengthStep();
                                    m_channel3.LengthStep();
                                    m_channel4.LengthStep();

                                    m_lengthTimer = 0;
                                }

                                if (m_sweepTimer == 4) // 128 Hz (20648 clocks: (8192 * 4))
                                {
                                    m_channel1.SweepStep();

                                    m_sweepTimer = 0;
                                }

                                if (m_envelopeTimer == 8) // 64 Hz (65536 clocks: (8192 * 8))
                                {
                                    m_channel1.VolumeEnvelopeStep();
                                    m_channel2.VolumeEnvelopeStep();
                                    m_channel4.VolumeEnvelopeStep();

                                    m_envelopeTimer = 0;
                                }

                                m_frameSequencerTimer -= 8192;
                            }

                            m_channel1.PeriodStep();
                            m_channel2.PeriodStep();
                            m_channel3.PeriodStep();
                            m_channel4.PeriodStep();

                            if (m_timeToGenerateSample > kTimeToUpdate)
                            {
                                m_channel1.UserEnabled = UserChannel1Enabled;
                                m_channel2.UserEnabled = UserChannel2Enabled;
                                m_channel3.UserEnabled = UserChannel3Enabled;
                                m_channel4.UserEnabled = UserChannel4Enabled;

                                int sampleL = 0;
                                int sampleR = 0;

                                if (MasterSoundEnabled)
                                {
                                    sampleL += m_channel1.GenerateSampleL();
                                    sampleL += m_channel2.GenerateSampleL();
                                    sampleL += m_channel3.GenerateSampleL();
                                    sampleL += m_channel4.GenerateSampleL();

                                    sampleR += m_channel1.GenerateSampleR();
                                    sampleR += m_channel2.GenerateSampleR();
                                    sampleR += m_channel3.GenerateSampleR();
                                    sampleR += m_channel4.GenerateSampleR();

                                    sampleL = (sampleL * ((1 + OutputVolumeLeft))) / 8;
                                    sampleR = (sampleR * ((1 + OutputVolumeRight))) / 8;
                                }

                                m_samples[m_outputWaveIdx * 2] = sampleL;
                                m_samples[m_outputWaveIdx * 2 + 1] = sampleR;


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
