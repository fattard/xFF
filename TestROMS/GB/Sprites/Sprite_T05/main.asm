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


;;
;;  Sprite_T05
;;  ==========
;;
;;  Creates all 40 sprites: 5 rows of 8 sprites
;;  All sprites use Tile index 1 and block size 8x8, and palette OBP0
;;
;;



.INCLUDE "..\..\common\cartDefs.inc"


    ; Disable LCD
    xor A
    ld ($FF00+$40), A

    ld A, $E4
    ld ($FF00+$47), A ; BGP
    ld ($FF00+$48), A ; OBP0
    ld ($FF00+$49), A ; OBP1
    
    
    xor A
	ld HL, $9fff
Clear_VRAM:
	ld [HL-], A
	bit 7, H
	jr NZ, Clear_VRAM
    
    
    ld C, $00
    ld HL, $FE00
    ld B, $40
Clear_OAM:
    dec B
    ld A, $FF
    ld [HL+], A ; Pos Y
    ld [HL+], A ; Pos X
    ld [HL+], A ; Tile idx
    xor A
    ld [HL+], A ; Attributes
    cp B
    jp NZ, Clear_OAM
    
    
 
    ld HL, $8000
    ld B, $8
    
Fill_Tile0: ; Filled with BGP[0]
    dec B
    ld A, $00
    ld [HL+], A
    ld A, $00
    ld [HL+], A
    ld A, $00
    cp B
    jp NZ, Fill_Tile0
    
    
    ld B, $8
Fill_Tile1: ; Sprite filled dark
    dec B
    ld A, $FF
    ld [HL+], A
    ld A, $FF
    ld [HL+], A
    ld A, $00
    cp B
    jp NZ, Fill_Tile1
    
    
    ld HL, $FE00
    ld B, $1C
    ld D, $5  ;$5
    
Sprite_Row:
    dec D
    ld C, $1C
    ld E, $8 ;$8
Sprite_Data:
    dec E
    ld A, B
    ld [HL+], A ; Pos Y
    ld A, C
    ld [HL+], A ; Pos X
    ld A, $01
    ld [HL+], A ; Tile Idx 1
    ld A, $00
    ld [HL+], A ; Attributes
    ld A, $10
    add C
    ld C, A
    xor A
    cp E    
    jp NZ, Sprite_Data
    
    ld A, $1C
    add B
    ld B, A
    xor A
    cp D
    jp NZ, Sprite_Row
    
    
    ; Enable LCD
    ld A, $93
    ld ($FF00+$40), A
        
Loop:
	jp Loop

	
