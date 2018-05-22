using System;
using System.Collections.Generic;

namespace AspNetCore2AuthNZ.Data
{
    public class Order
    {
        public int Id { get; set; }

        public List<OrderLine> Lines { get; set; }

        public DateTime? SentTime { get; set; }
    }
}