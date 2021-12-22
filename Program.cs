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
                                Channel = "Uptime",
                                Unit = PRTGUnit.TimeSeconds,
                                Value = serverInfo?.SingleOrDefault(i => i.Key.Equals("uptime_in_seconds")).Value ?? "0"
                            }
                        },
                        {
                            new PRTGResult
                            {
                                Channel = "Connected Clients",
                                Unit = PRTGUnit.Count,
                                Value = clientsInfo?.SingleOrDefault(i => i.Key.Equals("connected_clients")).Value ?? "0"
                            }
                        },
                        {
                            new PRTGResult
                            {
                                Channel = "Blocked Clients",
                                Unit = PRTGUnit.Count,
                                Value = clientsInfo?.SingleOrDefault(i => i.Key.Equals("blocked_clients")).Value ?? "0"
                            }
                        },
                        {
                            new PRTGResult
                            {
                                Channel = "Used Memory",
                                Unit = PRTGUnit.BytesMemory,
                                Value = memoryInfo?.SingleOrDefault(i => i.Key.Equals("used_memory")).Value ?? "0"
                            }
                        },
                        {
                            new PRTGResult
                            {
                                Channel = "Used Memory RSS",
                                Unit = PRTGUnit.BytesMemory,
                                Value = memoryInfo?.SingleOrDefault(i => i.Key.Equals("used_memory_rss")).Value ?? "0"
                            }
                        },
                        {
                            new PRTGResult
                            {
                                Channel = "Used Memory Peak",
                                Unit = PRTGUnit.BytesMemory,
                                Value = memoryInfo?.SingleOrDefault(i => i.Key.Equals("used_memory_peak")).Value ?? "0"
                            }
                        },
                        {
                            new PRTGResult
                            {
                                Channel = "Memory Fragmentation Ratio",
                                Unit = PRTGUnit.Custom,
                                Value = memoryInfo?.SingleOrDefault(i => i.Key.Equals("mem_fragmentation_ratio")).Value ?? "0",
                                Float = "1"
                            }
                        },
                        {
                            new PRTGResult()
                            {
                                Channel = "RDB Changes Since Last Save",
                                Unit = PRTGUnit.Custom,
                                Value = persistenceInfo?.SingleOrDefault(i => i.Key.Equals("rdb_changes_since_last_save")).Value == "ok" ? "1" : "0"
                            }
                        },
                        {
                            new PRTGResult()
                            {
                                Channel = "RDB Last Save Time",
                                Unit = PRTGUnit.Custom,
                                Value = persistenceInfo?.SingleOrDefault(i => i.Key.Equals("rdb_last_save_time")).Value == "ok" ? "1" : "0"
                            }
                        },
                        {
                            new PRTGResult
                            {
                                Channel = "RDB Last Background Save Status",
                                Unit = PRTGUnit.Count,
                                Value = persistenceInfo?.SingleOrDefault(i => i.Key.Equals("rdb_last_bgsave_status")).Value == "ok" ? "1" : "0",
                                ShowChart = "0"
                            }
                        },
                        {
                            new PRTGResult
                            {
                                Channel = "RDB Last Background Save Time in Seconds",
                                Unit = PRTGUnit.TimeSeconds,
                                Value = persistenceInfo?.SingleOrDefault(i => i.Key.Equals("rdb_last_bgsave_time_sec")).Value ?? "0"
                            }
                        },
                        {
                            new PRTGResult
                            {
                                Channel = "AOF Last Rewrite Time in Seconds",
                                Unit = PRTGUnit.TimeSeconds,
                                Value = persistenceInfo?.SingleOrDefault(i => i.Key.Equals("aof_last_rewrite_time_sec")).Value ?? "0"
                            }
                        },
                        {
                            new PRTGResult
                            {
                                Channel = "AOF Last Background Rewrite Status",
                                Unit = PRTGUnit.Count,
                                Value = persistenceInfo?.SingleOrDefault(i => i.Key.Equals("aof_last_bgrewrite_status")).Value == "ok" ? "1" : "0",
                                ShowChart = "0"
                            }
                        },
                        {
                            new PRTGResult
                            {
                                Channel = "AOF Last Write Status",
                                Unit = PRTGUnit.Count,
                                Value = persistenceInfo?.SingleOrDefault(i => i.Key.Equals("aof_last_write_status")).Value == "ok" ? "1" : "0",
                                ShowChart = "0"
                            }
                        },
                        {
                            new PRTGResult
                            {
                                Channel = "Total Connections Received",
                                Unit = PRTGUnit.Count,
                                Value = statsInfo?.SingleOrDefault(i => i.Key.Equals("total_connections_received")).Value ?? "0"
                            }
                        },
                        {
                            new PRTGResult
                            {
                                Channel = "Total Commands Processed",
                                Unit = PRTGUnit.Count,
                                Value = statsInfo?.SingleOrDefault(i => i.Key.Equals("total_commands_processed")).Value ?? "0"
                            }
                        },
                        {
                            new PRTGResult
                            {
                                Channel = "Instantaneous Operations per Second",
                                Unit = PRTGUnit.Count,
                                Value = statsInfo?.SingleOrDefault(i => i.Key.Equals("instantaneous_ops_per_sec")).Value ?? "0"
                            }
                        },
                        {
                            new PRTGResult()
                            {
                                Channel = "Instantaneous Input kbps",
                                Unit = PRTGUnit.BytesBandwidth,
                                Value = statsInfo?.SingleOrDefault(i => i.Key.Equals("instantaneous_input_kbps")).Value ?? "0",
                                //Float = "1"
                            }
                        },
                        {
                            new PRTGResult()
                            {
                                Channel = "Instantaneous Output kbps",
                                Unit = PRTGUnit.BytesBandwidth,
                                Value = statsInfo?.SingleOrDefault(i => i.Key.Equals("instantaneous_output_kbps")).Value ?? "0",
                                Float = "1"
                            }
                        },
                        {
                            new PRTGResult
                            {
                                Channel = "Total Net Input Bytes",
                                Unit = PRTGUnit.BytesBandwidth,
                                Value = statsInfo?.SingleOrDefault(i => i.Key.Equals("total_net_input_bytes")).Value ?? "0",
                                Float = "1"
                            }
                        },
                        {
                            new PRTGResult
                            {
                                Channel = "Total Net Output Bytes",
                                Unit = PRTGUnit.BytesBandwidth,
                                Value = statsInfo?.SingleOrDefault(i => i.Key.Equals("total_net_output_bytes")).Value ?? "0",
                                Float = "1"
                            }
                        },
                        {
                            new PRTGResult
                            {
                                Channel = "Rejected Connections",
                                Unit = PRTGUnit.Count,
                                Value = statsInfo?.SingleOrDefault(i => i.Key.Equals("rejected_connections")).Value ?? "0"
                            }
                        },
                        {
                            new PRTGResult
                            {
                                Channel = "Pubsub Channels",
                                Unit = PRTGUnit.Count,
                                Value = statsInfo?.SingleOrDefault(i => i.Key.Equals("pubsub_channels")).Value ?? "0"
                            }
                        },
                        {
                            new PRTGResult
                            {
                                Channel = "Pubsub Patterns",
                                Unit = PRTGUnit.Count,
                                Value = statsInfo?.SingleOrDefault(i => i.Key.Equals("pubsub_patterns")).Value ?? "0"
                            }
                        },
                        {
                            new PRTGResult
                            {
                                Channel = "Is Master",
                                Unit = PRTGUnit.Count,
                                Value = replicationInfo.SingleOrDefault(i => i.Key.Equals("role")).Value.Equals("master", StringComparison.InvariantCultureIgnoreCase) ? "1" : "0",
                                ShowChart = "0"
                            }
                        },
                        {
                            new PRTGResult
                            {
                                Channel = "Is Replication Backlog Active",
                                Unit = PRTGUnit.Count,
                                Value = replicationInfo?.SingleOrDefault(i => i.Key.Equals("repl_backlog_active")).Value ?? "0",
                                ShowChart = "0"
                            }
                        },
                        {
                            new PRTGResult
                            {
                                Channel = "Replication Backlog Size",
                                Unit = PRTGUnit.BytesMemory,
                                Value = replicationInfo?.SingleOrDefault(i => i.Key.Equals("repl_backlog_size")).Value ?? "0"
                            }
                        }
                        ,
                        {
                            new PRTGResult
                            {
                                Channel = "Replication Backlog First Size Byte Offset",
                                Unit = PRTGUnit.BytesMemory,
                                Value = replicationInfo?.SingleOrDefault(i => i.Key.Equals("repl_backlog_first_byte_offset")).Value ?? "0"
                            }
                        },
                        {
                            new PRTGResult
                            {
                                Channel = "Replication Backlog Histlen",
                                Unit = PRTGUnit.BytesMemory,
                                Value = replicationInfo?.SingleOrDefault(i => i.Key.Equals("repl_backlog_histlen")).Value ?? "0"
                            }
                        },
                        {
                            new PRTGResult
                            {
                                Channel = "Connected Slaves",
                                Unit = PRTGUnit.Count,
                                Value = replicationInfo?.SingleOrDefault(i => i.Key.Equals("connected_slaves")).Value ?? "0"
                            }
                        },
                        {
                            new PRTGResult
                            {
                                Channel = "Keys",
                                Unit = PRTGUnit.Count,
                                Value = SafeGetInt32(() => keyspaceInfo != null ? keyspaceInfo.SingleOrDefault(i => i.Key.Equals("db0")).Value.Split(',').Select(x => x.Split('=')).ToDictionary(x => x[0], x => x[1])["keys"] : "0")
                            }
                        },
                        {
                            new PRTGResult
                            {
                                Channel = "Keys Expires",
                                Unit = PRTGUnit.Count,
                                Value = SafeGetInt32(() => keyspaceInfo != null ? keyspaceInfo.SingleOrDefault(i => i.Key.Equals("db0")).Value.Split(',').Select(x => x.Split('=')).ToDictionary(x => x[0], x => x[1])["expires"] : "0")
                            }
                        },
                        {
                            new PRTGResult
                            {
                                Channel = "Keyspace Hits",
                                Unit = PRTGUnit.Count,
                                Value = statsInfo?.SingleOrDefault(i => i.Key.Equals("keyspace_hits")).Value ?? "0"
                            }
                        },
                        {
                            new PRTGResult
                            {
                                Channel = "Keyspace Misses",
                                Unit = PRTGUnit.Count,
                                Value = statsInfo?.SingleOrDefault(i => i.Key.Equals("keyspace_misses")).Value ?? "0"
                            }
                        },
                        {
                            new PRTGResult()
                            {
                                Channel = "Hit Rate",
                                Unit = PRTGUnit.Percent,
                                Value = SafeGetFloat(() => statsInfo != null ?
                                    (
                                        (100.0 *
                                         Convert.ToDouble(statsInfo.SingleOrDefault(i => i.Key.Equals("keyspace_hits")).Value) /
                                         (
                                             Convert.ToDouble(statsInfo.SingleOrDefault(i => i.Key.Equals("keyspace_hits")).Value) +
                                             Convert.ToDouble(statsInfo.SingleOrDefault(i => i.Key.Equals("keyspace_misses")).Value)
                                         )) 
                                    ).ToString() : "0"
                                ),
                                Float = "1"
                            }
                        },
                        {
                            new PRTGResult
                            {
                                Channel = "Used CPU Sys",
                                Unit = PRTGUnit.Custom,
                                Value = cpuInfo?.SingleOrDefault(i => i.Key.Equals("used_cpu_sys")).Value ?? "0",
                                Float = "1"
                            }
                        },
                        {
                            new PRTGResult
                            {
                                Channel = "Used CPU User",
                                Unit = PRTGUnit.Custom,
                                Value = cpuInfo?.SingleOrDefault(i => i.Key.Equals("used_cpu_user")).Value ?? "0",
                                Float = "1"
                            }
                        },
                        {
                            new PRTGResult
                            {
                                Channel = "Used CPU Sys Children",
                                Unit = PRTGUnit.Custom,
                                Value = cpuInfo?.SingleOrDefault(i => i.Key.Equals("used_cpu_sys_children")).Value ?? "0",
                                Float = "1"
                            }
                        },
                        {
                            new PRTGResult
                            {
                                Channel = "Used CPU User Children",
                                Unit = PRTGUnit.Custom,
                                Value = cpuInfo?.SingleOrDefault(i => i.Key.Equals("used_cpu_user_children")).Value ?? "0",
                                Float = "1"
                            }
                        },
                        {
                            new PRTGResult
                            {
                                Channel = "Cluster Enabled",
                                Unit = PRTGUnit.Count,
                                Value = clusterInfo?.SingleOrDefault(i => i.Key.Equals("cluster_enabled")).Value ?? "0",
                                ShowChart = "0"
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