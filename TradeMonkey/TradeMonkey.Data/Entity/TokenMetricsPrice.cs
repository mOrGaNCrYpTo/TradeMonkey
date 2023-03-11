using System.Text.Json.Serialization;

namespace TradeMonkey.Data.Entity
{
    public partial class TokenMetricsPrice
    {
        public int Id { get; set; }

        [JsonPropertyName("TOKEN_ID")]
        public int TokenId { get; set; }

        [JsonPropertyName("NAME")]
        public string Name { get; set; }

        [JsonPropertyName("SYMBOL")]
        public string Symbol { get; set; }

        [JsonPropertyName("CURRENT_PRICE")]
        public decimal CurrentPrice { get; set; }

        [JsonPropertyName("OPEN")]
        public decimal Open { get; set; } = default;

        [JsonPropertyName("HIGH")]
        public decimal High { get; set; } = default;

        [JsonPropertyName("LOW")]
        public decimal Low { get; set; } = default;

        [JsonPropertyName("CLOSE")]
        public decimal Close { get; set; } = default;

        [JsonPropertyName("VOLUME")]
        public decimal Volume { get; set; } = default;

        [JsonPropertyName("EPOCH")]
        public int Epoch { get; set; }

        [JsonPropertyName("DATE")]
        public DateTime Date { get; set; }
    }
}