﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using TradeMonkey.Data.Context;
using TradeMonkey.Data.Entity;

namespace TradeMonkey.Data.Context.Configurations
{
    public partial class ResistanceSupportDatumConfiguration : IEntityTypeConfiguration<ResistanceSupportDatum>
    {
        public void Configure(EntityTypeBuilder<ResistanceSupportDatum> entity)
        {
            entity.HasKey(e => new { e.Epoch, e.TokenId, e.Id }).HasName("PK__tmp_ms_x__BCA2396434F1159F");

            entity.Property(e => e.TokenId)
            .HasMaxLength(50)
            .IsUnicode(false);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<ResistanceSupportDatum> entity);
    }
}
