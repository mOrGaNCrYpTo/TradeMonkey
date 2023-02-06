using System.Text.Json;

namespace TradingViewHook
{
    public class Startup
    {
        private readonly string _kuCoinApiUrl = "https://sandbox.kucoin.com";
       // private readonly string _kuCoinApiUrl = "https://api.kucoin.com";
        private readonly string _kuCoinApiKey = "6273a279628550000159657a";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapPost("/webhook", async context =>
                {
                    var webhookRequest = await context.Request.Body.ReadAsync();

                    var requestJson = 
                        await JsonSerializer.DeserializeAsync<WebhookRequest>(webhookRequest);

                    // Extract information from the request JSON
                    var type = requestJson.type;
                    var orderType = requestJson.orderType;
                    var symbol = requestJson.symbol;
                    var amount = requestJson.amount;
                    var price = requestJson.price;

                    // Place an order based on the information extracted from the request JSON
                    await PlaceOrder(type, orderType, symbol, amount, price);

                    await context.Response.WriteAsync("Order placed successfully");
                });
            });
        }
    }
}
