﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using TradeMonkey.Data.Context;
using TradeMonkey.Data.Entity;

namespace TradeMonkey.Data.Context.Configurations
{
    public partial class CorrelationDatumConfiguration : IEntityTypeConfiguration<CorrelationDatum>
    {
        public void Configure(EntityTypeBuilder<CorrelationDatum> entity)
        {
            entity.HasKey(e => e.TokenId).HasName("PK_CorrelationDatum");

            entity.ToTable("Correlation_Datums");

            entity.Property(e => e.TokenId)
            .ValueGeneratedNever()
            .HasColumnName("Token_Id");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.Token2Name)
            .HasMaxLength(20)
            .IsUnicode(false)
            .HasColumnName("Token_2_Name");
            entity.Property(e => e.Token2Symbol).HasColumnName("Token_2_Symbol");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<CorrelationDatum> entity);
    }
}
