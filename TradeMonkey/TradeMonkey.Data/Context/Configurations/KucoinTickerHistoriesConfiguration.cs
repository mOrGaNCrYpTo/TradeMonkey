﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using TradeMonkey.Data.Context;
using TradeMonkey.Data.Entity;

namespace TradeMonkey.Data.Context.Configurations
{
    public partial class KucoinTickerHistoriesConfiguration : IEntityTypeConfiguration<KucoinTickerHistories>
    {
        public void Configure(EntityTypeBuilder<KucoinTickerHistories> entity)
        {
            entity.HasKey(e => new { e.unix, e.symbol }).HasName("PK_Kucoin_Tickers");

            entity.Property(e => e.symbol).HasMaxLength(50);
            entity.Property(e => e.Time_Frame)
            .IsRequired()
            .HasMaxLength(1)
            .IsUnicode(false)
            .IsFixedLength();

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<KucoinTickerHistories> entity);
    }
}
