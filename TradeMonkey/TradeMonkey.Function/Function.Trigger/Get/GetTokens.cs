using TradeMonkey.TokenMetrics.Domain.Services;

namespace TradeMonkey.TokenMetrics.Trigger.Get
{
    public class GetTokens
    {
        private readonly ILogger _logger;

        [InjectService]
        public GetTokensSvc GetTokensSvc { get; set; }

        public GetTokens(GetTokensSvc getTokensSvc) =>
            GetTokensSvc = getTokensSvc
                ?? throw new ArgumentNullException(nameof(GetTokensSvc));

        [Function(nameof(GetTokens))]
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "get-all_tokens")] HttpRequestData req,
            FunctionContext executionContext,
            string tokens,
            CancellationToken hostCancellationToken = default)
        {
            // validate
            if (string.IsNullOrEmpty(tokens))
                throw new Exception(FunctionEvents.TokenMetricsInvalidRequest);

            // create a linked token source
            var lts = CancellationTokenSource.CreateLinkedTokenSource(hostCancellationToken, executionContext.CancellationToken);
            var token = lts.Token;

            // throw and catch an exception if cancellation is requested
            token.ThrowIfCancellationRequested();

            // get logger from the context
            var logger = executionContext.GetLogger(nameof(GetTokens));
            logger.LogDebug(FunctionEvents.TokenMetricsRequestStarted);

            // create a response wrapper. Assume success unless we catch an exception
            HttpResponseData functionResponse = req.CreateResponse(HttpStatusCode.OK);

            try
            {
                // deserialize var request = JsonSerializer.Deserialize<Request>(req.Body);

                // execute the request and get the response. always forward the cancellation token
                // to the service
                var response = await GetTokensSvc.ExecuteAsync(tokens, token);

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