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

using System.Collections.Generic;

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
                    public class ObjAttributes : System.IComparable
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


                        private int m_fullMiscData;


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

                            m_fullMiscData = aValue;
                        }

                        public int GetMiscData( )
                        {
                            return m_fullMiscData;
                        }


                        public int CompareTo(object aObj)
                        {
                            int v = ((aObj as ObjAttributes).PosX - PosX);
                            if (v > 0)
                            {
                                return 1;
                            }
                            
                            else if (v == 0)
                            {
                                // Same pos, compares idx
                                return ((aObj as ObjAttributes).ID > ID) ? 1 : -1;
                            }

                            return -1;
                        }
                    }

                    ObjAttributes[] m_objAttrs;
                    List<ObjAttributes> m_objAttrsSorted;
                    //byte[] m_mem;


                    public OAM( )
                    {
                        m_objAttrs = new ObjAttributes[40];
                        m_objAttrsSorted = new List<ObjAttributes>(m_objAttrs.Length);
                        for (int i = 0; i < m_objAttrs.Length; ++i)
                        {
                            m_objAttrs[i] = new ObjAttributes(i);
                            m_objAttrsSorted.Add(m_objAttrs[i]);
                        }

                        //m_mem = new byte[40 * 4];
                    }


                    public ObjAttributes GetObjAttributes(int aIdx)
                    {
                        return m_objAttrs[aIdx];
                    }

                    public ObjAttributes GetObjAttributesSorted(int aIdx)
                    {
                        return m_objAttrsSorted[aIdx];
                    }


                    public void SortByPosX( )
                    {
                        m_objAttrsSorted.Sort();
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
