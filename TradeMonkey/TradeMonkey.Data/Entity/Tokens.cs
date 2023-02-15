﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable

namespace TradeMonkey.Data.Entity;

public partial class Tokens
{
    public int Token_Id { get; set; }

    public string Name { get; set; }

    public string Symbol { get; set; }

    public virtual ICollection<PricePredictionDatum> PricePredictionDatum { get; } = new List<PricePredictionDatum>();

    public virtual ICollection<QuantmetricsT1datum> QuantmetricsT1datum { get; } = new List<QuantmetricsT1datum>();

    public virtual ICollection<QuantmetricsT2datum> QuantmetricsT2datum { get; } = new List<QuantmetricsT2datum>();

    public virtual ICollection<SentimentsDatum> SentimentsDatum { get; } = new List<SentimentsDatum>();

    public virtual ICollection<TradingIndicatorDatum> TradingIndicatorDatum { get; } = new List<TradingIndicatorDatum>();
}