using AspNetCore2AuthNZ.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspNetCore2AuthNZ.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private ShopContext _shopContext;
        private IAuthorizationService _authzService;

        public CartController(ShopContext shopContext, IAuthorizationService authzService)
        {
            _shopContext = shopContext;
            _authzService = authzService;
        }

        private Order CurrentCart
        {
            get
            {
                var userId = User.FindFirst("sub")?.Value;
                return _shopContext.Orders.Include(o => o.Lines)
                .SingleOrDefault(o => o.SentTime == null && o.UserId == userId);
            }
        }

        public IActionResult Index()
        {
            return View(CurrentCart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Send()
        {
            var order = CurrentCart;

            order.SentTime = DateTime.Now;
            _shopContext.SaveChanges();

            return RedirectToAction("View", "Order", new { id = order.Id });
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Add(int product)
        {
            if(product == 4 && !(await _authzService.AuthorizeAsync(User, null, "VIP")).Succeeded)
            {
                return Forbid();
            }

            var order = CurrentCart;

            if(order == null)
            {
                order = new Order()
                {
                    Lines = new List<OrderLine>(),
                    UserId = User.FindFirst("sub")?.Value
                };
                
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

            return Json(_shopContext.GetCartItemCount(User));
        }
    }
}
