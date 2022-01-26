using System;
using System.Diagnostics;

namespace PRTG_Redis_Sensor.Utilities
{
	public class CastUtilities
	{
		public static string SafeGetInt32(Func<string> func)
		{
			try
			{
				return func();
			}
			catch (Exception ex)
			{
				Trace.TraceError(ex.ToString());
				return "0";
			}
		}

		public static string SafeGetFloat(Func<string> func)
		{
			try
			{
				var result = func();
				return result.Equals("NaN", StringComparison.InvariantCultureIgnoreCase) ? "0" : result;

			}
			catch (Exception ex)
			{
				Trace.TraceError(ex.ToString());
				return "0";
			}
		}

		public static string ConvertToString(byte[] cacheValue)
		{
			if (cacheValue == null || cacheValue.Length == 0)
				return "";

			return System.Text.UTF8Encoding.UTF8.GetString(cacheValue);
		}
	}
}
