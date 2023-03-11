using System.Text.Json.Serialization;

namespace TradeMonkey.Data.Entity
{
    public partial class TraderGradesDatum
    {
        public int Id { get; set; }

        [JsonPropertyName("TOKEN_ID")]
        public int TokenId { get; set; }

        [JsonPropertyName("DATE")]
        public string Date { get; set; }

        [JsonPropertyName("EPOCH")]
        public int Epoch { get; set; }

        [JsonPropertyName("NAME")]
        public string Name { get; set; }

        [JsonPropertyName("QUANT_GRADE")]
        public decimal QuantGrade { get; set; }

        [JsonPropertyName("SYMBOL")]
        public string Symbol { get; set; }

        [JsonPropertyName("TA_GRADE")]
        public decimal TaGrade { get; set; }

        [JsonPropertyName("TM_TRADER_GRADE")]
        public decimal TmTraderGrade { get; set; }
    }
}