dotnet publish -r win-x64 -c Release --self-contained false -o "bin\publish\xil_cli" -p:PublishSingleFile=false -p:IncludeAllContentForSelfExtract=false -p:PublishTrimmed=false -p:TrimMode=Link -p:DebugType=None -p:DebugSymbols=false
