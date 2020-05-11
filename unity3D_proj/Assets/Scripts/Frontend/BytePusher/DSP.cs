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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;


namespace xFF
{
    namespace Frontend
    {
        namespace Unity3D
        {
            namespace BytePusher
            {

                public class DSP : MonoBehaviour
                {
                    float Gain = 0.05f;

                    private int m_samplesBufferSize;
                    AudioStream m_stream;
                    byte[] m_audioBuffer;


                    void Awake()
                    {
                        AudioSettings.OnAudioConfigurationChanged += OnAudioConfigurationChanged;

                        var conf = AudioSettings.GetConfiguration();
                        conf.dspBufferSize = 256;
                        conf.speakerMode = AudioSpeakerMode.Mono;
                        conf.sampleRate = 15360;

                        m_samplesBufferSize = conf.dspBufferSize;

                        m_stream = new AudioStream();
                        m_stream.MaxBufferLength = (m_samplesBufferSize) * 2;
                        m_audioBuffer = new byte[m_samplesBufferSize];

                        AudioSettings.Reset(conf);

                        Gain = 1;
                    }


                    void OnDestroy()
                    {
                        AudioSettings.OnAudioConfigurationChanged -= OnAudioConfigurationChanged;
                    }


                    void OnAudioConfigurationChanged(bool deviceWasChanged)
                    {
                        Debug.Log(deviceWasChanged ? "Device was changed" : "Reset was called");
                        if (deviceWasChanged)
                        {
                            AudioConfiguration config = AudioSettings.GetConfiguration();
                            config.dspBufferSize = 256;
                            config.speakerMode = AudioSpeakerMode.Mono;
                            config.sampleRate = 15360;
                            AudioSettings.Reset(config);
                        }
                        GetComponent<AudioSource>().Play();
                    }


                    void OnAudioFilterRead(float[] data, int channels)
                    {
                        if (m_audioBuffer.Length != data.Length)
                        {
                            Debug.Log("Does DSPBufferSize or speakerMode changed? Audio disabled.");
                            return;
                        }

                        int r = m_stream.Read(m_audioBuffer, 0, data.Length);
                        for (int i = 0; i < r; ++i)
                        {
                            data[i] = Gain * (sbyte)(m_audioBuffer[i]) / 128.0f;
                        }
                    }


                    public void UpdateBuffer(byte[] aRAM, int aStartOffset)
                    {
                        m_stream.Write(aRAM, aStartOffset, 256);
                    }





                    private class AudioStream : Stream
                    {
                        private readonly Queue<byte> m_buffer = new Queue<byte>();
                        private long m_maxBufferLength = 8192;

                        public long MaxBufferLength
                        {
                            get { return m_maxBufferLength; }
                            set { m_maxBufferLength = value; }
                        }

                        public new void Dispose()
                        {
                            m_buffer.Clear();
                        }

                        public override void Flush()
                        {
                        }

                        public override long Seek(long aOffset, SeekOrigin aOrigin)
                        {
                            throw new NotImplementedException();
                        }

                        public override void SetLength(long aValue)
                        {
                            throw new NotImplementedException();
                        }

                        public override int Read(byte[] aBuffer, int aOffset, int aCount)
                        {

#if DEBUG
                            if (aOffset != 0)
                            {
                                throw new NotImplementedException("Offsets with value of non-zero are not supported");
                            }
                            if (aBuffer == null)
                            {
                                throw new ArgumentException("Buffer is null");
                            }
                            if (aOffset + aCount > aBuffer.Length)
                            {
                                throw new ArgumentException("The sum of offset and count is greater than the buffer length. ");
                            }
                            if (aOffset < 0 || aCount < 0)
                            {
                                throw new ArgumentOutOfRangeException("offset", "offset or count is negative.");
                            }
#endif


                            if (aCount == 0)
                            {
                                return 0;
                            }

                            int readLength = 0;

                            lock (m_buffer)
                            {
                                // fill the read buffer
                                for (; readLength < aCount && Length > 0; ++readLength)
                                {
                                    aBuffer[readLength] = m_buffer.Dequeue();
                                }
                            }

                            return readLength;
                        }

                        private bool ReadAvailable(int aCount)
                        {
                            return (Length >= aCount);
                        }

                        public override void Write(byte[] aBuffer, int aOffset, int aCount)
                        {

#if DEBUG
                            if (aBuffer == null)
                            {
                                throw new ArgumentException("Buffer is null");
                            }
                            if (aOffset + aCount > aBuffer.Length)
                            {
                                throw new ArgumentException("The sum of offset and count is greater than the buffer length. ");
                            }
                            if (aOffset < 0 || aCount < 0)
                            {
                                throw new ArgumentOutOfRangeException("offset", "offset or count is negative.");
                            }
#endif


                            if (aCount == 0)
                            {
                                return;
                            }

                            lock (m_buffer)
                            {
                                while (Length >= m_maxBufferLength)
                                {
                                    return;
                                }

                                // queue up the buffer data
                                for (int i = 0; i < aCount; ++i)
                                {
                                    m_buffer.Enqueue(aBuffer[i + aOffset]);
                                }
                            }
                        }

                        public override bool CanRead
                        {
                            get { return true; }
                        }

                        public override bool CanSeek
                        {
                            get { return false; }
                        }

                        public override bool CanWrite
                        {
                            get { return true; }
                        }

                        public override long Length
                        {
                            get { return m_buffer.Count; }
                        }

                        public override long Position
                        {
                            get { return 0; }
                            set { throw new NotImplementedException(); }
                        }
                    }


                }



            }
            // namespace BytePusher
        }
        // namespace Unity3D
    }
    // namespace Frontend
}
// namespace xFF
