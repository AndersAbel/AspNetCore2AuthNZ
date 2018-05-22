using AspNetCore2AuthNZ.Data;
using AspNetCore2AuthNZ.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore2AuthNZ.Controllers
{
    public class OrderController : Controller
    {
        private ShopContext _shopContext;

        public OrderController(ShopContext shopContext)
        {
            _shopContext = shopContext;
        }

        public IActionResult View(int id)
        {
            var model = _shopContext.Orders.Include(o => o.Lines).Single(o => o.Id == id);
            return View(model);
        }

        public IActionResult Index()
        {
            var model = _shopContext
                .Orders
                .Where(o => o.SentTime != null)
                .Select(o => new OrderListViewModel
                {
                    OrderId = o.Id,
                    ItemCont = o.Lines.Sum(ol => ol.Quantity),
                    SentTime = o.SentTime.Value
                });

            return View(model);
        }
    }
}
