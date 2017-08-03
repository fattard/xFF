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
                    byte[] m_audioBufferA = new byte[256];
                    byte[] m_audioBufferB = new byte[256];
                    int m_curBufferOffset = 0;
                    byte[] m_curAudioBuffer;
                    int m_buffIdx;

                    void Awake()
                    {
                        AudioSettings.OnAudioConfigurationChanged += OnAudioConfigurationChanged;

                        var conf = AudioSettings.GetConfiguration();
                        conf.dspBufferSize = 256;
                        conf.speakerMode = AudioSpeakerMode.Mono;
                        conf.sampleRate = 11025;

                        AudioSettings.Reset(conf);

                        m_curAudioBuffer = m_audioBufferA;
                    }
    

                    void OnAudioConfigurationChanged(bool deviceWasChanged)
                    {
                        Debug.Log(deviceWasChanged ? "Device was changed" : "Reset was called");
                        if (deviceWasChanged)
                        {
                            AudioConfiguration config = AudioSettings.GetConfiguration();
                            config.dspBufferSize = 256;
                            AudioSettings.Reset(config);
                        }
                        GetComponent<AudioSource>().Play();
                    }


                    void OnAudioFilterRead(float[] data, int channels)
                    {
                        for (int i = 0; i < data.Length; i++)
                        {
                            data[i] = (((sbyte)m_curAudioBuffer[i]) / 128.0f);
                        }
                    }


                    public void UpdateBuffer(byte[] aRAM, int aStartOffset)
                    {
                        for (int i = 0; i < m_curAudioBuffer.Length; ++i)
                        {
                            m_curAudioBuffer[i] = aRAM[i + aStartOffset];
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
