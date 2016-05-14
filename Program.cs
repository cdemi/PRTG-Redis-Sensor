using StackExchange.Redis;
using System.Linq;
using System;
using Newtonsoft.Json;

namespace PRTG_Redis_Sensor
{
    class Program
    {
        static void Main(string[] args)
        {
            var serverHostAndPort = args[0];

            var redis = ConnectionMultiplexer.Connect($"{serverHostAndPort},AllowAdmin=True");
            var server = redis.GetServer(serverHostAndPort);

            var info = server.Info();
            var serverInfo = info.SingleOrDefault(i => i.Key.Equals("Server", StringComparison.InvariantCultureIgnoreCase));
            var clientsInfo = info.SingleOrDefault(i => i.Key.Equals("Clients", StringComparison.InvariantCultureIgnoreCase));
            var memoryInfo = info.SingleOrDefault(i => i.Key.Equals("Memory", StringComparison.InvariantCultureIgnoreCase));
            var statsInfo = info.SingleOrDefault(i => i.Key.Equals("Stats", StringComparison.InvariantCultureIgnoreCase));
            var replicationInfo = info.SingleOrDefault(i => i.Key.Equals("Replication", StringComparison.InvariantCultureIgnoreCase));

            var response = new PRTGResponse()
            {
                Result = new System.Collections.Generic.List<PRTGResult>
                {
                    {
                        new PRTGResult()
                        {
                            Channel = "Uptime",
                            Unit = PRTGUnit.TimeSeconds,
                            Value = serverInfo.SingleOrDefault(i => i.Key.Equals("uptime_in_seconds")).Value
                        }
                    },
                    {
                        new PRTGResult()
                        {
                            Channel = "Connected Clients",
                            Unit = PRTGUnit.Count,
                            Value = clientsInfo.SingleOrDefault(i => i.Key.Equals("connected_clients")).Value
                        }
                    },
                    {
                        new PRTGResult()
                        {
                            Channel = "Blocked Clients",
                            Unit = PRTGUnit.Count,
                            Value = clientsInfo.SingleOrDefault(i => i.Key.Equals("blocked_clients")).Value
                        }
                    },
                    {
                        new PRTGResult()
                        {
                            Channel = "Used Memory",
                            Unit = PRTGUnit.BytesMemory,
                            Value = memoryInfo.SingleOrDefault(i => i.Key.Equals("used_memory")).Value
                        }
                    },
                    {
                        new PRTGResult()
                        {
                            Channel = "Memory Fragmentation Ratio",
                            Unit = PRTGUnit.Custom,
                            Float = 1,
                            Value = memoryInfo.SingleOrDefault(i => i.Key.Equals("mem_fragmentation_ratio")).Value
                        }
                    },
                    {
                        new PRTGResult()
                        {
                            Channel = "Total Connections Received",
                            Unit = PRTGUnit.Count,
                            Value = statsInfo.SingleOrDefault(i => i.Key.Equals("total_connections_received")).Value
                        }
                    },
                    {
                        new PRTGResult()
                        {
                            Channel = "Total Commands Processed",
                            Unit = PRTGUnit.Count,
                            Value = statsInfo.SingleOrDefault(i => i.Key.Equals("total_commands_processed")).Value
                        }
                    },
                    {
                        new PRTGResult()
                        {
                            Channel = "Instantaneous Operations per Second",
                            Unit = PRTGUnit.Count,
                            Value = statsInfo.SingleOrDefault(i => i.Key.Equals("instantaneous_ops_per_sec")).Value
                        }
                    },
                    {
                        new PRTGResult()
                        {
                            Channel = "Replication Role",
                            Unit = PRTGUnit.Custom,
                            Value = replicationInfo.SingleOrDefault(i => i.Key.Equals("role")).Value
                        }
                    },
                    {
                        new PRTGResult()
                        {
                            Channel = "Connected Slaves",
                            Unit = PRTGUnit.Count,
                            Value = replicationInfo.SingleOrDefault(i => i.Key.Equals("connected_slaves")).Value
                        }
                    },
                    {
                        new PRTGResult()
                        {
                            Channel = "Keys",
                            Unit = PRTGUnit.Count,
                            Value = server.DatabaseSize.ToString()
                        }
                    }
                }
            };

            Console.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            }));
        }
    }
}
