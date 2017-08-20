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
using xFF.PlatformSupport;

namespace xFF
{
    namespace Frontend
    {
        namespace Unity3D
        {
            namespace GB
            {


                public class GenericJoystick_GB : GBInput
                {
                    public GenericJoystick_GB(IPlatformInput aInput) : base(aInput)
                    {
                        AssignDefaultMappings();
                    }


                    public override void AssignDefaultMappings( )
                    {
                        int noKeyCode = -(m_input as GenericJoystick).BaseBtn;

                        m_mappedButtons.Add((int)GBButtons.A, new DigitalButton(noKeyCode));
                        m_mappedButtons.Add((int)GBButtons.B, new DigitalButton(noKeyCode));
                        m_mappedButtons.Add((int)GBButtons.Select, new DigitalButton(noKeyCode));
                        m_mappedButtons.Add((int)GBButtons.Start, new DigitalButton(noKeyCode));
                        m_mappedButtons.Add((int)GBButtons.DPadUp, new DigitalButton(noKeyCode));
                        m_mappedButtons.Add((int)GBButtons.DPadDown, new DigitalButton(noKeyCode));
                        m_mappedButtons.Add((int)GBButtons.DPadLeft, new DigitalButton(noKeyCode));
                        m_mappedButtons.Add((int)GBButtons.DPadRight, new DigitalButton(noKeyCode));
                    }


                    public override void ApplyCustomMappings(EmuCores.GB.ConfigsGB.InputProfile aProfile)
                    {
                        if (aProfile.inputType != "joystick")
                        {
                            return;
                        }

                        m_mappedButtons[(int)GBButtons.A] = ParseBtnCode(aProfile.buttonA);
                        m_mappedButtons[(int)GBButtons.B] = ParseBtnCode(aProfile.buttonB);
                        m_mappedButtons[(int)GBButtons.Select] = ParseBtnCode(aProfile.buttonSelect);
                        m_mappedButtons[(int)GBButtons.Start] = ParseBtnCode(aProfile.buttonStart);
                        m_mappedButtons[(int)GBButtons.DPadUp] = ParseBtnCode(aProfile.buttonDPadUp);
                        m_mappedButtons[(int)GBButtons.DPadDown] = ParseBtnCode(aProfile.buttonDPadDown);
                        m_mappedButtons[(int)GBButtons.DPadLeft] = ParseBtnCode(aProfile.buttonDPadLeft);
                        m_mappedButtons[(int)GBButtons.DPadRight] = ParseBtnCode(aProfile.buttonDPadRight);
                    }


                    MappedButton ParseBtnCode(string aCode)
                    {
                        if (string.IsNullOrEmpty(aCode))
                        {
                            return new DigitalButton((int)KeyCode.None);
                        }

                        // Analogic button
                        if (aCode.StartsWith("("))
                        {
                            string[] analogicParts = aCode.Substring(1, aCode.Length - 2).Split(new char[] { '|' });

                            if (analogicParts.Length != 3)
                            {
                                EmuEnvironment.ShowErrorBox("Config file error", "Invalid data found in Profile Input section: " + aCode);
                                return new DigitalButton((int)KeyCode.None);
                            }

                            int axisNum = 0;
                            float axisDir = 1.0f;
                            float neutralState = 0.0f;

                            if (!int.TryParse(analogicParts[0], out axisNum))
                            {
                                EmuEnvironment.ShowErrorBox("Config file error", "Invalid data found in Profile Input section: " + aCode);
                                return new DigitalButton((int)KeyCode.None);
                            }

                            if (!float.TryParse(analogicParts[0], out axisDir))
                            {
                                EmuEnvironment.ShowErrorBox("Config file error", "Invalid data found in Profile Input section: " + aCode);
                                return new DigitalButton((int)KeyCode.None);
                            }

                            if (!float.TryParse(analogicParts[0], out neutralState))
                            {
                                EmuEnvironment.ShowErrorBox("Config file error", "Invalid data found in Profile Input section: " + aCode);
                                return new DigitalButton((int)KeyCode.None);
                            }

                            return new AnalogicButton(axisNum, axisDir, neutralState);
                        }

                        // Digital button
                        else
                        {
                            int k = (int)KeyCode.None;

                            if (!int.TryParse(aCode, out k))
                            {
                                EmuEnvironment.ShowErrorBox("Config file error", "Invalid data found in Profile Input section: " + aCode);
                            }

                            return new DigitalButton(k);
                        }
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
