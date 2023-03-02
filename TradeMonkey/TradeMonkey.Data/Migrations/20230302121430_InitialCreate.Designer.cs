﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TradeMonkey.Data.Context;

#nullable disable

namespace TradeMonkey.Data.Migrations
{
    [DbContext(typeof(TmDBContext))]
    [Migration("20230302121430_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TradeMonkey.Data.Entity.Correlation_Datum", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Correlation")
                        .HasColumnType("float");

                    b.Property<DateTime>("Date")
                        .HasColumnType("date");

                    b.Property<long?>("Epoch")
                        .HasColumnType("bigint");

                    b.Property<string>("Token_2_Name")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<int?>("Token_2_Symbol")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK_CorrelationDatum");

                    b.ToTable("Correlation_Datums");
                });

            modelBuilder.Entity("TradeMonkey.Data.Entity.Indicator_Datum", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Epoch")
                        .HasColumnType("int");

                    b.Property<int>("LastTmGradeSignal")
                        .HasColumnType("int");

                    b.Property<double>("TmGradePercHighCoins")
                        .HasColumnType("float");

                    b.Property<int>("TmGradeSignal")
                        .HasColumnType("int");

                    b.Property<double>("TotalCryptoMcap")
                        .HasColumnType("float");

                    b.HasKey("Id")
                        .HasName("PK_IndicatorDatum");

                    b.ToTable("Indicator_Datums");
                });

            modelBuilder.Entity("TradeMonkey.Data.Entity.Indicies_Datum", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BL_Weight")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("date");

                    b.HasKey("Id")
                        .HasName("PK_IndiciesDatum");

                    b.ToTable("Indicies_Datums");
                });

            modelBuilder.Entity("TradeMonkey.Data.Entity.Kucoin_Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Updated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<decimal>("available")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(18, 0)")
                        .HasDefaultValueSql("((0.00))");

                    b.Property<decimal>("balance")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(18, 0)")
                        .HasDefaultValueSql("((0.00))");

                    b.Property<string>("currency")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)");

                    b.Property<int>("holds")
                        .HasColumnType("int");

                    b.Property<string>("type")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)");

                    b.HasKey("Id")
                        .HasName("PK_KucoinAccount");

                    b.ToTable("Kucoin_Accounts");
                });

            modelBuilder.Entity("TradeMonkey.Data.Entity.Kucoin_AllTick", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("averagePrice")
                        .HasColumnType("float");

                    b.Property<double>("buy")
                        .HasColumnType("float");

                    b.Property<double>("changePrice")
                        .HasColumnType("float");

                    b.Property<double>("changeRate")
                        .HasColumnType("float");

                    b.Property<double>("high")
                        .HasColumnType("float");

                    b.Property<double>("last")
                        .HasColumnType("float");

                    b.Property<double>("low")
                        .HasColumnType("float");

                    b.Property<double>("makerCoefficient")
                        .HasColumnType("float");

                    b.Property<double>("makerFeeRate")
                        .HasColumnType("float");

                    b.Property<double>("sell")
                        .HasColumnType("float");

                    b.Property<string>("symbol")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("symbolName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<double>("takerCoefficient")
                        .HasColumnType("float");

                    b.Property<double>("takerFeeRate")
                        .HasColumnType("float");

                    b.Property<DateTime>("timestamp")
                        .HasColumnType("datetime");

                    b.Property<double>("vol")
                        .HasColumnType("float");

                    b.Property<double>("volValue")
                        .HasColumnType("float");

                    b.HasKey("Id")
                        .HasName("PK_KucoinTicker");

                    b.ToTable("Kucoin_AllTicks");
                });

            modelBuilder.Entity("TradeMonkey.Data.Entity.PricePrediction_Datum", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Close")
                        .HasColumnType("float");

                    b.Property<int>("Epoch")
                        .HasColumnType("int");

                    b.Property<double>("High")
                        .HasColumnType("float");

                    b.Property<double>("Low")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<double>("Open")
                        .HasColumnType("float");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<int>("TokenId")
                        .HasColumnType("int");

                    b.Property<double>("Volume")
                        .HasColumnType("float");

                    b.HasKey("Id")
                        .HasName("PK_PricePredictionDatum");

                    b.HasIndex("TokenId");

                    b.ToTable("PricePrediction_Datums");
                });

            modelBuilder.Entity("TradeMonkey.Data.Entity.QuantmetricsT1_Datum", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Beta")
                        .HasColumnType("float");

                    b.Property<double>("Cagr")
                        .HasColumnType("float");

                    b.Property<double>("CumulativeReturn")
                        .HasColumnType("float");

                    b.Property<string>("Date")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<int>("EndPeriod")
                        .HasColumnType("int");

                    b.Property<int>("Epoch")
                        .HasColumnType("int");

                    b.Property<double>("MaxDrawdown")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<double>("Sharpe")
                        .HasColumnType("float");

                    b.Property<double>("Sortino")
                        .HasColumnType("float");

                    b.Property<int>("StartPeriod")
                        .HasColumnType("int");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<int>("TokenId")
                        .HasColumnType("int");

                    b.Property<double>("Volatility")
                        .HasColumnType("float");

                    b.HasKey("Id")
                        .HasName("PK_QuantmetricsT1Datum");

                    b.HasIndex("TokenId");

                    b.ToTable("QuantmetricsT1_Datums");
                });

            modelBuilder.Entity("TradeMonkey.Data.Entity.QuantmetricsT2_Datum", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("AvgDailyReturn")
                        .HasColumnType("float");

                    b.Property<double>("AvgDownMonth")
                        .HasColumnType("float");

                    b.Property<double>("AvgMontlyReturn")
                        .HasColumnType("float");

                    b.Property<double>("AvgUpMonth")
                        .HasColumnType("float");

                    b.Property<double>("BestDay")
                        .HasColumnType("float");

                    b.Property<double>("BestMonth")
                        .HasColumnType("float");

                    b.Property<string>("Date")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<int>("EndPeriod")
                        .HasColumnType("int");

                    b.Property<int>("Epoch")
                        .HasColumnType("int");

                    b.Property<double>("MaxDailyReturn")
                        .HasColumnType("float");

                    b.Property<double>("MaxMonthlyReturn")
                        .HasColumnType("float");

                    b.Property<double>("MinDailyReturn")
                        .HasColumnType("float");

                    b.Property<double>("MinMonthlyReturn")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<int>("StartPeriod")
                        .HasColumnType("int");

                    b.Property<double>("StdDailyReturn")
                        .HasColumnType("float");

                    b.Property<double>("StdMonthlyReturn")
                        .HasColumnType("float");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<int>("TokenId")
                        .HasColumnType("int");

                    b.Property<double>("WorstDay")
                        .HasColumnType("float");

                    b.Property<double>("WorstMonth")
                        .HasColumnType("float");

                    b.HasKey("Id")
                        .HasName("PK__Quantmet__0CC8D3666E48416A");

                    b.HasIndex("TokenId");

                    b.ToTable("QuantmetricsT2_Datums");
                });

            modelBuilder.Entity("TradeMonkey.Data.Entity.ResistanceSupport_Datum", b =>
                {
                    b.Property<int>("Epoch")
                        .HasColumnType("int");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<string>("TokenId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.ToTable("ResistanceSupport_Datums");
                });

            modelBuilder.Entity("TradeMonkey.Data.Entity.ScenarioAnalysis_Datum", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("CurrentDominance")
                        .HasColumnType("float");

                    b.Property<string>("Date")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<double>("Dominance")
                        .HasColumnType("float");

                    b.Property<int>("Epoch")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<double>("Prediction")
                        .HasColumnType("float");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("TokenId")
                        .HasColumnType("int");

                    b.Property<double>("TotalMarketCap")
                        .HasColumnType("float");

                    b.HasKey("Id")
                        .HasName("PK_ScenarioAnalysisDatum");

                    b.HasIndex("TokenId");

                    b.ToTable("ScenarioAnalysis_Datums");
                });

            modelBuilder.Entity("TradeMonkey.Data.Entity.Sentiments_Datum", b =>
                {
                    b.Property<int>("Epoch")
                        .HasColumnType("int");

                    b.Property<int>("TokenId")
                        .HasColumnType("int");

                    b.Property<string>("Date")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<double>("PolarityIndex")
                        .HasColumnType("float");

                    b.Property<string>("PolarityReddit")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("PolarityTelegram")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<double>("PolarityTwitter")
                        .HasColumnType("float");

                    b.Property<string>("SentimentIndex")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Epoch", "TokenId")
                        .HasName("PK__Sentimen__BA902D88A91C6C0B");

                    b.HasIndex("TokenId");

                    b.ToTable("Sentiments_Datums");
                });

            modelBuilder.Entity("TradeMonkey.Data.Entity.TokenMetrics_Token", b =>
                {
                    b.Property<int>("Token_Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)");

                    b.HasKey("Token_Id")
                        .HasName("PK_Tokens");

                    b.ToTable("TokenMetrics_Tokens");
                });

            modelBuilder.Entity("TradeMonkey.Data.Entity.TraderGrades_Datum", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Date")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("Epoch")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<double>("QuantGrade")
                        .HasColumnType("float");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<double>("TaGrade")
                        .HasColumnType("float");

                    b.Property<string>("TmTraderGrade")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("TokenId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id")
                        .HasName("PK_TraderGradesDatum");

                    b.ToTable("TraderGrades_Datums");
                });

            modelBuilder.Entity("TradeMonkey.Data.Entity.TradingIndicato_rDatum", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<double>("Close")
                        .HasColumnType("float");

                    b.Property<string>("Date")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("Epoch")
                        .HasColumnType("int");

                    b.Property<double?>("HoldingCumulativeRoi")
                        .HasColumnType("float");

                    b.Property<int>("LastSignal")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("Signal")
                        .HasColumnType("int");

                    b.Property<double?>("StrategyCummulativeRoi")
                        .HasColumnType("float");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("TokenId")
                        .HasColumnType("int");

                    b.Property<double>("Volume")
                        .HasColumnType("float");

                    b.HasKey("Id")
                        .HasName("PK_TradingIndicatorDatum");

                    b.HasIndex("TokenId");

                    b.ToTable("TradingIndicato_rDatums");
                });

            modelBuilder.Entity("TradeMonkey.Data.Entity.PricePrediction_Datum", b =>
                {
                    b.HasOne("TradeMonkey.Data.Entity.TokenMetrics_Token", "Token")
                        .WithMany("PricePrediction_Datum")
                        .HasForeignKey("TokenId")
                        .IsRequired()
                        .HasConstraintName("FK_PricePredictionDatum_Tokens");

                    b.Navigation("Token");
                });

            modelBuilder.Entity("TradeMonkey.Data.Entity.QuantmetricsT1_Datum", b =>
                {
                    b.HasOne("TradeMonkey.Data.Entity.TokenMetrics_Token", "Token")
                        .WithMany("QuantmetricsT1_Datum")
                        .HasForeignKey("TokenId")
                        .IsRequired()
                        .HasConstraintName("FK_QuantmetricsT1Datum_Tokens");

                    b.Navigation("Token");
                });

            modelBuilder.Entity("TradeMonkey.Data.Entity.QuantmetricsT2_Datum", b =>
                {
                    b.HasOne("TradeMonkey.Data.Entity.TokenMetrics_Token", "Token")
                        .WithMany("QuantmetricsT2_Datum")
                        .HasForeignKey("TokenId")
                        .IsRequired()
                        .HasConstraintName("FK_QuantmetricsT2Datum_Tokens");

                    b.Navigation("Token");
                });

            modelBuilder.Entity("TradeMonkey.Data.Entity.ScenarioAnalysis_Datum", b =>
                {
                    b.HasOne("TradeMonkey.Data.Entity.TokenMetrics_Token", "Token")
                        .WithMany("ScenarioAnalysis_Datum")
                        .HasForeignKey("TokenId")
                        .IsRequired()
                        .HasConstraintName("FK_ScenarioAnalysisDatum_Tokens");

                    b.Navigation("Token");
                });

            modelBuilder.Entity("TradeMonkey.Data.Entity.Sentiments_Datum", b =>
                {
                    b.HasOne("TradeMonkey.Data.Entity.TokenMetrics_Token", "Token")
                        .WithMany("Sentiments_Datum")
                        .HasForeignKey("TokenId")
                        .IsRequired()
                        .HasConstraintName("FK_SentimentsDatum_Tokens");

                    b.Navigation("Token");
                });

            modelBuilder.Entity("TradeMonkey.Data.Entity.TradingIndicato_rDatum", b =>
                {
                    b.HasOne("TradeMonkey.Data.Entity.TokenMetrics_Token", "Token")
                        .WithMany("TradingIndicato_rDatum")
                        .HasForeignKey("TokenId")
                        .IsRequired()
                        .HasConstraintName("FK_TradingIndicatorDatum_Tokens");

                    b.Navigation("Token");
                });

            modelBuilder.Entity("TradeMonkey.Data.Entity.TokenMetrics_Token", b =>
                {
                    b.Navigation("PricePrediction_Datum");

                    b.Navigation("QuantmetricsT1_Datum");

                    b.Navigation("QuantmetricsT2_Datum");

                    b.Navigation("ScenarioAnalysis_Datum");

                    b.Navigation("Sentiments_Datum");

                    b.Navigation("TradingIndicato_rDatum");
                });
#pragma warning restore 612, 618
        }
    }
}
