using System.Linq;
using System.Threading.Tasks;
using AspNetCore2AuthNZ.Data;
using Microsoft.AspNetCore.Authorization;

namespace AspNetCore2AuthNZ
{
    internal class MinExistingOrderHandler : AuthorizationHandler<MinExistingOrderRequirement>
    {
        private ShopContext _shopContext;

        public MinExistingOrderHandler(ShopContext shopContext)
        {
            _shopContext = shopContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinExistingOrderRequirement requirement)
        {
            if (_shopContext.Orders
                .Count(o => o.UserId == context.User.FindFirst("sub").Value && o.SentTime != null)
                >= requirement.Count)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}