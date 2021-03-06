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

namespace xFF
{
    namespace EmuCores
    {
        namespace GB
        {


            [System.Serializable]
            public class ConfigsGB
            {
                [System.Serializable]
                public class Paths
                {
                    public string romsPath = string.Empty;
                    public string savesPath = string.Empty;
                    public string statesPath = string.Empty;
                }


                [System.Serializable]
                public class BootROM
                {
                    public bool customEnabled = false;
                    public string path = string.Empty;
                    public int internalAnimType = 3;
                }


                [System.Serializable]
                public class Audio
                {
                    public bool soundChannel_1 = true;
                    public bool soundChannel_2 = true;
                    public bool soundChannel_3 = true;
                    public bool soundChannel_4 = true;
                }


                [System.Serializable]
                public class Graphics
                {
                    public int displayZoom = 2;
                }


                [System.Serializable]
                public class DMGColors
                {
                    [System.Serializable]
                    public class Color
                    {
                        public int r = 255;
                        public int g = 255;
                        public int b = 255;

                        public Color( ) { }


                        public Color(int aR, int aG, int aB)
                        {
                            Set(aR, aG, aB);
                        }


                        public void Set(int aR, int aG, int aB)
                        {
                            // Clamp between 0 and 255

                            r = System.Math.Max(0, System.Math.Min(aR, 255));
                            g = System.Math.Max(0, System.Math.Min(aG, 255));
                            b = System.Math.Max(0, System.Math.Min(aB, 255));
                        }
                    }

                    public Color colorDisabledLCD = new Color(224, 248, 208);
                    public Color color0 = new Color(224, 248, 208);
                    public Color color1 = new Color(136, 192, 112);
                    public Color color2 = new Color(52, 104, 86);
                    public Color color3 = new Color(8, 24, 32);
                }


                [System.Serializable]
                public class InputProfile
                {
                    public string inputType;
                    public string buttonA;
                    public string buttonB;
                    public string buttonSelect;
                    public string buttonStart;
                    public string buttonDPadUp;
                    public string buttonDPadDown;
                    public string buttonDPadLeft;
                    public string buttonDPadRight;

                    public InputProfile()
                    {
                        inputType = "none";

                        buttonA = string.Empty;
                        buttonB = string.Empty;
                        buttonSelect = string.Empty;
                        buttonStart = string.Empty;
                        buttonDPadUp = string.Empty;
                        buttonDPadDown = string.Empty;
                        buttonDPadLeft = string.Empty;
                        buttonDPadRight = string.Empty;
                    }
                }



                #region Data

                public Paths paths;
                public BootROM bootRomDMG;
                public BootROM bootRomSGB;
                public BootROM bootRomCGB;
                public Audio audio;
                public Graphics graphics;
                public DMGColors dmgColors;
                public InputProfile[] inputProfiles;
                public int inputProfileActiveIndex;
                public string emulatedSystem;
                public string linkPortDevice;

                #endregion Data


                public ConfigsGB( )
                {
                    paths = new Paths();
                    bootRomDMG = new BootROM();
                    bootRomSGB = new BootROM();
                    bootRomCGB = new BootROM();
                    audio = new Audio();
                    graphics = new Graphics();
                    dmgColors = new DMGColors();
                    inputProfiles = new InputProfile[4];
                    emulatedSystem = Defs.HardwareModel.DMG.ToString();
                    linkPortDevice = Defs.LinkPortDevices.None.ToString();

                    for (int i = 0; i < inputProfiles.Length; ++i)
                    {
                        inputProfiles[i] = new InputProfile();
                    }
                }
            }


        }
        // namespace GB
    }
    // namespace EmuCores
}
// namespace xFF
