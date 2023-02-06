using TradingViewHook;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapPost("/tradingviewHook", HandleRequest) =>
{
    
});

app.Run();

static async Task<IResult> HandleRequest(WebhookRequest request)
{
    // Check the request type
    if (request.RequestType == "buy")
    {
        // Handle a buy order
        return await HandleBuyOrder(symbol, amount, price);
    }
    else if (requestType == "sell")
    {
        // Handle a sell order
        return await HandleSellOrder(symbol, amount, price);
    }

    // Return an error
    return "Invalid request type";
}

// Handle a buy order from TradingView
static async Task<string> HandleBuyOrder(string symbol, decimal amount, decimal price)
{
    // Place the buy order
    var response = await kuCoinAPI.PlaceBuyOrder(symbol, amount, price);

    // Return the response
    return response;
}

// Handle a sell order from TradingView
static async Task<string> HandleSellOrder(string symbol, decimal amount, decimal price)
{
    // Place the sell order
    var response = await kuCoinAPI.PlaceSellOrder(symbol, amount, price);

    // Return the response
    return response;
}