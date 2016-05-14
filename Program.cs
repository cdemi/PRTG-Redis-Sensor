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
                            channel = "Memory Fragmentation Ratio",
                            unit = PRTGUnit.Custom,
                            Float = 1,
                            value = memoryInfo.SingleOrDefault(i => i.Key.Equals("mem_fragmentation_ratio")).Value
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
                            channel = "Is Master",
                            unit = PRTGUnit.Count,
                            value = replicationInfo.SingleOrDefault(i => i.Key.Equals("role")).Value.Equals("master", StringComparison.InvariantCultureIgnoreCase)?"1":"0"
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
                            value = server.DatabaseSize(0).ToString()
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
    }
}
