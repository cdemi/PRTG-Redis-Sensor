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
        public List<PRTGResult> Result { get; set; }
    }

    public class PRTGResult
    {

        public string Channel { get; set; }
        public string Value { get; set; }
        public PRTGUnit Unit { get; set; }
        public PRTGSize? SpeedSize { get; set; }
        public PRTGSize? VolumeSize { get; set; }
        public PRTGTime? SpeedTime { get; set; }
        public int? Float;
    }
}
