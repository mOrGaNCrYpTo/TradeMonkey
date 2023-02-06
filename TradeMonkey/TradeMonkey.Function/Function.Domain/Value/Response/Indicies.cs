public class Datum
{
    public float BL_WEIGHT { get; set; }
    public string DATE { get; set; }
    public int EPOCH { get; set; }
    public string EXCHANGE { get; set; }
    public float INITIAL_PRICE { get; set; }
    public string NAME { get; set; }
    public string SYMBOL { get; set; }
    public int TOKEN_ID { get; set; }
}

/// <summary>
/// Get the AI created model portfolio based on crypto exchange, risk and time horizon.
/// </summary>
public class Indicies : Base