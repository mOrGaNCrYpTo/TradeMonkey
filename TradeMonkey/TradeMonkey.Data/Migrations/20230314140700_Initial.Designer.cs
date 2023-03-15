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
    [Migration("20230314140700_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TradeMonkey.Data.Entity.CorrelationDatum", b =>
                {
                    b.Property<int>("TokenId")
                        .HasColumnType("int")
                        .HasColumnName("Token_Id");

                    b.Property<double>("Correlation")
                        .HasColumnType("float");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<long?>("Epoch")
                        .HasColumnType("bigint");

                    b.Property<string>("Token2Name")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("Token_2_Name");

                    b.Property<int?>("Token2Symbol")
                        .HasColumnType("int")
                        .HasColumnName("Token_2_Symbol");

                    b.HasKey("TokenId")
                        .HasName("PK_CorrelationDatum");

                    b.ToTable("Correlation_Datums", (string)null);
                });

            modelBuilder.Entity("TradeMonkey.Data.Entity.IndicatorDatum", b =>
                {
                    b.Property<int>("TokenId")
                        .HasColumnType("int")
                        .HasColumnName("Token_Id");

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

                    b.HasKey("TokenId")
                        .HasName("PK_IndicatorDatum");

                    b.ToTable("Indicator_Datums", (string)null);
                });

            modelBuilder.Entity("TradeMonkey.Data.Entity.IndiciesDatum", b =>
                {
                    b.Property<int>("TokenId")
                        .HasColumnType("int")
                        .HasColumnName("Token_Id");

                    b.Property<int>("BlWeight")
                        .HasColumnType("int")
                        .HasColumnName("BL_Weight");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime2");

                    b.HasKey("TokenId")
                        .HasName("PK_IndiciesDatum");

                    b.ToTable("Indicies_Datums", (string)null);
                });

            modelBuilder.Entity("TradeMonkey.Data.Entity.Kucoin24hourStats", b =>
                {
                    b.Property<string>("Symbol")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("AveragePrice")
                        .HasColumnType("decimal(18, 9)");

                    b.Property<decimal?>("BestAskPrice")
                        .HasColumnType("decimal(18, 9)");

                    b.Property<decimal?>("BestBidPrice")
                        .HasColumnType("decimal(18, 9)");

                    b.Property<decimal?>("ChangePercentage")
                        .HasColumnType("decimal(18, 9)");

                    b.Property<decimal?>("ChangePrice")
                        .HasColumnType("decimal(18, 9)");

                    b.Property<decimal?>("HighPrice")
                        .HasColumnType("decimal(18, 9)");

                    b.Property<decimal?>("LastPrice")
                        .HasColumnType("decimal(18, 9)");

                    b.Property<decimal?>("LowPrice")
                        .HasColumnType("decimal(18, 9)");

                    b.Property<decimal?>("MakerCoefficient")
                        .HasColumnType("decimal(18, 9)");

                    b.Property<decimal?>("MakerFeeRate")
                        .HasColumnType("decimal(18, 9)");

                    b.Property<decimal?>("QuoteVolume")
                        .HasColumnType("decimal(18, 9)");

                    b.Property<decimal?>("TakerCoefficient")
                        .HasColumnType("decimal(18, 9)");

                    b.Property<decimal?>("TakerFeeRate")
                        .HasColumnType("decimal(18, 9)");

                    b.Property<decimal?>("Volume")
                        .HasColumnType("decimal(18, 9)");

                    b.HasKey("Symbol", "Timestamp")
                        .HasName("PK_Kucoin24HourStat");

                    b.ToTable("Kucoin_24HourStats", (string)null);
                });

            modelBuilder.Entity("TradeMonkey.Data.Entity.KucoinAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Available")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(18, 0)")
                        .HasColumnName("available")
                        .HasDefaultValueSql("((0.00))");

                    b.Property<decimal>("Balance")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(18, 0)")
                        .HasColumnName("balance")
                        .HasDefaultValueSql("((0.00))");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("currency");

                    b.Property<int>("Holds")
                        .HasColumnType("int")
                        .HasColumnName("holds");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("type");

                    b.Property<DateTime>("Updated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.HasKey("Id")
                        .HasName("PK_KucoinAccount");

                    b.ToTable("Kucoin_Accounts", (string)null);
                });

            modelBuilder.Entity("TradeMonkey.Data.Entity.KucoinAllTick", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("AveragePrice")
                        .HasColumnType("float")
                        .HasColumnName("averagePrice");

                    b.Property<double>("Buy")
                        .HasColumnType("float")
                        .HasColumnName("buy");

                    b.Property<double>("ChangePrice")
                        .HasColumnType("float")
                        .HasColumnName("changePrice");

                    b.Property<double>("ChangeRate")
                        .HasColumnType("float")
                        .HasColumnName("changeRate");

                    b.Property<double>("High")
                        .HasColumnType("float")
                        .HasColumnName("high");

                    b.Property<double>("Last")
                        .HasColumnType("float")
                        .HasColumnName("last");

                    b.Property<double>("Low")
                        .HasColumnType("float")
                        .HasColumnName("low");

                    b.Property<double>("MakerCoefficient")
                        .HasColumnType("float")
                        .HasColumnName("makerCoefficient");

                    b.Property<double>("MakerFeeRate")
                        .HasColumnType("float")
                        .HasColumnName("makerFeeRate");

                    b.Property<double>("Sell")
                        .HasColumnType("float")
                        .HasColumnName("sell");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("symbol");

                    b.Property<string>("SymbolName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("symbolName");

                    b.Property<double>("TakerCoefficient")
                        .HasColumnType("float")
                        .HasColumnName("takerCoefficient");

                    b.Property<double>("TakerFeeRate")
                        .HasColumnType("float")
                        .HasColumnName("takerFeeRate");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime")
                        .HasColumnName("timestamp");

                    b.Property<double>("Vol")
                        .HasColumnType("float")
                        .HasColumnName("vol");

                    b.Property<double>("VolValue")
                        .HasColumnType("float")
                        .HasColumnName("volValue");

                    b.HasKey("Id")
                        .HasName("PK_KucoinTicker");

                    b.ToTable("Kucoin_AllTicks", (string)null);
                });

            modelBuilder.Entity("TradeMonkey.Data.Entity.KucoinTick", b =>
                {
                    b.Property<long>("Sequence")
                        .HasColumnType("bigint");

                    b.Property<decimal?>("BestAskPrice")
                        .HasColumnType("decimal(18, 8)");

                    b.Property<decimal?>("BestAskQuantity")
                        .HasColumnType("decimal(18, 8)");

                    b.Property<decimal?>("BestBidPrice")
                        .HasColumnType("decimal(18, 8)");

                    b.Property<decimal?>("BestBidQuantity")
                        .HasColumnType("decimal(18, 8)");

                    b.Property<decimal?>("LastPrice")
                        .HasColumnType("decimal(18, 8)");

                    b.Property<decimal?>("LastQuantity")
                        .HasColumnType("decimal(18, 8)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime");

                    b.HasKey("Sequence");

                    b.ToTable("Kucoin_Tick", (string)null);
                });

            modelBuilder.Entity("TradeMonkey.Data.Entity.KucoinTokenMetricsSymbol", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("KucoinSymbol")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("TokenMetricsSymbol")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<bool>("Trade")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("KucoinTokenMetricsSymbols");
                });

            modelBuilder.Entity("TradeMonkey.Data.Entity.PricePredictionDatum", b =>
                {
                    b.Property<int>("TokenId")
                        .HasColumnType("int")
                        .HasColumnName("Token_Id");

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

                    b.Property<int>("TokenId1")
                        .HasColumnType("int")
                        .HasColumnName("TokenId");

                    b.Property<double>("Volume")
                        .HasColumnType("float");

                    b.HasKey("TokenId")
                        .HasName("PK_PricePredictionDatum");

                    b.HasIndex(new[] { "TokenId1" }, "IX_PricePrediction_Datums_TokenId");

                    b.ToTable("PricePrediction_Datums", (string)null);
                });

            modelBuilder.Entity("TradeMonkey.Data.Entity.QuantmetricsT1Datums", b =>
                {
                    b.Property<int>("TokenId")
                        .HasColumnType("int")
                        .HasColumnName("Token_Id");

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

                    b.Property<int>("TokenId1")
                        .HasColumnType("int")
                        .HasColumnName("TokenId");

                    b.Property<double>("Volatility")
                        .HasColumnType("float");

                    b.HasKey("TokenId")
                        .HasName("PK_QuantmetricsT1Datum");

                    b.HasIndex(new[] { "TokenId1" }, "IX_QuantmetricsT1_Datums_TokenId");

                    b.ToTable("QuantmetricsT1_Datums", (string)null);
                });

            modelBuilder.Entity("TradeMonkey.Data.Entity.QuantmetricsT2Datums", b =>
                {
                    b.Property<int>("TokenId")
                        .HasColumnType("int")
                        .HasColumnName("Token_Id");

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

                    b.Property<int>("TokenId1")
                        .HasColumnType("int")
                        .HasColumnName("TokenId");

                    b.Property<double>("WorstDay")
                        .HasColumnType("float");

                    b.Property<double>("WorstMonth")
                        .HasColumnType("float");

                    b.HasKey("TokenId")
                        .HasName("PK__Quantmet__0CC8D3666E48416A");

                    b.HasIndex(new[] { "TokenId1" }, "IX_QuantmetricsT2_Datums_TokenId");

                    b.ToTable("QuantmetricsT2_Datums", (string)null);
                });

            modelBuilder.Entity("TradeMonkey.Data.Entity.ResistanceSupportDatum", b =>
                {
                    b.Property<int>("TokenId")
                        .HasColumnType("int")
                        .HasColumnName("Token_Id");

                    b.Property<int>("Epoch")
                        .HasColumnType("int");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.HasKey("TokenId");

                    b.ToTable("ResistanceSupport_Datums", (string)null);
                });

            modelBuilder.Entity("TradeMonkey.Data.Entity.ScenarioAnalysisDatum", b =>
                {
                    b.Property<int>("TokenId")
                        .HasColumnType("int")
                        .HasColumnName("Token_Id");

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

                    b.Property<int>("TokenId1")
                        .HasColumnType("int")
                        .HasColumnName("TokenId");

                    b.Property<double>("TotalMarketCap")
                        .HasColumnType("float");

                    b.HasKey("TokenId")
                        .HasName("PK_ScenarioAnalysisDatum");

                    b.HasIndex(new[] { "TokenId1" }, "IX_ScenarioAnalysis_Datums_TokenId");

                    b.ToTable("ScenarioAnalysis_Datums", (string)null);
                });

            modelBuilder.Entity("TradeMonkey.Data.Entity.SentimentsDatum", b =>
                {
                    b.Property<int>("TokenId")
                        .HasColumnType("int")
                        .HasColumnName("Token_Id");

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

                    b.HasKey("TokenId");

                    b.HasIndex(new[] { "TokenId" }, "IX_Sentiments_Datums_TokenId");

                    b.ToTable("Sentiments_Datums", (string)null);
                });

            modelBuilder.Entity("TradeMonkey.Data.Entity.TokenMetricsPrice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Close")
                        .HasColumnType("decimal(18, 12)");

                    b.Property<decimal>("CurrentPrice")
                        .HasColumnType("decimal(18, 12)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime");

                    b.Property<int>("Epoch")
                        .HasColumnType("int");

                    b.Property<decimal>("High")
                        .HasColumnType("decimal(18, 12)");

                    b.Property<decimal>("Low")
                        .HasColumnType("decimal(18, 12)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<decimal>("Open")
                        .HasColumnType("decimal(18, 12)");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)");

                    b.Property<int>("TokenId")
                        .HasColumnType("int");

                    b.Property<decimal>("Volume")
                        .HasColumnType("decimal(28, 8)");

                    b.HasKey("Id")
                        .HasName("PK__TokenMet__3214EC07D30134D8");

                    b.ToTable("TokenMetrics_Prices", (string)null);
                });

            modelBuilder.Entity("TradeMonkey.Data.Entity.TokenMetricsToken", b =>
                {
                    b.Property<int>("TokenId")
                        .HasColumnType("int")
                        .HasColumnName("Token_Id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("TokenId")
                        .HasName("PK_Tokens");

                    b.ToTable("TokenMetrics_Tokens", (string)null);
                });

            modelBuilder.Entity("TradeMonkey.Data.Entity.TraderGradesDatum", b =>
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

                    b.Property<decimal>("QuantGrade")
                        .HasColumnType("decimal(18, 12)");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<decimal>("TaGrade")
                        .HasColumnType("decimal(18, 12)");

                    b.Property<decimal>("TmTraderGrade")
                        .HasColumnType("decimal(18, 12)");

                    b.Property<int>("TokenId")
                        .HasColumnType("int")
                        .HasColumnName("Token_Id");

                    b.HasKey("Id")
                        .HasName("PK_TraderGrades_Datums_1");

                    b.ToTable("TraderGrades_Datums", (string)null);
                });

            modelBuilder.Entity("TradeMonkey.Data.Entity.TradingIndicatorDatum", b =>
                {
                    b.Property<int>("TokenId")
                        .HasColumnType("int")
                        .HasColumnName("Token_Id");

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

                    b.Property<int>("TokenId1")
                        .HasColumnType("int")
                        .HasColumnName("TokenId");

                    b.Property<double>("Volume")
                        .HasColumnType("float");

                    b.HasKey("TokenId")
                        .HasName("PK_TradingIndicatorDatum");

                    b.HasIndex(new[] { "TokenId1" }, "IX_TradingIndicato_rDatums_TokenId");

                    b.ToTable("TradingIndicator_Datums", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
