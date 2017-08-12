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
using xFF.EmuCores.GB.HW;

namespace xFF
{
    namespace Frontend
    {
        namespace Unity3D
        {
            namespace GB
            {

                public class LCDDisplay : MonoBehaviour
                {
                    public SingleDisplay gbDisplay;

                    Color[] m_LCDColor = new Color[4];
                    Color m_disabledLCDColor;


                    public void SetConfigs(EmuCores.GB.ConfigsGB aConfigs)
                    {
                        gbDisplay.displayWidth = 160;
                        gbDisplay.displayHeight = 144;
                        gbDisplay.displayZoomFactor = aConfigs.graphics.displayZoom;
                        gbDisplay.SetScreenStandard();

                        SetLCDColors(aConfigs);
                    }


                    public void SetLCDColors(EmuCores.GB.ConfigsGB aConfigs)
                    {
                        m_LCDColor[0] = new Color(aConfigs.dmgColors.color0.r / 255.0f, aConfigs.dmgColors.color0.g / 255.0f, aConfigs.dmgColors.color0.b / 255.0f);
                        m_LCDColor[1] = new Color(aConfigs.dmgColors.color1.r / 255.0f, aConfigs.dmgColors.color1.g / 255.0f, aConfigs.dmgColors.color1.b / 255.0f);
                        m_LCDColor[2] = new Color(aConfigs.dmgColors.color2.r / 255.0f, aConfigs.dmgColors.color2.g / 255.0f, aConfigs.dmgColors.color2.b / 255.0f);
                        m_LCDColor[3] = new Color(aConfigs.dmgColors.color3.r / 255.0f, aConfigs.dmgColors.color3.g / 255.0f, aConfigs.dmgColors.color3.b / 255.0f);

                        m_disabledLCDColor = new Color(aConfigs.dmgColors.colorDisabledLCD.r / 255.0f, aConfigs.dmgColors.colorDisabledLCD.g / 255.0f, aConfigs.dmgColors.colorDisabledLCD.b / 255.0f);
                    }


                    public void Render( )
                    {
                        gbDisplay.DrawDisplay(gbDisplay.Pixels);
                    }


                    public void DrawDisplay(PPU aPPU)
                    {
                        Color[] displayPixels = gbDisplay.Pixels;
                        byte[] vram = aPPU.VRAM;
                        int texWid = gbDisplay.TextureWidth;
                        int scrollY = aPPU.BGScrollY;
                        int scrollX = aPPU.BGScrollX;

                        int aux = 0;

                        int palData = aPPU.BackgroundPalette;

                        int mapDataOffset = ((aPPU.LCDControl & (1 << 3)) > 0) ? 0x1C00 : 0x1800;
                        int tileDataOffset = ((aPPU.LCDControl & (1 << 4)) > 0) ? 0x0000 : 0x0800;


                        for (int i = 0; i < 144; ++i)
                        {
                            for (int j = 0; j < 160; ++j)
                            {
                                int yPos = ((i + scrollY) % 256);
                                int xPos = ((j + scrollX) % 256);

                                aux = mapDataOffset + ((yPos / 8) * 32) + (xPos / 8);


                                int tileIdx = tileDataOffset;
                                if (tileDataOffset > 0)
                                {
                                    tileIdx += ((128 + ((sbyte)vram[aux])) * 16);
                                }
                                else
                                {
                                    tileIdx += (vram[aux] * 16);
                                }


                                int lineDataL = vram[tileIdx + (2 * (yPos % 8))];
                                int lineDataH = vram[tileIdx + (2 * (yPos % 8)) + 1];
                                int colData = 1 << (7 - (xPos % 8));
                                int palIdx = (((lineDataL & colData) > 0) ? 1 : 0) + (((lineDataH & colData) > 0) ? 2 : 0);

                                int colorIdx = 0;
                                int hi = 0;
                                int lo = 0;

                                // which bits of the colour palette does the colour id map to?
                                switch (palIdx)
                                {
                                    case 0: hi = 1; lo = 0; break;
                                    case 1: hi = 3; lo = 2; break;
                                    case 2: hi = 5; lo = 4; break;
                                    case 3: hi = 7; lo = 6; break;
                                }

                                // use the palette to get the colour
                                int color = 0;
                                color = ((palData >> hi) & 0x1) << 1;
                                color |= ((palData >> lo) & 0x1);

                                displayPixels[(i * texWid) + j] = m_LCDColor[color];
                            }
                        }
                    }


                    public void DrawTilemap(PPU aPPU)
                    {
                        Color[] displayPixels = gbDisplay.Pixels;
                        byte[] vram = aPPU.VRAM;
                        int texWid = gbDisplay.TextureWidth;

                        int aux = 0;

                        int palData = aPPU.BackgroundPalette;

                        int mapDataOffset = ((aPPU.LCDControl & (1 << 3)) > 0) ? 0x1C00 : 0x1800;
                        int tileDataOffset = ((aPPU.LCDControl & (1 << 4)) > 0) ? 0x0000 : 0x0800;


                        for (int i = 0; i < 256; ++i)
                        {
                            for (int j = 0; j < 256; ++j)
                            {
                                int yPos = ((i));
                                int xPos = ((j));

                                aux = mapDataOffset + ((yPos / 8) * 32) + (xPos / 8);

                                int tileIdx = tileDataOffset;
                                if (tileDataOffset > 0)
                                {
                                    tileIdx += ((128 + ((sbyte)vram[aux])) * 16);
                                }
                                else
                                {
                                    tileIdx += (vram[aux] * 16);
                                }
                                


                                int lineDataL = vram[tileIdx + (2 * (yPos % 8))];
                                int lineDataH = vram[tileIdx + (2 * (yPos % 8)) + 1];
                                int colData = 1 << (7 - (j % 8));
                                int palIdx = (((lineDataL & colData) > 0) ? 1 : 0) + (((lineDataH & colData) > 0) ? 2 : 0);

                                int colorIdx = 0;
                                int hi = 0;
                                int lo = 0;

                                // which bits of the colour palette does the colour id map to?
                                switch (palIdx)
                                {
                                    case 0: hi = 1; lo = 0; break;
                                    case 1: hi = 3; lo = 2; break;
                                    case 2: hi = 5; lo = 4; break;
                                    case 3: hi = 7; lo = 6; break;
                                }

                                // use the palette to get the colour
                                int color = 0;
                                color = ((palData >> hi) & 0x1) << 1;
                                color |= ((palData >> lo) & 0x1);

                                displayPixels[(i * texWid) + j] = m_LCDColor[color];
                            }
                        }
                    }


                    public void DrawTileset(PPU aPPU)
                    {
                        Color[] displayPixels = gbDisplay.Pixels;
                        byte[] vram = aPPU.VRAM;
                        int texWid = gbDisplay.TextureWidth;

                        int aux = 0;


                        for (int i = 0; i < 144; ++i)
                        {
                            for (int j = 0; j < 160; ++j)
                            {
                                aux = ((i / 8) * 20) + (j / 8);


                                int tileIdx = (16 * aux);

                                int lineDataL = vram[tileIdx + (2 * (i % 8))];
                                int lineDataH = vram[tileIdx + (2 * (i % 8)) + 1];
                                int colData = 1 << (7 - (j % 8));
                                int palIdx = (((lineDataL & colData) > 0) ? 1 : 0) + (((lineDataH & colData) > 0) ? 2 : 0);

                                displayPixels[(i * texWid) + j] = m_LCDColor[palIdx];
                            }
                        }
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
