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


                    public PPU( )
                    {
                        m_VRAM = new byte[0x2000]; // 8 KB

                        // Temp binding
                        RequestIRQ = (aIRQ_flag) => { };
                    }


                    public void CyclesStep(int aElapsedCycles)
                    {
                        m_cyclesElapsed += (uint)aElapsedCycles;

                        if (m_cyclesElapsed >= 456)
                        {
                            CurScanline = (CurScanline + 1) % 153;
                            m_cyclesElapsed -= 456;

                            // Start VBLANK
                            if (CurScanline == 144)
                            {
                                RequestIRQ(RegsIO_Bits.IF_VBLANK);
                            }
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
