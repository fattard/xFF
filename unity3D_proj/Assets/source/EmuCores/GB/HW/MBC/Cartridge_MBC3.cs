/*
*   This file is part of xFF
*   Copyright (C) 2017-2021 Fabio Attard
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
using System.Collections.Generic;

namespace xFF
{
    namespace EmuCores
    {
        namespace GB
        {
            namespace HW
            {
                namespace MBC
                {


                    public class Cartridge_MBC3 : Cartridge
                    {
                        public struct RTCData
                        {
                            public int sec;
                            public int min;
                            public int hour;
                            public int dayL;
                            public int miscData;

                            /// <summary>
                            /// Misc data - bit 0
                            /// </summary>
                            public int dayH
                            {
                                get => (miscData & 0x1);
                                set => miscData = (0xC0 & miscData) | (0x1 & value);
                            }

                            /// <summary>
                            /// Misc data - bit 6
                            /// </summary>
                            public int halt
                            {
                                get => ((miscData >> 6) & 0x1);
                                set => miscData = (0x81 & miscData) | ((0x1 & value) << 6);
                            }

                            /// <summary>
                            /// Misc data - bit 7
                            /// </summary>
                            public int day_carry
                            {
                                get => ((miscData >> 7) & 0x1);
                                set => miscData = (0x41 & miscData) | ((0x1 & value) << 7);
                            }


                            public void Tick()
                            {
                                if (halt == 0)
                                {
                                    ++sec;

                                    if (sec == 60)
                                    {
                                        sec = 0;
                                        ++min;

                                        if (min == 60)
                                        {
                                            min = 0;
                                            ++hour;

                                            if (hour == 24)
                                            {
                                                hour = 0;
                                                int day = dayL + 1;
                                                ++dayL;

                                                if (day > 0xFF)
                                                {
                                                    dayL = 0;

                                                    if (dayH == 1)
                                                    {
                                                        // day carry sticks to on, until explicitely reset manually
                                                        day_carry = 1;
                                                        dayH = 0;
                                                    }
                                                    else
                                                    {
                                                        dayH = 1;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    // Rollover bit range
                                    sec &= 0x3F;
                                    min &= 0x3F;
                                    hour &= 0x1F;

                                    UnityEngine.Debug.Log(string.Format("isOn={0} carry={1} day={2} {3:d2}:{4:d2}:{5:d2}\n", halt, day_carry, (dayH << 8) | (dayL), hour, min, sec));
                                }
                            }
                        }


                        int m_curBank_rom;
                        int m_curBank_sram;
                        bool m_isRAMEnabled;
                        int m_clockRegH;
                        int m_clockLatch;
                        int m_mask;
                        RTCData m_rtcData = new RTCData();
                        RTCData m_rtcData_latched = new RTCData();

                        int m_elapsedCycles;

                        bool m_usesSystemSyncedTime;
                        long m_lastRealtime;


                        public Cartridge_MBC3(CartridgeHeader aHeader)
                        {
                            m_cartHeader = aHeader;

                            int totalROMBanks = 0;

                            switch (m_cartHeader.ROMSize)
                            {
                                case 0x01: // 64KB (4 banks)
                                    totalROMBanks = 4;
                                    break;

                                case 0x02: // 128KB (8 banks)
                                    totalROMBanks = 8;
                                    break;

                                case 0x03: // 256KB (16 banks)
                                    totalROMBanks = 16;
                                    break;

                                case 0x04: // 512KB (32 banks)
                                    totalROMBanks = 32;
                                    break;

                                case 0x05: // 1MB (64 banks)
                                    totalROMBanks = 64;
                                    break;

                                case 0x06: // 2MB (128 banks)
                                    totalROMBanks = 128;
                                    break;

                                case 0x07: // 4MB (256 banks) - MBC30
                                    totalROMBanks = 256;
                                    break;

                                default:
                                    totalROMBanks = 2; // 32KB (2 banks)
                                    break;
                            }

                            m_mask = (totalROMBanks - 1);

                            while (m_romBanks.Count < totalROMBanks)
                            {
                                m_romBanks.Add(new byte[0x4000]);
                            }


                            int totalRAMBanks = 0;

                            switch (m_cartHeader.RAMSize)
                            {
                                case 0:
                                case 1: // value $01 was supposed to have 2KB RAM, but never proved
                                    totalRAMBanks = 0;
                                    break;

                                case 2:
                                    totalRAMBanks = 1;
                                    break;

                                case 3:
                                    totalRAMBanks = 4;
                                    break;

                                case 5:
                                    totalRAMBanks = 8; // MBC30
                                    break;

                                default:
                                    totalRAMBanks = 4;
                                    break;
                            }

                            while (m_ramBanks.Count < totalRAMBanks)
                            {
                                m_ramBanks.Add(new byte[0x2000]);

                                // Inits to 0xFF
                                for (int i = 0; i < m_ramBanks[m_ramBanks.Count - 1].Length; ++i)
                                {
                                    m_ramBanks[m_ramBanks.Count - 1][i] = 0xFF;
                                }
                            }

                            m_curBank_rom = 1;
                            m_curBank_sram = 0;
                            m_isRAMEnabled = false;
                            m_clockLatch = 0;
                            m_clockRegH = 0;

                            m_elapsedCycles = 0;
                            m_usesSystemSyncedTime = true;
                            m_lastRealtime = System.DateTimeOffset.Now.ToUnixTimeSeconds();

                            // RTC Support
                            if ((m_cartHeader.CartType == 0x0F) || (m_cartHeader.CartType == 0x10))
                            {
                                EmuGB.Instance.CPU.core.BindCyclesStep(CyclesStep);
                            }
                        }


                        public override int this[int aOffset]
                        {
                            get
                            {
                                if (aOffset < 0x4000)
                                {
                                    return m_romBanks[0][aOffset];
                                }

                                else if (aOffset < 0x8000)
                                {
                                    return m_romBanks[(m_mask & m_curBank_rom)][aOffset - 0x4000];
                                }

                                // SRAM
                                else if (aOffset >= 0xA000 && aOffset <= 0xBFFF)
                                {
                                    if (!m_isRAMEnabled || (m_ramBanks.Count == 0 && m_clockRegH == 0))
                                    {
                                        return 0xFF;
                                    }

                                    if (m_clockRegH == 0)
                                    {
                                        return m_ramBanks[m_curBank_sram][aOffset - 0xA000];
                                    }

                                    switch (m_clockRegH | m_curBank_sram)
                                    {
                                        case 0x08:
                                            return m_rtcData_latched.sec;

                                        case 0x09:
                                            return m_rtcData_latched.min;

                                        case 0x0A:
                                            return m_rtcData_latched.hour;

                                        case 0x0B:
                                            return m_rtcData_latched.dayL;

                                        case 0x0C:
                                            return m_rtcData_latched.miscData;

                                        default:
                                            break;
                                    }
                                }

                                // Invalid offset
                                return 0xFF;
                            }

                            set
                            {
                                if (aOffset <= 0x1FFF)
                                {
                                    m_isRAMEnabled = (0x0F & value) == 0x0A ? true : false;
                                }

                                else if (aOffset >= 0x2000 && aOffset <= 0x3FFF)
                                {
                                    m_curBank_rom = (m_mask & value);
                                    if (m_curBank_rom == 0)
                                    {
                                        m_curBank_rom = 1;
                                    }
                                }

                                else if (aOffset >= 0x4000 && aOffset <= 0x5FFF)
                                {
                                    m_curBank_sram = (0x3 & value);
                                    m_clockRegH = (0xC & value);
                                }

                                else if (aOffset >= 0x6000 && aOffset <= 0x7FFF)
                                {
                                    if ((m_clockLatch == 1) && (value == 0))
                                    {
                                        m_clockLatch = 0;
                                    }

                                    else if ((m_clockLatch == 0) && (value == 1))
                                    {
                                        m_clockLatch = 1;
                                        LatchRTC();
                                    }
                                }

                                // SRAM
                                else if (aOffset >= 0xA000 && aOffset <= 0xBFFF)
                                {
                                    if (!m_isRAMEnabled || (m_ramBanks.Count == 0 && m_clockRegH == 0))
                                    {
                                        // Ignore writes
                                        return;
                                    }

                                    if (m_clockRegH == 0)
                                    {
                                        m_ramBanks[m_curBank_sram][aOffset - 0xA000] = (byte)(0xFF & value);
                                    }

                                    else
                                    {
                                        switch (m_clockRegH | m_curBank_sram)
                                        {
                                            case 0x08:
                                                m_rtcData.sec = (0x3F & value);
                                                break;

                                            case 0x09:
                                                m_rtcData.min = (0x3F & value);
                                                break;

                                            case 0x0A:
                                                m_rtcData.hour = (0x1F & value);
                                                break;

                                            case 0x0B:
                                                m_rtcData.dayL = value;
                                                break;

                                            case 0x0C:
                                                m_rtcData.miscData = (0xC1 & value);
                                                break;

                                            default:
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                        

                        public override void SaveRAM(string filePath)
                        {
                            if (m_ramBanks.Count == 0)
                            {
                                return;
                            }

                            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(32 * 1024))
                            {
                                for (int i = 0; i < m_ramBanks.Count; ++i)
                                {
                                    ms.Write(m_ramBanks[i], 0, m_ramBanks[i].Length);
                                }

                                // Append RTC data
                                if ((m_cartHeader.CartType == 0x0F) || (m_cartHeader.CartType == 0x10))
                                {
                                    using (System.IO.BinaryWriter bw = new System.IO.BinaryWriter(ms))
                                    {
                                        bw.Write(m_rtcData.sec);
                                        bw.Write(m_rtcData.min);
                                        bw.Write(m_rtcData.hour);
                                        bw.Write(m_rtcData.dayL);
                                        bw.Write(m_rtcData.miscData);

                                        bw.Write(m_rtcData_latched.sec);
                                        bw.Write(m_rtcData_latched.min);
                                        bw.Write(m_rtcData_latched.hour);
                                        bw.Write(m_rtcData_latched.dayL);
                                        bw.Write(m_rtcData_latched.miscData);

                                        bw.Write(m_lastRealtime);
                                    }
                                }

                                System.IO.File.WriteAllBytes(filePath, ms.ToArray());
                            }
                        }


                        public override void LoadRAM(string filePath)
                        {
                            if (!System.IO.File.Exists(filePath))
                            {
                                return;
                            }

                            byte[] sav = System.IO.File.ReadAllBytes(filePath);

                            var size = m_ramBanks.Count * m_ramBanks[0].Length;

                            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(sav))
                            {
                                for (int i = 0; i < m_ramBanks.Count; ++i)
                                {
                                    ms.Read(m_ramBanks[i], 0, m_ramBanks[i].Length);
                                }

                                // Checks if RTC data is expected appended at file footer
                                if ((m_cartHeader.CartType == 0x0F) || (m_cartHeader.CartType == 0x10))
                                {
                                    int rtcFooterSize = (sav.Length - size);
                                    if (rtcFooterSize == 0x30 || rtcFooterSize == 0x2C)
                                    {
                                        using (System.IO.BinaryReader br = new System.IO.BinaryReader(ms))
                                        {
                                            m_rtcData.sec = br.ReadInt32();
                                            m_rtcData.min = br.ReadInt32();
                                            m_rtcData.hour = br.ReadInt32();
                                            m_rtcData.dayL = br.ReadInt32();
                                            m_rtcData.miscData = br.ReadInt32();

                                            m_rtcData_latched.sec = br.ReadInt32();
                                            m_rtcData_latched.min = br.ReadInt32();
                                            m_rtcData_latched.hour = br.ReadInt32();
                                            m_rtcData_latched.dayL = br.ReadInt32();
                                            m_rtcData_latched.miscData = br.ReadInt32();

                                            m_lastRealtime = br.ReadInt32();

                                            if (rtcFooterSize == 0x30)
                                            {
                                                m_lastRealtime |= ((long)br.ReadInt32() << 32);
                                            }
                                        }
                                    }
                                }
                            }
                        }


                        public static bool Validate(CartridgeHeader aHeader)
                        {
                            if (aHeader.CartType != 0x0F && aHeader.CartType != 0x10 && aHeader.CartType != 0x11 &&
                                aHeader.CartType != 0x12 && aHeader.CartType != 0x13)
                            {
                                return false;
                            }

                            if (aHeader.ROMSize > 0x07) // Limited to 128 banks of 32KB (256 banks of 32KB in MBC30)
                            {
                                return false;
                            }

                            if ((aHeader.RAMSize > 0x04)
                                && (aHeader.RAMSize != 0x05)) // Limited to 4 banks of 8KB (8 banks of 8KB in MBC30)
                            {
                                return false;
                            }

                            return true;
                        }


                        void LatchRTC()
                        {
                            m_rtcData_latched = m_rtcData;
                        }


                        void CyclesStep(int aElapsedCycles)
                        {
                            if (m_usesSystemSyncedTime)
                            {
                                var now = System.DateTimeOffset.Now.ToUnixTimeSeconds();
                                var elapsedSecs = (now - m_lastRealtime);

                                while ((m_rtcData.halt == 0) && (elapsedSecs > 0))
                                {
                                    m_rtcData.Tick();
                                    elapsedSecs--;
                                }

                                m_lastRealtime = now;
                            }

                            else if (!m_usesSystemSyncedTime && (m_rtcData.halt == 0))
                            {
                                m_elapsedCycles += aElapsedCycles;

                                if (m_elapsedCycles >= 4194304)
                                {
                                    m_rtcData.Tick();
                                    m_elapsedCycles -= 4194304;
                                }
                            }
                        }
                    }


                }
                // namespace MBC
            }
            // namespace HW
        }
        // namespace GB
    }
    // namespace EmuCores
}
// namespace xFF
