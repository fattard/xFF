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

; Original boot:  23440324 total ticks
; Current Custom: 25614188 total ticks


; Offsets
.define VALID_CHECK_OFFSET      $0148 ; for Cart Header: ROM Size
.define LOGO_DATA_OFFSET        $0104 ; for the Logo stored in cart header
.define TRADEMARK_SYMBOL_OFFSET $00F5 ; for the TrademarkSymbol data
.define ANIM_TYPE_OFFSET        $00FD ; for the AnimType data


; Custom Boot anim type - can be patched in memory at ANIM_TYPE_OFFSET
.define CUSTOM_BOOT_TYPE_NO_ANIM        $00 ; instant boot
.define CUSTOM_BOOT_TYPE_QUICK_ANIM     $03 ; 1 sec anim, showing logo and playing sounds
.define CUSTOM_BOOT_TYPE_FULL_ANIM      $AA ; 6 sec anim, scrolls logo to center and play sounds


.define FORCE_FULL_ANIM
.define FORCE_QUICK_ANIM


; ---------- Code Init ----------

Start:
; Init stack pointer
    ld sp, $FFFE

; Clear memory VRAM - ($8000-$9FFF)
ClearVRAM:
    xor a
    ld hl, VRAM
clearVRAMLoop
    ldi [hl], a
    bit 5, h
    jr z, clearVRAMLoop
    
; Setup audio
SetupAudio:
    ld a, $80
    ldh [<rNR52], a     ; [$FF26] = $80 : Turn on audio
    ldh [<rNR11], a     ; [$FF11] = $80 : Channel 1 wave duty
    ld a, $F3
    ldh [<rNR12], a     ; [$FF12] = $F3 : Channel 1 envelope
    ldh [<rNR51], a     ; [$FF25] = $F3 : Sound output routing
    ld a, $77
    ldh [<rNR50], a     ; [$FF24] = $77 : Master volume
    
; Setup BG Palette
SetupBGP:
    ld a, $FC
    ldh [<rBGP], a      ; [$FF47] = MakePal(0, 3, 3, 3)
    
        
; Check for Inserted Cartridge
CheckInvalidROM:
    ld de, VALID_CHECK_OFFSET
    ld a, [DE]          ; When there are no inserted cartridge, this area reads as $FF (Header: ROM size)
    cp $FF              
    jr z, InvalidROMInserted ; Invalid ROM found

    
; Check for Custom Boot Anim Type
CheckCustomAnimType:
    ld de, ANIM_TYPE_OFFSET
    ld a, [DE]          ; Read the desired anim type 
    cp CUSTOM_BOOT_TYPE_NO_ANIM
    jr z, BootType_NoAnim
    push af ; stores custom boot anim type
    
    
; Routine to copy the logo data to the VRAM
LoadLogoToVRAM:
    ; A nibble represents a 4-pixels line, 2 bytes represent a 4x4 tile, scaled to 8x8.
    ; Tiles are ordered left to right, top to bottom.
    ld de, LOGO_DATA_OFFSET ; Logo start
    ld hl, VRAM+$10         ; Copy starting from second tile in VRAM
loadLogoLoop
    ld a, [de] ; Read 2 rows
    ld b, a
    call DoubleBitsAndWriteRow
    call DoubleBitsAndWriteRow
    inc de
    ld a, e
    xor $34 ; End of logo
    jr nz, loadLogoLoop
    ; Load trademark symbol
    ld de, TRADEMARK_SYMBOL_OFFSET
    ld c,$08
loadTrademarkSymbolLoop:
    ld a,[de]
    inc de
    ldi [hl],a
    inc hl
    dec c
    jr nz, loadTrademarkSymbolLoop
    ; Set up tilemap
    ld a,$19      ; Trademark symbol
    ld [$9910], a ; ... put in the superscript position
    ld hl,$992F   ; Bottom right corner of the logo
    ld c,$0C      ; Tiles in a logo row
