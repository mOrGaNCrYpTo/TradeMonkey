﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using TradeMonkey.Data.Context;
using TradeMonkey.Data.Entity;

namespace TradeMonkey.Data.Context.Configurations
{
    public partial class CryptoDataConfiguration : IEntityTypeConfiguration<CryptoData>
    {
        public void Configure(EntityTypeBuilder<CryptoData> entity)
        {
            entity.HasKey(e => e.Id).HasName("PK__CryptoDa__3214EC07E502F28C");

            entity.ToTable("CryptoData");

            entity.Property(e => e.RateClose)
            .HasColumnType("decimal(18, 6)")
            .HasColumnName("rate_close");
            entity.Property(e => e.RateHigh)
            .HasColumnType("decimal(18, 6)")
            .HasColumnName("rate_high");
            entity.Property(e => e.RateLow)
            .HasColumnType("decimal(18, 6)")
            .HasColumnName("rate_low");
            entity.Property(e => e.RateOpen)
            .HasColumnType("decimal(18, 6)")
            .HasColumnName("rate_open");
            entity.Property(e => e.TimeClose)
            .HasColumnType("datetime")
            .HasColumnName("time_close");
            entity.Property(e => e.TimeOpen)
            .HasColumnType("datetime")
            .HasColumnName("time_open");
            entity.Property(e => e.TimePeriodEnd)
            .HasColumnType("datetime")
            .HasColumnName("time_period_end");
            entity.Property(e => e.TimePeriodStart)
            .HasColumnType("datetime")
            .HasColumnName("time_period_start");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<CryptoData> entity);
    }
}
