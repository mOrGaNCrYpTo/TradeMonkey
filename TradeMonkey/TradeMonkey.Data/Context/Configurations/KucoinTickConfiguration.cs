﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using TradeMonkey.Data.Context;
using TradeMonkey.Data.Entity;

namespace TradeMonkey.Data.Context.Configurations
{
    public partial class KucoinTickConfiguration : IEntityTypeConfiguration<KucoinTick>
    {
        public void Configure(EntityTypeBuilder<KucoinTick> entity)
        {
            entity.HasKey(e => e.Sequence);

            entity.ToTable("Kucoin_Tick");

            entity.Property(e => e.Sequence).ValueGeneratedNever();
            entity.Property(e => e.BestAskPrice).HasColumnType("decimal(18, 8)");
            entity.Property(e => e.BestAskQuantity).HasColumnType("decimal(18, 8)");
            entity.Property(e => e.BestBidPrice).HasColumnType("decimal(18, 8)");
            entity.Property(e => e.BestBidQuantity).HasColumnType("decimal(18, 8)");
            entity.Property(e => e.LastPrice).HasColumnType("decimal(18, 8)");
            entity.Property(e => e.LastQuantity).HasColumnType("decimal(18, 8)");
            entity.Property(e => e.Timestamp).HasColumnType("datetime");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<KucoinTick> entity);
    }
}
