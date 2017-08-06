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

using xFF.EmuCores.GB.HW;


namespace xFF
{
    namespace EmuCores
    {
        namespace GB
        {


            public class EmuGB
            {
                public delegate void DrawDisplayFunc(PPU aPPU);


                ConfigsGB m_configs;

                CPU m_cpu;
                PPU m_ppu;
                APU m_apu;

                MEM m_mem;


                bool m_paused;


                public ConfigsGB Configs
                {
                    get { return m_configs; }
                }


                public CPU CPU
                {
                    get { return m_cpu; }
                }


                public PPU PPU
                {
                    get { return m_ppu; }
                }


                public APU APU
                {
                    get { return m_apu; }
                }


                public MEM MEM
                {
                    get { return m_mem; }
                }


                public bool IsPaused
                {
                    get { return m_paused; }
                }


                public DrawDisplayFunc DrawDisplay;


                public EmuGB(ConfigsGB aConfigs)
                {
                    m_configs = aConfigs;

                    m_cpu = new CPU();
                    m_ppu = new PPU();
                    m_apu = new APU();

                    m_mem = new MEM();

                    // Start paused
                    m_paused = true;

                    // Temp binding
                    DrawDisplay = (aPPU) => { };
                }


                public void PowerOn( )
                {
                    m_paused = false;
                }


                public void EmulateFrame( )
                {
                    if (m_paused)
                    {
                        return;
                    }


                    m_cpu.Run();

                    DrawDisplay(m_ppu);
                }


                public void SetBootRom(byte[] aBootRom)
                {
                    
                }


                public bool LoadSimpleRom(byte[] aRomData)
                {
                    if (aRomData == null || aRomData.Length != 0x8000)
                    {
                        return false;
                    }
                    
                    return true;
                }


                public static bool IsValidROM(string aFilePath)
                {
                    if (aFilePath.ToLower().EndsWith(".gb") || aFilePath.ToLower().EndsWith(".gbc"))
                    {
                        return true;
                    }

                    return false;
                }
            }


        }
        // namespace GB
    }
    // namespace EmuCores
}
// namespace xFF