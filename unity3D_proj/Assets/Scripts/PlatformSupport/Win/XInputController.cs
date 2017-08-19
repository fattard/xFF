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

using XInputDotNetPure;


namespace xFF
{
    namespace PlatformSupport
    {


        public class XInputController : IPlatformInput
        {
            public enum Button
            {
                A,
                B,
                X,
                Y,
                RB,
                LB,
                RT,
                LT,
                RS,
                LS,
                Start,
                Back,
                Guide,

                DPadUp,
                DPadDown,
                DPadLeft,
                DPadRight
            }


            PlayerIndex m_playerIndex;
            GamePadState m_state;
            GamePadState m_prevState;


            public bool IsConnected
            {
                get { return m_state.IsConnected; }
            }


            public XInputController(int aIdx)
            {
                m_playerIndex = (PlayerIndex)aIdx;
            }


            public bool GetButtonDown(int aBtn)
            {
                bool btnState = false;

                switch ((Button)aBtn)
                {
                    case Button.A:
                        btnState = (m_prevState.Buttons.A == ButtonState.Released && m_state.Buttons.A == ButtonState.Pressed);
                        break;

                    case Button.B:
                        btnState = (m_prevState.Buttons.B == ButtonState.Released && m_state.Buttons.B == ButtonState.Pressed);
                        break;

                    case Button.X:
                        btnState = (m_prevState.Buttons.X == ButtonState.Released && m_state.Buttons.X == ButtonState.Pressed);
                        break;

                    case Button.Y:
                        btnState = (m_prevState.Buttons.Y == ButtonState.Released && m_state.Buttons.Y == ButtonState.Pressed);
                        break;

                    case Button.RB:
                        btnState = (m_prevState.Buttons.RightShoulder == ButtonState.Released && m_state.Buttons.RightShoulder == ButtonState.Pressed);
                        break;

                    case Button.LB:
                        btnState = (m_prevState.Buttons.LeftShoulder == ButtonState.Released && m_state.Buttons.LeftShoulder == ButtonState.Pressed);
                        break;

                    case Button.RT:
                        btnState = (m_prevState.Triggers.Right < 0.25f && m_state.Triggers.Right > 0.75f);
                        break;

                    case Button.LT:
                        btnState = (m_prevState.Triggers.Left < 0.25f && m_state.Triggers.Left > 0.75f);
                        break;

                    case Button.RS:
                        btnState = (m_prevState.Buttons.RightStick == ButtonState.Released && m_state.Buttons.RightStick == ButtonState.Pressed);
                        break;

                    case Button.LS:
                        btnState = (m_prevState.Buttons.LeftStick == ButtonState.Released && m_state.Buttons.LeftStick == ButtonState.Pressed);
                        break;

                    case Button.Start:
                        btnState = (m_prevState.Buttons.Start == ButtonState.Released && m_state.Buttons.Start == ButtonState.Pressed);
                        break;

                    case Button.Back:
                        btnState = (m_prevState.Buttons.Back == ButtonState.Released && m_state.Buttons.Back == ButtonState.Pressed);
                        break;

                    case Button.Guide:
                        btnState = (m_prevState.Buttons.Guide == ButtonState.Released && m_state.Buttons.Guide == ButtonState.Pressed);
                        break;

                    case Button.DPadUp:
                        btnState = (m_prevState.DPad.Up == ButtonState.Released && m_state.DPad.Up == ButtonState.Pressed);
                        break;

                    case Button.DPadDown:
                        btnState = (m_prevState.DPad.Down == ButtonState.Released && m_state.DPad.Down == ButtonState.Pressed);
                        break;

                    case Button.DPadLeft:
                        btnState = (m_prevState.DPad.Left == ButtonState.Released && m_state.DPad.Left == ButtonState.Pressed);
                        break;

                    case Button.DPadRight:
                        btnState = (m_prevState.DPad.Right == ButtonState.Released && m_state.DPad.Right == ButtonState.Pressed);
                        break;
                }

                return btnState;
            }


