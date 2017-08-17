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

using UnityEngine;

namespace xFF
{
    namespace Frontend
    {
        namespace Unity3D
        {
            namespace GB
            {

                public class GBInput : MonoBehaviour
                {
                    int m_keysState;


                    public bool A
                    {
                        get
                        {
                            return Input.GetKey(KeyCode.X);
                        }
                    }


                    public bool B
                    {
                        get
                        {
                            return Input.GetKey(KeyCode.Z);
                        }
                    }


                    public bool Select
                    {
                        get
                        {
                            return Input.GetKey(KeyCode.RightShift);
                        }
                    }


                    public bool Start
                    {
                        get
                        {
                            return Input.GetKey(KeyCode.Return);
                        }
                    }


                    public bool DPadUp
                    {
                        get
                        {
                            return Input.GetKey(KeyCode.UpArrow);
                        }
                    }


                    public bool DPadDown
                    {
                        get
                        {
                            return Input.GetKey(KeyCode.DownArrow);
                        }
                    }


                    public bool DPadLeft
                    {
                        get
                        {
                            return Input.GetKey(KeyCode.LeftArrow);
                        }
                    }


                    public bool DPadRight
                    {
                        get
                        {
                            return Input.GetKey(KeyCode.RightArrow);
                        }
                    }


                    public int GetKeysState()
                    {
                        return m_keysState;
                    }


                    void Update( )
                    {
                        int keys = 0;

                        // Prevent both Right/Left presses
                        if (DPadRight)
                            keys |= DPadRight ? (1 << 0) : 0;
                        else
                            keys |= DPadLeft ? (1 << 1) : 0;

                        // Prevent both Up/Down presses 
                        if (DPadUp)
                            keys |= DPadUp ? (1 << 2) : 0;
                        else
                            keys |= DPadDown ? (1 << 3) : 0;

                        keys |= A ? (1 << 4) : 0;
                        keys |= B ? (1 << 5) : 0;
                        keys |= Select ? (1 << 6) : 0;
                        keys |= Start ? (1 << 7) : 0;

                        

                        m_keysState = ~keys;
                    }
                }

            }
            // namespace GB
        }
        // namespace Unity3D
    }
    // namespace Frontend
}
// namespace xFF
