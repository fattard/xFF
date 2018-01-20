## Test ROMs Results

[Binary test ROMs are available here](https://gekkio.fi/files/mooneye-gb/latest/)
in a zip package and also as individual .gb files. They are automatically
built and deployed whenever there's new changes in the master branch.

Symbols:

* :+1: pass
* :x: fail
* :o: pass with caveats, see notes

### [Blargg's tests](http://gbdev.gg8.se/files/roms/blargg-gb-tests/)

| Test              | Result |
| ----------------- | ------ |
| cpu instrs        | :+1:   |
| dmg sound 2       | :x:    |
| instr timing      | :+1:   |
| halt bug          | :+1:   |
| mem timing 2      | :x:    |
| oam bug 2         | :x:    |
| cgb sound 2       | :x:    |

Notes:

* cpu_instrs fails on MGB/SGB2 hardware and emulators emulating them correctly.
  The ROM incorrectly detects the device as CGB, and attempts to perform a CPU
  speed change which causes a freeze (STOP instruction with joypad disabled)
* dmg_sound-2 test #10 fails on DMG-CPU A, DMG-CPU C, MGB, and SGB2
* oam_bug-2 fails on all CGB, AGB, and AGS devices
* cgb_sound-2 test #03 fails on CPU CGB, CPU CGB A, and CPU CGB B

### Mooneye GB acceptance tests

| Test                    | Result |
| ----------------------- | ------ |
| add sp e timing         | :+1:   |
| boot hwio dmg0          | :x:    |
| boot hwio dmgABCXmgb    | :x:    |
| boot hwio S             | :x:    |
| boot regs dmg0          | :+1:   |
| boot regs dmgABCX       | :+1:   |
| boot regs mgb           | :+1:   |
| boot regs sgb           | :+1:   |
| boot regs sgb2          | :+1:   |
| call timing             | :x:    |
| call timing2            | :x:    |
| call cc_timing          | :x:    |
| call cc_timing2         | :x:    |
| di timing GS            | :+1:   |
| div timing              | :+1:   |
| ei sequence             | :+1:   |
| ei timing               | :+1:   |
| halt ime0 ei            | :+1:   |
| halt ime0 nointr_timing | :+1:   |
| halt ime1 timing        | :+1:   |
| halt ime1 timing2 GS    | :+1:   |
| if ie registers         | :+1:   |
| intr timing             | :+1:   |
| jp timing               | :x:    |
| jp cc timing            | :x:    |
| ld hl sp e timing       | :+1:   |
| oam dma_restart         | :+1:   |
| oam dma start           | :x:    |
| oam dma timing          | :+1:   |
| pop timing              | :x:    |
| push timing             | :x:    |
| rapid di ei             | :+1:   |
| ret timing              | :x:    |
| ret cc timing           | :x:    |
| reti timing             | :x:    |
| reti intr timing        | :+1:   |
| rst timing              | :x:    |

Notes:

* Passes most boot tests only if you explicitly enable boot ROMs and give it the right one.

#### Bits (unusable bits in memory and registers)

| Test           | Result |
| -------------- | ------ |
| mem oam        | :+1:   |
| reg f          | :+1:   |
| unused_hwio GS | :+1:   |

#### GPU

| Test                        | Result |
| --------------------------- | ------ |
| hblank ly scx timing GS     | :x:    |
| intr 1 2 timing GS          | :+1:   |
| intr 2 0 timing             | :+1:   |
| intr 2 mode0 timing         | :x:    |
| intr 2 mode3 timing         | :x:    |
| intr 2 oam ok timing        | :x:    |
| intr 2 mode0 timing sprites | :x:    |
| lcdon timing dmgABCXmgbS    | :x:    |
| lcdon write timing GS       | :x:    |
| stat irq blocking           | :x:    |
| vblank stat intr GS         | :x:    |

#### Interrupt handling

| Test                        | Result | 
| --------------------------- | ------ | 
| ie push                     | :x:    | 

#### Serial

| Test                        | Result |
| --------------------------- | ------ |
| boot sclk align dmgABCXmgb  | :x:    |

#### Timer

| Test                 | Result |
| -------------------- | ------ |
| div write            | :+1:   |
| rapid toggle         | :+1:   |
| tim00 div trigger    | :+1:   |
| tim00                | :+1:   |
| tim01 div trigger    | :+1:   |
| tim01                | :+1:   |
| tim10 div trigger    | :+1:   |
| tim10                | :+1:   |
| tim11 div trigger    | :+1:   |
| tim11                | :+1:   |
| tima reload          | :+1:   |
| tima write reloading | :x:    |
| tma write reloading  | :x:    |

### Mooneye GB emulator-only tests

#### MBC1

| Test              | Result |
| ----------------- | ------ |
| bits ram en       | :+1:   |
| rom 512Kb         | :+1:   |
| rom 1Mb           | :+1:   |
| rom 2Mb           | :+1:   |
| rom 4Mb           | :+1:   |
| rom 8Mb           | :+1:   |
| rom 16Mb          | :+1:   |
| ram 64Kb          | :+1:   |
| ram 256Kb         | :+1:   |
| multicart rom 8Mb | :x:    |

Notes:

* Most emulators don't support MBC1 multicart ROMs at all


### Mooneye GB manual tests

| Test            | Result |
| --------------- | ------ |
| sprite priority | :+1:   |

### Mooneye GB misc tests

| Test            | Result |
| --------------- | ------ |
| boot hwio C     |        |
| boot regs A     |        |
| boot regs cgb   |        |

#### Bits

| Test          | Result |
| ------------- | ------ |
| unused hwio C |        |

#### GPU

| Test               | Result |
| ------------------ | ------ |
| vblank stat intr C |        |

### Test naming

Some tests are expected to pass only a single model:

* dmg = Game Boy
* mgb = Game Boy Pocket
* sgb = Super Game Boy
* sgb2 = Super Game Boy 2
* cgb = Game Boy Color
* agb = Game Boy Advance
* ags = Game Boy Advance SP

In addition to model differences, CPU revisions can affect the behaviour.
Revision 0 refers always to the initial version of a CPU (e.g. CPU CGB). AGB
and AGS use the same CPU models.  The following CPU models have several
revisions:

* DMG: 0, A, B, C, X (blob)
* CGB: 0, A, B, C, D, E
* AGB: 0, A, B, B E. Revision E also exists, but only in Game Boy Micro (OXY)
  so it is out of this project's scope.

In general, hardware can be divided to a couple of groups based on their
behaviour. Some tests are expected to pass on a single or multiple groups:

* G = dmg+mgb
* S = sgb+sgb2
* C = cgb+agb+ags
* A = agb+ags

For example, a test with GS in the name is expected to pass on dmg+mgb +
sgb+sgb2.
