﻿using TradeMonkey.Data.Entity;

public class TokenResponse
{
    public List<Tokens> Data { get; set; }
    public int Length { get; set; }
    public string Message { get; set; }
    public bool Success { get; set; }
}