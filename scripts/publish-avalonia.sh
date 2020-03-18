#!/bin/bash
dotnet publish ./src/ThemeMeUp.Avalonia -c Release -r linux-x64 /p:PublishSingleFile=true
cd src/ThemeMeUp.Avalonia/bin/Release/netcoreapp3.0/linux-x64/publish
mv ThemeMeUp.Avalonia theme-me-up
rm ThemeMeUp.Avalonia.pdb

