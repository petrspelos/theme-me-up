#!/bin/sh
dotnet publish -c Release -r linux-x64 --self-contained false
cp /home/user/Repos/petrspelos/theme-me-up/theme-me-up/bin/Release/netcoreapp3.0/linux-x64/publish/* /home/user/tools
