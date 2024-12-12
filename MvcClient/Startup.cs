// ***********************************************************************
// Assembly         : MvcClient
// Author           : 
// Created          : 11-27-2023
//
// Last Modified By : 
// Last Modified On : 04-22-2024
// ***********************************************************************
// <copyright file="Startup.cs" company="MvcClient">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Landstar.Cloud.Engines;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;

namespace MvcClient;

/// <summary>
/// Class Startup.
/// </summary>
public class Startup(IConfiguration configuration)
{
  /// <summary>
  /// Configures the services.
  /// </summary>
  /// <param name="services">The services.</param>
  public void ConfigureServices(IServiceCollection services)
  {

    IdentityModelEventSource.ShowPII = true;
    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
    services.Configure<KestrelServerOptions>(configuration.GetSection("Kestrel"));
    services.AddHttpClient(Microsoft.Extensions.Options.Options.DefaultName);

    services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


    services.AddAuthentication(options =>
        {
          options.DefaultScheme = "Cookies";
          options.DefaultChallengeScheme = "oidc";
        })
        .AddCookie(options =>
        {
          options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
          options.Cookie.Name = "is.session";
          options.Cookie.HttpOnly = true;
          options.SlidingExpiration = true; // Enables sliding expiration
          options.ExpireTimeSpan = TimeSpan.FromHours(1); // Set expiration time
        })
        .AddOpenIdConnect("oidc", options =>
        {
          options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
          options.RequireHttpsMetadata = false;

          options.Authority = configuration["oidc:authority"];
          options.ClientId = configuration["oidc:client_id"];
          options.ClientSecret = configuration["oidc:client_secret"];
          options.Scope.Add(configuration["oidc:scope"]);

          options.ResponseType = configuration["oidc:response_type"];
          options.SaveTokens = true;
          options.GetClaimsFromUserInfoEndpoint = true;

          options.ClaimActions.MapUniqueJsonKey("role", "role");
          options.TokenValidationParameters = new TokenValidationParameters
          {
            RoleClaimType = "role"
          };

          options.TokenHandler = new JwtSecurityTokenHandler
          {
            InboundClaimTypeMap = new Dictionary<string, string>(),
          };

        });

    
    services.AddRazorPages();
    services.AddControllersWithViews();
    services.AddLandstarCors(
      "https://identitydev.landstaronline.com",
      "http://localdev.landstaronline.com",
      "http://localdev.landstaronline.com:5000",
      "https://localdev.landstaronline.com:5001",
      "https://identityqa.landstaronline.com",
      "https://identity.landstaronline.com");


  }


  /// <summary>
  /// Configures the specified application.
  /// </summary>
  /// <param name="app">The application.</param>
  public static void Configure(IApplicationBuilder app)
  {
    app.UseDeveloperExceptionPage();

    var cultureInfo = new CultureInfo("en-US");
    cultureInfo.NumberFormat.CurrencySymbol = "$";

    CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
    CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;


    app.UseStaticFiles();
    app.UseHttpsRedirection();
    app.UseHsts();
    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();
    app.UseEndpoints(endpoints =>
    {

      endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
      endpoints.MapRazorPages();
    });

  }
}