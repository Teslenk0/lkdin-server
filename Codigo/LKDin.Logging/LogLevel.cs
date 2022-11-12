using System.Text.Json.Serialization;

namespace LKDin.Logging
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum LogLevel
    {
        INFO  = 00,
        WARN  = 01,
        ERROR = 02
    }
}