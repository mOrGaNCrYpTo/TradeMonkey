﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using TradeMonkey.Data.Context;
using TradeMonkey.Data.Entity;

namespace TradeMonkey.Data.Context.Configurations
{
    public partial class KucoinTicksConfiguration : IEntityTypeConfiguration<KucoinTick>
    {
        public void Configure(EntityTypeBuilder<KucoinTick> entity)
        {
            entity.HasIndex(e => e.Symbol, "IX_KucoinTicks_Symbol");

            entity.HasIndex(e => e.Timestamp, "IX_KucoinTicks_Timestamp");

            entity.Property(e => e.BestAskPrice).HasColumnType("decimal(18, 9)");
            entity.Property(e => e.BestAskQuantity).HasColumnType("decimal(18, 9)");
            entity.Property(e => e.BestBidPrice).HasColumnType("decimal(18, 9)");
            entity.Property(e => e.BestBidQuantity).HasColumnType("decimal(18, 9)");
            entity.Property(e => e.LastPrice).HasColumnType("decimal(18, 9)");
            entity.Property(e => e.LastQuantity).HasColumnType("decimal(18, 9)");
            entity.Property(e => e.Symbol)
            .IsRequired()
            .HasMaxLength(50);

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<KucoinTick> entity);
    }
}