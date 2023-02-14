namespace TradeMonkey.TokenMetrics.Domain.Services
{
    [RegisterService]
    public sealed class GetTokensSvc : BaseHttpSvc
    {
        [InjectService]
        public ApiRepository ApiRepo { get; private set; }

        public GetTokensSvc(ApiRepository apiRepository)
        {
            ApiRepo = apiRepository
                ?? throw new ArgumentNullException(nameof(apiRepository));
        }

        public Task<List<Token>> ExecuteAsync(string tokens, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            Request.TokenIds = tokens;
            ApiRepo.Url = Request.BuildUrl();

            return ApiRepo.GetTokensAsync(token);
        }
    }
}