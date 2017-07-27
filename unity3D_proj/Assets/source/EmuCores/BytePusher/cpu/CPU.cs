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

using xFF.Processors.OISC;
using xFF.EmuCores.BytePusher.mem;

namespace xFF
{
    namespace EmuCores
    {
        namespace BytePusher
        {
            namespace cpu
            {


                class CPU
                {
                    ByteByteJump m_bbj_core;
                    MMU m_mmu;

                    public const uint kCPUClock = 3932160; // ~3.93 MHz
                    public const uint kCyclesPerFrame = kCPUClock / 60; // 65536 cycles per frame


                    uint m_elapsedCycles;
                    uint m_userCyclesRate;


                    public uint UserCyclesRate
                    {
                        get { return m_userCyclesRate; }
                        set { m_userCyclesRate = (value > 0) ? value : 1; }
                    }


                    public void BindMMU(MMU aMMU)
                    {
                        m_bbj_core.BindAddressBUS(aMMU.Read24, aMMU.Write24);
                        m_mmu = aMMU;
                    }


                    public CPU( )
                    {
                        m_bbj_core = new ByteByteJump();

                        m_userCyclesRate = kCyclesPerFrame;
                        m_elapsedCycles = 0;
                    }


                    public void Run( )
                    {
                        if (m_elapsedCycles >= kCyclesPerFrame)
                        {
                            m_elapsedCycles = 0;
                        }

                        // Set start frame PC
                        m_bbj_core.RegPC = m_mmu.Read24(0x02);

                        while (m_elapsedCycles < kCyclesPerFrame)
                        {
                            m_bbj_core.Step();
                            ++m_elapsedCycles;

                            /*if ((m_elapsedCycles % m_userCyclesRate) == 0)
                            {
                                break;
                            }*/
                        }
                        
                    }
                }


            }
            // namespace cpu
        }
        // namespace BytePusher
    }
    // namespace EmuCores
}
// namespace xFF
