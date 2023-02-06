/// <summary>
/// Get the list of tokens and their associated ID supported by Token Metrics.
/// </summary>

public class AllTokens : Base
{
    public AllTokensData[] Data { get; set; }
}

public class AllTokensData
{
    public string NAME { get; set; }
    public string SYMBOL { get; set; }
    public int TOKEN_ID { get; set; }
}