# xFF - Experimental Emulator embedded in Unity3D

## Core Status ##
## [GB Emulation Status](GB_Status.md) ##
Game Boy core emulation. Initially, only the classic emulation will be supported.  
The current release build (v0.0.6) is able to run TestROMS, such as [Blargg's Test Suite](http://gbdev.gg8.se/files/roms/blargg-gb-tests/), some
[mooneye-gb tests](https://gekkio.fi/files/mooneye-gb/latest/), and also commercial games that uses mapper MBC1.   
Remember to check the [status page](GB_Status.md) for information on what is supported or not.
<br>
<br>
Below is a montage of all 6 Launch Titles running:
<br>
![Launch Games](sshots/GB/launch_games.png)
<br>
<br>
Below is the current montage of results from Blargg's tests:
![Test ROM result](sshots/GB/blarggs_test_suite.png)
<br>
The mem_timing test fails, as it requires cycle accurate emulation. It will take some time for this.    
Both DMG and CGB sound tests fails, as there's not sound support yet.     
The hardware bug tests also fails, as this kind of behaviour is not being emulated.
<br>
<br>
### Joypad Controls ###
Supports both Keyboard control and Xbox Controller (compatible with any XInput device).  
The controls uses pre-mapped keys for now:

GB Button | Keyboard | Xbox Controller
----------|----------|----------------
Button A | X | B
Button B | Z | A
Button Select | Right Shift | Back
Button Start | Enter/Return | Start
DPad Up | Arrow Up | DPad Up / Left Stick Up
DPad Down | Arrow Down | DPad Down / Left Stick Down
DPad Left | Arrow Left | DPad Left / Left Stick Left
DPad Right | Arrow Right | DPad Right / Left Stick Right
<br>

<br>
<br>

## [BytePusher Emulation Status](BytePusher_Status.md) ##
Special test core based on OISC ByteByteJump Processor.  
It supports full graphics, keyboard and sound.  
I think the sound is not stuttering anymore, would appreciate feedback.  
It's my first time coding procedural audio.
<br>
<br>
Below is a montage of 4 programs running:
![Test ROM result](sshots/BytePusher/sample1.png)


All known programs can be downloaded from [BytePusher home](https://esolangs.org/wiki/BytePusher#Programs).  
The keyboard emulation uses the real keyboard keys ('0'-'9', 'A'-'F')
