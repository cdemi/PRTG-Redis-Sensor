using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace PRTG_Redis_Sensor
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

            ConfigurationOptions configurationOptions = new()
            {
                EndPoints = { { args[0] } },
                AllowAdmin = true
            };

            if (args.Length > 1)
            {
                configurationOptions.Password = args[1];
            }

            var redisConnectionMultiplexer = ConnectionMultiplexer.Connect(configurationOptions);
            var redisServer = redisConnectionMultiplexer.GetServer(configurationOptions.EndPoints[0]);

            var info = redisServer.Info();
            var serverInfo = info.SingleOrDefault(i => i.Key.Equals("Server", StringComparison.InvariantCultureIgnoreCase));
            var clientsInfo = info.SingleOrDefault(i => i.Key.Equals("Clients", StringComparison.InvariantCultureIgnoreCase));
            var memoryInfo = info.SingleOrDefault(i => i.Key.Equals("Memory", StringComparison.InvariantCultureIgnoreCase));
            var persistenceInfo = info.SingleOrDefault(i => i.Key.Equals("Persistence", StringComparison.InvariantCultureIgnoreCase));
            var statsInfo = info.SingleOrDefault(i => i.Key.Equals("Stats", StringComparison.InvariantCultureIgnoreCase));
            var replicationInfo = info.SingleOrDefault(i => i.Key.Equals("Replication", StringComparison.InvariantCultureIgnoreCase));
            var keyspaceInfo = info.SingleOrDefault(i => i.Key.Equals("Keyspace", StringComparison.InvariantCultureIgnoreCase));
            var cpuInfo = info.SingleOrDefault(i => i.Key.Equals("CPU", StringComparison.InvariantCultureIgnoreCase));
            var clusterInfo = info.SingleOrDefault(i => i.Key.Equals("Cluster", StringComparison.InvariantCultureIgnoreCase));

            var response = new
            {
                prtg = new PRTGResponse
                {
                    result = new List<PRTGResult>
                    {
                        {
                            new PRTGResult
                            {
                                channel = "Uptime",
                                unit = PRTGUnit.TimeSeconds,
                                value = serverInfo?.SingleOrDefault(i => i.Key.Equals("uptime_in_seconds")).Value
                            }
                        },
                        {
                            new PRTGResult
                            {
                                channel = "Connected Clients",
                                unit = PRTGUnit.Count,
                                value = clientsInfo?.SingleOrDefault(i => i.Key.Equals("connected_clients")).Value
                            }
                        },
                        {
                            new PRTGResult
                            {
                                channel = "Blocked Clients",
                                unit = PRTGUnit.Count,
                                value = clientsInfo?.SingleOrDefault(i => i.Key.Equals("blocked_clients")).Value
                            }
                        },
                        {
                            new PRTGResult
                            {
                                channel = "Used Memory",
                                unit = PRTGUnit.BytesMemory,
                                value = memoryInfo?.SingleOrDefault(i => i.Key.Equals("used_memory")).Value
                            }
                        },
                        {
                            new PRTGResult
                            {
                                channel = "Used Memory RSS",
                                unit = PRTGUnit.BytesMemory,
                                value = memoryInfo?.SingleOrDefault(i => i.Key.Equals("used_memory_rss")).Value
                            }
                        },
                        {
                            new PRTGResult
                            {
                                channel = "Used Memory Peak",
                                unit = PRTGUnit.BytesMemory,
                                value = memoryInfo?.SingleOrDefault(i => i.Key.Equals("used_memory_peak")).Value
                            }
                        },
                        {
                            new PRTGResult
                            {
                                channel = "Memory Fragmentation Ratio",
                                unit = PRTGUnit.Custom,
                                Float = 1,
                                value = memoryInfo?.SingleOrDefault(i => i.Key.Equals("mem_fragmentation_ratio")).Value
                            }
                        },
                        {
                            new PRTGResult()
                            {
                                channel = "RDB Changes Since Last Save",
                                unit = PRTGUnit.Count,
                                value = persistenceInfo?.SingleOrDefault(i => i.Key.Equals("rdb_changes_since_last_save")).Value == "ok" ? "1" : "0"
                            }
                        },
                        {
                            new PRTGResult()
                            {
                                channel = "RDB Last Save Time",
                                unit = PRTGUnit.Custom,
                                value = persistenceInfo?.SingleOrDefault(i => i.Key.Equals("rdb_last_save_time")).Value == "ok" ? "1" : "0"
                            }
                        },
                        {
                            new PRTGResult
                            {
                                channel = "RDB Last Background Save Status",
                                unit = PRTGUnit.Count,
                                ShowChart = 0,
                                value = persistenceInfo?.SingleOrDefault(i => i.Key.Equals("rdb_last_bgsave_status")).Value == "ok" ? "1" : "0"
                            }
                        },
                        {
                            new PRTGResult
                            {
                                channel = "RDB Last Background Save Time in Seconds",
                                unit = PRTGUnit.TimeSeconds,
                                value = persistenceInfo?.SingleOrDefault(i => i.Key.Equals("rdb_last_bgsave_time_sec")).Value
                            }
                        },
                        {
                            new PRTGResult
                            {
                                channel = "AOF Last Rewrite Time in Seconds",
                                unit = PRTGUnit.TimeSeconds,
                                value = persistenceInfo?.SingleOrDefault(i => i.Key.Equals("aof_last_rewrite_time_sec")).Value
                            }
                        },
                        {
                            new PRTGResult
                            {
                                channel = "AOF Last Background Rewrite Status",
                                unit = PRTGUnit.Count,
                                ShowChart = 0,
                                value = persistenceInfo?.SingleOrDefault(i => i.Key.Equals("aof_last_bgrewrite_status")).Value == "ok" ? "1" : "0"
                            }
                        },
                        {
                            new PRTGResult
                            {
                                channel = "AOF Last Write Status",
                                unit = PRTGUnit.Count,
                                ShowChart = 0,
                                value = persistenceInfo?.SingleOrDefault(i => i.Key.Equals("aof_last_write_status")).Value == "ok" ? "1" : "0"
                            }
                        },
                        {
                            new PRTGResult
                            {
                                channel = "Total Connections Received",
                                unit = PRTGUnit.Count,
                                value = statsInfo?.SingleOrDefault(i => i.Key.Equals("total_connections_received")).Value
                            }
                        },
                        {
                            new PRTGResult
                            {
                                channel = "Total Commands Processed",
                                unit = PRTGUnit.Count,
                                value = statsInfo?.SingleOrDefault(i => i.Key.Equals("total_commands_processed")).Value
                            }
                        },
                        {
                            new PRTGResult
                            {
                                channel = "Instantaneous Operations per Second",
                                unit = PRTGUnit.Count,
                                value = statsInfo?.SingleOrDefault(i => i.Key.Equals("instantaneous_ops_per_sec")).Value
                            }
                        },
                        {
                            new PRTGResult()
                            {
                                channel = "Instantaneous Input kbps",
                                Float = 1,
                                unit = PRTGUnit.BytesBandwidth,
                                value = statsInfo?.SingleOrDefault(i => i.Key.Equals("instantaneous_input_kbps")).Value
                            }
                        },
                        {
                            new PRTGResult()
                            {
                                channel = "Instantaneous Output kbps",
                                Float = 1,
                                unit = PRTGUnit.BytesBandwidth,
                                value = statsInfo?.SingleOrDefault(i => i.Key.Equals("instantaneous_output_kbps")).Value
                            }
                        },
                        {
                            new PRTGResult
                            {
                                channel = "Total Net Input Bytes",
                                Float = 1,
                                unit = PRTGUnit.BytesBandwidth,
                                value = statsInfo?.SingleOrDefault(i => i.Key.Equals("total_net_input_bytes")).Value
                            }
                        },
                        {
                            new PRTGResult
                            {
                                channel = "Total Net Output Bytes",
                                Float = 1,
                                unit = PRTGUnit.BytesBandwidth,
                                value = statsInfo?.SingleOrDefault(i => i.Key.Equals("total_net_output_bytes")).Value
                            }
                        },
                        {
                            new PRTGResult
                            {
                                channel = "Rejected Connections",
                                unit = PRTGUnit.Count,
                                value = statsInfo?.SingleOrDefault(i => i.Key.Equals("rejected_connections")).Value
                            }
                        },
                        {
                            new PRTGResult
                            {
                                channel = "Pubsub Channels",
                                unit = PRTGUnit.Count,
                                value = statsInfo?.SingleOrDefault(i => i.Key.Equals("pubsub_channels")).Value
                            }
                        },
                        {
                            new PRTGResult
                            {
                                channel = "Pubsub Patterns",
                                unit = PRTGUnit.Count,
                                value = statsInfo?.SingleOrDefault(i => i.Key.Equals("pubsub_patterns")).Value
                            }
                        },
                        {
                            new PRTGResult
                            {
                                channel = "Is Master",
                                unit = PRTGUnit.Count,
                                ShowChart = 0,
                                value = replicationInfo != null ? (replicationInfo.SingleOrDefault(i => i.Key.Equals("role")).Value.Equals("master", StringComparison.InvariantCultureIgnoreCase) ? "1" : "0") : "0"
                            }
                        },
                        {
                            new PRTGResult
                            {
                                channel = "Is Replication Backlog Active",
                                unit = PRTGUnit.Count,
                                ShowChart = 0,
                                value = replicationInfo?.SingleOrDefault(i => i.Key.Equals("repl_backlog_active")).Value
                            }
                        },
                        {
                            new PRTGResult
                            {
                                channel = "Replication Backlog Size",
                                unit = PRTGUnit.BytesMemory,
                                value = replicationInfo?.SingleOrDefault(i => i.Key.Equals("repl_backlog_size")).Value
                            }
                        }
                        ,
                        {
                            new PRTGResult
                            {
                                channel = "Replication Backlog First Size Byte Offset",
                                unit = PRTGUnit.BytesMemory,
                                value = replicationInfo?.SingleOrDefault(i => i.Key.Equals("repl_backlog_first_byte_offset")).Value
                            }
                        },
                        {
                            new PRTGResult
                            {
                                channel = "Replication Backlog Histlen",
                                unit = PRTGUnit.BytesMemory,
                                value = replicationInfo?.SingleOrDefault(i => i.Key.Equals("repl_backlog_histlen")).Value
                            }
                        },
                        {
                            new PRTGResult
                            {
                                channel = "Connected Slaves",
                                unit = PRTGUnit.Count,
                                value = replicationInfo?.SingleOrDefault(i => i.Key.Equals("connected_slaves")).Value
                            }
                        },
                        {
                            new PRTGResult
                            {
                                channel = "Keys",
                                unit = PRTGUnit.Count,
                                value = SafeGetInt32(() => keyspaceInfo != null ? keyspaceInfo?.SingleOrDefault(i => i.Key.Equals("db0")).Value.Split(',').Select(x => x.Split('=')).ToDictionary(x => x[0], x => x[1])["keys"] : "0")
                            }
                        },
                        {
                            new PRTGResult
                            {
                                channel = "Keys Expires",
                                unit = PRTGUnit.Count,
                                value = SafeGetInt32(() => keyspaceInfo != null ? keyspaceInfo?.SingleOrDefault(i => i.Key.Equals("db0")).Value.Split(',').Select(x => x.Split('=')).ToDictionary(x => x[0], x => x[1])["expires"] : "0")
                            }
                        },
                        {
                            new PRTGResult
                            {
                                channel = "Keyspace Hits",
                                unit = PRTGUnit.Count,
                                value = statsInfo?.SingleOrDefault(i => i.Key.Equals("keyspace_hits")).Value
                            }
                        },
                        {
                            new PRTGResult
                            {
                                channel = "Keyspace Misses",
                                unit = PRTGUnit.Count,
                                value = statsInfo?.SingleOrDefault(i => i.Key.Equals("keyspace_misses")).Value
                            }
                        },
                        {
                            new PRTGResult()
                            {
                                channel = "Hit Rate",
                                unit = PRTGUnit.Percent,
                                Float=1,
                                value = SafeGetFloat(() =>
                                    (
                                        100.0 *
                                        Convert.ToDouble(statsInfo?.SingleOrDefault(i => i.Key.Equals("keyspace_hits")).Value) /
                                        (
                                            Convert.ToDouble(statsInfo?.SingleOrDefault(i => i.Key.Equals("keyspace_hits")).Value) +
                                            Convert.ToDouble(statsInfo?.SingleOrDefault(i => i.Key.Equals("keyspace_misses")).Value)
                                        )
                                    ).ToString()
                                )
                            }
                        },
                        {
                            new PRTGResult
                            {
                                channel = "Used CPU Sys",
                                unit = PRTGUnit.Custom,
                                Float=1,
                                value = cpuInfo?.SingleOrDefault(i => i.Key.Equals("used_cpu_sys")).Value
                            }
                        },
                        {
                            new PRTGResult
                            {
                                channel = "Used CPU User",
                                unit = PRTGUnit.Custom,
                                Float=1,
                                value = cpuInfo?.SingleOrDefault(i => i.Key.Equals("used_cpu_user")).Value
                            }
                        },
                        {
                            new PRTGResult
                            {
                                channel = "Used CPU Sys Children",
                                unit = PRTGUnit.Custom,
                                Float=1,
                                value = cpuInfo?.SingleOrDefault(i => i.Key.Equals("used_cpu_sys_children")).Value
                            }
                        },
                        {
                            new PRTGResult
                            {
                                channel = "Used CPU User Children",
                                unit = PRTGUnit.Custom,
                                Float=1,
                                value = cpuInfo?.SingleOrDefault(i => i.Key.Equals("used_cpu_user_children")).Value
                            }
                        },
                        {
                            new PRTGResult
                            {
                                channel = "Cluster Enabled",
                                unit = PRTGUnit.Count,
                                ShowChart = 0,
                                value = clusterInfo?.SingleOrDefault(i => i.Key.Equals("cluster_enabled")).Value
                            }
                        }
                    }
                }
            };

            Console.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            }));
        }

        private static string SafeGetInt32(Func<string> func)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                return "0";
            }
        }

        private static string SafeGetFloat(Func<string> func)
        {
            try
            {
                var result = func();
                return result.Equals("NaN", StringComparison.InvariantCultureIgnoreCase) ? "0" : result;

            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                return "0";
            }
        }
    }
}