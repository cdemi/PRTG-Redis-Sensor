using CommandLine;
using CommandLine.Text;
using Newtonsoft.Json;
using PRTG_Redis_Sensor.Model;
using PRTG_Redis_Sensor.Services;
using PRTG_Redis_Sensor.Services.Factories;
using System;
using System.Globalization;

namespace PRTG_Redis_Sensor
{
	internal class Program
	{
		private static void Main(string[] arguments)
		{
			// TODO: refactor, main has too many lines of code
			CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
			CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

			var parser = new Parser(with =>
			{
				with.CaseInsensitiveEnumValues = true;
				with.HelpWriter = null;
			});

			var result = parser.ParseArguments<CommandLineOptions>(arguments);

			result.MapResult(o =>
			{
				//TODO: move to a different service
				IRedisMonitorServiceFactory factory = new RedisMonitorServiceFactory();
				IRedisMonitorService service = null;

				// create service
				service = factory.Create(o.EndPoints, o.Password, o.DatabaseIndex, o.Type);

				// make prtg response
				PRTGMonitor response = service.Execute(o.CacheKeys, o.Transform); //TODO: review this interface, seems inelegant, cache keys and transform are not needed in every implementations

				// write to output
				Console.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented, new JsonSerializerSettings
				{
					NullValueHandling = NullValueHandling.Ignore
				}));

				return 1;
			},
			errs =>
			{
				var helpText = HelpText.AutoBuild(result, h =>
				{
					// Configure HelpText	 
					h.AddEnumValuesToHelpText = true;
					return h;
				}, e => e, verbsIndex: true);
				Console.WriteLine(helpText);
				return 1;
			});
		}
	}
}