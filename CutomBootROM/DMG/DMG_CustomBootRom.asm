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


.include "hardware.inc"



; --- Binary format ---
.rombanksize $100
.rombanks 1

; Target: total ticks 23440324


; Offset for Cart Header: ROM Size
.define VALID_CHECK_OFFSET $0148



; ---------- Code Init ----------

Start:
; Init stack pointer
    ld sp, $FFFE

; Clear memory VRAM - ($8000-$9FFF)
    xor a
    ld hl, VRAM
clearVRAMLoop
    ldi [hl], a
    bit 5, h
    jr z, clearVRAMLoop
    
; Setup audio
    ld a, $80
    ldh [<rNR52], a     ; [$FF26] = $80 : Turn on audio
    ldh [<rNR11], a     ; [$FF11] = $80 : Channel 1 wave duty
    ld a, $F3
    ldh [<rNR12], a     ; [$FF12] = $F3 : Channel 1 envelope
    ldh [<rNR51], a     ; [$FF25] = $F3 : Channel routing
    ld a, $C1
    ldh [<rNR13], a     ; [$FF13] = $C1 : Channel 1 low frequency byte
    ld a, $77
    ldh [<rNR50], a     ; [$FF24] = $77 : Master volume
    
; Setup BG Palette
    ld a, $FC
    ldh [<rBGP], a      ; [$FF47] = MakePal(0, 3, 3, 3)
    
    
    
; Check for Inserted Cartridge
    ld de, VALID_CHECK_OFFSET
    ld a, [DE]          ; When there are no inserted cartridge, this area reads as $FF (Header: ROM size)
    cp $FF              
    jr z, InvalidROMInserted ; Invalid ROM found
    inc de
    ld a, [DE]          ; When there are no inserted cartridge, this area reads as $FF (Header: SRAM size)
    cp $FF
    jr z, InvalidROMInserted ; Invalid ROM found
    jr FinishSetup
    
InvalidROMInserted
    call TurnOnLCD
bootTrapped
    jr bootTrapped
    
    
    
FinishSetup  
    call TurnOnLCD
; Set registers to match the original DMG boot
    ld hl, $01B0
    push hl
    pop af
    ld hl, $014D
    ld bc, $0013
    ld de, $00D8
    
; Ends boot process and starts game
    jp BootEnd
    
    
    

; Routine to turn LCD on and wait for a VBlank
TurnOnLCD:
; Init LCD
    ld a, $91
    ldh [<rLCDC], a     ; [$FF40] = (LCD_EN | BG_EN | CHAR_OFF_0)  : Turn on LCD
    
; Wait for VBlank
_wait_ly_143            ; wait for LY = 143 first to ensure a fresh vblank
    ldh a, (<rLY)
    cp 143
    jr nz, _wait_ly_143
_wait_ly_144            ; wait for LY = 144
    ldh a, (<rLY)
    cp 144
    jr nz, _wait_ly_144
    ret



.org $00FE

BootEnd:
    ldh [<rBOOT], a   ; Turn off BootROM and enables ROMBank 0 access     
