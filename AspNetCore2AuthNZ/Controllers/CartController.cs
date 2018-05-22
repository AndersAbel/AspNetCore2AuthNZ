using AspNetCore2AuthNZ.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore2AuthNZ.Controllers
{
    public class CartController : Controller
    {
        private ShopContext _shopContext;

        public CartController(ShopContext shopContext)
        {
            _shopContext = shopContext;
        }

        public IActionResult Index()
        {
            var model = _shopContext.Orders
                .Include(o => o.Lines)
                .SingleOrDefault(o => o.SentTime == null);

            return View(model);
        }

        [HttpPost]
        public IActionResult Add(int product)
        {
            var order = _shopContext.Orders
                .Include(o => o.Lines)
                .SingleOrDefault(o => o.SentTime == null);

            if(order == null)
            {
                order = new Order();
                order.Lines = new List<OrderLine>();
                _shopContext.Orders.Add(order);
            }

            var orderLine = order.Lines?.SingleOrDefault(ol => ol.ProductId == product);
            if (orderLine == null)
            {
                orderLine = new OrderLine { ProductId = product };
                order.Lines.Add(orderLine);
            }

            orderLine.Quantity += 1;

            _shopContext.SaveChanges();

            return Json(_shopContext.GetCartItemCount());
        }
    }
}
