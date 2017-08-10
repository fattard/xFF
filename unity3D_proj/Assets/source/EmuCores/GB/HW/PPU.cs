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

using xFF.EmuCores.GB.Defs;

namespace xFF
{
    namespace EmuCores
    {
        namespace GB
        {
            namespace HW
            {


                public class PPU
                {
                    byte[] m_VRAM;

                    uint m_cyclesElapsed;
                    uint m_scanlineTotalCyclesElapsed;

                    int m_lcdc;
                    int m_stat;
                    int m_operationMode;

                    CPU.RequestIRQFunc RequestIRQ;


                    public byte[] VRAM
                    {
                        get { return m_VRAM; }
                    }


                    public int BackgroundPalette
                    {
                        get;
                        set;
                    }


                    public int BGScrollX
                    {
                        get;
                        set;
                    }


                    public int BGScrollY
                    {
                        get;
                        set;
                    }


                    public int CurScanline
                    {
                        get;
                        set;
                    }


                    public int LCDControl
                    {
                        get { return m_lcdc; }
                        set
                        {
                            m_lcdc = (0xFF & value);

                            //BGDisplayOn = (value & RegsIO_Bits.LCDC_BGEN);
                        }
                    }


                    public int LCDControllerStatus
                    {
                        get { return m_stat; }
                        set
                        {
                            m_stat = (0x7F & value);
                        }
                    }


                    public PPU( )
                    {
                        m_VRAM = new byte[0x2000]; // 8 KB
                        m_operationMode = 2;
                        m_scanlineTotalCyclesElapsed = 0;
                        m_cyclesElapsed = 0;

                        // Temp binding
                        RequestIRQ = (aIRQ_flag) => { };
                    }


                    public void CyclesStep(int aElapsedCycles)
                    {
                        m_cyclesElapsed += (uint)aElapsedCycles;
                        m_scanlineTotalCyclesElapsed += (uint)aElapsedCycles;

                        switch (m_operationMode)
                        {
                            case 0: // H-Blank
                                if (m_cyclesElapsed >= 201)
                                {
                                    m_cyclesElapsed = 0;
                                    m_operationMode = 2;
                                }
                                if (m_scanlineTotalCyclesElapsed >= 456)
                                {
                                    CurScanline = (CurScanline + 1) % 153;
                                    m_scanlineTotalCyclesElapsed -= 456;

                                    // Start VBLANK
                                    if (CurScanline == 144)
                                    {
                                        m_operationMode = 1;
                                        m_cyclesElapsed = 0;
                                        RequestIRQ(RegsIO_Bits.IF_VBLANK);
                                    }
                                }
                                break;

                            case 1: // V-Blank
                                if (m_cyclesElapsed >= 456)
                                {
                                    CurScanline = (CurScanline + 1) % 153;
                                    m_cyclesElapsed -= 456;
                                }
                                if (m_scanlineTotalCyclesElapsed >= 4560)
                                {
                                    m_scanlineTotalCyclesElapsed = 0;
                                    m_cyclesElapsed = 0;
                                    m_operationMode = 2;
                                }
                                break;

                            case 2: // Accessing OAM
                                if (m_cyclesElapsed >= 77)
                                {
                                    m_cyclesElapsed = 0;
                                    m_operationMode = 3;
                                }
                                break;

                            case 3: // Accessing VRAM
                                if (m_cyclesElapsed >= 169)
                                {
                                    m_cyclesElapsed = 0;
                                    m_operationMode = 0;
                                }
                                break;
                        }
                    }

                    public void BindRequestIRQ(CPU.RequestIRQFunc aRequestIRQFunc)
                    {
                        RequestIRQ = aRequestIRQFunc;
                    }
                }


            }
            // namespace HW
        }
        // namespace GB
    }
    // namespace EmuCores
}
// namespace xFF
