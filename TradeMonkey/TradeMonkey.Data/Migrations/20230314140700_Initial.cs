using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradeMonkey.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Correlation_Datums",
                columns: table => new
                {
                    Token_Id = table.Column<int>(type: "int", nullable: false),
                    Correlation = table.Column<double>(type: "float", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Token_2_Name = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Token_2_Symbol = table.Column<int>(type: "int", nullable: true),
                    Epoch = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorrelationDatum", x => x.Token_Id);
                });

            migrationBuilder.CreateTable(
                name: "Indicator_Datums",
                columns: table => new
                {
                    Token_Id = table.Column<int>(type: "int", nullable: false),
                    Epoch = table.Column<int>(type: "int", nullable: false),
                    LastTmGradeSignal = table.Column<int>(type: "int", nullable: false),
                    TmGradePercHighCoins = table.Column<double>(type: "float", nullable: false),
                    TmGradeSignal = table.Column<int>(type: "int", nullable: false),
                    TotalCryptoMcap = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndicatorDatum", x => x.Token_Id);
                });

            migrationBuilder.CreateTable(
                name: "Indicies_Datums",
                columns: table => new
                {
                    Token_Id = table.Column<int>(type: "int", nullable: false),
                    BL_Weight = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndiciesDatum", x => x.Token_Id);
                });

            migrationBuilder.CreateTable(
                name: "Kucoin_24HourStats",
                columns: table => new
                {
                    Symbol = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HighPrice = table.Column<decimal>(type: "decimal(18,9)", nullable: true),
                    LowPrice = table.Column<decimal>(type: "decimal(18,9)", nullable: true),
                    Volume = table.Column<decimal>(type: "decimal(18,9)", nullable: true),
                    QuoteVolume = table.Column<decimal>(type: "decimal(18,9)", nullable: true),
                    LastPrice = table.Column<decimal>(type: "decimal(18,9)", nullable: true),
                    BestAskPrice = table.Column<decimal>(type: "decimal(18,9)", nullable: true),
                    BestBidPrice = table.Column<decimal>(type: "decimal(18,9)", nullable: true),
                    ChangePrice = table.Column<decimal>(type: "decimal(18,9)", nullable: true),
                    ChangePercentage = table.Column<decimal>(type: "decimal(18,9)", nullable: true),
                    AveragePrice = table.Column<decimal>(type: "decimal(18,9)", nullable: true),
                    TakerFeeRate = table.Column<decimal>(type: "decimal(18,9)", nullable: true),
                    MakerFeeRate = table.Column<decimal>(type: "decimal(18,9)", nullable: true),
                    TakerCoefficient = table.Column<decimal>(type: "decimal(18,9)", nullable: true),
                    MakerCoefficient = table.Column<decimal>(type: "decimal(18,9)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kucoin24HourStat", x => new { x.Symbol, x.Timestamp });
                });

            migrationBuilder.CreateTable(
                name: "Kucoin_Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    currency = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    type = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    balance = table.Column<decimal>(type: "decimal(18,0)", nullable: false, defaultValueSql: "((0.00))"),
                    available = table.Column<decimal>(type: "decimal(18,0)", nullable: false, defaultValueSql: "((0.00))"),
                    holds = table.Column<int>(type: "int", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KucoinAccount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kucoin_AllTicks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    symbol = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    symbolName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    buy = table.Column<double>(type: "float", nullable: false),
                    sell = table.Column<double>(type: "float", nullable: false),
                    changeRate = table.Column<double>(type: "float", nullable: false),
                    changePrice = table.Column<double>(type: "float", nullable: false),
                    high = table.Column<double>(type: "float", nullable: false),
                    low = table.Column<double>(type: "float", nullable: false),
                    vol = table.Column<double>(type: "float", nullable: false),
                    volValue = table.Column<double>(type: "float", nullable: false),
                    last = table.Column<double>(type: "float", nullable: false),
                    averagePrice = table.Column<double>(type: "float", nullable: false),
                    takerFeeRate = table.Column<double>(type: "float", nullable: false),
                    makerFeeRate = table.Column<double>(type: "float", nullable: false),
                    takerCoefficient = table.Column<double>(type: "float", nullable: false),
                    makerCoefficient = table.Column<double>(type: "float", nullable: false),
                    timestamp = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KucoinTicker", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kucoin_Tick",
                columns: table => new
                {
                    Sequence = table.Column<long>(type: "bigint", nullable: false),
                    LastPrice = table.Column<decimal>(type: "decimal(18,8)", nullable: true),
                    LastQuantity = table.Column<decimal>(type: "decimal(18,8)", nullable: true),
                    BestAskPrice = table.Column<decimal>(type: "decimal(18,8)", nullable: true),
                    BestAskQuantity = table.Column<decimal>(type: "decimal(18,8)", nullable: true),
                    BestBidPrice = table.Column<decimal>(type: "decimal(18,8)", nullable: true),
                    BestBidQuantity = table.Column<decimal>(type: "decimal(18,8)", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kucoin_Tick", x => x.Sequence);
                });

            migrationBuilder.CreateTable(
                name: "KucoinTokenMetricsSymbols",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KucoinSymbol = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    TokenMetricsSymbol = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Trade = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KucoinTokenMetricsSymbols", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PricePrediction_Datums",
                columns: table => new
                {
                    Token_Id = table.Column<int>(type: "int", nullable: false),
                    Close = table.Column<double>(type: "float", nullable: false),
                    Epoch = table.Column<int>(type: "int", nullable: false),
                    High = table.Column<double>(type: "float", nullable: false),
                    Low = table.Column<double>(type: "float", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Open = table.Column<double>(type: "float", nullable: false),
                    Symbol = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    TokenId = table.Column<int>(type: "int", nullable: false),
                    Volume = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PricePredictionDatum", x => x.Token_Id);
                });

            migrationBuilder.CreateTable(
                name: "QuantmetricsT1_Datums",
                columns: table => new
                {
                    Token_Id = table.Column<int>(type: "int", nullable: false),
                    Beta = table.Column<double>(type: "float", nullable: false),
                    Cagr = table.Column<double>(type: "float", nullable: false),
                    CumulativeReturn = table.Column<double>(type: "float", nullable: false),
                    Date = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    EndPeriod = table.Column<int>(type: "int", nullable: false),
                    Epoch = table.Column<int>(type: "int", nullable: false),
                    MaxDrawdown = table.Column<double>(type: "float", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Sharpe = table.Column<double>(type: "float", nullable: false),
                    Sortino = table.Column<double>(type: "float", nullable: false),
                    StartPeriod = table.Column<int>(type: "int", nullable: false),
                    Symbol = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    TokenId = table.Column<int>(type: "int", nullable: false),
                    Volatility = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuantmetricsT1Datum", x => x.Token_Id);
                });

            migrationBuilder.CreateTable(
                name: "QuantmetricsT2_Datums",
                columns: table => new
                {
                    Token_Id = table.Column<int>(type: "int", nullable: false),
                    AvgDailyReturn = table.Column<double>(type: "float", nullable: false),
                    AvgDownMonth = table.Column<double>(type: "float", nullable: false),
                    AvgMontlyReturn = table.Column<double>(type: "float", nullable: false),
                    AvgUpMonth = table.Column<double>(type: "float", nullable: false),
                    BestDay = table.Column<double>(type: "float", nullable: false),
                    BestMonth = table.Column<double>(type: "float", nullable: false),
                    Date = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    EndPeriod = table.Column<int>(type: "int", nullable: false),
                    Epoch = table.Column<int>(type: "int", nullable: false),
                    MaxDailyReturn = table.Column<double>(type: "float", nullable: false),
                    MaxMonthlyReturn = table.Column<double>(type: "float", nullable: false),
                    MinDailyReturn = table.Column<double>(type: "float", nullable: false),
                    MinMonthlyReturn = table.Column<double>(type: "float", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    StartPeriod = table.Column<int>(type: "int", nullable: false),
                    StdDailyReturn = table.Column<double>(type: "float", nullable: false),
                    StdMonthlyReturn = table.Column<double>(type: "float", nullable: false),
                    Symbol = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    TokenId = table.Column<int>(type: "int", nullable: false),
                    WorstDay = table.Column<double>(type: "float", nullable: false),
                    WorstMonth = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Quantmet__0CC8D3666E48416A", x => x.Token_Id);
                });

            migrationBuilder.CreateTable(
                name: "ResistanceSupport_Datums",
                columns: table => new
                {
                    Token_Id = table.Column<int>(type: "int", nullable: false),
                    Epoch = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResistanceSupport_Datums", x => x.Token_Id);
                });

            migrationBuilder.CreateTable(
                name: "ScenarioAnalysis_Datums",
                columns: table => new
                {
                    Token_Id = table.Column<int>(type: "int", nullable: false),
                    CurrentDominance = table.Column<double>(type: "float", nullable: false),
                    Date = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Dominance = table.Column<double>(type: "float", nullable: false),
                    Epoch = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Prediction = table.Column<double>(type: "float", nullable: false),
                    Symbol = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    TokenId = table.Column<int>(type: "int", nullable: false),
                    TotalMarketCap = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScenarioAnalysisDatum", x => x.Token_Id);
                });

            migrationBuilder.CreateTable(
                name: "Sentiments_Datums",
                columns: table => new
                {
                    Token_Id = table.Column<int>(type: "int", nullable: false),
                    Epoch = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    PolarityReddit = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    PolarityIndex = table.Column<double>(type: "float", nullable: false),
                    PolarityTelegram = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    PolarityTwitter = table.Column<double>(type: "float", nullable: false),
                    SentimentIndex = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Symbol = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sentiments_Datums", x => x.Token_Id);
                });

            migrationBuilder.CreateTable(
                name: "TokenMetrics_Prices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TokenId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Symbol = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    CurrentPrice = table.Column<decimal>(type: "decimal(18,12)", nullable: false),
                    Open = table.Column<decimal>(type: "decimal(18,12)", nullable: false),
                    High = table.Column<decimal>(type: "decimal(18,12)", nullable: false),
                    Low = table.Column<decimal>(type: "decimal(18,12)", nullable: false),
                    Close = table.Column<decimal>(type: "decimal(18,12)", nullable: false),
                    Volume = table.Column<decimal>(type: "decimal(28,8)", nullable: false),
                    Epoch = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TokenMet__3214EC07D30134D8", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TokenMetrics_Tokens",
                columns: table => new
                {
                    Token_Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Symbol = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.Token_Id);
                });

            migrationBuilder.CreateTable(
                name: "TraderGrades_Datums",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token_Id = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Epoch = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    QuantGrade = table.Column<decimal>(type: "decimal(18,12)", nullable: false),
                    Symbol = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    TaGrade = table.Column<decimal>(type: "decimal(18,12)", nullable: false),
                    TmTraderGrade = table.Column<decimal>(type: "decimal(18,12)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TraderGrades_Datums_1", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TradingIndicator_Datums",
                columns: table => new
                {
                    Token_Id = table.Column<int>(type: "int", nullable: false),
                    Close = table.Column<double>(type: "float", nullable: false),
                    Date = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Epoch = table.Column<int>(type: "int", nullable: false),
                    HoldingCumulativeRoi = table.Column<double>(type: "float", nullable: true),
                    LastSignal = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Signal = table.Column<int>(type: "int", nullable: false),
                    StrategyCummulativeRoi = table.Column<double>(type: "float", nullable: true),
                    Symbol = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    TokenId = table.Column<int>(type: "int", nullable: false),
                    Volume = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradingIndicatorDatum", x => x.Token_Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PricePrediction_Datums_TokenId",
                table: "PricePrediction_Datums",
                column: "TokenId");

            migrationBuilder.CreateIndex(
                name: "IX_QuantmetricsT1_Datums_TokenId",
                table: "QuantmetricsT1_Datums",
                column: "TokenId");

            migrationBuilder.CreateIndex(
                name: "IX_QuantmetricsT2_Datums_TokenId",
                table: "QuantmetricsT2_Datums",
                column: "TokenId");

            migrationBuilder.CreateIndex(
                name: "IX_ScenarioAnalysis_Datums_TokenId",
                table: "ScenarioAnalysis_Datums",
                column: "TokenId");

            migrationBuilder.CreateIndex(
                name: "IX_Sentiments_Datums_TokenId",
                table: "Sentiments_Datums",
                column: "Token_Id");

            migrationBuilder.CreateIndex(
                name: "IX_TradingIndicato_rDatums_TokenId",
                table: "TradingIndicator_Datums",
                column: "TokenId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Correlation_Datums");

            migrationBuilder.DropTable(
                name: "Indicator_Datums");

            migrationBuilder.DropTable(
                name: "Indicies_Datums");

            migrationBuilder.DropTable(
                name: "Kucoin_24HourStats");

            migrationBuilder.DropTable(
                name: "Kucoin_Accounts");

            migrationBuilder.DropTable(
                name: "Kucoin_AllTicks");

            migrationBuilder.DropTable(
                name: "Kucoin_Tick");

            migrationBuilder.DropTable(
                name: "KucoinTokenMetricsSymbols");

            migrationBuilder.DropTable(
                name: "PricePrediction_Datums");

            migrationBuilder.DropTable(
                name: "QuantmetricsT1_Datums");

            migrationBuilder.DropTable(
                name: "QuantmetricsT2_Datums");

            migrationBuilder.DropTable(
                name: "ResistanceSupport_Datums");

            migrationBuilder.DropTable(
                name: "ScenarioAnalysis_Datums");

            migrationBuilder.DropTable(
                name: "Sentiments_Datums");

            migrationBuilder.DropTable(
                name: "TokenMetrics_Prices");

            migrationBuilder.DropTable(
                name: "TokenMetrics_Tokens");

            migrationBuilder.DropTable(
                name: "TraderGrades_Datums");

            migrationBuilder.DropTable(
                name: "TradingIndicator_Datums");
        }
    }
}
