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


                public class SerialIO
                {
                    int m_transferData;
                    int m_transferCounter;

                    CPU.RequestIRQFunc RequestIRQ;

                    public int SerialTransferData
                    {
                        get
                        {
                            return m_transferData;
                        }
                        set
                        {
                            m_transferData = (0xFF & value);
                        }
                    }


                    public bool SerialTransferEnabled
                    {
                        get;
                        private set;
                    }


                    public int SerialClockMode
                    {
                        get;
                        private set;
                    }


                    public void SetControlData(int aData)
                    {
                        //TODO: check behaviour
                        if (SerialTransferEnabled)
                        {
                            return;
                        }

                        SerialClockMode = (0x01 & aData);
                        SerialTransferEnabled = ((0x80 & aData) > 0);

                        if (SerialTransferEnabled)
                        {
                            m_transferCounter = 512;
                        }
                    }


                    public int GetControlData( )
                    {
                        return 0x7E | (SerialClockMode) | ((SerialTransferEnabled) ? RegsIO_Bits.SC_EN : 0);
                    }


                    public SerialIO( )
                    {
                        SerialTransferData = 0;
                        SerialClockMode = 0;
                        SerialTransferEnabled = false;
                    }


                    public void CyclesStep(int aElapsedCycles)
                    {
                        if (SerialClockMode == 1)
                        {
                            m_transferCounter -= aElapsedCycles;

                            if (m_transferCounter <= 0)
                            {
                                m_transferData = 0xFF;
                                SerialTransferEnabled = false;

                                RequestIRQ(RegsIO_Bits.IF_SERIAL);

                                //TODO: quick hack to stop serial IRQ spam
                                m_transferCounter = 512;
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
