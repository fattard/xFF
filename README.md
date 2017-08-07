# xFF - Experimental Emulator embedded in Unity3D

## Core Status ##
## [GB Emulation Status](GB_Status.md) ##
Game Boy core emulation. Initially, only the classic emulation will be supported.  
The current release build (v0.0.3) is able to run TestROMS, such as Blargg's  
individual cpu_intrs, and also some mooneye-gb tests.
<br>
<br>
Below is the current montage of results from cpu_instrs individual tests:
![Test ROM result](sshots/GB/cpu_instrs_individual.png)
<br>
Instruction 'DAA' is stubbed for now, and I think I can blame my bad Half-Borrow  
implementation to cause many of the tests to fail.
<br>
<br>

## [BytePusher Emulation Status](BytePusher_Status.md) ##
Special test core based on OISC ByteByteJump Processor.  
It supports full graphics, keyboard and sound.  
Sound might be stuttering, it's my first time coding procedural audio.
