using PRTG_Redis_Sensor.Enums;
using PRTG_Redis_Sensor.Services.Impl;
using StackExchange.Redis;

namespace PRTG_Redis_Sensor.Services.Factories
{
	public class RedisMonitorServiceFactory : IRedisMonitorServiceFactory
	{
		/// <inheritdoc/>
		public IRedisMonitorService Create(string endPoints, string password, int? databaseIndex, MonitorServiceEnum monitorType)
		{
			ConfigurationOptions options = new()
			{
				EndPoints = { { endPoints } },
				AllowAdmin = true,
				DefaultDatabase = databaseIndex
			};

			if (!string.IsNullOrEmpty(password))
			{
				options.Password = password;
			}

			switch (monitorType)
			{
				case MonitorServiceEnum.Stats:
					return new StatsRedisMonitorService(options);
				case MonitorServiceEnum.CacheKeysValues:
					return new CacheValueRedisMonitorService(options);
				default:
					return new EmptyRedisMonitorService();
			}
		}
	}
}
