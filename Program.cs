using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Diagnostics;
using System.Linq;

namespace PRTG_Redis_Sensor
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string serverHostAndPort = args[0];
            string connectionConfig = $"{serverHostAndPort},AllowAdmin=True";
            connectionConfig += GetPasswordFromArgs(args);

            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(connectionConfig);
            IServer server = redis.GetServer(serverHostAndPort);

            IGrouping<string, System.Collections.Generic.KeyValuePair<string, string>>[] info = server.Info();
            IGrouping<string, System.Collections.Generic.KeyValuePair<string, string>> serverInfo = info.SingleOrDefault(i => i.Key.Equals("Server", StringComparison.InvariantCultureIgnoreCase));
            IGrouping<string, System.Collections.Generic.KeyValuePair<string, string>> clientsInfo = info.SingleOrDefault(i => i.Key.Equals("Clients", StringComparison.InvariantCultureIgnoreCase));
            IGrouping<string, System.Collections.Generic.KeyValuePair<string, string>> memoryInfo = info.SingleOrDefault(i => i.Key.Equals("Memory", StringComparison.InvariantCultureIgnoreCase));
            IGrouping<string, System.Collections.Generic.KeyValuePair<string, string>> persistenceInfo = info.SingleOrDefault(i => i.Key.Equals("Persistence", StringComparison.InvariantCultureIgnoreCase));
            IGrouping<string, System.Collections.Generic.KeyValuePair<string, string>> statsInfo = info.SingleOrDefault(i => i.Key.Equals("Stats", StringComparison.InvariantCultureIgnoreCase));
            IGrouping<string, System.Collections.Generic.KeyValuePair<string, string>> replicationInfo = info.SingleOrDefault(i => i.Key.Equals("Replication", StringComparison.InvariantCultureIgnoreCase));
            IGrouping<string, System.Collections.Generic.KeyValuePair<string, string>> keyspaceInfo = info.SingleOrDefault(i => i.Key.Equals("Keyspace", StringComparison.InvariantCultureIgnoreCase));
            IGrouping<string, System.Collections.Generic.KeyValuePair<string, string>> cpuInfo = info.SingleOrDefault(i => i.Key.Equals("CPU", StringComparison.InvariantCultureIgnoreCase));
            IGrouping<string, System.Collections.Generic.KeyValuePair<string, string>> clusterInfo = info.SingleOrDefault(i => i.Key.Equals("Cluster", StringComparison.InvariantCultureIgnoreCase));

            var response =
                new
                {
                    prtg = new PRTGResponse()
                    {
                        result = new System.Collections.Generic.List<PRTGResult>
                        {
                            {
                                new PRTGResult()
                                {
                                    channel = "Uptime",
                                    unit = PRTGUnit.TimeSeconds,
                                    value = serverInfo.SingleOrDefault(i => i.Key.Equals("uptime_in_seconds")).Value
                                }
                            },
                            {
                                new PRTGResult()
                                {
                                    channel = "Connected Clients",
                                    unit = PRTGUnit.Count,
                                    value = clientsInfo.SingleOrDefault(i => i.Key.Equals("connected_clients")).Value
                                }
                            },
                            {
                                new PRTGResult()
                                {
                                    channel = "Blocked Clients",
                                    unit = PRTGUnit.Count,
                                    value = clientsInfo.SingleOrDefault(i => i.Key.Equals("blocked_clients")).Value
                                }
                            },
                            {
                                new PRTGResult()
                                {
                                    channel = "Used Memory",
                                    unit = PRTGUnit.BytesMemory,
                                    value = memoryInfo.SingleOrDefault(i => i.Key.Equals("used_memory")).Value
                                }
                            },
                            {
                                new PRTGResult()
                                {
                                    channel = "Used Memory RSS",
                                    unit = PRTGUnit.BytesMemory,
                                    value = memoryInfo.SingleOrDefault(i => i.Key.Equals("used_memory_rss")).Value
                                }
                            },
                            {
                                new PRTGResult()
                                {
                                    channel = "Used Memory Peak",
                                    unit = PRTGUnit.BytesMemory,
                                    value = memoryInfo.SingleOrDefault(i => i.Key.Equals("used_memory_peak")).Value
                                }
                            },
                            {
                                new PRTGResult()
                                {
                                    channel = "Memory Fragmentation Ratio",
                                    unit = PRTGUnit.Custom,
                                    Float = 1,
                                    value = memoryInfo.SingleOrDefault(i => i.Key.Equals("mem_fragmentation_ratio")).Value
                                }
                            },
                            {
                                new PRTGResult()
                                {
                                    channel = "RDB Last Background Save Status",
                                    unit = PRTGUnit.Count,
                                    ShowChart = 0,
                                    value = persistenceInfo.SingleOrDefault(i => i.Key.Equals("rdb_last_bgsave_status")).Value == "ok" ? "1" : "0"
                                }
                            },
                            {
                                new PRTGResult()
                                {
                                    channel = "RDB Last Background Save Time in Seconds",
                                    unit = PRTGUnit.TimeSeconds,
                                    value = persistenceInfo.SingleOrDefault(i => i.Key.Equals("rdb_last_bgsave_time_sec")).Value
                                }
                            },
                            {
                                new PRTGResult()
                                {
                                    channel = "AOF Last Rewrite Time in Seconds",
                                    unit = PRTGUnit.TimeSeconds,
                                    value = persistenceInfo.SingleOrDefault(i => i.Key.Equals("aof_last_rewrite_time_sec")).Value
                                }
                            },
                            {
                                new PRTGResult()
                                {
                                    channel = "AOF Last Background Rewrite Status",
                                    unit = PRTGUnit.Count,
                                    ShowChart = 0,
                                    value = persistenceInfo.SingleOrDefault(i => i.Key.Equals("aof_last_bgrewrite_status")).Value == "ok" ? "1" : "0"
                                }
                            },
                            {
                                new PRTGResult()
                                {
                                    channel = "AOF Last Write Status",
                                    unit = PRTGUnit.Count,
                                    ShowChart = 0,
                                    value = persistenceInfo.SingleOrDefault(i => i.Key.Equals("aof_last_write_status")).Value == "ok" ? "1" : "0"
                                }
                            },
                            {
                                new PRTGResult()
                                {
                                    channel = "Total Connections Received",
                                    unit = PRTGUnit.Count,
                                    value = statsInfo.SingleOrDefault(i => i.Key.Equals("total_connections_received")).Value
                                }
                            },
                            {
                                new PRTGResult()
                                {
                                    channel = "Total Commands Processed",
                                    unit = PRTGUnit.Count,
                                    value = statsInfo.SingleOrDefault(i => i.Key.Equals("total_commands_processed")).Value
                                }
                            },
                            {
                                new PRTGResult()
                                {
                                    channel = "Instantaneous Operations per Second",
                                    unit = PRTGUnit.Count,
                                    value = statsInfo.SingleOrDefault(i => i.Key.Equals("instantaneous_ops_per_sec")).Value
                                }
                            },
                            {
                                new PRTGResult()
                                {
                                    channel = "Total Net Input Bytes",
                                    unit = PRTGUnit.BytesBandwidth,
                                    value = statsInfo.SingleOrDefault(i => i.Key.Equals("total_net_input_bytes")).Value
                                }
                            },
                            {
                                new PRTGResult()
                                {
                                    channel = "Total Net Output Bytes",
                                    unit = PRTGUnit.BytesBandwidth,
                                    value = statsInfo.SingleOrDefault(i => i.Key.Equals("total_net_output_bytes")).Value
                                }
                            },
                            {
                                new PRTGResult()
                                {
                                    channel = "Rejected Connections",
                                    unit = PRTGUnit.Count,
                                    value = statsInfo.SingleOrDefault(i => i.Key.Equals("rejected_connections")).Value
                                }
                            },
                            {
                                new PRTGResult()
                                {
                                    channel = "Pubsub Channels",
                                    unit = PRTGUnit.Count,
                                    value = statsInfo.SingleOrDefault(i => i.Key.Equals("pubsub_channels")).Value
                                }
                            },
                            {
                                new PRTGResult()
                                {
                                    channel = "Pubsib Patterns",
                                    unit = PRTGUnit.Count,
                                    value = statsInfo.SingleOrDefault(i => i.Key.Equals("pubsub_patterns")).Value
                                }
                            },
                            {
                                new PRTGResult()
                                {
                                    channel = "Is Master",
                                    unit = PRTGUnit.Count,
                                    ShowChart = 0,
                                    value = replicationInfo.SingleOrDefault(i => i.Key.Equals("role")).Value.Equals("master", StringComparison.InvariantCultureIgnoreCase) ? "1" : "0"
                                }
                            },
                            {
                                new PRTGResult()
                                {
                                    channel = "Is Replication Backlog Active",
                                    unit = PRTGUnit.Count,
                                    ShowChart = 0,
                                    value = replicationInfo.SingleOrDefault(i => i.Key.Equals("repl_backlog_active")).Value
                                }
                            },
                            {
                                new PRTGResult()
                                {
                                    channel = "Replication Backlog Size",
                                    unit = PRTGUnit.BytesMemory,
                                    value = replicationInfo.SingleOrDefault(i => i.Key.Equals("repl_backlog_size")).Value
                                }
                            }
                            ,
                            {
                                new PRTGResult()
                                {
                                    channel = "Replication Backlog First Size Byte Offset",
                                    unit = PRTGUnit.BytesMemory,
                                    value = replicationInfo.SingleOrDefault(i => i.Key.Equals("repl_backlog_first_byte_offset")).Value
                                }
                            },
                            {
                                new PRTGResult()
                                {
                                    channel = "Replication Backlog Histlen",
                                    unit = PRTGUnit.BytesMemory,
                                    value = replicationInfo.SingleOrDefault(i => i.Key.Equals("repl_backlog_histlen")).Value
                                }
                            },
                            {
                                new PRTGResult()
                                {
                                    channel = "Connected Slaves",
                                    unit = PRTGUnit.Count,
                                    value = replicationInfo.SingleOrDefault(i => i.Key.Equals("connected_slaves")).Value
                                }
                            },
                            {
                                new PRTGResult()
                                {
                                    channel = "Keys",
                                    unit = PRTGUnit.Count,
                                    value = SafeGetInt32(() => keyspaceInfo != null ? keyspaceInfo.SingleOrDefault(i => i.Key.Equals("db0")).Value.Split(',').Select(x => x.Split('=')).ToDictionary(x => x[0], x => x[1])["keys"] : "0")
                                }
                            },
                            {
                                new PRTGResult()
                                {
                                    channel = "Keys Expires",
                                    unit = PRTGUnit.Count,
                                    value = SafeGetInt32(() => keyspaceInfo != null ? keyspaceInfo.SingleOrDefault(i => i.Key.Equals("db0")).Value.Split(',').Select(x => x.Split('=')).ToDictionary(x => x[0], x => x[1])["expires"] : "0")
                                }
                            },
                            {
                                new PRTGResult()
                                {
                                    channel = "keyspace_hits",
                                    unit = PRTGUnit.Count,
                                    value = statsInfo.SingleOrDefault(i => i.Key.Equals("keyspace_hits")).Value
                                }
                            },
                            {
                                new PRTGResult()
                                {
                                    channel = "keyspace_misses",
                                    unit = PRTGUnit.Count,
                                    value = statsInfo.SingleOrDefault(i => i.Key.Equals("keyspace_misses")).Value
                                }
                            },
                            {
                                new PRTGResult()
                                {
                                    channel = "Used CPU Sys",
                                    unit = PRTGUnit.Custom,
                                    Float=1,
                                    value = cpuInfo.SingleOrDefault(i => i.Key.Equals("used_cpu_sys")).Value
                                }
                            },
                            {
                                new PRTGResult()
                                {
                                    channel = "Used CPU User",
                                    unit = PRTGUnit.Custom,
                                    Float=1,
                                    value = cpuInfo.SingleOrDefault(i => i.Key.Equals("used_cpu_user")).Value
                                }
                            },
                            {
                                new PRTGResult()
                                {
                                    channel = "Used CPU Sys Children",
                                    unit = PRTGUnit.Custom,
                                    Float=1,
                                    value = cpuInfo.SingleOrDefault(i => i.Key.Equals("used_cpu_sys_children")).Value
                                }
                            },
                            {
                                new PRTGResult()
                                {
                                    channel = "Used CPU User Children",
                                    unit = PRTGUnit.Custom,
                                    Float=1,
                                    value = cpuInfo.SingleOrDefault(i => i.Key.Equals("used_cpu_user_children")).Value
                                }
                            },
                            {
                                new PRTGResult()
                                {
                                    channel = "Cluster Enabled",
                                    unit = PRTGUnit.Count,
                                    ShowChart = 0,
                                    value = clusterInfo.SingleOrDefault(i => i.Key.Equals("cluster_enabled")).Value
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

        /// <summary>
        /// Gets the Redis login password if it is present in the 
        /// second program argument.
        /// </summary>
        /// <param name="args">Array of program arguments</param>
        /// <returns>The formatted password string</returns>
        private static string GetPasswordFromArgs(string[] args)
        {
            if (args.Length > 1)
            {
                string redisPassword = args[1];
                return $",Password={redisPassword}";
            }

            return string.Empty;
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
    }
}
