## WiFiLogger

A simple Windows tool written in C#, that scans Wi-Fi networks and saves the results to a text file .  

### Features

- Scans Wi-Fi networks using Windows `netsh` command.  
- Saves results (or errors) into `wifi_scan.txt`.  

### Requirements

[.Net Framework](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net48) v4.8 is required to run.

### Usage

1. Build with Visual Studio.
2. Run `WiFiLogger.exe` and it will generate the `wifi_scan.txt` with the results.

**Note:** Only detects networks visible to your local adapter.
