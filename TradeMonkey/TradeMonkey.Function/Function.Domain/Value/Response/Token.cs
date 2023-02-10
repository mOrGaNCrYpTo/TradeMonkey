/// <summary>
/// Get the list of tokens and their associated ID supported by Token Metrics.
/// </summary>

public class Token : Base
{
    public TokenData[] Data { get; set; }
}

public class TokenData
{
    public string Name { get; set; }
    public string Symbol { get; set; }
    public int TokenId { get; set; }
}