namespace TradeMonkey.TokenMetrics.Domain.Utillity
{
    public static class Extensions
    {
        public static string ToCommaDelimted(this List<int> items)
        {
            return string.Join(',', items);
        }
    }
}