using Newtonsoft.Json;

namespace PRTG_Redis_Sensor.Model
{
	public class PRTGMonitor
	{

		[JsonProperty(propertyName: "prtg")]
		public PRTGResponse Response { get; set; }
	}
}
