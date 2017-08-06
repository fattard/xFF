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

                    float lastUpdateTick;


                    void Start( )
                    {
                        EmuCores.GB.ConfigsGB configsGB = LoadConfigFile();

                        // Running direct from scene
                        if (EmuEnvironment.EmuCore == EmuEnvironment.Cores._Unknown)
                        {
                            EmuEnvironment.RomFilePath = frontendConfigs.romsPath[frontendConfigs.selectedRomIndex];

                            OverrideConfigsWithFrontend(configsGB);
                        }

                        gbDisplay.displayWidth = 160;
                        gbDisplay.displayHeight = 144;
                        gbDisplay.displayZoomFactor = configsGB.graphics.displayZoom;
                        gbDisplay.SetScreenStandard();

                        m_emuGB = new EmuCores.GB.EmuGB(configsGB);
                        //m_emuGB.DrawDisplay = DisplayRenderer;
                        //m_emuGB.PlayAudio = PlayAudio;
                        //m_emuGB.UpdateInputKeys = UpdateKeys;

                        LoadBootRom();
                        LoadROM();
                        
                        SaveConfigFile(configsGB);

                        lastUpdateTick = Time.realtimeSinceStartup;
                    }


                    void LoadBootRom( )
                    {
                        // Use internal boot rom
                        if (!m_emuGB.Configs.bootRomDMG.enabled || string.IsNullOrEmpty(m_emuGB.Configs.bootRomDMG.path))
                        {
                            byte[] devBootROM = Resources.Load<TextAsset>("GB/DMG_CustomBootRom").bytes;

                            m_emuGB.SetBootRom(devBootROM);
                        }

                        else
                        {
                            try
                            {
                                byte[] bootROM = System.IO.File.ReadAllBytes(m_emuGB.Configs.bootRomDMG.path);

                                if (bootROM.Length != 0x100) // 256 bytes
                                {
                                    throw new System.ArgumentException("Invalid BootROM format. Expected " + 0x100 + " bytes.\nFound " + bootROM.Length + ". File: " + m_emuGB.Configs.bootRomDMG.path);
                                }

                                m_emuGB.SetBootRom(bootROM);
                            }
                            catch (System.Exception e)
                            {
                                Debug.LogError("BootROM error - " + e.Message);
                            }
                        }
                    }


                    void LoadROM()
                    {
                        try
                        {
                            string path = EmuEnvironment.RomFilePath;

                            byte[] romData = System.IO.File.ReadAllBytes(path);

                            if (!m_emuGB.LoadSimpleRom(romData))
                            {
                                throw new System.ArgumentException("Invalid ROM format");
                            }
                        }
                        catch (System.Exception e)
                        {
                            Debug.LogError("Failed loading rom: " + e.Message);
                        }
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

                        // Prevent DBG unresponsive loops
                        float newTime = Time.realtimeSinceStartup;
                        if ((newTime - lastUpdateTick) > 5)
                        {
                            Debug.LogWarning("Breaking due to long loop last time.");
                            Debug.Break();
                        }
                        lastUpdateTick = newTime;
                    }


                    EmuCores.GB.ConfigsGB LoadConfigFile( )
                    {
                        try
                        {
                            string confFile = System.IO.File.ReadAllText(Application.dataPath + "/../core_GB.cfg");

                            EmuCores.GB.ConfigsGB confData = JsonUtility.FromJson<EmuCores.GB.ConfigsGB>(confFile);

                            return confData;
                        }
                        catch (System.Exception e)
                        {
                            return new EmuCores.GB.ConfigsGB();
                        }
                    }


                    void SaveConfigFile(EmuCores.GB.ConfigsGB aConfigs)
                    {
                        try
                        {
                            string confFile = JsonUtility.ToJson(aConfigs, prettyPrint: true);

                            System.IO.File.WriteAllText(Application.dataPath + "/../core_GB.cfg", confFile);
                        }
                        catch (System.Exception e)
                        {
                            Debug.LogError(e.Message);
                        }
                    }


                    void OverrideConfigsWithFrontend(EmuCores.GB.ConfigsGB aConf)
                    {
                        // Graphics
                        aConf.graphics.displayZoom = frontendConfigs.zoomFactor;

                        // Bootrom
                        aConf.bootRomDMG.enabled = false;
                        aConf.bootRomDMG.path = frontendConfigs.bootRomPath;

                        // DMG Colors
                        aConf.dmgColors.color0.Set((int)(255 * frontendConfigs.color0.r), (int)(255 * frontendConfigs.color0.g), (int)(255 * frontendConfigs.color0.b));
                        aConf.dmgColors.color1.Set((int)(255 * frontendConfigs.color1.r), (int)(255 * frontendConfigs.color1.g), (int)(255 * frontendConfigs.color1.b));
                        aConf.dmgColors.color2.Set((int)(255 * frontendConfigs.color2.r), (int)(255 * frontendConfigs.color2.g), (int)(255 * frontendConfigs.color2.b));
                        aConf.dmgColors.color3.Set((int)(255 * frontendConfigs.color3.r), (int)(255 * frontendConfigs.color3.g), (int)(255 * frontendConfigs.color3.b));
                        aConf.dmgColors.colorDisabledLCD.Set((int)(255 * frontendConfigs.color0.r), (int)(255 * frontendConfigs.color0.g), (int)(255 * frontendConfigs.color0.b));
                    }


                    public static void ConfigScene()
                    {
                        GameObject go = Resources.Load<GameObject>("EmuGB");
                        Instantiate(go);
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
