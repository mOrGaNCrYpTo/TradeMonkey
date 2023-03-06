﻿using Newtonsoft.Json;

namespace TradeMonkey.DataCollector.Value.Response
{
    public sealed class KucoinUpdateMessage<T>
    {
        [JsonProperty("subject")]
        public string Subject { get; set; } = string.Empty;

        [JsonProperty("topic")]
        public string Topic { get; set; } = string.Empty;

        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;

        [JsonProperty("data")]
        public T Data { get; set; } = default!;
    }
}