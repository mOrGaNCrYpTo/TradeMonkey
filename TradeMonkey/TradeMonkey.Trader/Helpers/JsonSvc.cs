namespace TradeMonkey.DataCollector.Helpers
{
    [RegisterService]
    public class JsonSvc
    {
        private readonly Log _log;
        private readonly JsonSerializerOptions _options;

        public JsonSvc()
        {
            _log = new Log("Kucoin");

            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public CallResult<T> Deserialize<T>(JsonElement obj, int? requestId = null)
        {
            try
            {
                return new
                    CallResult<T>(JsonSerializer.Deserialize<T>(obj.GetRawText() ?? string.Empty, _options));
            }
            catch (JsonException ex)
            {
                string message
                    = string.Format("{0}Deserialize JsonException: {1} data: {2}", requestId.HasValue ? $"[{requestId}] " : "", ex.Message, obj);

                _log.Write(LogLevel.Error, message);

                return new CallResult<T>(new DeserializeError(message, obj));
            }
            catch (Exception exception)
            {
                string arg = exception.ToLogString();

                string message3
                    = string.Format("{0}Deserialize Unknown Exception: {1}, data: {2}", requestId.HasValue ? $"[{requestId}] " : "", arg, obj);

                _log.Write(LogLevel.Error, message3);

                return new CallResult<T>(new DeserializeError(message3, obj));
            }
        }
    }
}