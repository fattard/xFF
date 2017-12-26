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
                    DMAController m_dmaController;
                    TimerController m_timerController;
                    Joypad m_joypad;
                    SerialIO m_serialIO;
                    Cartridge m_cartROM;


                    public byte[] BootRomData
                    {
                        get { return m_bootRom; }
                    }


                    public byte[] DBG_FullRAM
                    {
                        get { return m_dbg_FullRam; }
                    }


                    public Cartridge Cartridge
                    {
                        get { return m_cartROM; }
                    }


                    public MEM( )
                    {
                        m_dbg_FullRam = new byte[0x10000]; // 64 KB

                        // Fill default empty data into ROM area
                        for (int i = 0; i < 0x8000; ++i)
                        {
                            m_dbg_FullRam[i] = 0xFF;
                        }

                        // Fill default I/O area
                        for (int i = 0xFF00; i < 0xFF80; ++i)
                        {
                            m_dbg_FullRam[i] = 0xFF;
                        }

                        m_dbg_FullRam[0xFF50] = 0;

                        // Temp Binding
                        m_bootRom = m_dbg_FullRam;
                    }


                    public int Read8(int aAddress)
                    {
                        if (aAddress < 0x100 && (m_dbg_FullRam[RegsIO.BOOT] == 0))
                        {
                            return m_bootRom[aAddress];
                        }

                        else if (aAddress < 0x8000)
                        {
                            return m_cartROM[aAddress];
                        }

                        else if (aAddress >= 0x8000 && aAddress < 0xA000)
                        {
                            return m_ppu.VRAM[aAddress & 0x1FFF];
                        }

                        else if (aAddress >= 0xA000 && aAddress < 0xC000)
                        {
                            return m_cartROM[aAddress];
                        }

                        else if (aAddress >= 0xFE00 && aAddress < 0xFEA0)
                        {
                            if (m_dmaController.IsBusy)
                            {
                                return 0xFF;
                            }

                            return m_ppu.OAM[aAddress & 0xFF];
                        }

                        else if (aAddress == RegsIO.P1)
                        {
                            return (0xC0 | m_joypad.SelectedOutPort | (0x0F & m_joypad.KeysState));
                        }

                        else if (aAddress == RegsIO.LCDC)
                        {
                            return m_ppu.LCDControl;
                        }

                        else if (aAddress == RegsIO.STAT)
                        {
                            return m_ppu.LCDControllerStatus;
                        }

                        else if (aAddress == RegsIO.SCX)
                        {
                            return m_ppu.BGScrollX;
                        }

                        else if (aAddress == RegsIO.SCY)
                        {
                            return m_ppu.BGScrollY;
                        }

                        else if (aAddress == RegsIO.WX)
                        {
                            return m_ppu.WindowPosX;
                        }

                        else if (aAddress == RegsIO.WY)
                        {
                            return m_ppu.WindowPosY;
                        }

                        else if (aAddress == RegsIO.LY)
                        {
                            return m_ppu.CurScanline;
                        }

                        else if (aAddress == RegsIO.LYC)
                        {
                            return m_ppu.ScanlineComparer;
                        }

                        else if (aAddress == RegsIO.BGP)
                        {
                            return m_ppu.BackgroundPalette;
                        }

                        else if (aAddress == RegsIO.OBP0)
                        {
                            return m_ppu.ObjectPalette0;
                        }

                        else if (aAddress == RegsIO.OBP1)
                        {
                            return m_ppu.ObjectPalette1;
                        }

                        else if (aAddress == RegsIO.DIV)
                        {
                            return m_timerController.Divider;
                        }

                        else if (aAddress == RegsIO.TIMA)
                        {
                            return m_timerController.TimerCounter;
                        }

                        else if (aAddress == RegsIO.TMA)
                        {
                            return m_timerController.TimerModulo;
                        }

                        else if (aAddress == RegsIO.TAC)
                        {
                            return m_timerController.GetControllerData();
                        }

                        else if (aAddress == RegsIO.SB)
                        {
                            return m_serialIO.SerialTransferData;
                        }

                        else if (aAddress == RegsIO.SC)
                        {
                            return m_serialIO.GetControlData();
                        }

                        return m_dbg_FullRam[aAddress];
                    }


                    public void Write8(int aAddress, int aValue)
                    {
                        if (aAddress < 0x8000)
                        {
                            m_cartROM[aAddress] = aValue;
                        }

                        else if (aAddress >= 0x8000 && aAddress < 0xA000)
                        {
                            m_ppu.VRAM[aAddress & 0x1FFF] = (byte)aValue;
                        }

                        else if (aAddress >= 0xA000 && aAddress < 0xC000)
                        {
                            m_cartROM[aAddress] = aValue;
                        }

                        else if (aAddress >= 0xFE00 && aAddress < 0xFEA0)
                        {
                            m_ppu.OAM[aAddress & 0xFF] = (0xFF & aValue);
                        }

                        else if (aAddress == RegsIO.P1)
                        {
                            m_joypad.SelectedOutPort = (0x30 & aValue);
                        }

                        else if (aAddress == RegsIO.LCDC)
                        {
                            m_ppu.LCDControl = aValue;
                        }

                        else if (aAddress == RegsIO.STAT)
                        {
                            m_ppu.LCDControllerStatus = aValue;
                        }

                        else if (aAddress == RegsIO.BGP)
                        {
                            m_ppu.BackgroundPalette = (0xFF & aValue);
                        }

                        else if (aAddress == RegsIO.OBP0)
                        {
                            m_ppu.ObjectPalette0 = (0xFF & aValue);
                        }

                        else if (aAddress == RegsIO.OBP1)
                        {
                            m_ppu.ObjectPalette1 = (0xFF & aValue);
                        }

                        else if (aAddress == RegsIO.SCX)
                        {
                            m_ppu.BGScrollX = (0xFF & aValue);
                        }

                        else if (aAddress == RegsIO.SCY)
                        {
                            m_ppu.BGScrollY = (0xFF & aValue);
                        }

                        else if (aAddress == RegsIO.WX)
                        {
                            m_ppu.WindowPosX = (0xFF & aValue);
                        }

                        else if (aAddress == RegsIO.WY)
                        {
                            m_ppu.WindowPosY = (0xFF & aValue);
                        }

                        else if (aAddress == RegsIO.LY)
                        {
                            // Reset Scanline counter
                            m_ppu.CurScanline = 0;
                        }

                        else if (aAddress == RegsIO.LYC)
                        {
                            m_ppu.ScanlineComparer = (0xFF & aValue);
                        }

                        else if (aAddress == RegsIO.DMA)
                        {
                            m_dmaController.StartDMA_OAM((0xFF & aValue) << 8);
                        }

                        else if (aAddress == RegsIO.DIV)
                        {
                            // Resets DIV
                            m_timerController.Divider = 0;
                        }

                        else if (aAddress == RegsIO.TIMA)
                        {
                            m_timerController.TimerCounter = (0xFF & aValue);
                        }

                        else if (aAddress == RegsIO.TMA)
                        {
                            m_timerController.TimerModulo = (0xFF & aValue);
                        }

                        else if (aAddress == RegsIO.TAC)
                        {
                            m_timerController.SetControllerData(0x07 & aValue);
                        }

                        else if (aAddress == RegsIO.BOOT)
                        {
                            m_dbg_FullRam[aAddress] |= (byte)(RegsIO_Bits.BOOT_LOCK & aValue);
                        }

                        else if (aAddress == RegsIO.SB)
                        {
                            m_serialIO.SerialTransferData = aValue;
                        }

                        else if (aAddress == RegsIO.SC)
                        {
                            m_serialIO.SetControlData(aValue);
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


                    public void AttachDMAController(DMAController aDMA)
                    {
                        m_dmaController = aDMA;
                        m_dmaController.BindMEM(this);
                    }


                    public void AttachTimerController(TimerController aTimer)
                    {
                        m_timerController = aTimer;
                    }


                    public void AttachJoypad(Joypad aJoypad)
                    {
                        m_joypad = aJoypad;
                    }


                    public void AttachSerialIO(SerialIO aSerialIO)
                    {
                        m_serialIO = aSerialIO;
                    }


                    public void AttachCartridge(Cartridge aCart)
                    {
                        m_cartROM = aCart;
                    }


                    public void LoadNoCart( )
                    {
                        LoadSimpleRom(null, m_dbg_FullRam);
                    }
                    

                    public void LoadSimpleRom(CartridgeHeader aHeader, byte[] aRomData)
                    {
                        if (m_cartROM == null)
                        {
                            AttachCartridge(new MBC.Cartridge_Single(aHeader));
                        }

                        byte[] buffer = new byte[0x4000];

                        System.Buffer.BlockCopy(aRomData, 0, buffer, 0, 0x4000);
                        m_cartROM.SetROMBank(0, buffer);

                        System.Buffer.BlockCopy(aRomData, 0x4000, buffer, 0, 0x4000);
                        m_cartROM.SetROMBank(1, buffer);
                    }


                    public void LoadMBC1Rom(CartridgeHeader aHeader, byte[] aRomData)
                    {
                        if (m_cartROM == null)
                        {
                            AttachCartridge(new MBC.Cartridge_MBC1(aHeader));
                        }

                        byte[] buffer = new byte[0x4000];

                        int totalBanks = aRomData.Length / 0x4000;

                        for (int i = 0; i < totalBanks; ++i)
                        {
                            System.Buffer.BlockCopy(aRomData, (i * 0x4000), buffer, 0, 0x4000);
                            m_cartROM.SetROMBank(i, buffer);
                        }

                        if (EmuEnvironment.RomFilePath.EndsWith(".gbc"))
                        {
                            m_cartROM.LoadRAM(EmuEnvironment.RomFilePath.Replace(".gbc", ".sav"));
                        }

                        else if (EmuEnvironment.RomFilePath.EndsWith(".gb"))
                        {
                            m_cartROM.LoadRAM(EmuEnvironment.RomFilePath.Replace(".gb", ".sav"));
                        }

                        // Just append .sav to whatever name is
                        else
                        {
                            m_cartROM.LoadRAM(EmuEnvironment.RomFilePath + ".sav");
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
