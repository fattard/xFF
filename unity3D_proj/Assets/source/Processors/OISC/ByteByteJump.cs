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

                public delegate int MemoryBUSRead24Func(int aAddress);
                public delegate int MemoryBUSWrite24Func(int aAddress, int aValue);

                #endregion Delegates

                int m_PC;

                MemoryBUSRead24Func Read24;
                MemoryBUSWrite24Func Write24;


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
                /// <param name="aRead">A compatible delegate for reading 24 bits (Big Endian) value</param>
                /// <param name="aWrite">A compatible delegate for writing 24 bits (Big Endian) value</param>
                public void BindAddressBUS(MemoryBUSRead24Func aRead, MemoryBUSWrite24Func aWrite)
                {
                    Read24 = aRead;
                    Write24 = aWrite;
                }


                void Execute( )
                {
                    /* A, B, C
                     * 
                     * mem[A] <- mem[B]
                     * PC <- mem[C]
                     * 
                     */
                    {
                        Write24(m_PC + 3, Read24(m_PC));
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
