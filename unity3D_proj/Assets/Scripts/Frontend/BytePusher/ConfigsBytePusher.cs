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
using xFF.Frontend.Unity3D.BytePusher;

namespace xFF
{
    namespace Frontend
    {
        namespace Unity3D
        {
            namespace BytePusher
            {


                public class ConfigsBytePusher : MonoBehaviour
                {
                    [Header("Quick ROMS Selector")]
                    public int selectedRomIndex;
                    public string[] romsPath = new string[1];

                    [Header("Zoom Factor")]
                    public int zoomFactor = 1;
                }


            }
            // namespace BytePusher
        }
        // namespace Unity3D
    }
    // namespace Frontend
}
// namespace xFF
