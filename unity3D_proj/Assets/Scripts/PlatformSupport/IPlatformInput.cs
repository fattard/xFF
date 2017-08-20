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

        public enum InputType
        {
            Keyboard,
            Xbox,
            DualShock,
            Nintendo,
            GenericJoystick
        }



        public interface IPlatformInput
        {
            InputType InputType
            {
                get;
            }


            bool GetButtonDown(int aBtn);

            bool GetButtonUp(int aBtn);

            bool GetButton(int aBtn);

            float GetAxis(int aAxisIdx);

            void UpdateState();
        }
        

        public static class XboxInputType
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
                DPadRight,

                Menu = Start,
            }


            public enum Axis
            {
                LeftStickX,
                LeftStickY,
                RightStickX,
                RightStickY,
                RT,
                LT
            }
        }


        public static class DualshockInputType
        {
            public enum Button
            {
                Cross,
                Circle,
                Square,
                Triangle,
                R1,
                L1,
                R2,
                L2,
                R3,
                L3,
                Start,
                Select,
                PSButton,
                TouchPadButton,

                DPadUp,
                DPadDown,
                DPadLeft,
                DPadRight,

                PSVitaR = R2,
                PSVitaL = L2,

                Options = Start,
                Share = Select,
            }


            public enum Axis
            {
                LeftStickX,
                LeftStickY,
                RightStickX,
                RightStickY,
                R2,
                L2
            }
        }


        public static class NintendoInputType
        {
            public enum Button
            {
                A,
                B,
                X,
                Y,
                R,
                L,
                ZR,
                ZL,
                RS,
                LS,
                Start,
                Select,
                Home,

                DPadUp,
                DPadDown,
                DPadLeft,
                DPadRight,

                Plus = Start,
                Minus = Select,
                Z = ZR,
            }


            public enum Axis
            {
                LeftStickX,
                LeftStickY,
                RightStickX,
                RightStickY,
                ZR,
                ZL
            }
        }

    }
    // namespace PlatformSupport
}
// namespace xFF
