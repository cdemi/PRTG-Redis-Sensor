using CommandLine;
using PRTG_Redis_Sensor.Enums;

namespace PRTG_Redis_Sensor
{
	public class CommandLineOptions
	{
		[Option('t', "Monitor type", Required = true, HelpText = "Indicates the type of monitor to execute.\r\n")]
		public MonitorServiceEnum Type { get; set; }

		[Option('e', "Redis endpoints", Required = true, HelpText = "Specify the redis endpoints (i.e. server1:port;server2:port)")]
		public string EndPoints { get; set; }

		[Option('p', "Redis password", Required = false, HelpText = "Specify the redis password if required")]
		public string Password { get; set; }

		[Option('d', "Database index", Required = false, HelpText = "Specify the database index")]
		public int DatabaseIndex { get; set; }

		[Option('k', "Cache keys", Required = false, HelpText = "Keys whose values you want to retrieve from cache (separated by pipe | )")]
		public string CacheKeys { get; set; }

		[Option('r', "Transform value", Required = false, HelpText = "Elaborate the value retrieved from cache using the specified transformation.\r\n")]
		public MonitorCacheValueTransformEnum? Transform { get; set; }
	}
}
