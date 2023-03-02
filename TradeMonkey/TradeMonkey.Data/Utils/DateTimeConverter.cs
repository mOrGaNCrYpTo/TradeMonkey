using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using System.Globalization;

namespace TradeMonkey.Data.Utils
{
    public class DateTimeConverter : JsonConverter
    {
        private const long ticksPerSecond = 10000000L;
        private const decimal ticksPerMicrosecond = 10m;
        private const decimal ticksPerNanosecond = 0.01m;
        private static readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime ConvertFromSeconds(double seconds)
        {
            return _epoch.AddTicks((long)Math.Round(seconds * 10000000.0));
        }

        public static DateTime ConvertFromMilliseconds(double milliseconds)
        {
            return _epoch.AddTicks((long)Math.Round(milliseconds * 10000.0));
        }

        public static DateTime ConvertFromMicroseconds(long microseconds)
        {
            return _epoch.AddTicks((long)Math.Round((decimal)microseconds * 10m));
        }

        public static DateTime ConvertFromNanoseconds(long nanoseconds)
        {
            return _epoch.AddTicks((long)Math.Round((decimal)nanoseconds * 0.01m));
        }

        [return: NotNullIfNotNull("time")]
        public static long? ConvertToSeconds(DateTime? time)
        {
            if (time.HasValue)
            {
                return (long)Math.Round((time.Value - _epoch).TotalSeconds);
            }

            return null;
        }

        [return: NotNullIfNotNull("time")]
        public static long? ConvertToMilliseconds(DateTime? time)
        {
            if (time.HasValue)
            {
                return (long)Math.Round((time.Value - _epoch).TotalMilliseconds);
            }

            return null;
        }

        [return: NotNullIfNotNull("time")]
        public static long? ConvertToMicroseconds(DateTime? time)
        {
            if (time.HasValue)
            {
                return (long)Math.Round((decimal)(time.Value - _epoch).Ticks / 10m);
            }

            return null;
        }

        [return: NotNullIfNotNull("time")]
        public static long? ConvertToNanoseconds(DateTime? time)
        {
            if (time.HasValue)
            {
                return (long)Math.Round((decimal)(time.Value - _epoch).Ticks / 0.01m);
            }

            return null;
        }

        public override bool CanConvert(Type objectType)
        {
            if (!(objectType == typeof(DateTime)))
            {
                return objectType == typeof(DateTime?);
            }

            return true;
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }

            if (reader.TokenType == JsonToken.Integer)
            {
                long num = (long)reader.Value;
                if (num == 0L || num == -1)
                {
                    if (!(objectType == typeof(DateTime)))
                    {
                        return null;
                    }

                    return default(DateTime);
                }

                if (num < 19999999999L)
                {
                    return ConvertFromSeconds(num);
                }

                if (num < 19999999999999L)
                {
                    return ConvertFromMilliseconds(num);
                }

                if (num < 19999999999999999L)
                {
                    return ConvertFromMicroseconds(num);
                }

                return ConvertFromNanoseconds(num);
            }

            if (reader.TokenType == JsonToken.Float)
            {
                double num2 = (double)reader.Value;
                if (num2 == 0.0 || num2 == -1.0)
                {
                    if (!(objectType == typeof(DateTime)))
                    {
                        return null;
                    }

                    return default(DateTime);
                }

                if (num2 < 19999999999.0)
                {
                    return ConvertFromSeconds(num2);
                }

                return ConvertFromMilliseconds(num2);
            }

            if (reader.TokenType == JsonToken.String)
            {
                string text = (string)reader.Value;
                if (string.IsNullOrWhiteSpace(text))
                {
                    return null;
                }

                if (string.IsNullOrWhiteSpace(text) || text == "0" || text == "-1")
                {
                    if (!(objectType == typeof(DateTime)))
                    {
                        return null;
                    }

                    return default(DateTime);
                }

                if (text.Length == 8)
                {
                    if (!int.TryParse(text.Substring(0, 4), out var result) || !int.TryParse(text.Substring(4, 2), out var result2) || !int.TryParse(text.Substring(6, 2), out var result3))
                    {
                        Trace.WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss:fff} | Warning | Unknown DateTime format: " + reader.Value);
                        return null;
                    }

                    return new DateTime(result, result2, result3, 0, 0, 0, DateTimeKind.Utc);
                }

                if (text.Length == 6)
                {
                    if (!int.TryParse(text.Substring(0, 2), out var result4) || !int.TryParse(text.Substring(2, 2), out var result5) || !int.TryParse(text.Substring(4, 2), out var result6))
                    {
                        Trace.WriteLine("{DateTime.Now:yyyy/MM/dd HH:mm:ss:fff} | Warning | Unknown DateTime format: " + reader.Value);
                        return null;
                    }

                    return new DateTime(result4 + 2000, result5, result6, 0, 0, 0, DateTimeKind.Utc);
                }

                if (double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var result7))
                {
                    if (result7 < 19999999999.0)
                    {
                        return ConvertFromSeconds(result7);
                    }

                    if (result7 < 19999999999999.0)
                    {
                        return ConvertFromMilliseconds((long)result7);
                    }

                    if (result7 < 2E+16)
                    {
                        return ConvertFromMicroseconds((long)result7);
                    }

                    return ConvertFromNanoseconds((long)result7);
                }

                if (text.Length == 10)
                {
                    string[] array = text.Split('-');
                    if (!int.TryParse(array[0], out var result8) || !int.TryParse(array[1], out var result9) || !int.TryParse(array[2], out var result10))
                    {
                        Trace.WriteLine("{DateTime.Now:yyyy/MM/dd HH:mm:ss:fff} | Warning | Unknown DateTime format: " + reader.Value);
                        return null;
                    }

                    return new DateTime(result8, result9, result10, 0, 0, 0, DateTimeKind.Utc);
                }

                return DateTime.Parse(text, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
            }

            if (reader.TokenType == JsonToken.Date)
            {
                return (DateTime)reader.Value;
            }

            Trace.WriteLine("{DateTime.Now:yyyy/MM/dd HH:mm:ss:fff} | Warning | Unknown DateTime format: " + reader.Value);
            return null;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            DateTime? dateTime = (DateTime?)value;
            if (!dateTime.HasValue)
            {
                writer.WriteValue((DateTime?)null);
            }

            if (dateTime == default(DateTime))
            {
                writer.WriteValue((DateTime?)null);
            }
            else
            {
                writer.WriteValue((long)Math.Round(((DateTime)value - new DateTime(1970, 1, 1)).TotalMilliseconds));
            }
        }
    }
}