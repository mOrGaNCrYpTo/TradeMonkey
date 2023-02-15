using TradeMonkey.KuCoin.Domain.Services;

namespace TradeMonkey.KuCoin.Trigger.Get
{
    public class GetHoldings
    {
        private readonly ILogger _logger;

        [InjectService]
        public GetHoldingsSvc GetHoldingsSvc { get; set; }

        public GetHoldings(GetHoldingsSvc getHoldingsSvc) =>
            GetHoldingsSvc = getHoldingsSvc
                ?? throw new ArgumentNullException(nameof(getHoldingsSvc));

        [Function(nameof(GetHoldings))]
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "get-holdings")] HttpRequestData req,
            FunctionContext executionContext,
            string timeFrame,
            CancellationToken hostCancellationToken = default)
        {
            // validate
            if (string.IsNullOrEmpty(timeFrame))
                throw new Exception(FunctionEvents.TokenMetricsInvalidRequest);

            // create a linked token source
            var lts = CancellationTokenSource.CreateLinkedTokenSource(hostCancellationToken, executionContext.CancellationToken);
            var token = lts.Token;

            // throw and catch an exception if cancellation is requested
            token.ThrowIfCancellationRequested();

            // get logger from the context
            var logger = executionContext.GetLogger(nameof(GetHoldings));
            logger.LogDebug(FunctionEvents.TokenMetricsRequestStarted);

            // create a response wrapper. Assume success unless we catch an exception
            HttpResponseData functionResponse = req.CreateResponse(HttpStatusCode.OK);

            try
            {
                // deserialize var request = JsonSerializer.Deserialize<Request>(req.Body);

                // execute the request and get the response. always forward the cancellation token
                // to the service
                var response = await Domain.Services.GetHoldingsSvc.ExecuteAsync(token);

                await functionResponse.WriteAsJsonAsync(response);
            }
            catch (OperationCanceledException oex)
            {
                logger.LogError($"{FunctionEvents.OperationCanceled}{oex.Message}");
                functionResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                logger.LogError($"{FunctionEvents.UnknownExceptionOccured}{ex.Message}");
                functionResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            }
            finally
            {
                logger.LogDebug(FunctionEvents.TokenMetricsRequestCompleted);
            }

            return functionResponse;
        }
    }
}