using System.ComponentModel.DataAnnotations;

namespace TradeMonkey.Data.Entity
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}