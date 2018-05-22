using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore2AuthNZ.Models
{
    public class OrderListViewModel
    {
        public int OrderId { get; set; }

        public int ItemCont { get; set; }

        public DateTime SentTime { get; set; }
    }
}
