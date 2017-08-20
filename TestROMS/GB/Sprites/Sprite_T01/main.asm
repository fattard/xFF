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
;;  Sprite_T01
;;  ==========
;;
;;  Creates 5 sprites: one at each corner of screen,
;;  and the last one in the middle center.
;;  All sprites use Tile index 1 and block size 8x8, and palette OBP0
;;
;;



.INCLUDE "..\..\common\cartDefs.inc"


    ld B, $90 ; scanline 144
Wait_VBL:
    ld A, ($FF44)
    cp B
    jp NZ, Wait_VBL


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
    ; Sprite data 1 - Top Left
    ld A, $10
    ld [HL+], A ; Pos Y
    ld A, $8
    ld [HL+], A ; Pos X
    ld A, $01
    ld [HL+], A ; Tile Idx 1
    ld A, $00
    ld [HL+], A ; Attributes
    
    ; Sprite data 2 - Bottom Left
    ld A, $98
    ld [HL+], A ; Pos Y
    ld A, $8
    ld [HL+], A ; Pos X
    ld A, $01
    ld [HL+], A ; Tile Idx 1
    ld A, $00
    ld [HL+], A ; Attributes
    
    ; Sprite data 3 - Top Right
    ld A, $10
    ld [HL+], A ; Pos Y
    ld A, $A0
    ld [HL+], A ; Pos X
    ld A, $01
    ld [HL+], A ; Tile Idx 1
    ld A, $00
    ld [HL+], A ; Attributes
    
    ; Sprite data 4 - Bottom Right
    ld A, $98
    ld [HL+], A ; Pos Y
    ld A, $A0
    ld [HL+], A ; Pos X
    ld A, $01
    ld [HL+], A ; Tile Idx 1
    ld A, $00
    ld [HL+], A ; Attributes
    
    
    ; Sprite data 4 - Middle Center
    ld A, $54
    ld [HL+], A ; Pos Y
    ld A, $54
    ld [HL+], A ; Pos X
    ld A, $01
    ld [HL+], A ; Tile Idx 1
    ld A, $00
    ld [HL+], A ; Attributes
    
    
    ; Enable LCD
    ld A, $93
    ld ($FF00+$40), A
        
Loop:
    jp Loop

    
