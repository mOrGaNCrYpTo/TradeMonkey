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
            entity.HasKey(e => new { e.Epoch, e.Id }).HasName("PK__tmp_ms_x__6FE99DA692145329");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<IndicatorDatum> entity);
    }
}
