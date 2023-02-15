global using Microsoft.Azure.Functions.Worker;
global using Microsoft.Azure.Functions.Worker.Http;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;

global using Quickwire;
global using Quickwire.Attributes;

global using TradeMonkey.KuCoin.Domain.Repository;
global using TradeMonkey.KuCoin.Domain.Value.Constants;
global using TradeMonkey.KuCoin.Domain.Value.Request;
global using TradeMonkey.KuCoin.Domain.Value.Response;
global using TradeMonkey.KuCoin.Function.Domain.Services;