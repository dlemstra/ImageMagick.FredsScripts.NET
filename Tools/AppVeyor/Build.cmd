@echo off
call "VsDevCmd.cmd"

powershell -ExecutionPolicy Unrestricted ..\Scripts\AppVeyor\Build.ps1