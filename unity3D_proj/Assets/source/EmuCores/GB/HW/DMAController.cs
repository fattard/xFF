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

using System.Collections.Generic;

namespace xFF
{
    namespace EmuCores
    {
        namespace GB
        {
            namespace HW
            {


                public class DMAController
                {
                    class DMAJob
                    {
                        int lastSourceAddress;
                        int lastTargetAddress;
                        int length;
                        int totalElapsedCycles;
                        bool isBusy;
                        bool isRunning;
                        DMAController controller;


                        public void Prepare(DMAController aController, int aSourceAddress, int aTargetAddress, int aLength)
                        {
                            lastSourceAddress = aSourceAddress;
                            lastTargetAddress = aTargetAddress;
                            length = aLength;
                            controller = aController;
                            totalElapsedCycles = 0;
                            isRunning = true;
                        }


                        public void Run(int aElapsedCycles)
                        {
                            while (aElapsedCycles > 0 && isRunning)
                            {
                                totalElapsedCycles += 4;

                                // Start up cycle
                                if (totalElapsedCycles < (8 + 4)) // 4 setup cycles + 8 cycles from 'ldh [DMA], a'
                                {
                                    aElapsedCycles -= 4;
                                    continue;
                                }

                                isBusy = true;

                                if (length > 0)
                                {
                                    controller.m_mem.Write8(lastTargetAddress++, controller.m_mem.Read8(lastSourceAddress++));
                                    --length;
                                }


                                // Finish cycle
                                if (length == 0 && totalElapsedCycles > 648)
                                {
                                    isBusy = false;
                                    isRunning = false;
                                }


                                aElapsedCycles -= 4;
                            }
                        }


                        public bool IsBusy
                        {
                            get { return isBusy; }
                        }


                        public bool IsRunning
                        {
                            get { return isRunning; }
                        }
                    }


                    MEM m_mem;
                    DMAJob m_job;

                    public bool IsBusy
                    {
                        get { return m_job.IsBusy; }
                    }

                    
                    public DMAController( )
                    {
                        m_job = new DMAJob();
                    }


                    public void StartDMA_OAM(int aStartAddress)
                    {
                        m_job.Prepare(this, aStartAddress, 0xFE00, 160);
                    }


                    public void BindMEM(MEM aMem)
                    {
                        m_mem = aMem;
                    }

                    public void CyclesStep(int aElapsedCycles)
                    {
                        if (m_job.IsRunning)
                        {
                            m_job.Run(aElapsedCycles);
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