            public bool GetButtonUp(int aBtn)
            {
                bool btnState = false;

                switch ((Button)aBtn)
                {
                    case Button.A:
                        btnState = (m_prevState.Buttons.A == ButtonState.Pressed && m_state.Buttons.A == ButtonState.Released);
                        break;

                    case Button.B:
                        btnState = (m_prevState.Buttons.B == ButtonState.Pressed && m_state.Buttons.B == ButtonState.Released);
                        break;

                    case Button.X:
                        btnState = (m_prevState.Buttons.X == ButtonState.Pressed && m_state.Buttons.X == ButtonState.Released);
                        break;

                    case Button.Y:
                        btnState = (m_prevState.Buttons.Y == ButtonState.Pressed && m_state.Buttons.Y == ButtonState.Released);
                        break;

                    case Button.RB:
                        btnState = (m_prevState.Buttons.RightShoulder == ButtonState.Pressed && m_state.Buttons.RightShoulder == ButtonState.Released);
                        break;

                    case Button.LB:
                        btnState = (m_prevState.Buttons.LeftShoulder == ButtonState.Pressed && m_state.Buttons.LeftShoulder == ButtonState.Released);
                        break;

                    case Button.RT:
                        btnState = (m_prevState.Triggers.Right > 0.75f && m_state.Triggers.Right < 0.25f);
                        break;

                    case Button.LT:
                        btnState = (m_prevState.Triggers.Left > 0.75f && m_state.Triggers.Left < 0.25f);
                        break;

                    case Button.RS:
                        btnState = (m_prevState.Buttons.RightStick == ButtonState.Pressed && m_state.Buttons.RightStick == ButtonState.Released);
                        break;

                    case Button.LS:
                        btnState = (m_prevState.Buttons.LeftStick == ButtonState.Pressed && m_state.Buttons.LeftStick == ButtonState.Released);
                        break;

                    case Button.Start:
                        btnState = (m_prevState.Buttons.Start == ButtonState.Pressed && m_state.Buttons.Start == ButtonState.Released);
                        break;

                    case Button.Back:
                        btnState = (m_prevState.Buttons.Back == ButtonState.Pressed && m_state.Buttons.Back == ButtonState.Released);
                        break;

                    case Button.Guide:
                        btnState = (m_prevState.Buttons.Guide == ButtonState.Pressed && m_state.Buttons.Guide == ButtonState.Released);
                        break;

                    case Button.DPadUp:
                        btnState = (m_prevState.DPad.Up == ButtonState.Pressed && m_state.DPad.Up == ButtonState.Released);
                        break;

                    case Button.DPadDown:
                        btnState = (m_prevState.DPad.Down == ButtonState.Pressed && m_state.DPad.Down == ButtonState.Released);
                        break;

                    case Button.DPadLeft:
                        btnState = (m_prevState.DPad.Left == ButtonState.Pressed && m_state.DPad.Left == ButtonState.Released);
                        break;

                    case Button.DPadRight:
                        btnState = (m_prevState.DPad.Right == ButtonState.Pressed && m_state.DPad.Right == ButtonState.Released);
                        break;
                }

                return btnState;
            }


            public bool GetButton(int aBtn)
            {
                bool btnState = false;

                switch ((Button)aBtn)
                {
                    case Button.A:
                        btnState = (m_prevState.Buttons.A == ButtonState.Pressed);
                        break;

                    case Button.B:
                        btnState = (m_prevState.Buttons.B == ButtonState.Pressed);
                        break;

                    case Button.X:
                        btnState = (m_prevState.Buttons.X == ButtonState.Pressed);
                        break;

                    case Button.Y:
                        btnState = (m_prevState.Buttons.Y == ButtonState.Pressed);
                        break;

                    case Button.RB:
                        btnState = (m_prevState.Buttons.RightShoulder == ButtonState.Pressed);
                        break;

                    case Button.LB:
                        btnState = (m_prevState.Buttons.LeftShoulder == ButtonState.Pressed);
                        break;

                    case Button.RT:
                        btnState = (m_prevState.Triggers.Right > 0.75f);
                        break;

                    case Button.LT:
                        btnState = (m_prevState.Triggers.Left > 0.75f);
                        break;

                    case Button.RS:
                        btnState = (m_prevState.Buttons.RightStick == ButtonState.Pressed);
                        break;

                    case Button.LS:
                        btnState = (m_prevState.Buttons.LeftStick == ButtonState.Pressed);
                        break;

                    case Button.Start:
                        btnState = (m_prevState.Buttons.Start == ButtonState.Pressed);
                        break;

                    case Button.Back:
                        btnState = (m_prevState.Buttons.Back == ButtonState.Pressed);
                        break;

                    case Button.Guide:
                        btnState = (m_prevState.Buttons.Guide == ButtonState.Pressed);
                        break;

                    case Button.DPadUp:
                        btnState = (m_prevState.DPad.Up == ButtonState.Pressed);
                        break;

                    case Button.DPadDown:
                        btnState = (m_prevState.DPad.Down == ButtonState.Pressed);
                        break;

                    case Button.DPadLeft:
                        btnState = (m_prevState.DPad.Left == ButtonState.Pressed);
                        break;

                    case Button.DPadRight:
                        btnState = (m_prevState.DPad.Right == ButtonState.Pressed);
                        break;
                }

                return btnState;
            }


            public void UpdateState( )
            {
                m_prevState = m_state;
                m_state = GamePad.GetState(m_playerIndex);
            }


            public static bool TestIfConnected(int aIdx)
            {
                return GamePad.GetState((PlayerIndex)aIdx).IsConnected;
            }

        }


    }
    // namespace PlatformSupport
}
// namespace xFF

#endif
