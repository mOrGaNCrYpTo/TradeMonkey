namespace TradeMonkey.DataCollector.Services
{
    [RegisterService]
    public sealed class KucoinAccountSvc
    {
        [InjectService]
        public KucoinRepository Repo { get; private set; }

        /// <summary>
        /// </summary>
        /// <param name="repository"> </param>
        /// <exception cref="ArgumentNullException"> </exception>
        public KucoinAccountSvc(KucoinRepository repository)
        {
            Repo = repository ??
                throw new ArgumentNullException(nameof(repository));
        }

        public async Task<List<KucoinAccount>> GetAccountsAsync(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var ret = await Repo.GetAccountsAsync(ct);
            return ret.Data.ToList();
        }
    }
}