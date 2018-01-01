;;
;;
;;   This file is part of xFF
;;   Copyright (C) 2017 Fabio Attard
;;
;;   This program is free software: you can redistribute it and/or modify
;;   it under the terms of the GNU General Public License as published by
;;   the Free Software Foundation, either version 3 of the License, or
;;   (at your option) any later version.
;;
;;   This program is distributed in the hope that it will be useful,
;;   but WITHOUT ANY WARRANTY; without even the implied warranty of
;;   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
;;   GNU General Public License for more details.
;;
;;   You should have received a copy of the GNU General Public License
;;   along with this program.  If not, see <http://www.gnu.org/licenses/>.
;;
;;   Additional Terms 7.b and 7.c of GPLv3 apply to this file:
;;       * Requiring preservation of specified reasonable legal notices or
;;         author attributions in that material or in the Appropriate Legal
;;         Notices displayed by works containing it.
;;       * Prohibiting misrepresentation of the origin of that material,
;;         or requiring that modified versions of such material be marked in
;;         reasonable ways as different from the original version.
;;
;;



.memorymap
  defaultslot 1
  slot 0 start $0000 size $4000
  slot 1 start $4000 size $4000
  slot 2 start $C000 size $1000
  slot 3 start $D000 size $1000
  slot 4 start $A000 size $2000
  slot 5 start $FF80 size $007F
.endme

.define INTR_VBLANK (1 << 0)
.define INTR_STAT   (1 << 1)
.define INTR_TIMER  (1 << 2)
.define INTR_SERIAL (1 << 3)
.define INTR_JOYPAD (1 << 4)

.define INTR_VEC_VBLANK $40
.define INTR_VEC_STAT   $48
.define INTR_VEC_TIMER  $50
.define INTR_VEC_SERIAL $58
.define INTR_VEC_JOYPAD $60

.define VRAM  $8000
.define OAM   $FE00
.define HIRAM $FF80

.define VRAM_LEN  $2000
.define OAM_LEN   $A0
.define HIRAM_LEN $7F

.define rP1    $FF00
.define rSB    $FF01
.define rSC    $FF02
.define rDIV   $FF04
.define rTIMA  $FF05
.define rTMA   $FF06
.define rTAC   $FF07
.define rIF    $FF0F
.define rNR10  $FF10
.define rNR11  $FF11
.define rNR12  $FF12
.define rNR13  $FF13
.define rNR14  $FF14
.define rNR21  $FF16
.define rNR22  $FF17
.define rNR23  $FF18
.define rNR24  $FF19
.define rNR30  $FF1A
.define rNR31  $FF1B
.define rNR32  $FF1C
.define rNR33  $FF1D
.define rNR34  $FF1E
.define rNR41  $FF20
.define rNR42  $FF21
.define rNR43  $FF22
.define rNR44  $FF23
.define rNR50  $FF24
.define rNR51  $FF25
.define rNR52  $FF26
.define rLCDC  $FF40
.define rSTAT  $FF41
.define rSCY   $FF42
.define rSCX   $FF43
.define rLY    $FF44
.define rLYC   $FF45
.define rDMA   $FF46
.define rBGP   $FF47
.define rOBP0  $FF48
.define rOBP1  $FF49
.define rWY    $FF4A
.define rWX    $FF4B
.define rKEY1  $FF4D
.define rVBK   $FF4F
.define rBOOT  $FF50
.define rHDMA1 $FF51
.define rHDMA2 $FF52
.define rHDMA3 $FF53
.define rHDMA4 $FF54
.define rHDMA5 $FF55
.define rRP    $FF56
.define rBCPS  $FF68
.define rBCPD  $FF69
.define rOCPS  $FF6A
.define rOCPD  $FF6B
.define rSVBK  $FF70
.define rIE    $FFFF
