/// <summary>
/// Get the top 10 and bottom 10 correlation of coin with the top 100 market cap tokens.
/// </summary>
public class Correlation
{
    public CorrelationData[] Data { get; set; }
}

public class CorrelationData
{
    public float Correlation { get; set; }
    public string Date { get; set; }
    public int Epoch { get; set; }
    public string Name { get; set; }
    public string Symbol { get; set; }
    public string Token2Name { get; set; }
    public string Token2Symbol { get; set; }
    public int TokenId { get; set; }
}