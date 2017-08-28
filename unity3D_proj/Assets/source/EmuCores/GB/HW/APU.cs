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


                    public byte[] OutputWave
                    {
                        get { return m_outputWave; }
                        set { m_outputWave = value; }
                    }


                    public int this[int aAddress]
                    {
                        get
                        {
                            return m_regs[aAddress - RegsIO.NR10];
                        }

                        set
                        {
                            //SetReg(aAddress, value);
                        }
                    }



                    public void UpdateOutputWave( )
                    {
                        //OutputSound(ref m_outputWave);
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
