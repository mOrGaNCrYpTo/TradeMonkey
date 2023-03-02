using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradeMonkey.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Correlation_Datums",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Correlation = table.Column<double>(type: "float", nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    Token_2_Name = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Token_2_Symbol = table.Column<int>(type: "int", nullable: true),
                    Epoch = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorrelationDatum", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Indicator_Datums",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Epoch = table.Column<int>(type: "int", nullable: false),
                    LastTmGradeSignal = table.Column<int>(type: "int", nullable: false),
                    TmGradePercHighCoins = table.Column<double>(type: "float", nullable: false),
                    TmGradeSignal = table.Column<int>(type: "int", nullable: false),
                    TotalCryptoMcap = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndicatorDatum", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Indicies_Datums",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BL_Weight = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndiciesDatum", x => x.Id);
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
                name: "ResistanceSupport_Datums",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Epoch = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    TokenId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "TokenMetrics_Tokens",
                columns: table => new
                {
                    Token_Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Symbol = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
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
                    Date = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Epoch = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    QuantGrade = table.Column<double>(type: "float", nullable: false),
                    Symbol = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    TaGrade = table.Column<double>(type: "float", nullable: false),
                    TmTraderGrade = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    TokenId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TraderGradesDatum", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PricePrediction_Datums",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    table.PrimaryKey("PK_PricePredictionDatum", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PricePredictionDatum_Tokens",
                        column: x => x.TokenId,
                        principalTable: "TokenMetrics_Tokens",
                        principalColumn: "Token_Id");
                });

            migrationBuilder.CreateTable(
                name: "QuantmetricsT1_Datums",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    table.PrimaryKey("PK_QuantmetricsT1Datum", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuantmetricsT1Datum_Tokens",
                        column: x => x.TokenId,
                        principalTable: "TokenMetrics_Tokens",
                        principalColumn: "Token_Id");
                });

            migrationBuilder.CreateTable(
                name: "QuantmetricsT2_Datums",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    table.PrimaryKey("PK__Quantmet__0CC8D3666E48416A", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuantmetricsT2Datum_Tokens",
                        column: x => x.TokenId,
                        principalTable: "TokenMetrics_Tokens",
                        principalColumn: "Token_Id");
                });

            migrationBuilder.CreateTable(
                name: "ScenarioAnalysis_Datums",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    table.PrimaryKey("PK_ScenarioAnalysisDatum", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScenarioAnalysisDatum_Tokens",
                        column: x => x.TokenId,
                        principalTable: "TokenMetrics_Tokens",
                        principalColumn: "Token_Id");
                });

            migrationBuilder.CreateTable(
                name: "Sentiments_Datums",
                columns: table => new
                {
                    Epoch = table.Column<int>(type: "int", nullable: false),
                    TokenId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK__Sentimen__BA902D88A91C6C0B", x => new { x.Epoch, x.TokenId });
                    table.ForeignKey(
                        name: "FK_SentimentsDatum_Tokens",
                        column: x => x.TokenId,
                        principalTable: "TokenMetrics_Tokens",
                        principalColumn: "Token_Id");
                });

            migrationBuilder.CreateTable(
                name: "TradingIndicato_rDatums",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_TradingIndicatorDatum", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TradingIndicatorDatum_Tokens",
                        column: x => x.TokenId,
                        principalTable: "TokenMetrics_Tokens",
                        principalColumn: "Token_Id");
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
                column: "TokenId");

            migrationBuilder.CreateIndex(
                name: "IX_TradingIndicato_rDatums_TokenId",
                table: "TradingIndicato_rDatums",
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
                name: "Kucoin_Accounts");

            migrationBuilder.DropTable(
                name: "Kucoin_AllTicks");

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
                name: "TraderGrades_Datums");

            migrationBuilder.DropTable(
                name: "TradingIndicato_rDatums");

            migrationBuilder.DropTable(
                name: "TokenMetrics_Tokens");
        }
    }
}
