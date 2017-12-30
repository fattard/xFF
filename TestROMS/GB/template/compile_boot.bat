@echo on

call clean.bat

for %%f in (*.asm) do call ..\..\wla\wla-gb.exe -v -DBOOT_TEST %%f


@echo off
echo [objects] > linkfile
for %%f in (*.o) do echo %%f >> linkfile
@echo on


call ..\..\wla\wlalink.exe -S linkfile Test_boot.bin

pause