tilemapLoop
    dec a
    jr z, tilemapDone
    ldd [hl], a
    dec c
    jr nz, tilemapLoop
    ld l,$0F ; Jump to top row
    jr tilemapLoop
tilemapDone

; Set vertical scroll register
    ld a, $64
    ldh [<rSCY], a    
    call TurnOnLCD
    
    pop af ; restore custom boot anim type
    cp CUSTOM_BOOT_TYPE_QUICK_ANIM
    jr z, BootType_QuickAnim ; skip to quick part
    
ScrollLogo:
    ld b, 3
    call WaitBFrames
    ldh a, [<rSCY]
    dec a
    ldh [<rSCY], a
    jr nz, ScrollLogo
    
BootType_QuickAnim
    xor a
    ldh [<rSCY], a  ; forces vertical scroll to zero
    call ShowLogoAndPlayAudioAnim
    jr FinishSetup 
   
 
BootType_NoAnim
    call TurnOnLCD
    
FinishSetup:
; Set registers to match the original DMG boot
    ld hl, $01B0
    push hl
    pop af
    ld hl, $014D
    ld bc, $0013
    ld de, $00D8
    
; Ends boot process and starts game
    jr BootEnd
    

    
; Invalid ROM size was read, must trap here
InvalidROMInserted
    call TurnOnLCD
bootTrapped
    jr bootTrapped      ; ------ traps execution here ------
    


; Routine to turn LCD on and wait for a VBlank
TurnOnLCD:
; Init LCD
    ld a, $91
    ldh [<rLCDC], a     ; [$FF40] = (LCD_EN | BG_EN | CHAR_ADDR_0) : Turn on LCD
; Wait for VBlank
    call WaitFrame
    ret
    
    
    
; Routine to show Logo and play sounds
ShowLogoAndPlayAudioAnim:
; Wait ~0.75 seconds
    ld b, 5
    call WaitBFrames

    ; Play first sound
    ld a, $83
    call PlaySound
    ld b, 5
    call WaitBFrames
    ; Play second sound
    ld a, $C1
    call PlaySound

; Wait ~0.9 seconds
    ld b, 50
    call WaitBFrames
    ret
    

    
; Routine to expand and scale Logo data into VRAM 
; by doubling the most significant 4 bits, then b is shifted by 4
DoubleBitsAndWriteRow:
    ld a, 4
    ld c, 0
doubleCurrentBit
    sla b
    push af
    rl c
    pop af
    rl c
    dec a
    jr nz, doubleCurrentBit
    ld a, c
; Write as two rows
    ldi [hl], a
    inc hl
    ldi [hl], a
    inc hl
    ret
    
    
    
; Routine to wait for a frame
WaitFrame:
    ldh a, [<rLY]
    cp 143
    jr nz, WaitFrame
wait_ly_144
    ldh a, [<rLY]
    cp 144
    jr nz, wait_ly_144
    ret

    
; Routine to wait a specific number of frames
WaitBFrames:
    call WaitFrame
    dec b
    jr nz, WaitBFrames
    ret

    
; Routine to play the logo tones
PlaySound:
    ldh [<rNR13], a
    ld a, $87
    ldh [<rNR14], a
    ret
    
    
.org TRADEMARK_SYMBOL_OFFSET
    .db $3C,$42,$B9,$A5,$B9,$A5,$42,$3C
    
.org ANIM_TYPE_OFFSET
; Compile definition of hardcoded boot type anim data
.ifdef FORCE_FULL_ANIM
    .db CUSTOM_BOOT_TYPE_FULL_ANIM
.else .ifdef FORCE_QUICK_ANIM  
    .db CUSTOM_BOOT_TYPE_QUICK_ANIM
.else
    .db CUSTOM_BOOT_TYPE_NO_ANIM
.endif
.endif

    
; ---------- Last code ----------
.org $00FE
BootEnd
    ldh [<rBOOT], a   ; Turn off BootROM and enables IRQ vectors access     
