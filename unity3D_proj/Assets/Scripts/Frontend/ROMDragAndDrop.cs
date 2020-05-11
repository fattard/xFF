/*
*   This file is part of xFF
*   Copyright (C) 2020 Fabio Attard
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

using System.Collections.Generic;
using UnityEngine;
using B83.Win32;


namespace xFF.Frontend.Unity3D
{

    public class ROMDragAndDrop : MonoBehaviour
    {
        List<string> log = new List<string>();
        void OnEnable()
        {
            // must be installed on the main thread to get the right thread id.
            UnityDragAndDropHook.InstallHook();
            UnityDragAndDropHook.OnDroppedFiles += OnFiles;
        }
        void OnDisable()
        {
            UnityDragAndDropHook.UninstallHook();
        }

        void OnFiles(List<string> aFiles, POINT aPos)
        {
            string newFile = aFiles[0];

            xFF.Launch.s_draggedROMPath = aFiles[0];

            if (EmuCores.GB.EmuGB.IsValidROM(newFile))
            {
                EmuEnvironment.EmuCore = EmuEnvironment.Cores._Unknown;
                UnityEngine.SceneManagement.SceneManager.LoadScene("Launch");
            }

            if (EmuCores.BytePusher.EmuBytePusher.IsValidROM(newFile))
            {
                EmuEnvironment.EmuCore = EmuEnvironment.Cores._Unknown;
                UnityEngine.SceneManagement.SceneManager.LoadScene("Launch");
            }
        }

    }
}
// namespace xFF.Frontend.Unity3D
