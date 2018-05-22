using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore2AuthNZ.Models
{
    public static class Product
    {
        private static readonly string[] productNames = new[]
        {
            "Brown Chocolate",
            "Dark Chocolate",
            "White Chocolate",
            "VIP Package"
        };

        public static string GetName(int productNo)
        {
            return productNames[productNo - 1];
        }
    }
}
