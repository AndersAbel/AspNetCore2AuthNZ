using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCore2AuthNZ.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore2AuthNZ
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddDbContext<ShopContext>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString("DBConnection")));

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(opt =>
            {
                opt.DefaultScheme = "Cookies";
                opt.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", opt =>
            {
                opt.SignInScheme = "Cookies";

                opt.Authority = "https://localhost:44380";
                opt.ClientId = "mvc";
            })
            .AddIdentityServerAuthentication(opt =>
            {
                opt.Authority = "https://localhost:44380";
                opt.ApiName = "cart";
            });

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("ViewOrder", p =>
                p.RequireAssertion(c => c.User.HasClaim("role", "Administrator")
                || c.User.FindFirst("sub").Value == ((Order)c.Resource).UserId));

                opt.AddPolicy("VIP", p => p.AddRequirements(new MinExistingOrderRequirement(1)));
            });

            services.AddScoped<IAuthorizationHandler, MinExistingOrderHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
