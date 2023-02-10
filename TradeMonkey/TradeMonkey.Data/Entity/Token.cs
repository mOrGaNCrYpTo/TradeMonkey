namespace TradeMonkey.Data.Entity
{
    public class Token
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public int TokenId { get; set; }
        public object TraderGradesDatum { get; internal set; }
    }
}