﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Sustainsys.Saml2;
using Sustainsys.Saml2.Configuration;

namespace IdSrv4
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients());

            services.AddAuthentication()
                .AddGoogle("Google", opt =>
                {
                    opt.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                    opt.ClientId = "291695820691-htajtml8utg9jlg1f0i54k3ev95d8rjm.apps.googleusercontent.com";
                    opt.ClientSecret = "EFASXK5e7SCOU07RS9wtdiem";
                })
                .AddSaml2("Corporate Saml2", opt =>
                {
                    opt.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                    opt.SPOptions = new SPOptions
                    {
                        EntityId = new EntityId("https://localhost:44380/Saml2")
                    };

                    opt.IdentityProviders.Add(
                        new IdentityProvider(new EntityId("https://stubidp.sustainsys.com/Metadata"), opt.SPOptions)
                        {
                            LoadMetadata = true
                        });
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseIdentityServer();

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
