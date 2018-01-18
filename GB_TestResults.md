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
| halt bug          | :x:    |
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
| add sp e timing         | :x:    |
| boot hwio dmg0          | :x:    |
| boot hwio dmgABCXmgb    | :x:    |
| boot hwio S             | :x:    |
| boot regs dmg0          | :x:    |
| boot regs dmgABCX       | :x:    |
| boot regs mgb           | :x:    |
| boot regs sgb           | :x:    |
| boot regs sgb2          | :x:    |
| call timing             | :x:    |
| call timing2            | :x:    |
| call cc_timing          | :x:    |
| call cc_timing2         | :x:    |
| di timing GS            | :x:    |
| div timing              | :x:    |
| ei sequence             | :x:    |
| ei timing               | :x:    |
| halt ime0 ei            | :x:    |
| halt ime0 nointr_timing | :x:    |
| halt ime1 timing        | :x:    |
| halt ime1 timing2 GS    | :x:    |
| if ie registers         | :x:    |
| intr timing             | :x:    |
| jp timing               | :x:    |
| jp cc timing            | :x:    |
| ld hl sp e timing       | :x:    |
| oam dma_restart         | :x:    |
| oam dma start           | :x:    |
| oam dma timing          | :x:    |
| pop timing              | :x:    |
| push timing             | :x:    |
| rapid di ei             | :x:    |
| ret timing              | :x:    |
| ret cc timing           | :x:    |
| reti timing             | :x:    |
| reti intr timing        | :x:    |
| rst timing              | :x:    |

Notes:

* Passes most boot tests only if you explicitly enable boot ROMs and give it the right one.
  This makes sense for DMG0, MGB, and SGB2 because they are not selectable, but SGB should work
  without boot ROMs out of the box.

#### Bits (unusable bits in memory and registers)

| Test           | Result |
| -------------- | ------ |
| mem oam        | :x:    |
| reg f          | :x:    |
| unused_hwio GS | :x:    |

#### GPU

| Test                        | Result |
| --------------------------- | ------ |
| hblank ly scx timing GS     | :x:    |
| intr 1 2 timing GS          | :x:    |
| intr 2 0 timing             | :x:    |
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
| div write            | :x:    |
| rapid toggle         | :x:    |
| tim00 div trigger    | :x:    |
| tim00                | :x:    |
| tim01 div trigger    | :x:    |
| tim01                | :x:    |
| tim10 div trigger    | :x:    |
| tim10                | :x:    |
| tim11 div trigger    | :x:    |
| tim11                | :x:    |
| tima reload          | :x:    |
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
| sprite priority | :x:    |

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
