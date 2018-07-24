REM @echo off
cd EscapeToEarth

REM Source: https://stackoverflow.com/questions/44074121/build-net-core-console-application-to-output-an-exe
dotnet publish -c Release -r win10-x64

REM for Linux builds!
REM dotnet publish -c Release -r ubuntu.16.10-x64