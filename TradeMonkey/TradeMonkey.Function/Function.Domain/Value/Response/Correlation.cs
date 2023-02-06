/// <summary>
/// Get the top 10 and bottom 10 correlation of coin with the top 100 market cap tokens.
/// </summary>
public class Correlation
{
    public CorrelationData[] Data { get; set; }
}

public class CorrelationData
{
    public float CORRELATION { get; set; }
    public string DATE { get; set; }
    public int EPOCH { get; set; }
    public string NAME { get; set; }
    public string SYMBOL { get; set; }
    public string TOKEN_2_NAME { get; set; }
    public string TOKEN_2_SYMBOL { get; set; }
    public int TOKEN_ID { get; set; }
}