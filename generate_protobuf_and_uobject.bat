@echo off
setlocal enabledelayedexpansion

REM Check if input file is provided
if "%~1"=="" (
    echo Error: Input file path is required.
    goto :usage
)

REM Set input file path
set "INPUT_FILE=%~1"

REM Run ProtoCCS.exe
echo Running ProtoCCS.exe...
ProtoCCS.exe "%INPUT_FILE%"

REM Check if ProtoCCS.exe executed successfully
if %ERRORLEVEL% neq 0 (
    echo Error: ProtoCCS.exe execution failed.
    goto :end
)

REM Run protobuf2uobject.exe
echo Running protobuf2uobject.exe...
protobuf2uobject.exe -i "%INPUT_FILE%"

REM Check if protobuf2uobject.exe executed successfully
if %ERRORLEVEL% neq 0 (
    echo Error: protobuf2uobject.exe execution failed.
    goto :end
)

echo All programs executed successfully.

:end
exit /b %ERRORLEVEL%

:usage
echo Usage: %~nx0 <input_file_path>
exit /b 1
