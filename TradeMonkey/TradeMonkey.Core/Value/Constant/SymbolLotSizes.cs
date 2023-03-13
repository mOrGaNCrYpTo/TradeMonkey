namespace TradeMonkey.Trader.Value.Constant
{
    public static class SymbolLotSizes
    {
        private static readonly Dictionary<string, decimal> _symbolLotSizePairs = new Dictionary<string, decimal>()
        {
            { "BTC-USDT", 0.001m },
            { "ETH-USDT", 0.01m },
            { "LTC-USDT", 0.1m }
            // add more symbol-lot size pairs here
        };

        public static decimal GetLotSize(string symbol)
        {
            return _symbolLotSizePairs.TryGetValue(symbol, out decimal lotSize)
                ? lotSize
                : throw new ArgumentException($"Lot size not found for symbol {symbol}");
        }
    }
}