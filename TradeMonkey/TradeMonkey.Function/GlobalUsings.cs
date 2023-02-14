global using Microsoft.Azure.Functions.Worker.Http;
global using Microsoft.Azure.Functions.Worker;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;

global using Quickwire.Attributes;
global using Quickwire;

global using TradeMonkey.TokenMetrics.Domain.Value.Constants;
global using TradeMonkey.TokenMetrics.Domain.Repository;
global using TradeMonkey.TokenMetrics.Domain.Value.Response;
global using TradeMonkey.TokenMetrics.Domain.Value.Request;
global using TradeMonkey.Function.Domain.Services;