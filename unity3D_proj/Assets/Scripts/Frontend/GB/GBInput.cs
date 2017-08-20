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


                public abstract class GBInput : PlatformSupport.InputHelper
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

                    public static List<GBInput> InputList
                    {
                        get { return s_inputList; }
                    }

                    protected static List<GBInput> s_inputList = new List<GBInput>(4);

                    protected Dictionary<int, MappedButton> m_mappedButtons;
                    protected PlatformSupport.IPlatformInput m_input;
                    protected int m_keysState;


                    protected GBInput(PlatformSupport.IPlatformInput aInput)
                    {
                        m_input = aInput;
                        m_mappedButtons = new Dictionary<int, MappedButton>(8);
                    }


                    public virtual bool A
                    {
                        get
                        {
                            return m_mappedButtons[(int)GBButtons.A].Held(m_input);
                        }
                    }


                    public virtual bool B
                    {
                        get
                        {
                            return m_mappedButtons[(int)GBButtons.B].Held(m_input);
                        }
                    }


                    public virtual bool Select
                    {
                        get
                        {
                            return m_mappedButtons[(int)GBButtons.Select].Held(m_input);
                        }
                    }


                    public virtual bool Start
                    {
                        get
                        {
                            return m_mappedButtons[(int)GBButtons.Start].Held(m_input);
                        }
                    }


                    public virtual bool DPadUp
                    {
                        get
                        {
                            return m_mappedButtons[(int)GBButtons.DPadUp].Held(m_input);
                        }
                    }


                    public virtual bool DPadDown
                    {
                        get
                        {
                            return m_mappedButtons[(int)GBButtons.DPadDown].Held(m_input);
                        }
                    }


                    public virtual bool DPadLeft
                    {
                        get
                        {
                            return m_mappedButtons[(int)GBButtons.DPadLeft].Held(m_input);
                        }
                    }


                    public virtual bool DPadRight
                    {
                        get
                        {
                            return m_mappedButtons[(int)GBButtons.DPadRight].Held(m_input);
                        }
                    }


                    public int GetKeysState( )
                    {
                        return m_keysState;
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


                    public abstract void AssignDefaultMappings( );

                    public abstract void ApplyCustomMappings(EmuCores.GB.ConfigsGB.InputProfile aProfile);


                    public static GBInput BuildInput(EmuCores.GB.ConfigsGB.InputProfile aProfile, List<PlatformSupport.IPlatformInput> aInputList)
                    {
                        // Build input list
                        for (int i = 0; i < aInputList.Count; ++i)
                        {
                            if (aInputList[i].InputType == PlatformSupport.InputType.Keyboard)
                            {
                                s_inputList.Add(new KeyboardInput_GB(aInputList[i]));
                            }

                            else if (aInputList[i].InputType == PlatformSupport.InputType.Xbox)
                            {
                                s_inputList.Add(new XboxInput_GB(aInputList[i]));
                            }

                            else if (aInputList[i] is PlatformSupport.GenericJoystick)
                            {
                                s_inputList.Add(new GenericJoystick_GB(aInputList[i]));
                            }
                        }

                        if (aInputList.Count == 1)
                        {
                            return s_inputList[0];
                        }

                        return new MasterInput_GB();


                        //TODO: apply custom profiles
#if _DISABLED
                        for (int i = 0; i < aInputList.Count; ++i)
                        {
                            if (aProfile.inputType == "keyboard" && aInputList[i] is PlatformSupport.KeyboardInput)
                            {
                                var kbd = new KeyboardInput_GB(aInputList[i]);
                                kbd.ApplyCustomMappings(aProfile);
                                return kbd;
                            }

                            else if (aProfile.inputType == "xinput" && aInputList[i] is PlatformSupport.XboxInputType)
                            {
                                var xinput = new XboxInput_GB(aInputList[i]);
                                xinput.ApplyCustomMappings(aProfile);
                                return xinput;
                            }

                            else if (aProfile.inputType == "joystick" && aInputList[i] is PlatformSupport.GenericJoystick)
                            {
                                var joystick = new GenericJoystick_GB(aInputList[i]);
                                joystick.ApplyCustomMappings(aProfile);
                                return joystick;
                            }
                        }

                        // Build default for platform
                        var defaultInput = aInputList[0];
                        if (defaultInput is PlatformSupport.KeyboardInput)
                        {
                            return new KeyboardInput_GB(defaultInput);
                        }
                        else if (defaultInput is PlatformSupport.XboxInputType)
                        {
                            return new XboxInput_GB(defaultInput);
                        }

                        // Keyboard as fallback
                        return new KeyboardInput_GB(defaultInput);
#endif // _DISABLED
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
