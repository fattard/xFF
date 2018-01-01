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
                    int m_internalTransferCounter;
                    int m_shiftsCount;
                    int m_syncSysClockCounter;

                    CPU.RequestIRQFunc RequestIRQ;


                    /// <summary>
                    /// Accessor for Reg SB (0xFF01)
                    /// Shift register that will shift out MSB
                    /// and receive a shift in LSB whenever
                    /// selected clock signal is received.
                    /// </summary>
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


                    /// <summary>
                    /// Accessor for Reg SC (0xFF02)
                    /// Enable transfer flag and clock mode
                    /// are controlled by this register
                    /// </summary>
                    public int SerialControlData
                    {
                        get { return GetControlData(); }
                        set { SetControlData(value); }
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


                    void SetControlData(int aData)
                    {
                        SerialClockMode = (0x01 & aData);
                        SerialTransferEnabled = ((0x80 & aData) > 0);
                            
                        if (SerialTransferEnabled && SerialClockMode == 1)
                        {
                            m_shiftsCount = 0;
                            m_internalTransferCounter = m_syncSysClockCounter;
                        }
                    }


                    int GetControlData( )
                    {
                        return ((SerialTransferEnabled) ? RegsIO_Bits.SC_EN : 0) | 0x7E | SerialClockMode;
                    }


                    public SerialIO( )
                    {
                        SerialTransferData = 0;
                        SerialClockMode = 0;
                        SerialTransferEnabled = false;

                        m_internalTransferCounter = 0;
                        m_syncSysClockCounter = 0;
                    }


                    public void CyclesStep(int aElapsedCycles)
                    {
                        while (aElapsedCycles > 0)
                        {
                            if (SerialTransferEnabled && SerialClockMode == 1)
                            {
                                m_internalTransferCounter++;

                                if (m_internalTransferCounter >= 512)
                                {
                                    SendBitOut((0x80 & m_transferData) >> 7);
                                    m_transferData = (0xFF & ((m_transferData << 1) | ReceiveBitIn()));

                                    if (m_shiftsCount >= 8)
                                    {
                                        SerialTransferEnabled = false;
                                        RequestIRQ(RegsIO_Bits.IF_SERIAL);
                                    }


                                    m_internalTransferCounter -= 512;
                                }
                            }
                            
                            m_syncSysClockCounter = (m_syncSysClockCounter + 1) % 8;

                            aElapsedCycles--;
                        }
                    }

                    public void BindRequestIRQ(CPU.RequestIRQFunc aRequestIRQFunc)
                    {
                        RequestIRQ = aRequestIRQFunc;
                    }


                    void SendBitOut(int aVal)
                    {
                        //TODO: use serial device communication
                        m_shiftsCount++;
                    }


                    int ReceiveBitIn( )
                    {
                        //TODO: use serial device communication
                        return 1;
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
