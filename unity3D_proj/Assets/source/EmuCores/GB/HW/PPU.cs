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


                /// <summary>
                /// Wrapper class to Reg LCDC
                /// </summary>
                public class LCDControlInfo
                {
                    public bool IsLCDEnabled { get; private set; }
                    public bool IsWindowEnabled { get; private set; }
                    public bool IsSpritesEnabled { get; private set; }
                    public bool IsBGEnabled { get; private set; }

                    public int WindowBaseMapAddress { get; private set; }
                    public int BgBaseMapAddress { get; private set; }

                    public int SpriteMode { get; private set; }

                    public int BaseTilesAddress { get; private set; }


                    public void Set(int aValue)
                    {
                        IsLCDEnabled = ((aValue & RegsIO_Bits.LCDC_EN) > 0);
                        IsWindowEnabled = ((aValue & RegsIO_Bits.LCDC_WEN) > 0);
                        IsSpritesEnabled = ((aValue & RegsIO_Bits.LCDC_OBJEN) > 0);
                        IsBGEnabled = ((aValue & RegsIO_Bits.LCDC_BGEN) > 0);

                        WindowBaseMapAddress = ((aValue & RegsIO_Bits.LCDC_WMAP) > 0) ? 0x1C00 : 0x1800;
                        BgBaseMapAddress = ((aValue & RegsIO_Bits.LCDC_BGMAP) > 0) ? 0x1C00 : 0x1800;

                        BaseTilesAddress = ((aValue & RegsIO_Bits.LCDC_TILE) > 0) ? 0x0000 : 0x0800;
                    }
                }


                public class PPU
                {
                    const int kTotalOperationCycles = 70224;


                    byte[] m_VRAM;
                    OAM m_OAM;

                    uint m_cyclesElapsed;
                    uint m_scanlineTotalCyclesElapsed;

                    int m_totalOperationCycles;

                    int m_lcdc;
                    int m_stat;
                    int m_curScanline;
                    int m_scanlineToCompare;
                    int m_operationMode;

                    LCDControlInfo m_lcdControlInfo;

                    CPU.RequestIRQFunc RequestIRQ;
                    EmuGB.DrawDisplayLineFunc DrawDisplayLine;


                    public byte[] VRAM
                    {
                        get { return m_VRAM; }
                    }


                    public OAM OAM
                    {
                        get { return m_OAM; }
                    }


                    public int BackgroundPalette
                    {
                        get;
                        set;
                    }


                    public int ObjectPalette0
                    {
                        get;
                        set;
                    }


                    public int ObjectPalette1
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


                    public int WindowPosX
                    {
                        get;
                        set;
                    }


                    public int WindowPosY
                    {
                        get;
                        set;
                    }


                    /// <summary>
                    /// Accessor for Reg LY (0xFF44)
                    /// </summary>
                    public int CurScanline
                    {
                        get
                        {
                            // If LCD is disabled, always return 0
                            if (!m_lcdControlInfo.IsLCDEnabled)
                            {
                                return 0;
                            }

                            return m_curScanline;
                        }

                        set { m_curScanline = (0xFF & value); }
                    }


                    /// <summary>
                    /// Accessor for Reg LYC (0xFF45)
                    /// </summary>
                    public int ScanlineComparer
                    {
                        get { return m_scanlineToCompare; }
                        set { m_scanlineToCompare = (0xFF & value); }
                    }


                    /// <summary>
                    /// Accessor for Reg LCDC (0xFF40)
                    /// </summary>
                    public int LCDControl
                    {
                        get { return m_lcdc; }
                        set
                        {
                            m_lcdc = (0xFF & value);
                            m_lcdControlInfo.Set(m_lcdc);
                        }
                    }


                    /// <summary>
                    /// Helper for LCDC reg
                    /// </summary>
                    public LCDControlInfo LCDControlInfo
                    {
                        get { return m_lcdControlInfo; }
                    }


                    /// <summary>
                    /// Accessor for Reg STAT (0xFF41)
                    /// </summary>
                    public int LCDControllerStatus
                    {
                        get
                        {
                            // If LCD is disabled, Mode is always 0
                            if (!m_lcdControlInfo.IsLCDEnabled)
                            {
                                return (0x80 | (0x7C & m_stat));
                            }

                            return (0x80 | m_stat);
                        }
                        set
                        {
                            m_stat = (0x7F & value);
                        }
                    }


                    public int OperationMode
                    {
                        get { return m_operationMode; }
                    }


                    public PPU( )
                    {
                        m_VRAM = new byte[0x2000]; // 8 KB
                        m_OAM = new OAM();
                        m_operationMode = 0;
                        m_scanlineTotalCyclesElapsed = 0;
                        m_cyclesElapsed = 0;

                        m_lcdControlInfo = new LCDControlInfo();
                        m_lcdControlInfo.Set(0);

                        // Temp binding
                        RequestIRQ = (aIRQ_flag) => { };
                    }


                    public void CyclesStep(int aElapsedCycles)
                    {
                        if (!m_lcdControlInfo.IsLCDEnabled)
                        {
                            m_totalOperationCycles = kTotalOperationCycles;
                            m_curScanline = 0;
                            m_cyclesElapsed = 0;
                            m_operationMode = 0;
                            m_scanlineTotalCyclesElapsed = 0;
                            return;
                        }


                        m_cyclesElapsed += (uint)aElapsedCycles;
                        m_scanlineTotalCyclesElapsed += (uint)aElapsedCycles;

                        switch (m_operationMode)
                        {
                            case 0: // H-Blank
                                {
                                    if (m_cyclesElapsed >= 201)
                                    {
                                        m_cyclesElapsed = 0;
                                        m_operationMode = 2;

                                        if ((m_stat & RegsIO_Bits.STAT_INTM2) > 0)
                                        {
                                            RequestIRQ(RegsIO_Bits.IF_STAT);
                                        }
                                    }
                                    if (m_scanlineTotalCyclesElapsed >= 456)
                                    {
                                        DrawDisplayLine(this);

                                        CurScanline = (CurScanline + 1) % 153;
                                        m_scanlineTotalCyclesElapsed -= 456;

                                        // Start VBLANK
                                        if (CurScanline == 144)
                                        {
                                            m_operationMode = 1;
                                            m_cyclesElapsed = 0;
                                            RequestIRQ(RegsIO_Bits.IF_VBLANK);

                                            if ((m_stat & RegsIO_Bits.STAT_INTM1) > 0)
                                            {
                                                RequestIRQ(RegsIO_Bits.IF_STAT);
                                            }
                                        }

                                        if (CurScanline == ScanlineComparer && ((m_stat & RegsIO_Bits.STAT_INTLYC) > 0))
                                        {
                                            RequestIRQ(RegsIO_Bits.IF_STAT);
                                        }
                                    }
                                }
                                break;

                            case 1: // V-Blank
                                {
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

                                        if ((m_stat & RegsIO_Bits.STAT_INTM2) > 0)
                                        {
                                            RequestIRQ(RegsIO_Bits.IF_STAT);
                                        }
                                    }
                                }
                                break;

                            case 2: // Accessing OAM
                                {
                                    if (m_cyclesElapsed >= 77)
                                    {
                                        m_cyclesElapsed = 0;
                                        m_operationMode = 3;
                                    }
                                }
                                break;

                            case 3: // Accessing VRAM
                                {
                                    if (m_cyclesElapsed >= 169)
                                    {
                                        m_cyclesElapsed = 0;
                                        m_operationMode = 0;

                                        if ((m_stat & RegsIO_Bits.STAT_INTM0) > 0)
                                        {
                                            RequestIRQ(RegsIO_Bits.IF_STAT);
                                        }
                                    }
                                }
                                break;
                        }

                        m_stat = (0x78 & m_stat) | (0x03 & m_operationMode) | ((CurScanline == ScanlineComparer) ? RegsIO_Bits.STAT_LYC : 0);
                    }

                    public void BindRequestIRQ(CPU.RequestIRQFunc aRequestIRQFunc)
                    {
                        RequestIRQ = aRequestIRQFunc;
                    }


                    public void BindDrawDisplayLine(EmuGB.DrawDisplayLineFunc aDrawDisplayLineFunc)
                    {
                        DrawDisplayLine = aDrawDisplayLineFunc;
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
