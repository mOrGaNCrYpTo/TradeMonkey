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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "get-all_tokens")] HttpRequestData req,
            FunctionContext executionContext,
            string tokens,
            CancellationToken hostCancellationToken = default)
        {
            // validate
            if (string.IsNullOrEmpty(tokens))
                throw new Exception(FunctionEvents.TokenMetricsInvalidRequest);

            // get logger from the context
            var logger = executionContext.GetLogger(nameof(GetAllTokens));
            logger.LogDebug(FunctionEvents.TokenMetricsRequestStarted);

            // create a linked token source
            var lts = CancellationTokenSource.CreateLinkedTokenSource(hostCancellationToken, executionContext.CancellationToken);
            var token = lts.Token;

            // throw and catch an exception if cancellation is requested
            token.ThrowIfCancellationRequested();

            // create a response wrapper. Assume success unless we catch an exception
            HttpResponseData functionResponse = req.CreateResponse(HttpStatusCode.OK);

            try
            {
                // deserialize
                var request = JsonSerializer.Deserialize<TokenMetricsRequest>(req.Body);

                // execute the request and get the response. always forward the cancellation token
                // to the service
                var response = await _getPeruseCategoriesSrvc.ExecuteAsync(request, token);

                await functionResponse.WriteAsJsonAsync(response);
                return functionResponse;
            }
            catch (OperationCanceledException oex)
            {
                logger.LogError($"{FunctionEvents.OperationCancelled}{oex.Message}");
                functionResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                return functionResponse;
            }
            catch (Exception ex)
            {
                logger.LogError($"{FunctionEvents.UnknownExceptionOccured}{ex.Message}");
                functionResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                return functionResponse;
            }
            finally
            {
                logger.LogDebug(FunctionEvents.GetPeruseCategoriesCompleted);
            }

            return response;
        }
    }
}