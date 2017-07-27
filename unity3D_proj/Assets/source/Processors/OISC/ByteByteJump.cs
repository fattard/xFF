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
        namespace OISC
        {

            public class ByteByteJump
            {
                #region Delegates


                public delegate int MemoryBUSRead8Func(int aAddress);
                public delegate void MemoryBUSWrite8Func(int aAddress, int aValue);
                public delegate int MemoryBUSRead24Func(int aAddress);
                public delegate void MemoryBUSWrite24Func(int aAddress, int aValue);

                #endregion Delegates

                int m_PC;

                MemoryBUSRead8Func Read8;
                MemoryBUSWrite8Func Write8;
                MemoryBUSRead24Func Read24;
                MemoryBUSWrite24Func Write24;


                /// <summary>
                /// Program Counter
                /// </summary>
                public int RegPC
                {
                    get { return m_PC; }
                    set { m_PC = (0xFFFFFF & value); }
                }


                public ByteByteJump( )
                {
                    Reset();
                }


                /// <summary>
                /// Resets the processor state
                /// </summary>
                public void Reset( )
                {
                    m_PC = 0;
                }


                /// <summary>
                /// Binds delegates to handle memory access.
                /// </summary>
                /// <param name="aRead8">A compatible delegate for reading 8 bits value</param>
                /// <param name="aWrite8">A compatible delegate for writing 8 bits value</param>
                /// <param name="aRead24">A compatible delegate for reading 24 bits (Big Endian) value</param>
                /// <param name="aWrite24">A compatible delegate for writing 24 bits (Big Endian) value</param>
                public void BindAddressBUS(MemoryBUSRead8Func aRead8, MemoryBUSWrite8Func aWrite8, MemoryBUSRead24Func aRead24, MemoryBUSWrite24Func aWrite24)
                {
                    Read8 = aRead8;
                    Write8 = aWrite8;

                    Read24 = aRead24;
                    Write24 = aWrite24;
                }


                /// <summary>
                /// Process 1 instruction cycle
                /// </summary>
                public void Step( )
                {
                    Execute();
                }


                void Execute( )
                {
                    /* A, B, C
                     * 
                     * mem[B] <- mem[A]
                     * PC <- C
                     * 
                     */
                    {
                        /*
                         *      // Full Code:
                         *
                         *      int A = Read24(m_PC);  
                         *      int B = Read24(m_PC + 3);
                         * 
                         *      // mem[B] = mem[A];
                         *      Write8(B, Read8(A)); 
                         * 
                         *      m_PC = Read24(m_PC + 6); // C
                         * 
                         */


                        Write8(Read24(m_PC + 3), Read8(Read24(m_PC)));
                        m_PC = Read24(m_PC + 6);
                    }
                }
            }

        }
        // namespace OISC
    }
    // namespace Processors
}
// namespace xFF
