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
using xFF.Frontend.Unity3D.GB;

namespace xFF
{
    namespace Frontend
    {
        namespace Unity3D
        {
            namespace GB
            {


                public class ConfigsGB : MonoBehaviour
                {
                    public enum BootRomMode
                    {
                        InternalNoAnim = 0x00,
                        InternalQuickAnim = 0x03,
                        InternalFullAnim = 0xAA,

                        ExternalFile = -1,
                    }

                    [Header("DMG Bootrom")]
                    public BootRomMode bootRomMode = BootRomMode.InternalNoAnim;
                    public string bootRomPath = "";

                    [Header("Quick ROMS Selector")]
                    public int selectedRomIndex;
                    public string[] romsPath = new string[1];

                    [Header("DMG Display Colors")]
                    public Color color0;
                    public Color color1;
                    public Color color2;
                    public Color color3;

                    [Header("Zoom Factor")]
                    public int zoomFactor = 1;
                }


            }
            // namespace GB
        }
        // namespace Unity3D
    }
    // namespace Frontend
}
// namespace xFF
