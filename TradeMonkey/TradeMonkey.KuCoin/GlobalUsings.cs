global using System.Net.WebSockets;

global using Microsoft.Azure.Functions.Worker;
global using Microsoft.Azure.Functions.Worker.Http;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Primitives;

global using Newtonsoft.Json;
global using Newtonsoft.Json.Linq;

global using Quickwire;
global using Quickwire.Attributes;

global using TradeMonkey.KuCoin.Domain.Repository;
global using TradeMonkey.KuCoin.Domain.Value.Constants;
global using TradeMonkey.KuCoin.Domain.Value.Request;
global using TradeMonkey.KuCoin.Domain.Value.Response;
global using TradeMonkey.KuCoin.Function.Domain.Services;
global using TradeMonkey.Data.Entity;

global using Kucoin.Net.Clients;
global using Kucoin.Net.Objects.Models.Spot;

//global using Kucoin.Net.Objects.Spot;