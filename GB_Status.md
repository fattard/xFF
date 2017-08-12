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
ADD r     | :heavy_check_mark: |                    |                    |                   
ADD n     | :heavy_check_mark: |                    |                    |                   
ADD [HL]  | :heavy_check_mark: |                    |                    |                   
ADC r     | :heavy_check_mark: |                    |                    |                   
ADC n     | :heavy_check_mark: |                    |                    |                   
ADC [HL]  | :heavy_check_mark: |                    |                    |                   
SUB r     | :heavy_check_mark: |                    |                    |                   
SUB n     | :heavy_check_mark: |                    |                    |                   
SUB [HL]  | :heavy_check_mark: |                    |                    |                   
SBC r     | :heavy_check_mark: |                    |                    |                   
SBC n     | :heavy_check_mark: |                    |                    |                   
SBC [HL]  | :heavy_check_mark: |                    |                    |                   
AND r     | :heavy_check_mark: |                    |                    |                   
AND n     | :heavy_check_mark: |                    |                    |                   
AND [HL]  | :heavy_check_mark: |                    |                    |                   
OR r      | :heavy_check_mark: |                    |                    |                   
OR n      | :heavy_check_mark: |                    |                    |                   
OR [HL]   | :heavy_check_mark: |                    |                    |                   
XOR r     | :heavy_check_mark: |                    |                    |                   
XOR n     | :heavy_check_mark: |                    |                    |                   
XOR [HL]  | :heavy_check_mark: |                    |                    |                   
CP r      | :heavy_check_mark: |                    |                    |                   
CP n      | :heavy_check_mark: |                    |                    |                   
CP [HL]   | :heavy_check_mark: |                    |                    |                   
INC r     | :heavy_check_mark: |                    |                    |                   
INC [HL]  | :heavy_check_mark: |                    |                    |                   
DEC r     | :heavy_check_mark: |                    |                    |                   
DEC [HL]  | :heavy_check_mark: |                    |                    |                   

16-bit Arithmetic |   Implementation   | Checked Operation  |   Checked Flags    |   Checked Timing
----------------- | ------------------ | ------------------ | ------------------ | ------------------
ADD HL, ss        | :heavy_check_mark: |                    |                    |                   
ADD SP, e         | :heavy_check_mark: |                    |                    |                   
INC ss            | :heavy_check_mark: |                    |                    |                   
DEC ss            | :heavy_check_mark: |                    |                    |                   

Rotate / Shift |   Implementation   | Checked Operation  |   Checked Flags    |   Checked Timing
-------------- | ------------------ | ------------------ | ------------------ | ------------------
RLCA           | :heavy_check_mark: |                    |                    |                   
RLA            | :heavy_check_mark: |                    |                    |                   
RRCA           | :heavy_check_mark: |                    |                    |                   
RRA            | :heavy_check_mark: |                    |                    |                   
RLC r          | :heavy_check_mark: |                    |                    |                   
RLC [HL]       | :heavy_check_mark: |                    |                    |                   
RL r           | :heavy_check_mark: |                    |                    |                   
RL [HL]        | :heavy_check_mark: |                    |                    |                   
RRC r          | :heavy_check_mark: |                    |                    |                   
RRC [HL]       | :heavy_check_mark: |                    |                    |                   
RR r           | :heavy_check_mark: |                    |                    |                   
RR [HL]        | :heavy_check_mark: |                    |                    |                   
SLA r          | :heavy_check_mark: |                    |                    |                   
SLA [HL]       | :heavy_check_mark: |                    |                    |                   
SRA r          | :heavy_check_mark: |                    |                    |                   
SRA [HL]       | :heavy_check_mark: |                    |                    |                   
SRL r          | :heavy_check_mark: |                    |                    |                   
SRL [HL]       | :heavy_check_mark: |                    |                    |                   
SWAP r         | :heavy_check_mark: |                    |                    |                   
SWAP [HL]      | :heavy_check_mark: |                    |                    |                   

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
DAA              | :x: |                    |                    |                   
CPL              | :heavy_check_mark: |                    |                    |                   
NOP              | :heavy_check_mark: |                    |                    |                   
CCF              | :heavy_check_mark: |                    |                    |                   
SCF              | :heavy_check_mark: |                    |                    |                   
DI               | :o: |                    |                    |                   
EI               | :o: |                    |                    |                   
HALT             | :o: |                    |                    |                   
STOP             | :o: |                    |                    |                   


## GB Emu - Hardware Emulation
Interrupts | Status
---------- | ------
V-Blank    | :o:
LCD        | :soon:
Timer
Serial
Joypad

Graphics   | Status
------------ | ------
Backgrounds  | :soon:
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
