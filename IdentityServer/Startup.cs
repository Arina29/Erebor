// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using Duende.IdentityServer.Configuration.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using IdentityServer.Data;
using IdentityServer.Models.DomainModels;
using Microsoft.AspNetCore.Http;

namespace IdentityServer;

public class Startup
{
    public IWebHostEnvironment Environment { get; }
    public IConfiguration Configuration { get; }

    public Startup(IWebHostEnvironment environment, IConfiguration configuration)
    {
        Environment = environment;
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        var migrationAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
        var connectionString = Configuration.GetConnectionString("AuthContext");
        if (connectionString is null)
        {
            throw new NullReferenceException("Auth Connection string is null");
        }

        services.AddControllersWithViews();

        services.AddDbContext<AuthDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddIdentity<EreborUser, IdentityRole>()
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddDefaultTokenProviders();

        var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                options.EmitStaticAudienceClaim = true;
            })
            .AddAspNetIdentity<EreborUser>()
            .AddConfigurationStore(opt =>
                opt.ConfigureDbContext = c => c.UseSqlServer(connectionString,
                    sql => sql.MigrationsAssembly(migrationAssembly)))
            .AddOperationalStore(opt =>
            {
                opt.TokenCleanupInterval = 600;
                opt.ConfigureDbContext = o => o.UseSqlServer(connectionString,
                    sql => sql.MigrationsAssembly(migrationAssembly));
            });

        services.AddAuthentication().AddCookie("cookie", options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromHours(2);

            // sliding or absolute
            options.SlidingExpiration = false;

            // host prefixed cookie name
            options.Cookie.Name = "__Host-spa";

            // strict SameSite handling
            options.Cookie.SameSite = SameSiteMode.Strict;
        });


        services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()));

        services.AddMvc(options =>
        {
            options.EnableEndpointRouting = false;
        });

        // not recommended for production - you need to store your key material somewhere secure
        if (Environment.IsDevelopment())
        {
            builder.AddDeveloperSigningCredential();
        }
    }

    public void Configure(IApplicationBuilder app)
    {
        if (Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();

        app.UseRouting();
        app.UseIdentityServer();
        app.UseAuthorization();
        app.UseCors("AllowAll");

        app.UseMvc(routes =>
        {
            routes.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Index}/{id?}");
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
        });
    }
}