global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Azure.Functions.Worker;
global using Microsoft.Azure.Functions.Worker.Http;
global using Microsoft.Extensions.Logging;

global using TradeMonkey.Function.Domain.Value.Response;

global using Quickwire.Attributes;

global using Microsoft.EntityFrameworkCore;
global using Quickwire;

global using TradeMonkey.Function.Domain.Repository;
global using TradeMonkey.Function.Domain.Value.Constants;

global using System.Text.Json.Serialization;
global using System.Text.Json;

// Alias the System.Text.Json namespace to JsonSerializer
global using JsonSerializer = System.Text.Json.JsonSerializer;