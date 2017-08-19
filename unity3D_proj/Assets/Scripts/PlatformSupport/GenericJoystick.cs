#if UNITY_STANDALONE || UNITY_EDITOR

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


namespace xFF
{
    namespace PlatformSupport
    {


        public class GenericJoystick : IPlatformInput
        {

            int m_baseBtn;


            public GenericJoystick(int aIdx)
            {
                switch (aIdx)
                {
                    case 0:
                        m_baseBtn = (int)KeyCode.Joystick1Button0;
                        break;

                    case 1:
                        m_baseBtn = (int)KeyCode.Joystick2Button0;
                        break;

                    case 2:
                        m_baseBtn = (int)KeyCode.Joystick3Button0;
                        break;

                    case 3:
                        m_baseBtn = (int)KeyCode.Joystick4Button0;
                        break;

                    case 4:
                        m_baseBtn = (int)KeyCode.Joystick5Button0;
                        break;

                    case 5:
                        m_baseBtn = (int)KeyCode.Joystick6Button0;
                        break;

                    case 6:
                        m_baseBtn = (int)KeyCode.Joystick7Button0;
                        break;

                    case 7:
                        m_baseBtn = (int)KeyCode.Joystick8Button0;
                        break;

                    default:
                        m_baseBtn = (int)KeyCode.Joystick1Button0;
                        break;
                }
            }
            

            public bool GetButtonDown(int aBtn)
            {
                return Input.GetKeyDown((KeyCode)(m_baseBtn + aBtn));
            }


            public bool GetButtonUp(int aBtn)
            {
                return Input.GetKeyUp((KeyCode)(m_baseBtn + aBtn));
            }


            public bool GetButton(int aBtn)
            {
                return Input.GetKey((KeyCode)(m_baseBtn + aBtn));
            }


            public void UpdateState()
            {
            }


        }


    }
    // namespace PlatformSupport
}
// namespace xFF

#endif
