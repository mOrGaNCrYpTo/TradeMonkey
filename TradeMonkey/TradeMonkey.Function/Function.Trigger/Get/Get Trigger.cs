using TradeMonkey.Data.Entity;
using TradeMonkey.TokenMetrics.Domain.Services;

namespace TradeMonkey.TokenMetrics.Trigger.Get
{
    public class GetGrades
    {
        private readonly ILogger _logger;

        [InjectService]
        public GetTokensSvc GetTokensSvc { get; set; }

        public GetGrades(GetTokensSvc getTokensSvc) =>
            GetTokensSvc = getTokensSvc
                ?? throw new ArgumentNullException(nameof(GetTokensSvc));

        [Function(nameof(GetGrades))]
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "get-grades")] HttpRequestData req,
            FunctionContext executionContext,
            string timeFrame,
            string symbols,
            CancellationToken hostCancellationToken = default)
        {
            // validate
            if (string.IsNullOrEmpty(timeFrame))
                throw new Exception(FunctionEvents.TokenMetricsInvalidRequest);

            // create a linked token source
            var lts = CancellationTokenSource.CreateLinkedTokenSource(hostCancellationToken, executionContext.CancellationToken);
            var ct = lts.Token;

            // throw and catch an exception if cancellation is requested
            ct.ThrowIfCancellationRequested();

            // get logger from the context
            var logger = executionContext.GetLogger(nameof(GetGrades));
            logger.LogDebug(FunctionEvents.TokenMetricsRequestStarted);

            // create a response wrapper. Assume success unless we catch an exception
            HttpResponseData functionResponse = req.CreateResponse(HttpStatusCode.OK);

            try
            {
                // deserialize var request = JsonSerializer.Deserialize<Request>(req.Body);

                // execute the request and get the response. always forward the cancellation token
                // to the service
                var response = await GetTokensSvc.ExecuteAsync(symbols, ct);

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