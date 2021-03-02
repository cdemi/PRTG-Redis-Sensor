[![Build, Release and Publish](https://github.com/cdemi/PRTG-Redis-Sensor/actions/workflows/release-publish.yml/badge.svg)](https://github.com/cdemi/PRTG-Redis-Sensor/actions/workflows/release-publish.yml) [![GitHub release (latest SemVer)](https://img.shields.io/github/v/release/cdemi/PRTG-Redis-Sensor?sort=semver)](https://github.com/cdemi/PRTG-Redis-Sensor/releases/latest) [![GitHub](https://img.shields.io/github/license/cdemi/PRTG-Redis-Sensor)](https://github.com/cdemi/PRTG-Redis-Sensor/blob/master/LICENSE)

# PRTG-Redis-Sensor
A Redis Sensor for PRTG. Read more at: https://blog.cdemi.io/monitoring-redis-in-prtg

# Requirements
* PRTG Version 16 (Does not work in PRTG Version 14)
* Redis Server >= 3.2.11

# Usage
## Download or Compile
You can download the latest `PRTG.Redis.Sensor.exe` from [GitHub Releases](https://github.com/cdemi/PRTG-Redis-Sensor/releases/latest), or you can download the source-code and compile it yourself

## Run the executable
`"PRTG.Redis.Sensor.exe" <ServerIPorHostname:Port> <Password?>`

### Example

- `"PRTG.Redis.Sensor.exe" myredis.domain.local:6379`
- `"PRTG.Redis.Sensor.exe" myredis.domain.local:6379 somepassword`

## Add the sensor on PRTG
1. Copy the binary `PRTG.Redis.Sensor.exe` to the custom sensors folder of the probe `%programfiles(x86)%\PRTG Network Monitor\Custom Sensors\EXEXML\`
1. Add an `EXE/Script Advanced` sensor to a device
2. Select `PRTG.Redis.Sensor.exe` from the EXE/Script list
3. Set the parameters to pass to the executable (see the samples)


