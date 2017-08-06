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

using xFF.EmuCores.BytePusher;
using xFF.EmuCores.BytePusher.cpu;
using xFF.EmuCores.BytePusher.mem;

namespace xFF
{
    namespace EmuCores
    {
        namespace BytePusher
        {


            public class EmuBytePusher
            {
                ConfigsBytePusher m_configs;
                CPU m_cpu;
                MMU m_mmu;


                public delegate void LCDDriverDrawFunc(byte[] aVRAM, int aStartOffset);
                public delegate void AudioDriverPlayFunc(byte[] aRAM, int aStartOffset);
                public delegate void InputDriverFunc(byte[] aRAM, int aStartOffset);


                public LCDDriverDrawFunc DrawDisplay;
                public AudioDriverPlayFunc PlayAudio;
                public InputDriverFunc UpdateInputKeys;

                public ConfigsBytePusher Configs
                {
                    get { return m_configs; }
                }


                public EmuBytePusher(ConfigsBytePusher aConfigs)
                {
                    m_configs = aConfigs;
                    m_cpu = new CPU();
                    m_mmu = new MMU();

                    m_cpu.BindMMU(m_mmu);

                    // Temp bind
                    DrawDisplay = (aVRAM, aStartOffset) => { };
                    PlayAudio = (aRAM, aStartOffset) => { };
                    UpdateInputKeys = (aRAM, aStartOffset) => { };
                }


                public void EmulateFrame( )
                {
                    UpdateInputKeys(m_mmu.FullRAM, 0x00);
                    m_cpu.Run();
                    DrawDisplay(m_mmu.FullRAM, m_mmu.Read8(0x05) << 16);
                    PlayAudio(m_mmu.FullRAM, ((m_mmu.Read8(0x06) << 8) | (m_mmu.Read8(0x07))) << 8);
                }


                public bool LoadRom(byte[] aRomData)
                {
                    if (aRomData == null || aRomData.Length >= 0x1000000) // 16 MB
                    {
                        return false;
                    }

                    // Loads ROM data to RAM
                    System.Array.Copy(aRomData, 0, m_mmu.FullRAM, 0, aRomData.Length);

                    // Clear remaining RAM data
                    for (int i = aRomData.Length; i < m_mmu.FullRAM.Length; ++i)
                    {
                        m_mmu.FullRAM[i] = 0;
                    }

                    return true;
                }

                
                public static bool IsValidROM(string aFilePath)
                {
                    if (aFilePath.ToLower().EndsWith(".bytepusher") || aFilePath.ToLower().EndsWith(".bp"))
                    {
                        return true;
                    }

                    return false;
                }
            }


        }
        // namespace BytePusher
    }
    // namespace EmuCores
}
// namespace xFF
