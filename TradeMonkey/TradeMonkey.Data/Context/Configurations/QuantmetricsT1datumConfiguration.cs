﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using TradeMonkey.Data.Context;
using TradeMonkey.Data.Entity;

namespace TradeMonkey.Data.Context.Configurations
{
    public partial class QuantmetricsT1DatumConfiguration : IEntityTypeConfiguration<QuantmetricsT1Datum>
    {
        public void Configure(EntityTypeBuilder<QuantmetricsT1Datum> entity)
        {
            entity.HasKey(e => e.Epoch).HasName("PK__Quantmet__0CC8D366723652D6");

            entity.Property(e => e.Epoch).ValueGeneratedNever();
            entity.Property(e => e.Date)
            .IsRequired()
            .HasMaxLength(255)
            .IsUnicode(false);
            entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(255)
            .IsUnicode(false);
            entity.Property(e => e.Symbol)
            .IsRequired()
            .HasMaxLength(255)
            .IsUnicode(false);

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<QuantmetricsT1Datum> entity);
    }
}
