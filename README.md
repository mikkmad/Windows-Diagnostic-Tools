# Windows Diagnostic Tools

A lightweight utility to run common Windows diagnostic commands for network and system health troubleshooting.

## Overview

**Windows Diagnostic Tools** provides a simple graphical user interface (GUI) to execute a set of predefined Windows commands commonly used to troubleshoot network connectivity issues and system performance problems. Instead of manually running commands from the command prompt, users can perform these tasks with just a few clicks.

## Use Cases

### 1. Network Connectivity Issues

Having internet problems? Press a button and let the tool run the five commands:
1. `ipconfig /flushdns`
2. `ipconfig /release`
3. `ipconfig /renew`
4. `ipconfig /registerdns`
5. `netsh winsock reset`

### 2. System Slowness

Is Windows running slow? Press a button to run `sfc /scannow` followed by `DISM /Online /Cleanup-Image /RestoreHealth`.

## Requirements

- **Operating System**: Windows 10 or later
- **.NET Runtime**: .NET 8 or later
- **Administrator Privileges**: Required to execute `sfc /scannow` and `DISM /Online /Cleanup-Image /RestoreHealth`.

## Installation

### Option 1: Download Pre-built Release

1. Visit the [Releases](https://github.com/mikkmad/Windows-Diagnostic-Tools/releases) page
2. Download the latest release
3. Extract the files to your preferred location
4. Run `WindowsDiagnosticTools.exe` with administrator privileges

### Option 2: Build from Source

#### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/) (optional, or use command line)

#### Building

1. Clone the repository:
```
git clone https://github.com/mikkmad/Windows-Diagnostic-Tools.git cd Windows-Diagnostic-Tools
```

2. Build the project:
```
dotnet build --configuration (Release | Debug)
```

3. Run the application:
```
dotnet run --configuration Release
```
Or navigate to the build output directory and run the executable directly.

## Usage

1. Launch the application with administrator privileges
2. Select the diagnostic task you want to run:
- **Network Diagnostics**: Resets network configuration and DNS cache
- **System Health**: Scans and repairs system files
3. Click the corresponding button and wait for the process to complete
4. Review the results in the output window

## Disclaimer

This tool is not guaranteed to fix issues, merely to provide an easy way to run often-recommended actions if you're facing network or system slowness symptoms. Always ensure you have backups before running system diagnostic tools.

