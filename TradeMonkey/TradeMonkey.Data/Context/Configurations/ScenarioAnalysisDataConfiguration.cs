﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using TradeMonkey.Data.Context;
using TradeMonkey.Data.Entity;

namespace TradeMonkey.Data.Context.Configurations
{
    public partial class ScenarioAnalysisDataConfiguration : IEntityTypeConfiguration<ScenarioAnalysisData>
    {
        public void Configure(EntityTypeBuilder<ScenarioAnalysisData> entity)
        {
            entity.HasKey(e => new { e.Epoch, e.TokenId, e.Id }).HasName("PK__tmp_ms_x__BCA23964E7351B1D");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Date)
            .IsRequired()
            .HasMaxLength(50)
            .IsUnicode(false);
            entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(50)
            .IsUnicode(false);
            entity.Property(e => e.Symbol)
            .IsRequired()
            .HasMaxLength(50)
            .IsUnicode(false);

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<ScenarioAnalysisData> entity);
    }
}