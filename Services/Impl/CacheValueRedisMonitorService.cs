using Microsoft.Extensions.Caching.StackExchangeRedis;
using Newtonsoft.Json;
using PRTG_Redis_Sensor.Enums;
using PRTG_Redis_Sensor.Model;
using PRTG_Redis_Sensor.Services.Base;
using PRTG_Redis_Sensor.Utilities;
using StackExchange.Redis;
using System;
using System.Collections.Generic;

namespace PRTG_Redis_Sensor.Services.Impl
{
	/// <summary>
	/// Retrieves a Redis key cache value
	/// </summary>
	public class CacheValueRedisMonitorService : BaseRedisMonitorService
	{
		public CacheValueRedisMonitorService(ConfigurationOptions options) : base(options) { }

		protected override PRTGMonitor InternalExecute(IConnectionMultiplexer connection, string parameter, MonitorCacheValueTransformEnum? transform)
		{
			using RedisCache cache = new RedisCache(new RedisCacheOptions { ConfigurationOptions = _options });

			if (string.IsNullOrEmpty(parameter))
				return null;

			string[] cacheKeys = parameter.Split("|");
			List<PRTGResult> elements = new List<PRTGResult>();

			foreach (var cacheKey in cacheKeys)
			{
				byte[] cacheValue = cache.Get(cacheKey);
				string value = CastUtilities.ConvertToString(cacheValue);

				if (transform.HasValue)
				{
					switch (transform.Value)
					{
						case MonitorCacheValueTransformEnum.ElapsedMinutes:
							DateTime dateValue = JsonConvert.DeserializeObject<DateTime>(value);
							value = Convert.ToInt64(DateTime.Now.Subtract(dateValue).TotalMinutes).ToString();
							break;
					}
				}

				elements.Add(new PRTGResult
				{
					Channel = cacheKey,
					Unit = PRTGUnit.Custom,
					Value = value,
				});
			}

			return new PRTGMonitor
			{
				Response = new PRTGResponse
				{
					result = elements
				}
			};
		}
	}
}
