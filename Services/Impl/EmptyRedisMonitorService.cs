using PRTG_Redis_Sensor.Enums;
using PRTG_Redis_Sensor.Model;

namespace PRTG_Redis_Sensor.Services.Impl
{
	public class EmptyRedisMonitorService : IRedisMonitorService
	{
		public PRTGMonitor Execute(string parameter, MonitorCacheValueTransformEnum? transform)
		{
			return new PRTGMonitor { };
		}
	}
}
