﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace TradeMonkey.Data.Entity;

public partial class KucoinTick
{
    public long Sequence { get; set; }

    public decimal? LastPrice { get; set; }

    public decimal? LastQuantity { get; set; }

    public decimal? BestAskPrice { get; set; }

    public decimal? BestAskQuantity { get; set; }

    public decimal? BestBidPrice { get; set; }

    public decimal? BestBidQuantity { get; set; }

    public DateTime Timestamp { get; set; }
}