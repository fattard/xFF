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


.MEMORYMAP
SLOTSIZE $4000
DEFAULTSLOT 0
SLOT 0 $0000
SLOT 1 $4000
.ENDME

.ROMBANKSIZE $4000
.ROMBANKS 2

.RAMSIZE 0
.EMPTYFILL $FF
.CARTRIDGETYPE 0
.COMPUTEGBCHECKSUM
.COMPUTEGBCOMPLEMENTCHECK
.ROMDMG
.NAME "TEST ROM"




; --- Cartridge header ---

.bank 0 slot 0

.org $000
.section "Bootstrap-Jump"
  .db $00 ; nop
  jp $150
.ends

.org $100
.section "Header" force
  .db $00 ; nop
  jp $150
.ends


.org $104
.section "Nintendo-Logo" force

.DB $CE $ED $66 $66 $CC $0D $00 $0B $03 $73 $00 $83 $00 $0C $00 $0D
.DB $00 $08 $11 $1F $88 $89 $00 $0E $DC $CC $6E $E6 $DD $DD $D9 $99
.DB $BB $BB $67 $63 $6E $0E $EC $CC $DD $DC $99 $9F $BB $B9 $33 $3E
.ends

.org $14C
.section "Header-Extra" force
  .db $00 ; ROM version
.ends


.orga $150
