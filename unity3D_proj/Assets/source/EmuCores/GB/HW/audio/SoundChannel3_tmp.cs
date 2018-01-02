
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

                        private int totalLength;
                        private int cyclePos;
                        private int cycleLength = 2;
                        private int amplitude;
                        private int sampleRate;
                        public void SetFrequency(int gbFrequency)
                        {
                            float frequency = 65536f / (float)(2048 - gbFrequency);

                            cycleLength = (int)((float)(256f * sampleRate) / frequency);
                            if (cycleLength == 0)
                                cycleLength = 1;
                        }


                        public void SetSampleRate(int sr)
                        {
                            sampleRate = sr;
                        }

                        public void Play(byte[] b, int numSamples, int numChannels, int[] aWaveform)
                        {
                            int val;

                            if (m_lengthCounter > 0 || IsContinuous)
                            {
                                for (int r = 0; r < numSamples; r++)
                                {
                                    int samplePos = (31 * cyclePos) / cycleLength;
                                    val = aWaveform[samplePos % 32] >> m_volumeShift << 1;

                                    if (LeftOutputEnabled)
                                        b[r * numChannels] += (byte)val;
                                    if (RightOutputEnabled)
                                        b[r * numChannels + 1] += (byte)val;
                                    //if ((channel & CHAN_MONO) != 0)
                                    //b [r * numChannels] = b [r * numChannels + 1] += (byte)val;

                                    cyclePos = (cyclePos + 256) % cycleLength;
                                }
                            }
                        }
                        

                    }
                    


                }
            }
        }
    }
}
