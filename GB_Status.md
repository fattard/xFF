## GB Emu - Processor Sharp LR35902
#### Legend:
- :soon: - in progress
- :heavy_check_mark: - Done
- :o: - Implemented but inaccurate
- :x: - Implemented but incorrect


Hardware Mode | Supported
------------- | ---------
DMG (Game Boy) | :soon:
MGB/MGL (Game Boy Pocket/Light)
SGB (Super Game Boy)
SGB2 (Super Game Boy 2)
CGB (Game Boy Color)


8-bit Transfers  |   Implementation   | Checked Operation  |   Checked Flags    |   Checked Timing
---------------  | ------------------ | ------------------ | ------------------ | ------------------
LD r, r'         | :heavy_check_mark: |                    |                    |                   
LD r, n          | :heavy_check_mark: |                    |                    |                   
LD r, [HL]       | :heavy_check_mark: |                    |                    |                   
LD [HL], r       | :heavy_check_mark: |                    |                    |                   
LD [HL], n       | :heavy_check_mark: |                    |                    |                   
LD A, [BC]       | :heavy_check_mark: |                    |                    |                   
LD A, [DE]       | :heavy_check_mark: |                    |                    |                   
LD A, [$FF00+C]  | :heavy_check_mark: |                    |                    |                   
LD [$FF00+C], A  | :heavy_check_mark: |                    |                    |                   
LDH A, [$FF00+n] | :heavy_check_mark: |                    |                    |                   
LDH [$FF00+n], A | :heavy_check_mark: |                    |                    |                   
LD A, [nn]       | :heavy_check_mark: |                    |                    |                   
LD [nn], A       | :heavy_check_mark: |                    |                    |                   
LD A, [HL+]      | :heavy_check_mark: |                    |                    |                   
LD A, [HL-]      | :heavy_check_mark: |                    |                    |                   
LD [BC], A       | :heavy_check_mark: |                    |                    |                   
LD [DE], A       | :heavy_check_mark: |                    |                    |                   
LD [HL+], A      | :heavy_check_mark: |                    |                    |                   
LD [HL-], A      | :heavy_check_mark: |                    |                    |                   

16-bit Transfers |   Implementation   | Checked Operation  |   Checked Flags    |   Checked Timing
---------------- | ------------------ | ------------------ | ------------------ | ------------------
LD dd, nn        | :heavy_check_mark: |                    |                    |                   
LD SP, HL        | :heavy_check_mark: |                    |                    |                   
PUSH qq          | :heavy_check_mark: |                    |                    |                   
POP qq           | :heavy_check_mark: |                    |                    |                   
LD HL, SP+e      | :heavy_check_mark: |                    |                    |                   
LD [nn], SP      | :heavy_check_mark: |                    |                    |                   

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
ADD HL, ss        | :heavy_check_mark: |                    |                    |                   
ADD SP, e         | :heavy_check_mark: |                    |                    |                   
INC ss            | :heavy_check_mark: |                    |                    |                   
DEC ss            | :heavy_check_mark: |                    |                    |                   

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
BIT b, r       | :heavy_check_mark: |                    |                    |                   
BIT b, [HL]    | :heavy_check_mark: |                    |                    |                   
SET b, r       | :heavy_check_mark: |                    |                    |                   
SET b, [HL]    | :heavy_check_mark: |                    |                    |                   
RES b, r       | :heavy_check_mark: |                    |                    |                   
RES b, [HL]    | :heavy_check_mark: |                    |                    |                   

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
CALL nn       | :heavy_check_mark: |                    |                    |                   
CALL NZ, nn   | :heavy_check_mark: |                    |                    |                   
CALL Z, nn    | :heavy_check_mark: |                    |                    |                   
CALL NC, nn   | :heavy_check_mark: |                    |                    |                   
CALL C, nn    | :heavy_check_mark: |                    |                    |                   
RET           | :heavy_check_mark: |                    |                    |                   
RETI          | :heavy_check_mark: |                    |                    |                   
RET NZ        | :heavy_check_mark: |                    |                    |                   
RET Z         | :heavy_check_mark: |                    |                    |                   
RET NC        | :heavy_check_mark: |                    |                    |                   
RET C         | :heavy_check_mark: |                    |                    |                   
RST t         | :heavy_check_mark: |                    |                    |                   

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
GBC Palettes

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
Infrared
GB Mobile
GB Camera
Barcode
