using PRTG_Redis_Sensor.Enums;

namespace PRTG_Redis_Sensor.Services.Factories
{
	public interface IRedisMonitorServiceFactory
	{
		/// </summary>
		/// <param name="endPoints">Redis endpoint separated by semicolon</param>
		/// <param name="password">Redis password</param>
		/// <param name="databaseIndex">Redis default database to be usedonnect</param>
		/// <param name="cacheKeys">Keys whose values you want to retrieve from cache (separated by pipe | )</param>
		/// <returns></returns>
		IRedisMonitorService Create(string endPoints, string password, int? databaseIndex, MonitorServiceEnum monitorType);
	}
}
