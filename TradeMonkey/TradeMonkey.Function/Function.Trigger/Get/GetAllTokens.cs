using Microsoft.Azure.Functions.Worker;

namespace TradeMonkey.Function.Trigger.Get
{
    public class GetAllTokens
    {
        private readonly ILogger _logger;

        public GetAllTokens(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetAllTokens>();
        }

        [Function(nameof(GetAllTokens))]
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req,
            FunctionContext context,
            CancellationToken hostCancellationToken)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var lts = CancellationTokenSource.CreateLinkedTokenSource(hostCancellationToken, context.CancellationToken);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return response;
        }
    }
}