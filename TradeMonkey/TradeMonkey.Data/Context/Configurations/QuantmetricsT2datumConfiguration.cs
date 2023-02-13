﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using TradeMonkey.Data.Context;
using TradeMonkey.Data.Entity;

namespace TradeMonkey.Data.Context.Configurations
{
    public partial class QuantmetricsT2datumConfiguration : IEntityTypeConfiguration<QuantmetricsT2datum>
    {
        public void Configure(EntityTypeBuilder<QuantmetricsT2datum> entity)
        {
            entity.HasKey(e => e.Id).HasName("PK__Quantmet__0CC8D3666E48416A");

            entity.ToTable("QuantmetricsT2Datum");

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

            entity.HasOne(d => d.Token).WithMany(p => p.QuantmetricsT2datum)
            .HasForeignKey(d => d.TokenId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_QuantmetricsT2Datum_Tokens");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<QuantmetricsT2datum> entity);
    }
}
