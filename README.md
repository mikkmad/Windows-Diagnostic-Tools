# Windows Diagnostic Tools

This is by no means a finished product (messy, repeateable code), and the UI is... something.

Functionality wise; it works as intended. Some errors might not be easily readable as there's some encoding errors going on. I've tried fixing this using a vast amount of different encodings, but they all produce artifacts in different scenarios; sadly, no shoe fits all here.

It should work on all versions of Windows having the `DISM` and `ipconfig` toolsets. Only tested on `Windows 11, build 22631.3527`.

It is built using the Avalonia UI framework.

## Use-case

### 1. Have you ever had internet connectivity issues? 
Fear not! Instead of googling on your phone for the `ipconfig` commands, this tool provides a one-button-to-do-it-all functionality. Press it, and wait for the program to run all five commands:
1. `ipconfig /flushdns`
2. `ipconfig /release`
3. `ipconfig /renew`
4. `ipconfig /registerdns`
5. `netsh winsock reset`

Hopefully that fixes your issues!

### 2. Is Windows running slow?
Fear not! Instead of wasting time away searching for the what-was-the-name-again tool DISM, a press of a button runs `sfc /scannow` followed by `DISM /Online /Cleanup-Image /RestoreHealth`.

## Disclaimer
This tool is not guaranteed to fix your issues, merely to provide an easy way to run the often-recommended actions, if you're facing Network or lazy-Windows symptoms.
