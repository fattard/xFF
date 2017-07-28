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


                    void Start()
                    {
                        EmuCores.BytePusher.ConfigsBytePusher configsBytePusher = LoadConfigFile();

                        // Running direct from scene
                        if (EmuEnvironment.EmuCore == EmuEnvironment.Cores._Unknown)
                        {
                            EmuEnvironment.RomFilePath = frontendConfigs.romsPath[frontendConfigs.selectedRomIndex];

                            OverrideConfigsWithFrontend(configsBytePusher);
                        }


                        emuDisplay.displayWidth = 256;
                        emuDisplay.displayHeight = 256;
                        emuDisplay.displayZoomFactor = configsBytePusher.graphics.displayZoom;
                        emuDisplay.SetScreenStandard();

                        m_emuBytePusher = new EmuCores.BytePusher.EmuBytePusher(configsBytePusher);
                        m_emuBytePusher.DrawDisplay = DisplayRenderer;

                        try
                        {
                            byte[] romData = System.IO.File.ReadAllBytes(EmuEnvironment.RomFilePath);

                            if (!m_emuBytePusher.LoadRom(romData))
                            {
                                throw new System.ArgumentException("Invalid ROM format");
                            }
                        }
                        catch (System.Exception e)
                        {
                            Debug.LogError("Failed loading rom: " + e.Message);
                        }

                        SaveConfigFile(configsBytePusher);
                    }


                    void Update()
                    {
                        m_emuBytePusher.EmulateFrame();
                    }


                    void DisplayRenderer(byte[] aVRAM, int aStartOffset)
                    {
                        int texWidth = emuDisplay.TextureWidth;

                        Color[] displayPixels = emuDisplay.Pixels;
                        for (int i = 0; i < 256; ++i)
                        {
                            for (int j = 0; j < 256; ++j)
                            {
                                byte c = aVRAM[aStartOffset + ((i * 256) + j)];
                                if (c >= 216)
                                {
                                    displayPixels[(i * texWidth) + j] = Color.black;
                                }
                                else
                                {
                                    int blue = c % 6;
                                    int green = ((c - blue) / 6) % 6;
                                    int red = ((c - blue - (6 * green)) / 36) % 6;

                                    displayPixels[(i * texWidth) + j] = new Color((red * 0x33) / 255.0f, (green * 0x33) / 255.0f, (blue * 0x33) / 255.0f);
                                }

                            }
                        }

                        emuDisplay.DrawDisplay(displayPixels);
                    }



                    EmuCores.BytePusher.ConfigsBytePusher LoadConfigFile()
                    {
                        try
                        {
                            string confFile = System.IO.File.ReadAllText(Application.dataPath + "/../core_BytePusher.cfg");

                            EmuCores.BytePusher.ConfigsBytePusher confData = JsonUtility.FromJson<EmuCores.BytePusher.ConfigsBytePusher>(confFile);

                            return confData;
                        }
                        catch (System.Exception e)
                        {
                            return new EmuCores.BytePusher.ConfigsBytePusher();
                        }
                    }


                    void SaveConfigFile(EmuCores.BytePusher.ConfigsBytePusher aConfigs)
                    {
                        try
                        {
                            string confFile = JsonUtility.ToJson(aConfigs, prettyPrint: true);

                            System.IO.File.WriteAllText(Application.dataPath + "/../core_BytePusher.cfg", confFile);
                        }
                        catch (System.Exception e)
                        {
                            Debug.LogError(e.Message);
                        }
                    }


                    void OverrideConfigsWithFrontend(EmuCores.BytePusher.ConfigsBytePusher aConf)
                    {
                        aConf.graphics.displayZoom = frontendConfigs.zoomFactor;
                    }



                    public static void ConfigScene( )
                    {
                        GameObject go = Resources.Load<GameObject>("EmuBytePusher");
                        Instantiate(go);
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
