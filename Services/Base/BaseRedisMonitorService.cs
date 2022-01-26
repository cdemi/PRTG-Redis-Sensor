using PRTG_Redis_Sensor.Enums;
using PRTG_Redis_Sensor.Model;
using StackExchange.Redis;
using System;

namespace PRTG_Redis_Sensor.Services.Base
{
	public abstract class BaseRedisMonitorService : IRedisMonitorService
	{
		protected readonly ConfigurationOptions _options;
		protected readonly IConnectionMultiplexer _connection;

		public BaseRedisMonitorService(ConfigurationOptions options)
		{
			_options = options ?? throw new ArgumentNullException(nameof(options));
		}

		protected abstract PRTGMonitor InternalExecute(IConnectionMultiplexer connection, string parameter, MonitorCacheValueTransformEnum? transform);

		public PRTGMonitor Execute(string parameter, MonitorCacheValueTransformEnum? transform)
		{
			// prepare connection
			// TODO: use a factory, for mocking/tests purpose
			using (var connection = ConnectionMultiplexer.Connect(_options))
			{
				return InternalExecute(connection, parameter, transform);
			}
		}
	}
}
