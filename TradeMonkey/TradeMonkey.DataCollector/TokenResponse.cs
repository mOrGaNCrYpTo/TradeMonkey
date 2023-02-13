public class Token
{
    public string NAME { get; set; }
    public string SYMBOL { get; set; }
    public int TOKEN_ID { get; set; }
}

public class TokenResponse
{
    public List<Token> data { get; set; }
    public int length { get; set; }
    public string message { get; set; }
    public bool success { get; set; }
}