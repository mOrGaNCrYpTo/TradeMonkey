﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace TradeMonkey.Data.Entity;

public partial class TradingIndicato_rDatum
{
    public int Id { get; set; }

    public double Close { get; set; }

    public string Date { get; set; }

    public int Epoch { get; set; }

    public double? HoldingCumulativeRoi { get; set; }

    public int LastSignal { get; set; }

    public string Name { get; set; }

    public int Signal { get; set; }

    public double? StrategyCummulativeRoi { get; set; }

    public string Symbol { get; set; }

    public int TokenId { get; set; }

    public double Volume { get; set; }

    public virtual TokenMetrics_Token Token { get; set; }
}