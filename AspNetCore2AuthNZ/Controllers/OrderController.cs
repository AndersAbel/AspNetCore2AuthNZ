using AspNetCore2AuthNZ.Data;
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
    }
}
