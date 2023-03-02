using Newtonsoft.Json;

using System.Diagnostics;

namespace TradeMonkey.Data.Utils
{
    public abstract class BaseConverter<T> : JsonConverter where T : struct
    {
        private readonly bool quotes;

        protected abstract List<KeyValuePair<T, string>> Mapping { get; }

        protected BaseConverter(bool useQuotes)
        {
            quotes = useQuotes;
        }

        private bool GetValue(string value, out T result)
        {
            string value2 = value;
            KeyValuePair<T, string> keyValuePair = Mapping.FirstOrDefault<KeyValuePair<T, string>>((KeyValuePair<T, string> kv) => kv.Value.Equals(value2, StringComparison.InvariantCulture));
            if (keyValuePair.Equals(default(KeyValuePair<T, string>)))
            {
                keyValuePair = Mapping.FirstOrDefault<KeyValuePair<T, string>>((KeyValuePair<T, string> kv) => kv.Value.Equals(value2, StringComparison.InvariantCultureIgnoreCase));
            }

            if (!keyValuePair.Equals(default(KeyValuePair<T, string>)))
            {
                result = keyValuePair.Key;
                return true;
            }

            result = default(T);
            return false;
        }

        private string GetValue(T value)
        {
            return Mapping.FirstOrDefault<KeyValuePair<T, string>>((KeyValuePair<T, string> v) => v.Key.Equals(value))!.Value;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            string text = (value == null) ? null : GetValue((T)value);
            if (quotes)
            {
                writer.WriteValue(text);
            }
            else
            {
                writer.WriteRawValue(text);
            }
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }

            string value = reader.Value!.ToString();
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            if (!GetValue(value, out var result))
            {
                Trace.WriteLine(string.Format("{0:yyyy/MM/dd HH:mm:ss:fff} | Warning | Cannot map enum value. EnumType: {1}, Value: {2}, Known values: {3}. If you think {4} should added please open an issue on the Github repo", DateTime.Now, typeof(T), reader.Value, string.Join(", ", Mapping.Select<KeyValuePair<T, string>, string>((KeyValuePair<T, string> m) => m.Value)), reader.Value));
                return null;
            }

            return result;
        }

        public T ReadString(string data)
        {
            string data2 = data;
            return Mapping.FirstOrDefault<KeyValuePair<T, string>>((KeyValuePair<T, string> v) => v.Value == data2)!.Key;
        }

        public override bool CanConvert(Type objectType)
        {
            if (!(objectType == typeof(T)))
            {
                return Nullable.GetUnderlyingType(objectType) == typeof(T);
            }

            return true;
        }
    }
}