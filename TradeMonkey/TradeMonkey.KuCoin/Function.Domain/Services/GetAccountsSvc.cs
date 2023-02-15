namespace TradeMonkey.KuCoin.Domain.Services
{
    [RegisterService]
    public sealed class GetAccountsSvc : BaseHttpSvc
    {
        [InjectService]
        public ApiRepository ApiRepo { get; private set; }

        public GetAccountsSvc(ApiRepository apiRepository)
        {
            ApiRepo = apiRepository
                ?? throw new ArgumentNullException(nameof(apiRepository));
        }

        public Task<List<Token>> ExecuteAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            Request.TokenIds = tokens;
            ApiRepo.Url = Request.BuildUrl();

            return ApiRepo.GetAccountsAsync(token);
        }
    }
}