# PRTG-Redis-Sensor
A Redis Sensor for PRTG. Read more at: https://blog.cdemi.io/monitoring-redis-in-prtg

# Requirements
* .NET Framework 4.5 or better
* PRTG Version 16 (Does not work in PRTG Version 14)
* Redis Server

# Usage
## Download or Compile
You can download the latest `PRTG-Redis-Sensor-Windows.zip` from [GitHub Releases](https://github.com/cdemi/PRTG-Redis-Sensor/releases), or you can download the source-code and compile it yourself

## Run the executable
`"PRTG Redis Sensor.exe" <ServerAndPort> <Password?>`

## Add the sensor on PRTG
1. Copy the binary `PRTG Redis Sensor.exe` to the custom sensors folder of the probe `%programfiles(x86)%\PRTG Network Monitor\Custom Sensors\EXEXML\`
1. Add an `EXE/Script Advanced` sensor to a device
2. Select `PRTG Redis Sensor.exe` from the EXE/Script list
3. Set the parameters to pass to the executable (see the samples)

## Sensor parameters samples

- `myredis.domain.local:6379`
- `myredis.domain.local:6379 somepassword`
