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

namespace xFF
{

    public class Launch : MonoBehaviour
    {
        public string[] editorArgs;

        bool invalidArgs;

        void Awake( )
        {
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = 60;

            Screen.SetResolution(640, 480, false);
        }


        void Start( )
        {
            string[] args = System.Environment.GetCommandLineArgs();

            if (Application.isEditor)
            {
                args = editorArgs;
            }

            
            string input = "";
            for (int i = 0; i < args.Length; i++)
            {
                Debug.Log("ARG " + i + ": " + args[i]);
                /*if (args [i] == "-folderInput")
                {
                    input = args [i + 1];
                }*/

                if (args[i].ToLower().StartsWith("-core="))
                {
                    string[] coreArg = args[i].Split('=');
                    if (coreArg.Length == 2)
                    {
                        switch (coreArg[1])
                        {
                            case "bytepusher":
                                EmuEnvironment.EmuCore = EmuEnvironment.Cores.BytePusher;
                                break;

                            case "gb":
                                EmuEnvironment.EmuCore = EmuEnvironment.Cores.GB;
                                break;
                        }
                    }
                }

                // Last arg is ROMFilePath
                else if ((i + 1 == args.Length) && !args[i].EndsWith(".exe"))
                {
                    EmuEnvironment.RomFilePath = args[i];

                    if (EmuEnvironment.EmuCore == EmuEnvironment.Cores._Unknown)
                    {
                        if (EmuCores.GB.EmuGB.IsValidROM(EmuEnvironment.RomFilePath))
                        {
                            EmuEnvironment.EmuCore = EmuEnvironment.Cores.GB;
                        }

                        if (EmuCores.BytePusher.EmuBytePusher.IsValidROM(EmuEnvironment.RomFilePath))
                        {
                            EmuEnvironment.EmuCore = EmuEnvironment.Cores.BytePusher;
                        }
                    }
                }
                
            }


            if (EmuEnvironment.EmuCore != EmuEnvironment.Cores._Unknown)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("EmuRuntime");
            }

            else
            {
                invalidArgs = true;
            }
        }
    }
}
// namespace xFF
