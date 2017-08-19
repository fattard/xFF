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
using System.Collections.Generic;

namespace xFF
{
    namespace Frontend
    {
        namespace Unity3D
        {
            namespace GB
            {

                public class GBInput : MonoBehaviour
                {
                    public enum GBButtons
                    {
                        A,
                        B,
                        Select,
                        Start,
                        DPadDown,
                        DPadUp,
                        DPadLeft,
                        DPadRight
                    }

                    public class InputHelper
                    {
                        PlatformSupport.IPlatformInput m_input;
                        Dictionary<int, int> m_mappedButtons;

                        public bool GetButton(GBButtons aGBBtn)
                        {
                            return m_input.GetButton(m_mappedButtons[(int)aGBBtn]);
                        }

                        public bool GetButtonDown(GBButtons aGBBtn)
                        {
                            return m_input.GetButtonDown(m_mappedButtons[(int)aGBBtn]);
                        }

                        public bool GetButtonUp(GBButtons aGBBtn)
                        {
                            return m_input.GetButtonUp(m_mappedButtons[(int)aGBBtn]);
                        }


                        public InputHelper(PlatformSupport.IPlatformInput aInput)
                        {
                            m_input = aInput;
                            m_mappedButtons = new Dictionary<int, int>(8);


                            if (m_input is PlatformSupport.KeyboardInput)
                            {
                                m_mappedButtons.Add((int)GBButtons.A, (int)KeyCode.X);
                                m_mappedButtons.Add((int)GBButtons.B, (int)KeyCode.Z);
                                m_mappedButtons.Add((int)GBButtons.Select, (int)KeyCode.RightShift);
                                m_mappedButtons.Add((int)GBButtons.Start, (int)KeyCode.Return);
                                m_mappedButtons.Add((int)GBButtons.DPadDown, (int)KeyCode.DownArrow);
                                m_mappedButtons.Add((int)GBButtons.DPadUp, (int)KeyCode.UpArrow);
                                m_mappedButtons.Add((int)GBButtons.DPadLeft, (int)KeyCode.LeftArrow);
                                m_mappedButtons.Add((int)GBButtons.DPadRight, (int)KeyCode.RightArrow);
                            }

                    #if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
                            else if (m_input is PlatformSupport.XInputController)
                            {
                                m_mappedButtons.Add((int)GBButtons.A, (int)PlatformSupport.XInputController.Button.B);
                                m_mappedButtons.Add((int)GBButtons.B, (int)PlatformSupport.XInputController.Button.A);
                                m_mappedButtons.Add((int)GBButtons.Select, (int)PlatformSupport.XInputController.Button.Back);
                                m_mappedButtons.Add((int)GBButtons.Start, (int)PlatformSupport.XInputController.Button.Start);
                                m_mappedButtons.Add((int)GBButtons.DPadDown, (int)PlatformSupport.XInputController.Button.DPadDown);
                                m_mappedButtons.Add((int)GBButtons.DPadUp, (int)PlatformSupport.XInputController.Button.DPadUp);
                                m_mappedButtons.Add((int)GBButtons.DPadLeft, (int)PlatformSupport.XInputController.Button.DPadLeft);
                                m_mappedButtons.Add((int)GBButtons.DPadRight, (int)PlatformSupport.XInputController.Button.DPadRight);
                            }
                    #endif

                            else
                            {
                                m_mappedButtons.Add((int)GBButtons.A, 3);
                                m_mappedButtons.Add((int)GBButtons.B, 2);
                                m_mappedButtons.Add((int)GBButtons.Select, 9);
                                m_mappedButtons.Add((int)GBButtons.Start, 10);
                                m_mappedButtons.Add((int)GBButtons.DPadDown, 11);
                                m_mappedButtons.Add((int)GBButtons.DPadUp, 12);
                                m_mappedButtons.Add((int)GBButtons.DPadLeft, 13);
                                m_mappedButtons.Add((int)GBButtons.DPadRight, 14);
                            }
                        }


                        public int GetMappedCode(GBButtons aBtn)
                        {
                            return m_mappedButtons[(int)aBtn];
                        }


                        public void SetMappedCode(GBButtons aBtn, int aCode)
                        {
                            m_mappedButtons[(int)aBtn] = aCode;
                        }

                        public PlatformSupport.IPlatformInput GetPlatformInput( )
                        {
                            return m_input;
                        }
                    }


                    List<InputHelper> m_helpers;
                    InputHelper m_selectedInput;
                    int m_keysState;


                    public List<InputHelper> GetInputList( )
                    {
                        return m_helpers;
                    }


                    public bool A
                    {
                        get
                        {
                            return m_selectedInput.GetButton(GBButtons.A);
                        }
                    }


                    public bool B
                    {
                        get
                        {
                            return m_selectedInput.GetButton(GBButtons.B);
                        }
                    }


                    public bool Select
                    {
                        get
                        {
                            return m_selectedInput.GetButton(GBButtons.Select);
                        }
                    }


                    public bool Start
                    {
                        get
                        {
                            return m_selectedInput.GetButton(GBButtons.Start);
                        }
                    }


                    public bool DPadUp
                    {
                        get
                        {
                            return m_selectedInput.GetButton(GBButtons.DPadUp);
                        }
                    }


                    public bool DPadDown
                    {
                        get
                        {
                            return m_selectedInput.GetButton(GBButtons.DPadDown);
                        }
                    }


                    public bool DPadLeft
                    {
                        get
                        {
                            return m_selectedInput.GetButton(GBButtons.DPadLeft);
                        }
                    }


                    public bool DPadRight
                    {
                        get
                        {
                            return m_selectedInput.GetButton(GBButtons.DPadRight);
                        }
                    }


                    public int GetKeysState()
                    {
                        return m_keysState;
                    }


                    public void Init( )
                    {
                        m_helpers = new List<InputHelper>(4);

                        var inputs = PlatformSupport.PlatformFactory.GetPlatform().GetConnectedInputs();
                        for (int i = 0; i < inputs.Count; ++i)
                        {
                            m_helpers.Add(new InputHelper(inputs[i]));
                        }

                        m_selectedInput = m_helpers[0];
                    }


                    public void UpdateState( )
                    {
                        int keys = 0;

                        // Prevent both Right/Left presses
                        if (DPadRight)
                            keys |= DPadRight ? (1 << 0) : 0;
                        else
                            keys |= DPadLeft ? (1 << 1) : 0;

                        // Prevent both Up/Down presses 
                        if (DPadUp)
                            keys |= DPadUp ? (1 << 2) : 0;
                        else
                            keys |= DPadDown ? (1 << 3) : 0;

                        keys |= A ? (1 << 4) : 0;
                        keys |= B ? (1 << 5) : 0;
                        keys |= Select ? (1 << 6) : 0;
                        keys |= Start ? (1 << 7) : 0;

                        

                        m_keysState = ~keys;
                    }
                }

            }
            // namespace GB
        }
        // namespace Unity3D
    }
    // namespace Frontend
}
// namespace xFF
