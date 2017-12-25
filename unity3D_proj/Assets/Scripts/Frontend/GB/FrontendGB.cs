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

                            if (EmuCores.GB.HW.MBC.Cartridge_Single.Validate(cartridgeHeader))
                            {
                                if (!m_emuGB.LoadSimpleRom(cartridgeHeader, romData))
                                {
                                    throw new System.ArgumentException("Invalid ROM header info");
                                }
                            }

                            else if (EmuCores.GB.HW.MBC.Cartridge_MBC1.Validate(cartridgeHeader))
                            { 
                                if (!m_emuGB.LoadMBC1Rom(cartridgeHeader, romData))
                                {
                                    throw new System.ArgumentException("Invalid MBC1 ROM header info");
                                }
                            }

                            else if (EmuCores.GB.HW.MBC.Cartridge_MBC3.Validate(cartridgeHeader))
                            {
                                if (!m_emuGB.LoadMBC3Rom(cartridgeHeader, romData))
                                {
                                    throw new System.ArgumentException("Invalid MBC3 ROM header info");
                                }
                            }

                            else if (EmuCores.GB.HW.MBC.Cartridge_MBC5.Validate(cartridgeHeader))
                            {
                                if (!m_emuGB.LoadMBC5Rom(cartridgeHeader, romData))
                                {
                                    throw new System.ArgumentException("Invalid MBC5 ROM header info");
                                }
                            }

                            else
                            {
                                throw new System.ArgumentException("Some MBC mappers are not supported yet.\nPlease, be patient :)");
                            }
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


                    #if UNITY_EDITOR
                        // Prevent DBG unresponsive loops
                        float newTime = Time.realtimeSinceStartup;
                        if ((newTime - lastUpdateTick) > 5)
                        {
                            Debug.LogWarning("Breaking due to long loop last time.");
                            Debug.Break();
                        }
                        lastUpdateTick = newTime;
                    #endif
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
                            EmuEnvironment.ShowErrorBox("GB Emu Error", e.Message);
                        }
                    }


                    void OverrideConfigsWithFrontend(EmuCores.GB.ConfigsGB aConf)
                    {
                        // Graphics
                        aConf.graphics.displayZoom = frontendConfigs.zoomFactor;

                        // Bootrom
                        aConf.bootRomDMG.enabled = frontendConfigs.bootRomEnabled;
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
