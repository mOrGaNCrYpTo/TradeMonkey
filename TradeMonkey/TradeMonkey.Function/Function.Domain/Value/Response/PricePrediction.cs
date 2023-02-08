﻿namespace TradeMonkey.TokenMetrics.Domain.Value.Response
{
    public sealed class PricePrediction : Base
    {
        public PricePredictionData[] Data { get; set; }
    }

    public class PricePredictionData
    {
        public float CLOSE { get; set; }
        public int EPOCH { get; set; }
        public float HIGH { get; set; }
        public float LOW { get; set; }
        public string NAME { get; set; }
        public float OPEN { get; set; }
        public string SYMBOL { get; set; }
        public string TOKEN_ID { get; set; }
        public float VOLUME { get; set; }
    }
}