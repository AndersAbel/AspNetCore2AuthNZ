using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspNetCore2AuthNZ.Data
{
    public class ShopContext : DbContext
    {
        public ShopContext(DbContextOptions<ShopContext> options)
            : base(options)
        { }

        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderLine>()
                .HasKey(ol => new { ol.OrderId, ol.ProductId });
        }

        public int GetCartItemCount(ClaimsPrincipal user)
        {
            var userId = user.FindFirst("sub")?.Value;

            return Orders.Include(o => o.Lines)
            .SingleOrDefault(o => o.SentTime == null
            && o.UserId == userId)
            ?.Lines.Sum(ol => ol.Quantity) ?? 0;
        }
    }
}
