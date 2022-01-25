using PRTG_Redis_Sensor.Enums;
using PRTG_Redis_Sensor.Model;

namespace PRTG_Redis_Sensor.Services
{
	public interface IRedisMonitorService
	{
		PRTGMonitor Execute(string parameter, MonitorCacheValueTransformEnum? transform);
	}
}
