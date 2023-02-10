/// <summary>
/// Get the AI created model portfolio based on crypto Exchange, risk and time horizon.
/// </summary>
public class Indicies : Base
{
    private IndiciesDatum[] Data { get; set; }
}

public class IndiciesDatum
{
    public float BLWeight { get; set; }
    public string Date { get; set; }
    public int Epoch { get; set; }
    public string Exchange { get; set; }
    public float InitialPrice { get; set; }
    public string Name { get; set; }
    public string Symbol { get; set; }
    public int TokenId { get; set; }
}