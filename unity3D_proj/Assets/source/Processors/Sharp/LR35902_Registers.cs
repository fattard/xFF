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

namespace xFF
{
    namespace Processors
    {
        namespace Sharp
        {


            public partial class LR35902
            {


                public class Registers
                {
                    #region Defs
                    const int rA = 0x07;
                    const int rB = 0x00;
                    const int rC = 0x01;
                    const int rD = 0x02;
                    const int rE = 0x03;
                    const int rH = 0x04;
                    const int rL = 0x05;
                    #endregion Defs

                    int[] m_regArr = new int[8];

                    int m_SP;
                    int m_PC;

                    Flags m_F = new Flags();


                    /// <summary>
                    /// Accumulator: A
                    /// </summary>
                    public int A
                    {
                        get { return m_regArr[rA]; }
                        set { m_regArr[rA] = (0xFF & value); }
                    }


                    /// <summary>
                    /// Flag Register: F
                    /// </summary>
                    public Flags F
                    {
                        get { return m_F; }
                    }


                    /// <summary>
                    /// Aux Register: B
                    /// </summary>
                    public int B
                    {
                        get { return m_regArr[rB]; }
                        set { m_regArr[rB] = (0xFF & value); }
                    }


                    /// <summary>
                    /// Aux Register: C
                    /// </summary>
                    public int C
                    {
                        get { return m_regArr[rC]; }
                        set { m_regArr[rC] = (0xFF & value); }
                    }


                    /// <summary>
                    /// Aux Register: D
                    /// </summary>
                    public int D
                    {
                        get { return m_regArr[rD]; }
                        set { m_regArr[rD] = (0xFF & value); }
                    }


                    /// <summary>
                    /// Aux Register: E
                    /// </summary>
                    public int E
                    {
                        get { return m_regArr[rE]; }
                        set { m_regArr[rE] = (0xFF & value); }
                    }


                    /// <summary>
                    /// Aux Register: H
                    /// </summary>
                    public int H
                    {
                        get { return m_regArr[rH]; }
                        set { m_regArr[rH] = (0xFF & value); }
                    }


                    /// <summary>
                    /// Aux Register: L
                    /// </summary>
                    public int L
                    {
                        get { return m_regArr[rL]; }
                        set { m_regArr[rL] = (0xFF & value); }
                    }


                    /// <summary>
                    /// Stack Pointer: SP 
                    /// </summary>
                    public int SP
                    {
                        get { return m_SP; }
                        set { m_SP = (0xFFFF & value); }
                    }


                    /// <summary>
                    /// Program Counter: PC
                    /// </summary>
                    public int PC
                    {
                        get { return m_PC; }
                        set { m_PC = (0xFFFF & value); }
                    }


                    /// <summary>
                    /// Data Pointer Register: AF
                    /// </summary>
                    public int AF
                    {
                        get { return ((m_regArr[rA] << 8) | m_F.Packed); }
                        set
                        {
                            m_regArr[rA] = (0xFF & (value >> 8));
                            m_F.Packed = (0xFF & value);
                        }
                    }


                    /// <summary>
                    /// Data Pointer Register: BC
                    /// </summary>
                    public int BC
                    {
                        get { return ((m_regArr[rB] << 8) | (m_regArr[rC])); }
                        set
                        {
                            m_regArr[rB] = (0xFF & (value >> 8));
                            m_regArr[rC] = (0xFF & value);
                        }
                    }


                    /// <summary>
                    /// Data Pointer Register: DE
                    /// </summary>
                    public int DE
                    {
                        get { return ((m_regArr[rD] << 8) | (m_regArr[rE])); }
                        set
                        {
                            m_regArr[rD] = (0xFF & (value >> 8));
                            m_regArr[rE] = (0xFF & value);
                        }
                    }


                    /// <summary>
                    /// Data Pointer Register: HL
                    /// </summary>
                    public int HL
                    {
                        get { return ((m_regArr[rH] << 8) | (m_regArr[rL])); }
                        set
                        {
                            m_regArr[rH] = (0xFF & (value >> 8));
                            m_regArr[rL] = (0xFF & value);
                        }
                    }


                    /// <summary>
                    /// Indexer for the 8-bit registers
                    /// </summary>
                    /// <param name="aRegIndex">The register index</param>
                    /// <returns>The value of Register specified by index</returns>
                    public int this[int aRegIndex]
                    {
                        get { return m_regArr[aRegIndex]; }
                        set { m_regArr[aRegIndex] = (0xFF & value); }
                    }
                }



                public class Flags
                {
                    int m_C;
                    int m_H;
                    int m_N;
                    int m_Z;


                    /// <summary>
                    /// Carry flag: C
                    /// </summary>
                    public int C
                    {
                        get { return m_C; }
                        set { m_C = (value != 0) ? 1 : 0; }
                    }


                    /// <summary>
                    /// Half-Carry flag: H
                    /// </summary>
                    public int H
                    {
                        get { return m_H; }
                        set { m_H = (value != 0) ? 1 : 0; }
                    }


                    /// <summary>
                    /// Subtract flag: N
                    /// </summary>
                    public int N
                    {
                        get { return m_N; }
                        set { m_N = (value != 0) ? 1 : 0; }
                    }


                    /// <summary>
                    /// Zero flag: Z
                    /// </summary>
                    public int Z
                    {
                        get { return m_Z; }
                        set { m_Z = (value != 0) ? 1 : 0; }
                    }


                    /// <summary>
                    /// Packed flags as an 8-bit register
                    /// </summary>
                    public int Packed
                    {
                        // Bits 0-3: unused (always zero)

                        get { return (m_C << 4 | m_H << 5 | m_N << 6 | m_Z << 7); }
                        set
                        {
                            m_C = ((value & 0x10) >> 4);
                            m_H = ((value & 0x20) >> 5);
                            m_N = ((value & 0x40) >> 6);
                            m_Z = ((value & 0x80) >> 7);
                        }
                    }
                }


            }


        }
        // namespace Sharp
    }
    // namespace Processors
}
// namespace xFF


