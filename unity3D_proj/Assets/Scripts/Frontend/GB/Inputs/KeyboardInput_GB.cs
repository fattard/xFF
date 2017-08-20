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


                public class KeyboardInput_GB : GBInput
                {
                    public KeyboardInput_GB(IPlatformInput aInput) : base(aInput)
                    {
                        AssignDefaultMappings();
                    }


                    public override void AssignDefaultMappings( )
                    {
                        m_mappedButtons.Add((int)GBButtons.A, new DigitalButton((int)KeyCode.X));
                        m_mappedButtons.Add((int)GBButtons.B, new DigitalButton((int)KeyCode.Z));
                        m_mappedButtons.Add((int)GBButtons.Select, new DigitalButton((int)KeyCode.RightShift));
                        m_mappedButtons.Add((int)GBButtons.Start, new DigitalButton((int)KeyCode.Return));
                        m_mappedButtons.Add((int)GBButtons.DPadUp, new DigitalButton((int)KeyCode.UpArrow));
                        m_mappedButtons.Add((int)GBButtons.DPadDown, new DigitalButton((int)KeyCode.DownArrow));
                        m_mappedButtons.Add((int)GBButtons.DPadLeft, new DigitalButton((int)KeyCode.LeftArrow));
                        m_mappedButtons.Add((int)GBButtons.DPadRight, new DigitalButton((int)KeyCode.RightArrow));
                    }


                    public override void ApplyCustomMappings(EmuCores.GB.ConfigsGB.InputProfile aProfile)
                    {
                        if (aProfile.inputType != "keyboard")
                        {
                            return;
                        }

                        try
                        {
                            m_mappedButtons[(int)GBButtons.A] = new DigitalButton(int.Parse(aProfile.buttonA));
                            m_mappedButtons[(int)GBButtons.B] = new DigitalButton(int.Parse(aProfile.buttonB));
                            m_mappedButtons[(int)GBButtons.Select] = new DigitalButton(int.Parse(aProfile.buttonSelect));
                            m_mappedButtons[(int)GBButtons.Start] = new DigitalButton(int.Parse(aProfile.buttonStart));
                            m_mappedButtons[(int)GBButtons.DPadUp] = new DigitalButton(int.Parse(aProfile.buttonDPadUp));
                            m_mappedButtons[(int)GBButtons.DPadDown] = new DigitalButton(int.Parse(aProfile.buttonDPadDown));
                            m_mappedButtons[(int)GBButtons.DPadLeft] = new DigitalButton(int.Parse(aProfile.buttonDPadLeft));
                            m_mappedButtons[(int)GBButtons.DPadRight] = new DigitalButton(int.Parse(aProfile.buttonDPadRight));
                        }
                        catch (System.Exception e)
                        {
                            EmuEnvironment.ShowErrorBox("Config file error", "Invalid data in Profile Input section: " + e.Message);
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
