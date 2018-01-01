@echo on

call clean.bat

for %%f in (*.asm) do call ..\..\wla\wla-gb.exe -v %%f


@echo off
echo [objects] > linkfile
for %%f in (*.o) do echo %%f >> linkfile
@echo on


call ..\..\wla\wlalink.exe -S linkfile Test.gb

pause