﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using TradeMonkey.Data.Context;
using TradeMonkey.Data.Entity;

namespace TradeMonkey.Data.Context.Configurations
{
    public partial class TokenMetricsResSuppDatumConfiguration : IEntityTypeConfiguration<TokenMetricsResSuppDatum>
    {
        public void Configure(EntityTypeBuilder<TokenMetricsResSuppDatum> entity)
        {
            entity.HasKey(e => new { e.TokenId, e.Epoch });

            entity.ToTable("TokenMetrics_ResistanceSupport");

            entity.Property(e => e.TokenId).HasColumnName("Token_Id");
            entity.Property(e => e.DateCreated)
            .HasDefaultValueSql("(getdate())")
            .HasColumnType("datetime");
            entity.Property(e => e.Level).HasColumnType("decimal(18, 15)");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<TokenMetricsResSuppDatum> entity);
    }
}