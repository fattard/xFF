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

namespace xFF
{
    namespace Frontend
    {
        namespace Unity3D
        {
            namespace GB
            {


                public class FrontendGB : MonoBehaviour
                {
                    public ConfigsGB frontendConfigs;
                    public SingleDisplay gbDisplay;

                    EmuCores.GB.EmuGB m_emuGB;


                    void Start( )
                    {
                        EmuCores.GB.ConfigsGB configGB = new EmuCores.GB.ConfigsGB();

                        // Bootrom
                        configGB.bootRomDMG.enabled = true;
                        configGB.bootRomDMG.path = frontendConfigs.bootRomPath;

                        // DMG Colors
                        configGB.dmgColors.color0.Set((int)(255 * frontendConfigs.color0.r), (int)(255 * frontendConfigs.color0.g), (int)(255 * frontendConfigs.color0.b));
                        configGB.dmgColors.color1.Set((int)(255 * frontendConfigs.color1.r), (int)(255 * frontendConfigs.color1.g), (int)(255 * frontendConfigs.color1.b));
                        configGB.dmgColors.color2.Set((int)(255 * frontendConfigs.color2.r), (int)(255 * frontendConfigs.color2.g), (int)(255 * frontendConfigs.color2.b));
                        configGB.dmgColors.color3.Set((int)(255 * frontendConfigs.color3.r), (int)(255 * frontendConfigs.color3.g), (int)(255 * frontendConfigs.color3.b));
                        configGB.dmgColors.colorDisabledLCD.Set((int)(255 * frontendConfigs.color0.r), (int)(255 * frontendConfigs.color0.g), (int)(255 * frontendConfigs.color0.b));

                        gbDisplay.displayWidth = 160;
                        gbDisplay.displayHeight = 144;
                        gbDisplay.displayZoomFactor = frontendConfigs.zoomFactor;
                        gbDisplay.SetScreenStandard();

                        m_emuGB = new EmuCores.GB.EmuGB(configGB);
                    }


                    void Update( )
                    {
                        //TODO: display test
                        Color[] displayPixels = gbDisplay.Pixels;
                        for (int i = 0; i < 144; ++i)
                        {
                            for (int j = 0; j < 160; ++j)
                            {
                                displayPixels[(i * 512) + j] = frontendConfigs.color0;
                            }
                        }

                        gbDisplay.DrawDisplay(displayPixels);
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
