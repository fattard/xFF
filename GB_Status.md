## GB Emu - Processor Sharp LR35902
#### Legend:
- :soon: - in progress
- :heavy_check_mark: - Done
- :o: - Implemented but inaccurate
- :x: - Implemented but incorrect


Hardware Mode | Supported
------------- | ---------
DMG (Game Boy)
MGB/MGL (Game Boy Pocket/Light)
SGB (Super Game Boy)
SGB2 (Super Game Boy 2)
CGB (Game Boy Color)


8-bit Transfers  |   Implementation   | Checked Operation  |   Checked Flags    |   Checked Timing
---------------  | ------------------ | ------------------ | ------------------ | ------------------
LD r, r'
LD r, n
LD r, [HL]
LD [HL], r
LD [HL], n
LD A, [BC]
LD A, [DE]
LDH A, [$FF00+C]
LDH [$FF00+C], A
LDH A, [$FF00+n]
LDH [$FF00+n], A
LD A, [nn]
LD [nn], A
LD A, [HL+]
LD A, [HL-]
LD [BC], A
LD [DE], A
LD [HL+], A
LD [HL-], A

16-bit Transfers |   Implementation   | Checked Operation  |   Checked Flags    |   Checked Timing
---------------- | ------------------ | ------------------ | ------------------ | ------------------
LD dd, nn
LD SP, HL
PUSH qq
POP qq
LD HL, SP+e
LD [nn], SP

8-bit ALU |   Implementation   | Checked Operation  |   Checked Flags    |   Checked Timing
--------- | ------------------ | ------------------ | ------------------ | ------------------
ADD r
ADD n
ADD [HL]
ADC r
ADC n
ADC [HL]
SUB r
SUB n
SUB [HL]
SBC r
SBC n
SBC [HL]
AND r
AND n
AND [HL]
OR r
OR n
OR [HL]
XOR r
XOR n
XOR [HL]
CP r
CP n
CP [HL]
INC r
INC [HL]
DEC r
DEC [HL]

16-bit Arithmetic |   Implementation   | Checked Operation  |   Checked Flags    |   Checked Timing
----------------- | ------------------ | ------------------ | ------------------ | ------------------
ADD HL, ss
ADD SP, e
INC ss
DEC ss

Rotate / Shift |   Implementation   | Checked Operation  |   Checked Flags    |   Checked Timing
-------------- | ------------------ | ------------------ | ------------------ | ------------------
RLCA
RLA
RRCA
RRA
RLC r
RLC [HL]
RL r
RL [HL]
RRC r
RRC [HL]
RR r
RR [HL]
SLA r
SLA [HL]
SRA r
SRA [HL]
SRL r
SRL [HL]
SWAP r
SWAP [HL]

Bit Operations |   Implementation   | Checked Operation  |   Checked Flags    |   Checked Timing
-------------- | ------------------ | ------------------ | ------------------ | ------------------
BIT b, r
BIT b, [HL]
SET b, r
SET b, [HL]
RES b, r
RES b, [HL]

Jump Instructions |   Implementation   | Checked Operation  |   Checked Flags    |   Checked Timing
----------------- | ------------------ | ------------------ | ------------------ | ------------------
JP nn             | :heavy_check_mark: |                    |                    |                   
JP NZ, nn         | :heavy_check_mark: |                    |                    |                   
JP Z, nn          | :heavy_check_mark: |                    |                    |                   
JP NC, nn         | :heavy_check_mark: |                    |                    |                   
JP C, nn          | :heavy_check_mark: |                    |                    |                   
JR e              | :heavy_check_mark: |                    |                    |                   
JR NZ, e          | :heavy_check_mark: |                    |                    |                   
JR Z, e           | :heavy_check_mark: |                    |                    |                   
JR NC, e          | :heavy_check_mark: |                    |                    |                   
JR C, e           | :heavy_check_mark: |                    |                    |                   
JP HL             | :heavy_check_mark: |                    |                    |                   

Call / Return |   Implementation   | Checked Operation  |   Checked Flags    |   Checked Timing
------------- | ------------------ | ------------------ | ------------------ | ------------------
CALL nn
CALL NZ, nn
CALL Z, nn
CALL NC, nn
CALL C, nn
RET
RETI
RET NZ
RET Z
RET NC
RET C
RST t

Misc             |   Implementation   | Checked Operation  |   Checked Flags    |   Checked Timing
---------------- | ------------------ | ------------------ | ------------------ | ------------------
DAA
CPL
NOP              | :heavy_check_mark: |                    |                    |                   
CCF
SCF
DI
EI
HALT
STOP


## GB Emu - Hardware Emulation
Interrupts | Status
---------- | ------
V-Blank
LCD
Timer
Serial
Joypad

Graphics   | Status
---------- | ------
Backgrounds
Sprites

Audio | Status
----- | ------
Channel 1
Channel 2
Channel 3
Channel 4

ROM Mappers | Status
----------- | ------
MBC1
MBC3
MBC5
MBC5 (w/ Rumble)
MBC6
MBC7
HuC-1
HuC-3
MMM01
TAMA5

Devices | Status
------- | ------
Link Cable
Pocket Printer
GB Mobile
GB Camera
Barcode
