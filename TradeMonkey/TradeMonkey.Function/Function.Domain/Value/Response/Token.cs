/// <summary>
/// Get the list of tokens and their associated ID supported by Token Metrics.
/// </summary>

public class Token : Base
{
    public TokenData[] Data { get; set; }
}

public class TokenData
{
    public string NAME { get; set; }
    public string SYMBOL { get; set; }
    public int TOKEN_ID { get; set; }
}