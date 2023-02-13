using TradeMonkey.DataCollector;

public static class Program
{
    public static void Main()
    {
        CancellationToken token = new CancellationToken();
        TokenMetrics _tokenMetrics = new TokenMetrics();
        _tokenMetrics.UpsertTokensAsync();
    }
}