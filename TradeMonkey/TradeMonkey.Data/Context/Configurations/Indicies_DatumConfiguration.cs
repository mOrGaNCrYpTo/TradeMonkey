﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using TradeMonkey.Data.Context;
using TradeMonkey.Data.Entity;

namespace TradeMonkey.Data.Context.Configurations
{
    public partial class Indicies_DatumConfiguration : IEntityTypeConfiguration<Indicies_Datum>
    {
        public void Configure(EntityTypeBuilder<Indicies_Datum> entity)
        {
            entity.HasKey(e => e.Id).HasName("PK_IndiciesDatum");

            entity.Property(e => e.Date).HasColumnType("date");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Indicies_Datum> entity);
    }
}