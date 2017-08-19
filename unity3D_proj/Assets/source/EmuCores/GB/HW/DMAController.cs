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
                        int elapsedCycles;
                        int totalElapsedCycles;
                        DMAController controller;


                        public void Prepare(DMAController aController, int aSourceAddress, int aTargetAddress, int aLength)
                        {
                            lastSourceAddress = aSourceAddress;
                            lastTargetAddress = aTargetAddress;
                            length = aLength;
                            controller = aController;
                            elapsedCycles = 0;
                            totalElapsedCycles = 0;
                        }


                        public void Run(int aElapsedCycles)
                        {
                            elapsedCycles += aElapsedCycles;
                            totalElapsedCycles += aElapsedCycles;

                            while (elapsedCycles >= 4 && length > 0)
                            {
                                controller.m_mem.Write8(lastTargetAddress++, controller.m_mem.Read8(lastSourceAddress++));
                                --length;

                                elapsedCycles -= 4;
                            }

                            if (totalElapsedCycles >= 671)
                            {
                                controller.m_jobs.Remove(this);
                                controller.m_availableJobs.Add(this);
                            }
                        }
                    }


                    MEM m_mem;

                    List<DMAJob> m_jobs;
                    List<DMAJob> m_availableJobs;

                    public bool IsBusy
                    {
                        get { return m_jobs.Count > 0; }
                    }

                    
                    public DMAController( )
                    {
                        m_jobs = new List<DMAJob>();
                        m_availableJobs = new List<DMAJob>();

                        m_availableJobs.Add(new DMAJob());
                    }


                    public void StartDMA_OAM(int aStartAddress)
                    {
                        if (m_availableJobs.Count == 0)
                        {
                            return;
                        }

                        DMAJob job = m_availableJobs[0];
                        m_availableJobs.RemoveAt(0);

                        job.Prepare(this, aStartAddress, 0xFE00, 160);

                        m_jobs.Add(job);
                    }


                    public void BindMEM(MEM aMem)
                    {
                        m_mem = aMem;
                    }

                    public void CyclesStep(int aElapsedCycles)
                    {
                        if (IsBusy)
                        {
                            for (int i = 0; i < m_jobs.Count; ++i)
                            {
                                m_jobs[i].Run(aElapsedCycles);
                            }
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
