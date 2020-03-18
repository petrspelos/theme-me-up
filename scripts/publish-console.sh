#!/bin/bash
dotnet publish ./src/ThemeMeUp.ConsoleApp -c Release -r linux-x64 /p:PublishSingleFile=true
cd src/ThemeMeUp.ConsoleApp/bin/Release/netcoreapp3.1/linux-x64/publish
mv ThemeMeUp.ConsoleApp theme-me-up
rm ThemeMeUp.ConsoleApp.pdb

