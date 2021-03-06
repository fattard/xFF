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
                    public LCDDisplay lcdDisplay;
                    public DSP dsp;
                    

                    PlatformSupport.IPlatform m_platform;
                    EmuCores.GB.EmuGB m_emuGB;
                    GBInput m_gbInput;

                    float lastUpdateTick;


                    void Awake( )
                    {
                        m_platform = PlatformSupport.PlatformFactory.GetPlatform();
                    }


                    IEnumerator Start( )
                    {
                        this.enabled = false;
                        while (!m_platform.IsInited)
                        {
                            yield return null;
                        }
                        this.enabled = true;

                        EmuCores.GB.ConfigsGB configsGB = LoadConfigFile();

                        // Find preferred input
                        m_gbInput = GBInput.BuildInput(configsGB.inputProfiles[configsGB.inputProfileActiveIndex], m_platform.GetConnectedInputs());
                            

                        // Running direct from scene
                        if (EmuEnvironment.EmuCore == EmuEnvironment.Cores._Unknown)
                        {
                            EmuEnvironment.RomFilePath = frontendConfigs.romsPath[frontendConfigs.selectedRomIndex];

                            OverrideConfigsWithFrontend(configsGB);
                        }

                        lcdDisplay.SetConfigs(configsGB);

                        m_emuGB = new EmuCores.GB.EmuGB(configsGB);
                        m_emuGB.DrawDisplay = lcdDisplay.DrawDisplay;
                        m_emuGB.DrawDisplayLine = lcdDisplay.DrawDisplayLine;
                        m_emuGB.PlayAudio = dsp.PlayAudio;
                        dsp.ConfigBuffers(m_emuGB.APU);
                        dsp.channel1Enabled = configsGB.audio.soundChannel_1;
                        dsp.channel2Enabled = configsGB.audio.soundChannel_2;
                        dsp.channel3Enabled = configsGB.audio.soundChannel_3;
                        dsp.channel4Enabled = configsGB.audio.soundChannel_4;
                        m_emuGB.GetKeysState = m_gbInput.GetKeysState;
                        //m_emuGB.PlayAudio = PlayAudio;
                        //m_emuGB.UpdateInputKeys = UpdateKeys;

                        m_emuGB.BindLogger(Debug.Log, Debug.LogWarning, Debug.LogError);

                        LoadBootRom();

                        if (!string.IsNullOrEmpty(EmuEnvironment.RomFilePath))
                        {
                            LoadROM();
                        }
                        
                        SaveConfigFile(configsGB);

                        lastUpdateTick = Time.realtimeSinceStartup;

                        //m_emuGB.CPU.UserCyclesRate = 500;

                        m_emuGB.PowerOn();
                    }


                    private void OnDestroy()
                    {
                        if (EmuEnvironment.RomFilePath.EndsWith(".gbc"))
                        {
                            m_emuGB.MEM.Cartridge.SaveRAM(EmuEnvironment.RomFilePath.Replace(".gbc", ".sav"));
                        }

                        else if (EmuEnvironment.RomFilePath.EndsWith(".gb"))
                        {
                            m_emuGB.MEM.Cartridge.SaveRAM(EmuEnvironment.RomFilePath.Replace(".gb", ".sav"));
                        }

                        // Just append .sav to whatever name is
                        else
                        {
                            m_emuGB.MEM.Cartridge.SaveRAM(EmuEnvironment.RomFilePath + ".sav");
                        }
                        
                    }


                    void LoadBootRom( )
                    {
                        // Use internal boot rom
                        if (!m_emuGB.Configs.bootRomDMG.customEnabled || string.IsNullOrEmpty(m_emuGB.Configs.bootRomDMG.path))
                        {
                            byte[] devBootROM = Resources.Load<TextAsset>("GB/DMG_CustomBootRom").bytes;

                            // Patch Anim Type
                            const int kAnimTypeOffset = 0x00FD;
                            switch (m_emuGB.Configs.bootRomDMG.internalAnimType)
                            {
                                case 0x03: // Quick Anim
                                    devBootROM[kAnimTypeOffset] = 0x03;
                                    break;

                                case 0xAA: // Full Anim
                                    devBootROM[kAnimTypeOffset] = 0xAA;
                                    break;

                                default: // No Anim
                                    devBootROM[kAnimTypeOffset] = 0x00;
                                    break;
                            }

                            m_emuGB.SetBootRom(devBootROM);
                        }

                        else
                        {
                            try
                            {
                                byte[] bootROM = System.IO.File.ReadAllBytes(m_emuGB.Configs.bootRomDMG.path);

                                if (bootROM.Length != 0x100) // 256 bytes
                                {
                                    throw new System.ArgumentException("Invalid BootROM format. Expected " + 0x100 + " bytes.\nFound " + bootROM.Length + ". File: " + m_emuGB.Configs.bootRomDMG.path + "\nThe internal BootROM will be used.");
                                }

                                m_emuGB.SetBootRom(bootROM);
                            }
                            catch (System.Exception e)
                            {
                                EmuEnvironment.ShowErrorBox("GB Emu Error", "BootROM error:\n" + e.Message);

                                // Fallback to internal boot rom
                                byte[] devBootROM = Resources.Load<TextAsset>("GB/DMG_CustomBootRom").bytes;

                                m_emuGB.SetBootRom(devBootROM);
                            }
                        }
                    }


                    void LoadROM()
                    {
                        try
                        {
                            string path = EmuEnvironment.RomFilePath;

                            byte[] romData = System.IO.File.ReadAllBytes(path);

                            EmuCores.GB.HW.CartridgeHeader cartridgeHeader = new EmuCores.GB.HW.CartridgeHeader(romData);

                            EmuCores.GB.HW.Cartridge cart = EmuCores.GB.HW.Cartridge.Mount(cartridgeHeader, romData);

                            m_emuGB.LoadCart(cart);
                        }
                        catch (System.Exception e)
                        {
                            EmuEnvironment.ShowErrorBox("GB Emu Error", "Failed loading rom:\n" + e.Message);
                            m_emuGB.LoadNoCart();
                        }
                    }


                    void Update( )
                    {
                        m_platform.UpdateState();
                        m_gbInput.UpdateState();

                        m_emuGB.EmulateFrame();
                        lcdDisplay.Render();

                        // Chack for screenshot key
                        /*if (Input.GetKeyDown(KeyCode.F12))
                        {
                            SaveScreenshot();
                        }*/


                    #if UNITY_EDITOR
                        // Prevent DBG unresponsive loops
                        float newTime = Time.realtimeSinceStartup;
                        if ((newTime - lastUpdateTick) > 5)
                        {
                            Debug.LogWarning("Breaking due to long loop last time.");
                            Debug.Break();
                        }
                        lastUpdateTick = newTime;


                        // Realtime color change
                        OverrideConfigsColors(m_emuGB.Configs);
                        lcdDisplay.SetLCDColors(m_emuGB.Configs);
                    #endif
                    }


                    EmuCores.GB.ConfigsGB LoadConfigFile( )
                    {
                        try
                        {
                            if (!System.IO.File.Exists(Application.dataPath + "/../core_GB.cfg"))
                            {
                                return new EmuCores.GB.ConfigsGB();
                            }

                            string confFile = System.IO.File.ReadAllText(Application.dataPath + "/../core_GB.cfg");

                            EmuCores.GB.ConfigsGB confData = JsonUtility.FromJson<EmuCores.GB.ConfigsGB>(confFile);

                            return confData;
                        }
                        catch (System.Exception e)
                        {
                            EmuEnvironment.ShowErrorBox("GB Emu Error", "Invalid data in config file \"core_gb.cfg\"\n\n" + e.Message);
                            if (!Application.isEditor)
                            {
                                Application.Quit();
                                return null;
                            }

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
                            EmuEnvironment.ShowErrorBox("GB Emu Error", e.Message);
                        }
                    }


                    void SaveScreenshot( )
                    {
                        string timestamp = "_" + System.DateTime.Now.ToFileTime() + ".png";

                        if (EmuEnvironment.RomFilePath.EndsWith(".gbc"))
                        {
                            lcdDisplay.TakeScreenshot(EmuEnvironment.RomFilePath.Replace(".gbc", timestamp));
                        }

                        else if (EmuEnvironment.RomFilePath.EndsWith(".gb"))
                        {
                            lcdDisplay.TakeScreenshot(EmuEnvironment.RomFilePath.Replace(".gb", timestamp));
                        }

                        // Just append timestamp to whatever name is
                        else
                        {
                            lcdDisplay.TakeScreenshot(EmuEnvironment.RomFilePath + timestamp);
                        }
                        
                    }


                    void OverrideConfigsWithFrontend(EmuCores.GB.ConfigsGB aConf)
                    {
                        // Graphics
                        aConf.graphics.displayZoom = frontendConfigs.zoomFactor;

                        // Bootrom
                        aConf.bootRomDMG.customEnabled = (frontendConfigs.bootRomMode == ConfigsGB.BootRomMode.ExternalFile);
                        aConf.bootRomDMG.path = frontendConfigs.bootRomPath;
                        aConf.bootRomDMG.internalAnimType = (int)frontendConfigs.bootRomMode;

                        // DMG Colors
                        OverrideConfigsColors(aConf);

                        // Audio
                        aConf.audio.soundChannel_1 = dsp.channel1Enabled;
                        aConf.audio.soundChannel_2 = dsp.channel2Enabled;
                        aConf.audio.soundChannel_3 = dsp.channel3Enabled;
                        aConf.audio.soundChannel_4 = dsp.channel4Enabled;
                    }


                    void OverrideConfigsColors(EmuCores.GB.ConfigsGB aConf)
                    {
                        // DMG Colors
                        aConf.dmgColors.color0.Set((int)(255 * frontendConfigs.color0.r), (int)(255 * frontendConfigs.color0.g), (int)(255 * frontendConfigs.color0.b));
                        aConf.dmgColors.color1.Set((int)(255 * frontendConfigs.color1.r), (int)(255 * frontendConfigs.color1.g), (int)(255 * frontendConfigs.color1.b));
                        aConf.dmgColors.color2.Set((int)(255 * frontendConfigs.color2.r), (int)(255 * frontendConfigs.color2.g), (int)(255 * frontendConfigs.color2.b));
                        aConf.dmgColors.color3.Set((int)(255 * frontendConfigs.color3.r), (int)(255 * frontendConfigs.color3.g), (int)(255 * frontendConfigs.color3.b));
                        aConf.dmgColors.colorDisabledLCD.Set((int)(255 * frontendConfigs.color0.r), (int)(255 * frontendConfigs.color0.g), (int)(255 * frontendConfigs.color0.b));
                    }


                    public static void ConfigScene()
                    {
                        PlatformSupport.PlatformFactory.CreatePlatform();

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
