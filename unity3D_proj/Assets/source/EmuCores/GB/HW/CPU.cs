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

using xFF.Processors.Sharp;

namespace xFF
{
    namespace EmuCores
    {
        namespace GB
        {
            namespace HW
            {


                public class CPU
                {
                    public const uint kCPUClock = 4194304; // ~4.19 MHz
                    public const uint kCyclesPerFrame = kCPUClock / 60; // 69905 cycles per frame


                    LR35902 m_gb_core;
                    MEM m_mem;

                    uint m_cyclesElapsed;
                    uint m_userCyclesRate;

                    public uint UserCyclesRate
                    {
                        get { return m_userCyclesRate; }
                        set { m_userCyclesRate = value; }
                    }


                    public LR35902 ProcessorState
                    {
                        get { return m_gb_core; }
                    }


                    public CPU( )
                    {
                        m_gb_core = new LR35902();

                        m_gb_core.BindCyclesStep(CyclesStep);

                        m_userCyclesRate = kCyclesPerFrame;
                        m_cyclesElapsed = 0;
                    }


                    public void BindMemBUS(MEM aMem)
                    {
                        m_mem = aMem;

                        m_gb_core.BindBUS(aMem.Read8, aMem.Write8);
                    }


                    public void Run( )
                    {
                        while (m_cyclesElapsed < m_userCyclesRate)
                        {
                            //UnityEngine.Debug.Log(m_gb_core.ToString());

                            m_gb_core.Fetch();
                            m_gb_core.DecodeAndExecute();
                        }


                        //UnityEngine.Debug.Log(m_cyclesElapsed + " - " + m_gb_core.ToString());

                        // Reset cycles counter 
                        m_cyclesElapsed -= m_userCyclesRate;
                    }


                    void CyclesStep(int aElapsedCycles)
                    {
                        m_cyclesElapsed += (uint)aElapsedCycles;
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
