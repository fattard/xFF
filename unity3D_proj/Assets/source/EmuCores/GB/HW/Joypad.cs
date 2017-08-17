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


                public class Joypad
                {
                    public delegate int GetKeysStateFunc();

                    int m_prev_p14_keysState;
                    int m_prev_p15_keysState;

                    int m_p14_keysState;
                    int m_p15_keysState;

                    int m_selectedPort;

                    EmuGB.GetKeysStateFunc GetKeysState;
                    CPU.RequestIRQFunc RequestIRQ;


                    public int SelectedOutPort
                    {
                        get { return m_selectedPort; }
                        set
                        {
                            m_selectedPort = (0x30 & value);
                            if (m_selectedPort == 0x30)
                            {
                                RequestIRQ(RegsIO_Bits.IF_JOYPAD);
                            }
                        }
                    }


                    public int KeysState
                    {
                        get
                        {
                            if (SelectedOutPort == 0x20)
                            {
                                return m_p14_keysState;
                            }

                            else if (SelectedOutPort == 0x10)
                            {
                                return m_p15_keysState;
                            }

                            else
                            {
                                return ~0;
                            }
                        }
                    }


                    public Joypad( )
                    {
                        // Temp binding
                        RequestIRQ = (aIRQ_flag) => { };
                        GetKeysState = () => { return 0; };

                        SelectedOutPort = 0x30;
                    }



                    public void UpdateKeys()
                    {
                        m_prev_p14_keysState = m_p14_keysState;
                        m_prev_p15_keysState = m_p15_keysState;

                        int keys = GetKeysState();

                        m_p14_keysState = (0x0F & keys);
                        m_p15_keysState = (0x0F & (keys >> 4));

                    }


                    public void BindRequestIRQ(CPU.RequestIRQFunc aRequestIRQFunc)
                    {
                        RequestIRQ = aRequestIRQFunc;
                    }


                    public void BindGetKeysStateFunc(EmuGB.GetKeysStateFunc aGetKeysStateFunc)
                    {
                        GetKeysState = aGetKeysStateFunc;
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
