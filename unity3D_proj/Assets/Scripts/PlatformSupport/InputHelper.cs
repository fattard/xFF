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

        
        public abstract class InputHelper
        {


            protected interface MappedButton
            {
                bool Held(IPlatformInput aInput);

                string Code
                {
                    get;
                }
            }


            protected class DigitalButton : MappedButton
            {
                int m_btnIdx;

                public bool Held(IPlatformInput aInput)
                {
                    return aInput.GetButton(m_btnIdx);
                }


                public string Code
                {
                    get { return m_btnIdx.ToString(); }
                }


                public DigitalButton(int aBtnIdx)
                {
                    m_btnIdx = aBtnIdx;
                }
            }


            protected class AnalogicButton : MappedButton
            {
                protected static readonly float kGenericAxisActionRange = 0.50f;

                int axisNum;
                float axisDirection;
                float neutralState;


                public bool Held(IPlatformInput aInput)
                {
                    float v = aInput.GetAxis(axisNum);
                    return (axisDirection > 0) ? v > (neutralState + kGenericAxisActionRange) : v < (neutralState - kGenericAxisActionRange);
                }


                public string Code
                {
                    get { return "(" + axisNum + "|" + axisDirection + "|" + neutralState + ")"; }
                }


                public AnalogicButton(int aAxisNum, float aAxisDir, float aNeutralState)
                {
                    axisNum = aAxisNum;
                    axisDirection = aAxisDir;
                    neutralState = aNeutralState;
                }
            }

        }

    }
    // namespace PlatformSupport
}
// namespace xFF
