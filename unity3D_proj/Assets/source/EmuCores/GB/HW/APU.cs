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

using xFF.EmuCores.GB;
using xFF.EmuCores.GB.Defs;
using xFF.EmuCores.GB.HW;


namespace xFF
{
    namespace EmuCores
    {
        namespace GB
        {
            namespace HW
            {


                public partial class APU
                {
                    
                    byte[] m_outputWave;
                    int[] m_regs = new int[0x30];
                    

                    public EmuGB.PlayAudioFunc PlayAudio = (aAPU) => { };


                    public bool MasterSoundEnabled
                    {
                        get;
                        set;
                    }


                    public int OutputVolumeLeft
                    {
                        get;
                        set;
                    }


                    public int OutputVolumeRight
                    {
                        get;
                        set;
                    }


                    public bool ExternalInputLeftEnabled
                    {
                        get;
                        set;
                    }


                    public bool ExternalInputRightEnabled
                    {
                        get;
                        set;
                    }


                    public byte[] OutputWave
                    {
                        get { return m_outputWave; }
                        set { m_outputWave = value; }
                    }


                    public int this[int aAddress]
                    {
                        get
                        {
                            switch (aAddress)
                            {
                                case RegsIO.NR50:
                                    return  (OutputVolumeRight) | (OutputVolumeLeft << 4)
                                            | (ExternalInputRightEnabled ? (1 << 3) : 0)
                                            | (ExternalInputLeftEnabled ? (1 << 7) : 0);


                                case RegsIO.NR51:
                                    //TODO: split to each channel
                                    return m_regs[aAddress - RegsIO.NR10];


                                case RegsIO.NR52:
                                    return 0x70 | (MasterSoundEnabled ? (1 << 7) : 0)
                                            // TODO: get real flags
                                            | (0x0F & m_regs[aAddress - RegsIO.NR10]);


                                default:
                                    return m_regs[aAddress - RegsIO.NR10];
                            }
                        }

                        set
                        {
                            switch (aAddress)
                            {
                                case RegsIO.NR50:
                                    {
                                        OutputVolumeRight = (0x07 & value);
                                        OutputVolumeLeft = (0x07 & (value >> 4));
                                        ExternalInputRightEnabled = ((0x08 & value) > 0);
                                        ExternalInputLeftEnabled = ((0x80 & value) > 0);
                                    }
                                    break;


                                case RegsIO.NR51:
                                    {
                                        //TODO: split to each channel
                                        m_regs[aAddress - RegsIO.NR10] = (0xFF & value);
                                        //SetReg(aAddress, value);
                                    }
                                    break;


                                case RegsIO.NR52:
                                    {
                                        MasterSoundEnabled = ((0x80 & value) > 0);
                                        
                                        //TODO: split channel On flags
                                        m_regs[aAddress - RegsIO.NR10] = (0xFF & value);
                                    }
                                    break;


                                default:
                                    m_regs[aAddress - RegsIO.NR10] = (0xFF & value);
                                    //SetReg(aAddress, value);
                                    break;
                            }
                        }
                    }



                    public void UpdateOutputWave( )
                    {
                        //OutputSound(ref m_outputWave);
                    }



                    public void CyclesStep(int aElapsedCycles)
                    {

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
