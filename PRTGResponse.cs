using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace PRTG_Redis_Sensor
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PRTGUnit
    {
        BytesBandwidth,
        BytesMemory,
        BytesDisk,
        Temperature,
        Percent,
        TimeResponse,
        TimeSeconds,
        Custom,
        Count,
        CPU,
        BytesFile,
        SpeedDisk,
        SpeedNet,
        TimeHours
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PRTGSize
    {
        One,
        Kilo,
        Mega,
        Giga,
        Tera,
        Byte,
        KiloByte,
        MegaByte,
        GigaByte,
        TeraByte,
        Bit,
        KiloBit,
        MegaBit,
        GigaBit,
        TeraBit
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PRTGTime
    {
        Second,
        Minute,
        Hour,
        Day
    }
    public class PRTGResponse
    {
        public List<PRTGResult> result { get; set; }
    }

    public class PRTGResult
    {
        [JsonProperty(propertyName: "channel")]
        public string Channel { get; set; }

        [JsonProperty(propertyName: "value")]
        public string Value { get; set; }

        [JsonProperty(propertyName: "unit")]
        public PRTGUnit Unit { get; set; }

        [JsonProperty(propertyName: "showChart")]
        public string ShowChart { get; set; }

        [JsonProperty(propertyName:"float")]
        public string Float { get; set; }
    }
}
