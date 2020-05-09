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
using xFF.Frontend.Unity3D.BytePusher;
using xFF.Frontend.Unity3D.GB;
//using System.Windows.Forms;

namespace xFF
{


    public class EmuEnvironment : MonoBehaviour
    {
        public enum Cores
        {
            _Unknown,

            BytePusher,
            GB,           
        }


        public static Cores EmuCore
        {
            get;
            set;
        }


        public static string RomFilePath
        {
            get;
            set;
        }



        static EmuEnvironment( )
        {
            EmuCore = Cores._Unknown;
            RomFilePath = string.Empty;
        }



        void Awake( )
        {
            switch (EmuCore)
            {
                case Cores.BytePusher:
                    FrontendBytePusher.ConfigScene();
                    break;

                case Cores.GB:
                    FrontendGB.ConfigScene();
                    break;
            }
        }


        public static void ShowErrorBox(string aTitle, string aMsg)
        {
            if (UnityEngine.Application.isEditor)
            {
                Debug.LogError(aMsg);
            }

            else
            {
                Debug.LogError(aMsg);
                //MessageBox.Show(aMsg, aTitle, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }


        public static void ShowWarningBox(string aTitle, string aMsg)
        {
            if (UnityEngine.Application.isEditor)
            {
                Debug.LogWarning(aMsg);
            }

            else
            {
                Debug.LogWarning(aMsg);
                //MessageBox.Show(aMsg, aTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            }
        }
    }


}
// namespace xFF
