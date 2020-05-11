@echo off
setlocal

rem This is the relative migrations path
set MIGRATIONS_PATH=%~dp0..\src\LightTown\LightTown.Server.Data\Migrations

rem Skip removing the old directory if it doesn't exist and go to the "create migrations" part
IF NOT EXIST %MIGRATIONS_PATH% GOTO NOMIGRATIONSDIR

rem Set the migrations path the the absolute path using the "set the current directory" hack and remove return to the old directory afterwards
pushd "%MIGRATIONS_PATH%"
set MIGRATIONS_PATH=%CD%
popd

echo Current migrations directory:
echo %MIGRATIONS_PATH%

rem Promt the user so they can verify that the migrations directory is correct and if they want to continue
:PROMPT
SET /P AREYOUSURE=Old migrations directory found, this script will remove the old migrations directory, are you sure? (Y/[N]) 
IF /I "%AREYOUSURE%" NEQ "Y" GOTO END

rem Remove the migrations directory based on the MIGRATIONS_PATH variable
@RD /S /Q %MIGRATIONS_PATH%

:NOMIGRATIONSDIR

rem Move the current directory to the parent directory which is the data project that contains LightTownContext.cs which it will need for the command below
cd %MIGRATIONS_PATH%\..\

rem The dotnet command for creating migrations for the current directory
dotnet ef migrations add InitialCreate

:END
endlocal
pause