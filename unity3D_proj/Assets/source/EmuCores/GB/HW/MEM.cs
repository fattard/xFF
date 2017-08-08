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


                public class MEM
                {
                    byte[] m_bootRom;

                    byte[] m_dbg_FullRam;

                    CPU m_cpu;
                    PPU m_ppu;



                    public byte[] BootRomData
                    {
                        get { return m_bootRom; }
                    }


                    public byte[] DBG_FullRAM
                    {
                        get { return m_dbg_FullRam; }
                    }


                    public MEM( )
                    {
                        m_dbg_FullRam = new byte[0x10000]; // 64 KB

                        // Fill default empty data into ROM area
                        for (int i = 0; i < 0x8000; ++i)
                        {
                            m_dbg_FullRam[i] = 0xFF;
                        }

                        // Temp Binding
                        m_bootRom = m_dbg_FullRam;
                    }


                    public int Read8(int aAddress)
                    {
                        if (aAddress < 0x100 && (m_dbg_FullRam[RegsIO.BOOT] == 0))
                        {
                            return m_bootRom[aAddress];
                        }

                        else if (aAddress >= 0x8000 && aAddress < 0xA000)
                        {
                            return m_ppu.VRAM[aAddress & 0x1FFF];
                        }

                        else if (aAddress == RegsIO.P1)
                        {
                            return 0xCF; // Default: all not pressed
                        }

                        /*else if (aAddress == 0xFF41)
                        {
                            //return 1;
                            return 0;
                        }*/

                        else if (aAddress == RegsIO.SCX)
                        {
                            return m_ppu.BGScrollX;
                        }

                        else if (aAddress == RegsIO.SCY)
                        {
                            return m_ppu.BGScrollY;
                        }

                        else if (aAddress == RegsIO.LY)
                        {
                            return m_ppu.CurScanline;
                        }

                        else if (aAddress == RegsIO.BGP)
                        {
                            return m_ppu.BackgroundPalette;
                        }

                        return m_dbg_FullRam[aAddress];
                    }


                    public void Write8(int aAddress, int aValue)
                    {
                        if (aAddress < 0x8000)
                        {
                            return;
                        }

                        else if (aAddress >= 0x8000 && aAddress < 0xA000)
                        {
                            m_ppu.VRAM[aAddress & 0x1FFF] = (byte)aValue;
                        }

                        else if (aAddress == RegsIO.P1)
                        {
                            
                        }

                        else if (aAddress == RegsIO.BGP)
                        {
                            m_ppu.BackgroundPalette = (0xFF & aValue);
                        }

                        else if (aAddress == RegsIO.SCX)
                        {
                            m_ppu.BGScrollX = (0xFF & aValue);
                        }

                        else if (aAddress == RegsIO.SCY)
                        {
                            m_ppu.BGScrollY = (0xFF & aValue);
                        }

                        else if (aAddress == RegsIO.LY)
                        {
                            // Reset Scanline counter
                            m_ppu.CurScanline = 0;
                        }

                        else if (aAddress == RegsIO.BOOT)
                        {
                            m_dbg_FullRam[aAddress] |= (byte)(RegsIO_Bits.BOOT_LOCK & aValue);
                        }

                        else if (aAddress == 0xFF02 && aValue == 0x81)
                        {
                            //emuDbg.Write(Read8(0xFF01));
                        }

                        else
                        {
                            m_dbg_FullRam[aAddress] = (byte)(0xFF & aValue);
                        }
                    }


                    public void SetBootRom(byte[] aBootRom)
                    {
                        m_bootRom = aBootRom;
                    }


                    public void AttachCPU(CPU aCPU)
                    {
                        m_cpu = aCPU;
                        m_cpu.BindMemBUS(this);
                    }


                    public void AttachPPU(PPU aPPU)
                    {
                        m_ppu = aPPU;
                    }


                    public void LoadSimpleRom(byte[] aRomData)
                    {
                        for (int i = 0; i < 0x8000; ++i)
                        {
                            m_dbg_FullRam[i] = aRomData[i];
                        }
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
