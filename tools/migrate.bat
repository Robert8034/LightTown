@echo off
setlocal
:PROMPT
SET /P AREYOUSURE=This will remove the old migrations folder, are you sure? (Y/[N]) 
IF /I "%AREYOUSURE%" NEQ "Y" GOTO END

@RD /S /Q "%~dp0../src/LightTown/LightTown.Server.Data/Migrations"

cd "%~dp0../src/LightTown/LightTown.Server.Data"

dotnet ef migrations add InitialCreate

:END
endlocal

pause