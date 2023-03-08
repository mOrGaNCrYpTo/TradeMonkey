﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using TradeMonkey.Data.Context;
using TradeMonkey.Data.Entity;

namespace TradeMonkey.Data.Context.Configurations
{
    public partial class IndicatorDatumConfiguration : IEntityTypeConfiguration<IndicatorDatum>
    {
        public void Configure(EntityTypeBuilder<IndicatorDatum> entity)
        {
            entity.HasKey(e => e.TokenId).HasName("PK_IndicatorDatum");

            entity.ToTable("Indicator_Datums");

            entity.Property(e => e.TokenId)
            .ValueGeneratedNever()
            .HasColumnName("Token_Id");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<IndicatorDatum> entity);
    }
}