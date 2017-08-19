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
                    public GBInput gbInput;

                    PlatformSupport.IPlatform m_platform;
                    EmuCores.GB.EmuGB m_emuGB;

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

                        gbInput.Init();

                        EmuCores.GB.ConfigsGB configsGB = LoadConfigFile();

                        SetDefaultInputMappings(configsGB, gbInput);
                        ApplyCustomInputMapping(configsGB, gbInput);

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
                        m_emuGB.GetKeysState = gbInput.GetKeysState;
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

                            if (!m_emuGB.LoadSimpleRom(romData))
                            {
                                throw new System.ArgumentException("MBC mappers are not supported yet.\nPlease, be patient :)");
                            }
                        }
                        catch (System.Exception e)
                        {
                            EmuEnvironment.ShowErrorBox("GB Emu Error", "Failed loading rom:\n" + e.Message);
                        }
                    }


                    void Update( )
                    {
                        m_platform.UpdateState();
                        gbInput.UpdateState();

                        m_emuGB.EmulateFrame();
                        lcdDisplay.Render();

                        

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


                    void SetDefaultInputMappings(EmuCores.GB.ConfigsGB aConf, GBInput aGBInput)
                    {
                        var inputs = aGBInput.GetInputList();

                        if (aConf.inputProfiles[0].inputType == "none")
                        {
                            if (inputs[0].GetPlatformInput() is PlatformSupport.KeyboardInput)
                            {
                                aConf.inputProfiles[0].inputType = "keyboard";
                            }

                    #if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
                            else if (inputs[0].GetPlatformInput() is PlatformSupport.XInputController)
                            {
                                aConf.inputProfiles[0].inputType = "xinput";
                            }
                    #endif

                            else
                            {
                                aConf.inputProfiles[0].inputType = "joystick";
                            }

                            aConf.inputProfiles[0].buttonA = inputs[0].GetMappedCode(GBInput.GBButtons.A).ToString();
                            aConf.inputProfiles[0].buttonB = inputs[0].GetMappedCode(GBInput.GBButtons.B).ToString();
                            aConf.inputProfiles[0].buttonSelect = inputs[0].GetMappedCode(GBInput.GBButtons.Select).ToString();
                            aConf.inputProfiles[0].buttonStart = inputs[0].GetMappedCode(GBInput.GBButtons.Start).ToString();
                            aConf.inputProfiles[0].buttonDPadUp = inputs[0].GetMappedCode(GBInput.GBButtons.DPadUp).ToString();
                            aConf.inputProfiles[0].buttonDPadDown = inputs[0].GetMappedCode(GBInput.GBButtons.DPadDown).ToString();
                            aConf.inputProfiles[0].buttonDPadLeft = inputs[0].GetMappedCode(GBInput.GBButtons.DPadLeft).ToString();
                            aConf.inputProfiles[0].buttonDPadRight = inputs[0].GetMappedCode(GBInput.GBButtons.DPadRight).ToString();
                        }
                    }


                    void ApplyCustomInputMapping(EmuCores.GB.ConfigsGB aConf, GBInput aGBInput)
                    {
                        var inputs = aGBInput.GetInputList();

                        if ((inputs[0].GetPlatformInput() is PlatformSupport.KeyboardInput) && (aConf.inputProfiles[0].inputType == "keyboard")
                #if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
                        || (inputs[0].GetPlatformInput() is PlatformSupport.XInputController) && (aConf.inputProfiles[0].inputType == "xinput")
                #endif
                        || (inputs[0].GetPlatformInput() is PlatformSupport.GenericJoystick) && (aConf.inputProfiles[0].inputType == "joystick"))
                        {
                            inputs[0].SetMappedCode(GBInput.GBButtons.A, int.Parse(aConf.inputProfiles[0].buttonA));
                            inputs[0].SetMappedCode(GBInput.GBButtons.B, int.Parse(aConf.inputProfiles[0].buttonB));
                            inputs[0].SetMappedCode(GBInput.GBButtons.Select, int.Parse(aConf.inputProfiles[0].buttonSelect));
                            inputs[0].SetMappedCode(GBInput.GBButtons.Start, int.Parse(aConf.inputProfiles[0].buttonStart));
                            inputs[0].SetMappedCode(GBInput.GBButtons.DPadUp, int.Parse(aConf.inputProfiles[0].buttonDPadUp));
                            inputs[0].SetMappedCode(GBInput.GBButtons.DPadDown, int.Parse(aConf.inputProfiles[0].buttonDPadDown));
                            inputs[0].SetMappedCode(GBInput.GBButtons.DPadLeft, int.Parse(aConf.inputProfiles[0].buttonDPadLeft));
                            inputs[0].SetMappedCode(GBInput.GBButtons.DPadRight, int.Parse(aConf.inputProfiles[0].buttonDPadRight));
                        }
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
