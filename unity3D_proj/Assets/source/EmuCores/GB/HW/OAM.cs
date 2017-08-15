﻿/*
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
    namespace EmuCores
    {
        namespace GB
        {
            namespace HW
            {


                public class OAM
                {
                    public class ObjAttributes
                    {
                        public int ID
                        {
                            get;
                            private set;
                        }

                        public int PosX
                        {
                            get;
                            set;
                        }

                        public int PosY
                        {
                            get;
                            set;
                        }

                        public int TileIdx
                        {
                            get;
                            set;
                        }

                        public int BGPriority
                        {
                            get;
                            set;
                        }

                        public bool FlipH
                        {
                            get;
                            set;
                        }

                        public bool FlipV
                        {
                            get;
                            set;
                        }

                        public int ObjPalIdx
                        {
                            get;
                            set;
                        }


                        public ObjAttributes(int aID)
                        {
                            ID = aID;
                            PosX = -16;
                            PosY = -8;
                        }


                        public void SetMiscData(int aValue)
                        {
                            ObjPalIdx = 0x01 & (aValue >> 4);
                            FlipH = (aValue & (1 << 5)) > 0;
                            FlipV = (aValue & (1 << 6)) > 0;
                            BGPriority = 0x01 & (aValue >> 7);
                        }

                        public int GetMiscData( )
                        {
                            return (ObjPalIdx << 4) | ((FlipH) ? (1 << 5) : 0) | ((FlipV) ? (1 << 6) : 0) | (BGPriority << 7);
                        }
                    }


                    ObjAttributes[] m_objAttrs;
                    //byte[] m_mem;


                    public OAM( )
                    {
                        m_objAttrs = new ObjAttributes[40];
                        for (int i = 0; i < m_objAttrs.Length; ++i)
                        {
                            m_objAttrs[i] = new ObjAttributes(i);
                        }

                        //m_mem = new byte[40 * 4];
                    }


                    public ObjAttributes GetObjAttributes(int aIdx)
                    {
                        return m_objAttrs[aIdx];
                    }


                    public int this[int aAddress]
                    {
                        get
                        {
                            
                            int baseObj = aAddress / 4;
                            int attr = aAddress % 4;

                            int ret = 0xFF;
                            switch (attr)
                            {
                                case 0:
                                    ret = m_objAttrs[baseObj].PosY;
                                    break;

                                case 1:
                                    ret = m_objAttrs[baseObj].PosX;
                                    break;

                                case 2:
                                    ret = m_objAttrs[baseObj].TileIdx;
                                    break;

                                case 3:
                                    ret = m_objAttrs[baseObj].GetMiscData();
                                    break;
                            }

                            return ret;
                            

                            //return m_mem[aAddress];
                        }

                        set
                        {
                            
                            int baseObj = aAddress / 4;
                            int attr = aAddress % 4;

                            switch (attr)
                            {
                                case 0:
                                    m_objAttrs[baseObj].PosY = (0xFF & value);
                                    break;

                                case 1:
                                    m_objAttrs[baseObj].PosX = (0xFF & value);
                                    break;

                                case 2:
                                    m_objAttrs[baseObj].TileIdx = (0xFF & value);
                                    break;

                                case 3:
                                    m_objAttrs[baseObj].SetMiscData(0xFF & value);
                                    break;
                            }
                            

                            //m_mem[aAddress] = (byte)(0xFF & value);
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
