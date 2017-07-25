﻿/*
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
using xFF.Frontend.Unity3D.BytePusher;

namespace xFF
{
    namespace Frontend
    {
        namespace Unity3D
        {
            namespace BytePusher
            {


                public class FrontendBytePusher : MonoBehaviour
                {
                    public ConfigsBytePusher frontendConfigs;
                    public SingleDisplay emuDisplay;

                    EmuCores.BytePusher.EmuBytePusher m_emuBytePusher;


                    void Start( )
                    {
                        EmuCores.BytePusher.ConfigsBytePusher configsBytePusher = new EmuCores.BytePusher.ConfigsBytePusher();

                        
                        emuDisplay.displayWidth = 256;
                        emuDisplay.displayHeight = 256;
                        emuDisplay.displayZoomFactor = frontendConfigs.zoomFactor;
                        emuDisplay.SetScreenStandard();

                        m_emuBytePusher = new EmuCores.BytePusher.EmuBytePusher(configsBytePusher);
                    }


                    void Update( )
                    {
                        m_emuBytePusher.EmulateFrame();

                        //TODO: display test
                        Color[] displayPixels = emuDisplay.Pixels;
                        for (int i = 0; i < 256; ++i)
                        {
                            for (int j = 0; j < 256; ++j)
                            {
                                displayPixels[(i * 512) + j] = Color.black;
                            }
                        }

                        emuDisplay.DrawDisplay(displayPixels);
                    }
                }


            }
            // namespace BytePusher
        }
        // namespace Unity3D
    }
    // namespace Frontend
}
// namespace xFF