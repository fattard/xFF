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
using xFF;
using xFF.Frontend.Unity3D.GB;
using xFF.EmuCores.GB.HW;

namespace xFF
{
    namespace Frontend
    {
        namespace Unity3D
        {
            namespace GB
            {

                public class LCDDisplay : MonoBehaviour
                {
                    public SingleDisplay gbDisplay;

                    Color[] m_LCDColor = new Color[4];
                    Color m_disabledLCDColor;


                    public void SetConfigs(EmuCores.GB.ConfigsGB aConfigs)
                    {
                        gbDisplay.displayWidth = 160;
                        gbDisplay.displayHeight = 144;
                        gbDisplay.displayZoomFactor = aConfigs.graphics.displayZoom;
                        gbDisplay.SetScreenStandard();

                        SetLCDColors(aConfigs);
                    }


                    public void SetLCDColors(EmuCores.GB.ConfigsGB aConfigs)
                    {
                        m_LCDColor[0] = new Color(aConfigs.dmgColors.color0.r / 255.0f, aConfigs.dmgColors.color0.g / 255.0f, aConfigs.dmgColors.color0.b / 255.0f);
                        m_LCDColor[1] = new Color(aConfigs.dmgColors.color1.r / 255.0f, aConfigs.dmgColors.color1.g / 255.0f, aConfigs.dmgColors.color1.b / 255.0f);
                        m_LCDColor[2] = new Color(aConfigs.dmgColors.color2.r / 255.0f, aConfigs.dmgColors.color2.g / 255.0f, aConfigs.dmgColors.color2.b / 255.0f);
                        m_LCDColor[3] = new Color(aConfigs.dmgColors.color3.r / 255.0f, aConfigs.dmgColors.color3.g / 255.0f, aConfigs.dmgColors.color3.b / 255.0f);

                        m_disabledLCDColor = new Color(aConfigs.dmgColors.colorDisabledLCD.r / 255.0f, aConfigs.dmgColors.colorDisabledLCD.g / 255.0f, aConfigs.dmgColors.colorDisabledLCD.b / 255.0f);
                    }


                    public void Render( )
                    {
                        gbDisplay.DrawDisplay(gbDisplay.Pixels);
                    }


                    public void DrawDisplay(PPU aPPU)
                    {
                        //TODO: display test
                        Color[] displayPixels = gbDisplay.Pixels;
                        for (int i = 0; i < 144; ++i)
                        {
                            for (int j = 0; j < 160; ++j)
                            {
                                displayPixels[(i * 512) + j] = m_LCDColor[0];
                            }
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
