﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using TradeMonkey.Data.Context;
using TradeMonkey.Data.Entity;

namespace TradeMonkey.Data.Context.Configurations
{
    public partial class QuantmetricsT1datumConfiguration : IEntityTypeConfiguration<QuantmetricsT1datum>
    {
        public void Configure(EntityTypeBuilder<QuantmetricsT1datum> entity)
        {
            entity.ToTable("QuantmetricsT1Datum");

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

            entity.HasOne(d => d.Token).WithMany(p => p.QuantmetricsT1datum)
            .HasForeignKey(d => d.Token_Id)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_QuantmetricsT1Datum_Tokens");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<QuantmetricsT1datum> entity);
    }
}
