using AspNetCore2AuthNZ.Data;
using AspNetCore2AuthNZ.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore2AuthNZ.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private ShopContext _shopContext;
        private IAuthorizationService _authZService;

        public OrderController(ShopContext shopContext, IAuthorizationService authorizationService)
        {
            _shopContext = shopContext;
            _authZService = authorizationService;
        }

        public async Task<IActionResult> View(int id)
        {
            var model = _shopContext.Orders.Include(o => o.Lines).Single(o => o.Id == id);

            if ((await _authZService.AuthorizeAsync(User, model, "ViewOrder")).Succeeded)
            {
                return View(model);
            }
            return new ForbidResult();
        }

        public IActionResult Index()
        {
            var model = _shopContext
                .Orders
                .Where(o => o.SentTime != null)
                .Where(o => o.UserId == User.FindFirst("sub").Value 
                || User.HasClaim("role", "Administrator"))
                .Select(o => new OrderListViewModel
                {
                    OrderId = o.Id,
                    ItemCont = o.Lines.Sum(ol => ol.Quantity),
                    SentTime = o.SentTime.Value,
                    UserId = o.UserId
                });

            return View(model);
        }
    }
}
