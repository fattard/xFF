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
using System.Collections.Generic;
using xFF.PlatformSupport;

namespace xFF
{
    namespace Frontend
    {
        namespace Unity3D
        {
            namespace GB
            {


                public class MasterInput_GB : GBInput
                {
                    public MasterInput_GB() : base(null)
                    {
                    }


                    public override bool A
                    {
                        get
                        {
                            bool state = false;
                            for (int i = 0; (i < s_inputList.Count) && (!state); ++i)
                            {
                                state = s_inputList[i].A;
                            }

                            return state;
                        }
                    }


                    public override bool B
                    {
                        get
                        {
                            bool state = false;
                            for (int i = 0; (i < s_inputList.Count) && (!state); ++i)
                            {
                                state = s_inputList[i].B;
                            }

                            return state;
                        }
                    }


                    public override bool Select
                    {
                        get
                        {
                            bool state = false;
                            for (int i = 0; (i < s_inputList.Count) && (!state); ++i)
                            {
                                state = s_inputList[i].Select;
                            }

                            return state;
                        }
                    }


                    public override bool Start
                    {
                        get
                        {
                            bool state = false;
                            for (int i = 0; (i < s_inputList.Count) && (!state); ++i)
                            {
                                state = s_inputList[i].Start;
                            }

                            return state;
                        }
                    }


                    public override bool DPadUp
                    {
                        get
                        {
                            bool state = false;
                            for (int i = 0; (i < s_inputList.Count) && (!state); ++i)
                            {
                                state = s_inputList[i].DPadUp;
                            }

                            return state;
                        }
                    }


                    public override bool DPadDown
                    {
                        get
                        {
                            bool state = false;
                            for (int i = 0; (i < s_inputList.Count) && (!state); ++i)
                            {
                                state = s_inputList[i].DPadDown;
                            }

                            return state;
                        }
                    }


                    public override bool DPadLeft
                    {
                        get
                        {
                            bool state = false;
                            for (int i = 0; (i < s_inputList.Count) && (!state); ++i)
                            {
                                state = s_inputList[i].DPadLeft;
                            }

                            return state;
                        }
                    }


                    public override bool DPadRight
                    {
                        get
                        {
                            bool state = false;
                            for (int i = 0; (i < s_inputList.Count) && (!state); ++i)
                            {
                                state = s_inputList[i].DPadRight;
                            }

                            return state;
                        }
                    }


                    public override void AssignDefaultMappings()
                    {
                        
                    }


                    public override void ApplyCustomMappings(EmuCores.GB.ConfigsGB.InputProfile aProfile)
                    {
                        
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
