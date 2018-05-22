using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCore2AuthNZ.Data
{
    [Table("OrderLines")]
    public class OrderLine
    {
        public int OrderId { get; set; }

        public int ProductId { get; set; }
    }
}