﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using TradeMonkey.Data.Context;
using TradeMonkey.Data.Entity;

namespace TradeMonkey.Data.Context.Configurations
{
    public partial class IndiciesDatumConfiguration : IEntityTypeConfiguration<IndiciesDatum>
    {
        public void Configure(EntityTypeBuilder<IndiciesDatum> entity)
        {
            entity.Property(e => e.Date).HasColumnType("date");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<IndiciesDatum> entity);
    }
}
