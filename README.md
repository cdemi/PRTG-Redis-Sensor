[![Build Status](https://dev.azure.com/christopherdemicoli/PRTG-Redis-Sensor/_apis/build/status/cdemi.PRTG-Redis-Sensor?branchName=master)](https://dev.azure.com/christopherdemicoli/PRTG-Redis-Sensor/_build/latest?definitionId=4&branchName=master) [![Release Status](https://vsrm.dev.azure.com/christopherdemicoli/_apis/public/Release/badge/b37cea1a-e46d-4c31-9b53-b13c3df81028/1/1)](https://github.com/cdemi/PRTG-Redis-Sensor/releases/latest)

# PRTG-Redis-Sensor
A Redis Sensor for PRTG. Read more at: https://blog.cdemi.io/monitoring-redis-in-prtg

# Requirements
* .NET Framework 4.5 or better
* PRTG Version 16 (Does not work in PRTG Version 14)
* Redis Server

# Usage
## Download or Compile
You can download the latest `PRTG-Redis-Sensor-Windows.zip` from [GitHub Releases](https://github.com/cdemi/PRTG-Redis-Sensor/releases/latest), or you can download the source-code and compile it yourself

## Run the executable
`"PRTG Redis Sensor.exe" <ServerIPorHostname:Port> <Password?>`

### Example

- `"PRTG Redis Sensor.exe" myredis.domain.local:6379`
- `"PRTG Redis Sensor.exe" myredis.domain.local:6379 somepassword`

## Add the sensor on PRTG
1. Copy the binary `PRTG Redis Sensor.exe` to the custom sensors folder of the probe `%programfiles(x86)%\PRTG Network Monitor\Custom Sensors\EXEXML\`
1. Add an `EXE/Script Advanced` sensor to a device
2. Select `PRTG Redis Sensor.exe` from the EXE/Script list
3. Set the parameters to pass to the executable (see the samples)
