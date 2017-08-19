#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN

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
    namespace PlatformSupport
    {


        public class WinPlatform : MonoBehaviour, IPlatform
        {
            List<IPlatformInput> m_inputs;
            bool m_isInited;


            #region IPlatform Implementations

            public bool IsInited
            {
                get { return m_isInited; }
            }

            public bool SupportsResolutionChange
            {
                get { return true; }
            }


            public List<IPlatformInput> GetConnectedInputs( )
            {
                return m_inputs;
            }

            #endregion IPlatform Implementations


            public static IPlatform Create( )
            {
                GameObject go = new GameObject("Platform_Win");
                IPlatform p = go.AddComponent<WinPlatform>();

                return p;
            }


            void Awake( )
            {
                QualitySettings.vSyncCount = 1;
                Application.targetFrameRate = 60;

                m_inputs = new List<IPlatformInput>(4);

                

                // Force DLL link
                {
                    XInputDotNetPure.GamePadState x_state = XInputDotNetPure.GamePad.GetState(0);
                }

                Invoke("DelayedAwake", 0.2f);
            }


            void DelayedAwake( )
            {
                m_inputs.Add(new KeyboardInput());

                int totalXInputs = 0;
                for (int i = 0; i < 4; ++i)
                {
                    if (XInputController.TestIfConnected(i))
                    {
                        m_inputs.Add(new XInputController(i));
                        ++totalXInputs;
                    }
                }


                string[] unityInput = Input.GetJoystickNames();
                for (int i = 0; i < unityInput.Length; ++i)
                {
                    // Ignore XInput controllers
                    if (unityInput[i].StartsWith("Controller (XBOX") && totalXInputs > 0)
                    {
                        --totalXInputs;
                        continue;
                    }

                    m_inputs.Add(new GenericJoystick(i));
                }


                m_isInited = true;
            }


            public void UpdateState( )
            {
                for (int i = 0; i < m_inputs.Count; ++i)
                {
                    m_inputs[i].UpdateState();
                }
            }
        }


    }
    // namespace PlatformSupport
}
// namespace xFF

#endif
