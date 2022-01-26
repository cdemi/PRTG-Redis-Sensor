[![Build, Release and Publish](https://github.com/cdemi/PRTG-Redis-Sensor/actions/workflows/release-publish.yml/badge.svg)](https://github.com/cdemi/PRTG-Redis-Sensor/actions/workflows/release-publish.yml) [![GitHub release (latest SemVer)](https://img.shields.io/github/v/release/cdemi/PRTG-Redis-Sensor?sort=semver)](https://github.com/cdemi/PRTG-Redis-Sensor/releases/latest) [![GitHub](https://img.shields.io/github/license/cdemi/PRTG-Redis-Sensor)](https://github.com/cdemi/PRTG-Redis-Sensor/blob/master/LICENSE)

# PRTG Redis Sensor
A Redis Sensor for PRTG. Read more at: https://blog.cdemi.io/monitoring-redis-in-prtg

# Requirements
* PRTG Version 16 (Does not work in PRTG Version 14)
* Redis Server >= 3.2.11

# Usage
## Download or Compile
You can download the latest `PRTG.Redis.Sensor.exe` from [GitHub Releases](https://github.com/cdemi/PRTG-Redis-Sensor/releases/latest), or you can download the source-code and compile it yourself

## Run the executable (get help)
`"PRTG.Redis.Sensor.exe" `

```
PRTG Redis Sensor <version>
MIT

  t, Monitor type       Required. Indicates the type of monitor to execute.
                        Valid values: Stats, CacheKeysValues

  e, Redis endpoints    Required. Specify the redis endpoints (i.e.
                        server1:port;server2:port)

  p, Redis password     Specify the redis password if required

  d, Database index     Specify the database index

  k, Cache keys         Keys whose values you want to retrieve from cache
                        (separated by pipe | )

  r, Transform value    Elaborate the value retrieved from cache using the
                        specified transformation.
                        Valid values: ElapsedMinutes
```

## Run the executable (get generic stats)
`"PRTG.Redis.Sensor.exe" -t Stats -e <ServerIPorHostname:Port> -p <Password?>`

## Run the executable (get cache keys values)
`"PRTG.Redis.Sensor.exe" -t CacheKeysValues -e <ServerIPorHostname:Port> -p <Password?> -k key1|key2|...`

## Run the executable (work on a differend database index than default one?)
`"PRTG.Redis.Sensor.exe" -t <MonitorType> -e <ServerIPorHostname:Port> -p <Password?> -d <DatabaseIndex?>`

## Retrieve a cache value and manipulate the value result
In case of datetime cache values, as PRTG usually manages numeric values, you probably need to retrieve the amount of time elapsed from that value:

`"PRTG.Redis.Sensor.exe" -t <MonitorType> -e <ServerIPorHostname:Port> -p <Password?> -d <DatabaseIndex?> -k key1 -r ElapsedMinutes`

### Examples

- `"PRTG.Redis.Sensor.exe" -t Stats -e myredis.domain.local:6379`
- `"PRTG.Redis.Sensor.exe" -t Stats -e myredis.domain.local:6379 -p somepassword`
- `"PRTG.Redis.Sensor.exe" -t CacheKeysValues -e myredis.domain.local:6379 -p somepassword -k cachekey1`

### Breaking changes (starting on version 3.0)
Note that from version 3.0 this tool will require extra arguments to properly define the input values.
Calling the executable without parameters will give you some help on how to use it.

## Add the sensor on PRTG
1. Copy the binary `PRTG.Redis.Sensor.exe` to the custom sensors folder of the probe `%programfiles(x86)%\PRTG Network Monitor\Custom Sensors\EXEXML\`
1. Add an `EXE/Script Advanced` sensor to a device
2. Select `PRTG.Redis.Sensor.exe` from the EXE/Script list
3. Set the parameters to pass to the executable (see the samples)


