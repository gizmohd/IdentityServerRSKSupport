// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-22-2024
//
// Last Modified By : 
// Last Modified On : 04-22-2024
// ***********************************************************************
// <copyright file="HostingExtensions.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
global using Rsk.WsFederation.EntityFramework.DbContexts;
using Duende.IdentityServer;
using Duende.IdentityServer.Configuration;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Storage;
using Duende.IdentityServer.Validation;
using Flurl;
using Flurl.Http;
using IdentityExpress.Identity;
using Landstar.Identity.Data;
using Landstar.Identity.Exceptions;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Rsk.Saml.Configuration;
using Rsk.Saml.DuendeIdentityServer.EntityFramework;
using Rsk.Saml.DuendeIdentityServer.EntityFramework.Stores;
using Rsk.Saml.EntityFramework.DbContexts;
using Rsk.Saml.IdentityProvider.Storage.EntityFramework.DbContexts;
using Rsk.WsFederation.Configuration;
using Rsk.WsFederation.EntityFramework.Stores;
using Serilog;
using System.Reflection;
using System.Security.Claims;




namespace Landstar.Identity;
/// <summary>
/// Class HostingExtensions.
/// </summary>
public static class HostingExtensions
{
  const string APPLICATION_NAME = "Landstar.Identity";

  
  /// <summary>
  /// Gets the API engine version.
  /// </summary>
  /// <value>The API engine version.</value>
  public static string API_ENGINE_VERSION
  {
    get
    {
      Assembly assembly = typeof(Duende.IdentityServer.Internal.DefaultConcurrencyLock<object>).Assembly;
      System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
      return fvi.FileVersion ?? "2.0";
    }
  }

  /// <summary>
  /// Gets the enable automatic provision user.
  /// </summary>
  /// <value>The enable automatic provision user.</value>
  public static bool EnableAutoProvisionUser => bool.Parse(Configuration["EnableAutoProvisionUser"] ?? "false");

  /// <summary>
  /// Gets the configuration.
  /// </summary>
  /// <value>The configuration.</value>
  public static IConfiguration Configuration => Landstar.Identity.ConfigurationExtensions.Configuration;

  /// <summary>
  /// Sets the enable sensitive logging.
  /// </summary>
  /// <value>The enable sensitive logging.</value>
  static bool EnableSensitiveLogging
  {
    get
    {
      _ = bool.TryParse(Landstar.Identity.ConfigurationExtensions.Configuration["EnableSensitiveDataLogging"], out bool enable);
      return enable;
    }
  }

  /// <summary>
  /// Configures the services.
  /// </summary>
  /// <param name="builder">The builder.</param>
  /// <returns>Microsoft.AspNetCore.Builder.WebApplication.</returns>
  public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
  {
    ArgumentNullException.ThrowIfNull(builder);

    builder.Services.AddRazorPages()
                    .AddViewLocalization();

    builder.Services.AddExceptionHandler<CustomExceptionHandler>();
    builder.Services.AddKendo();
    builder.Services.ConfigureIdentityServer();

    builder.Services.ConfigureAuthrorization();

    builder.Services.ConfigureApiVersioning();

    builder.Services.Configure<PasswordHasherOptions>(opt => opt.IterationCount = 210_000);

    

    builder.Services.AddKendo();

    builder.Services.AddHttpContextAccessor();

    builder.Services.AddOptions<List<Models.PublicUserData>>().Bind(builder.Configuration.GetSection("Authentication:PublicUsers"));

    builder.Services.Configure<RequestLocalizationOptions>(options =>
    {
      string[] supportedCultures = new[] { en_US, fr_CA, es_MX };
      options.ApplyCurrentCultureToResponseHeaders = true;
      options.AddSupportedCultures(supportedCultures)
             .AddSupportedUICultures(supportedCultures)
             .SetDefaultCulture(en_US);
      options.AddInitialRequestCultureProvider(new CustomRequestCultureProvider(async context =>
      {
        // My custom request culture logic
        return await Task.FromResult(new ProviderCultureResult("en"));
      }));
    });


    builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
      options.ForwardedHeaders =
          ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    });

    builder.Services.AddCors(options =>
    {
      options.AddPolicy("AllowAllOrigins",
          builder =>
          {
            builder.AllowAnyOrigin()
                     .AllowAnyMethod()
                     .AllowAnyHeader();
          });
    });



    builder.Services.AddDistributedMemoryCache();

    builder.Services.AddDbContext<IdentityExpressDbContext>(IdentityServerBuilder, ServiceLifetime.Transient);
    builder.Services.AddDbContext<WsFederationConfigurationDbContext>(IdentityServerBuilder, ServiceLifetime.Transient);
    builder.Services.AddDbContext<ApplicationDbContext>(IdentityServerBuilder, ServiceLifetime.Transient);
    builder.Services.AddDbContext<SamlConfigurationDbContext>(IdentityServerBuilder, ServiceLifetime.Transient);
    builder.Services.AddDbContext<SamlArtifactDbContext>(IdentityServerBuilder, ServiceLifetime.Transient);
    builder.Services.AddDbContext<ConfigurationDbContext>(IdentityServerBuilder, ServiceLifetime.Transient);

    builder.Services.AddScoped<IWsFederationConfigurationDbContext, WsFederationConfigurationDbContext>();
    builder.Services.AddOutputCache(outputCache =>
    {
      outputCache.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(60);
      outputCache.AddBasePolicy(p => p.Expire(TimeSpan.FromMilliseconds(0)));
    });

    return builder.Build();
  }
  /// <summary>
  /// Configures the identity server.
  /// </summary>
  /// <param name="services">The services.</param>
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S138:Functions should not have too many lines of code", Justification = "Initialization Method")]
  private static void ConfigureIdentityServer(this IServiceCollection services)
  {
    _ = bool.TryParse(Configuration["EnableUserSessions"], out bool _enableUserSessions);

    IdentityBuilder identity = services.AddIdentity<IdentityExpressUser, IdentityExpressRole>(
       options =>
       {

         PasswordOptions pOptions = new();
         IConfigurationSection passwordOptions = Configuration.GetSection("Authentication:PasswordOptions");
         if (int.TryParse(passwordOptions["RequireLength"], out int requiredLength))
         {
           pOptions.RequiredLength = requiredLength;
         }

         if (int.TryParse(passwordOptions["RequireUniqueChars"], out int requireUniqueChars))
         {
           pOptions.RequiredUniqueChars = requireUniqueChars;
         }

         if (bool.TryParse(passwordOptions["RequireNonAlphanumeric"], out bool requireNonAlphanumeric))
         {
           pOptions.RequireNonAlphanumeric = requireNonAlphanumeric;
         }

         if (bool.TryParse(passwordOptions["RequireLowercase"], out bool requireLowercase))
         {
           pOptions.RequireLowercase = requireLowercase;
         }

         if (bool.TryParse(passwordOptions["RequireUpperCase"], out bool requireUpperCase))
         {
           pOptions.RequireUppercase = requireUpperCase;
         }

         if (bool.TryParse(passwordOptions["RequireDigit"], out bool requireDigit))
         {
           pOptions.RequireDigit = requireDigit;
         }

         _ = bool.TryParse(passwordOptions["RequireConfirmedEmail"], out bool requireConfirmedEmail);


         options.User.RequireUniqueEmail = true;

         options.Password = pOptions;
         options.SignIn.RequireConfirmedEmail = requireConfirmedEmail;
         options.SignIn.RequireConfirmedPhoneNumber = false;
         options.User.RequireUniqueEmail = true;

       })
     .AddRoles<IdentityExpressRole>()
     .AddEntityFrameworkStores<IdentityExpressDbContext>()
     .AddUserStore<IdentityExpressUserStore>()
     .AddRoleStore<IdentityExpressRoleStore>()
     .AddIdentityExpressUserClaimsPrincipalFactory()
     .AddDefaultTokenProviders();

    services.ConfigureApplicationCookie(options =>
    {
      if (!int.TryParse(Configuration["Authentication:RememberMeExpirationDays"], out int expirationDays))
      {
        expirationDays = 1;
      }
      // Set the expiration time for the authentication cookie
      options.ExpireTimeSpan = TimeSpan.FromDays(expirationDays); // Matches the RememberMeDuration
      options.SlidingExpiration = true;
    });

    services.AddSession(options =>
    {
      options.IdleTimeout = TimeSpan.FromHours(2);
      options.Cookie.HttpOnly = true;
      options.Cookie.IsEssential = true;
    });

    services.TryAddScoped<IdentityExpressUserStore>();


    IIdentityServerBuilder isBuilder = services
          .AddIdentityServer(options =>
          {
            options.ServerSideSessions.UserDisplayNameClaimType = "name";

            options.ServerSideSessions.RemoveExpiredSessions = true;
            options.ServerSideSessions.RemoveExpiredSessionsBatchSize = 100;
            options.ServerSideSessions.ExpiredSessionsTriggerBackchannelLogout = true;


            options.Events.RaiseErrorEvents = true;
            options.Events.RaiseInformationEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseSuccessEvents = true;

            options.Endpoints.EnableEndSessionEndpoint = true;

            // see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
            options.EmitStaticAudienceClaim = true;
            options.Authentication.CookieLifetime = TimeSpan.FromDays(30);
            options.Authentication.CookieSlidingExpiration = true;

            options.IssuerUri = Configuration["IssuerUri"];

            options.KeyManagement.Enabled = true;
            options.KeyManagement.RotationInterval = TimeSpan.FromDays(30);
            options.KeyManagement.PropagationTime = TimeSpan.FromDays(2);
            options.KeyManagement.RetentionDuration = TimeSpan.FromDays(7);
            options.KeyManagement.DeleteRetiredKeys = true;

            options.KeyManagement.SigningAlgorithms =
            [
              // RS256 for older clients (with additional X.509 wrapping)
              new SigningAlgorithmOptions(SecurityAlgorithms.RsaSha256) { UseX509Certificate = true },    
              
              // PS256
              new SigningAlgorithmOptions(SecurityAlgorithms.RsaSsaPssSha256),
   
              // ES256
              new SigningAlgorithmOptions(SecurityAlgorithms.EcdsaSha256)
            ];

            if (!int.TryParse(Configuration["Authentication:Cache:ClientStoreCacheTimeOutMinutes"], out int cacheTimeout))
            {
              cacheTimeout = 15;
            }

            options.Caching.ClientStoreExpiration = TimeSpan.FromMinutes(cacheTimeout);
            if (!int.TryParse(Configuration["Authentication:Cache:IdentityProviderCacheTimeOutMinutes"], out int ipCacheTimeout))
            {
              ipCacheTimeout = 15;
            }

            options.Caching.IdentityProviderCacheDuration = TimeSpan.FromMinutes(ipCacheTimeout);

            if (!int.TryParse(Configuration["Authentication:Cache:ResourceStoreCacheTimeOutMinutes"], out int resCacheTimeout))
            {
              resCacheTimeout = 15;
            }
            options.Caching.ResourceStoreExpiration = TimeSpan.FromMinutes(resCacheTimeout);

            if (!int.TryParse(Configuration["Authentication:Cache:CorsCacheTimeOutMinutes"], out int corsCacheTimeout))
            {
              corsCacheTimeout = 15;
            }
            options.Caching.CorsExpiration = TimeSpan.FromMinutes(corsCacheTimeout);

            options.LicenseKey = Configuration["Licensing:IdentityServer:Key"];

            options.UserInteraction.ErrorUrl = "/home/error";
          })

          .AddAspNetIdentity<IdentityExpressUser>()
          .AddJwtBearerClientAuthentication()

          .AddAppAuthRedirectUriValidator()
    #region WSFederation

       .AddRelyingPartyStore<RelyingPartyStore>()
       .AddWsFederationPlugin(options =>
       {
         options.LicenseKey = Configuration["Licensing:WsFed:Key"];
         options.Licensee = Configuration["Licensing:WsFed:Licensee"];
       })

    #endregion
    #region SAML
       .AddSamlDynamicProvider(options =>
       {
         options.LicenseKey = Configuration["Licensing:SAML2P:Key"];
         options.Licensee = Configuration["Licensing:SAML2P:Licensee"];
         _ = int.TryParse(Configuration["Licensing:SAML2P:TimeComparisonTolerance"], out int tolerance);
         options.TimeComparisonTolerance = tolerance;
       })
        .AddIdentityProviderStore<SamlIdentityProviderStore>()
        .AddSamlPlugin(options =>
        {
          options.LicenseKey = Configuration["Licensing:SAML2P:Key"];
          options.Licensee = Configuration["Licensing:SAML2P:Licensee"];
          options.WantAuthenticationRequestsSigned = false;
          _ = int.TryParse(Configuration["Licensing:SAML2P:TimeComparisonTolerance"], out int tolerance);
          options.TimeComparisonTolerance = tolerance;
        })
        .AddSamlConfigurationStore(options =>
        {
          options.ConfigureDbContext = IdentityServerBuilder;
        })
        .AddSamlArtifactStore(options =>
        {
          options.ConfigureDbContext = IdentityServerBuilder;
        })
    #endregion
         .AddOperationalStore(options =>
         {
           options.ConfigureDbContext = IdentityServerBuilder;

           // this enables automatic token cleanup. this is optional.
           options.EnableTokenCleanup = true;
           options.TokenCleanupInterval = 3600;
         })
        .AddConfigurationStore(options =>
        {
          options.ConfigureDbContext = IdentityServerBuilder;
        })
        .AddConfigurationStoreCache();

    if (_enableUserSessions)
    {
      isBuilder.AddServerSideSessions();
    }


    services.AddOperationalDbContext(options =>
    {
      options.ConfigureDbContext = IdentityServerBuilder;
    });

    services.AddConfigurationDbContext(options =>
    {
      options.ConfigureDbContext = IdentityServerBuilder;
    });

    services.AddScoped<IUserStore<IdentityExpressUser>>(
      x => new IdentityExpressUserStore(x.GetService<IdentityExpressDbContext>()) { AutoSaveChanges = true });

    services.ConfigureAuthentication();
  }

  private static void ConfigureAuthentication(this IServiceCollection services)
  {
    List<string> oidcCache = ["oidc"];

    Microsoft.AspNetCore.Authentication.AuthenticationBuilder auth = services.AddAuthentication();
    auth.AddGoogle("google", "Login with Google", options =>
    {
      options.SignInScheme = Duende.IdentityServer.IdentityServerConstants.ExternalCookieAuthenticationScheme;
      if (Configuration["Authentication:Google:ClientId"] != null &&
          Configuration["Authentication:Google:ClientSecret"] != null)
      {
        options.ClientId = Configuration["Authentication:Google:ClientId"];
        options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
      }
    });


    auth.AddCookie(options =>
    {
      // Configure the client application to use sliding sessions
      options.SlidingExpiration = true;
      options.Cookie.HttpOnly = true;
      options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
      options.Cookie.Path = "/";
      options.Cookie.IsEssential = true;
      options.Cookie.Name = APPLICATION_NAME;
      options.SlidingExpiration = true;
      options.ExpireTimeSpan = TimeSpan.FromMinutes(int.Parse(Configuration["CookieExpireTimeMinutes"] ?? "30"));

      options.Events.OnSigningIn = context =>
      {
        HttpRequest request = context.HttpContext.Request;
        string domain = request.Host.Host; // Use the request host as the domain
        context.CookieOptions.Domain = domain;
        return Task.CompletedTask;
      };
    });



    auth.AddOpenIdConnect("oidc", "Landstar Online", options =>
    {

      options.Authority = Configuration["Authentication:SiteMinder:Authority"];
      options.CorrelationCookie.Path = "/";

      options.CorrelationCookie.IsEssential = true;
      options.CorrelationCookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;

      options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;

      options.NonceCookie.IsEssential = true;
      options.NonceCookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
      options.NonceCookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;

      options.NonceCookie.Path = "/";

      options.SaveTokens = true;

      options.Configuration = new Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectConfiguration
      {
        JwksUri = Configuration["Authentication:SiteMinder:JwksUri"],
        TokenEndpoint = Configuration["Authentication:SiteMinder:TokenEndpoint"],
        UserInfoEndpoint = Configuration["Authentication:SiteMinder:UserInfoEndpoint"],
        AuthorizationEndpoint = Configuration["Authentication:SiteMinder:AuthorizationEndpoint"],
        EndSessionEndpoint = Configuration["Authentication:SiteMinder:EndSessionEndpoint"],
        LogoutSessionSupported = true,
        HttpLogoutSupported = true
      };

      options.SaveTokens = true;

      options.Scope.Add("openid");
      options.Scope.Add("email");
      options.Scope.Add("profile");
      options.Scope.Add("groups");
      options.Scope.Add("lsuniqueid");

      options.ResponseType = "code";

      options.ClientId = Configuration["Authentication:SiteMinder:ClientId"];
      options.ClientSecret = Configuration["Authentication:SiteMinder:ClientSecret"];

      options.SaveTokens = true;
      options.GetClaimsFromUserInfoEndpoint = true;

      options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
      options.SignOutScheme = IdentityServerConstants.SignoutScheme;

      options.SignedOutRedirectUri = Configuration["Authentication:SiteMinder:LandstarLoggedOutUrl"].SetQueryParam("redirect_uri", Configuration["IssuerUri"]);

    });

    //if (bool.TryParse(Configuration["Authentication:LinkedIn:Enabled"], out bool linkedInEnabled) && linkedInEnabled)
    //{
    //  auth.AddOAuth("linkedin", "Login with LinkedIn", options =>
    //  {
    //    options.RemoteAuthenticationTimeout = TimeSpan.FromMinutes(3);
    //    options.ClientId = Configuration["Authentication:LinkedIn:ClientId"];
    //    options.ClientSecret = Configuration["Authentication:LinkedIn:ClientSecret"];

    //    options.AuthorizationEndpoint = Configuration["Authentication:SiteMinder:AuthorizationEndpoint"];
    //    options.UserInformationEndpoint = Configuration["Authentication:SiteMinder:UserInformationEndpoint"];

    //    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

    //    options.CallbackPath = "/signin-linkedin";

    //    options.Scope.AddRange(Configuration["Authentication:LinkedIn:Scopes"].Split(' '));
    //    options.Events = new OAuthEvents
    //    {
    //      OnCreatingTicket = async context =>
    //      {
    //        var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
    //        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    //        request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, context.AccessToken);

    //        var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
    //        response.EnsureSuccessStatusCode();
    //        var json = await response.Content.ReadAsStringAsync(cancellationToken: default);
    //        var user = (JsonElement)System.Text.Json.JsonSerializer.Deserialize(json, typeof(JsonElement));
    //        context.RunClaimActions(user);
    //      }
    //    };
    //  });
    //  oidcCache.Add("linkedin");
    //}

    if (bool.TryParse(Configuration["Authentication:ADFS:Enabled"], out bool adfsEnabled) && adfsEnabled)
    {
      auth.AddOpenIdConnect("oidc-adfs", "Landstar Corporate", options =>
      {
        options.RemoteAuthenticationTimeout = TimeSpan.FromMinutes(3);
        options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
        options.SignOutScheme = IdentityServerConstants.SignoutScheme;
        options.CallbackPath = "/signin-oidc-adfs";
        options.SignedOutCallbackPath = "/signout-callback-oidc-adfs";
        options.Authority = $"https://login.microsoftonline.com/{Configuration["Authentication:ADFS:TenantId"]}";
        options.SaveTokens = true;
        options.ClientId = Configuration["Authentication:ADFS:ApplicationId"];
        options.ClientSecret = Configuration["Authentication:ADFS:Secret"];
        options.ResponseType = OpenIdConnectResponseType.Code;
        options.Scope.Add("adfs");
        options.Scope.Add("email");
        options.Scope.Add("phone");
        options.Scope.Add("groups");
        options.Scope.Add("address");
        options.Scope.Add("profile");
        options.Scope.Add("openid");
        options.Scope.Add("groups.read");
        options.Scope.Add("user.read");
        options.GetClaimsFromUserInfoEndpoint = true;

        options.Events = new Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectEvents
        {
          OnTicketReceived = async ctx =>
          {

            List<string> groups = ctx.Principal.FindAll("groups")
              .Select(c => c.Value)
              .Where(a => a.StartsWith("G", StringComparison.OrdinalIgnoreCase)) //Only adding AD Groups otherwise the token is too large.
              .ToList();
            ClaimsIdentity user = (ClaimsIdentity)ctx.HttpContext.User.Identity;
            foreach (string group in groups)
            {
              user.AddClaim(new Claim("group", group));
            }
            await Task.CompletedTask;
          }

        };
        options.TokenValidationParameters = new TokenValidationParameters { NameClaimType = "name", RoleClaimType = "role" };

      });
      oidcCache.Add("oidc-adfs");

    }

    services.AddOidcStateDataFormatterCache([.. oidcCache]);
  }
   
  private static void ConfigureApiVersioning(this IServiceCollection services)
  {
    // This gives us the ability to use default version numbering or override it if necessary.
    services.AddApiVersioning(option =>
    {
      option.DefaultApiVersion = new ApiVersion(1, 0);
      option.ReportApiVersions = true;
      option.AssumeDefaultVersionWhenUnspecified = true;
    });


  }

  static void IdentityServerBuilder(DbContextOptionsBuilder options) =>
    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
      o => o.MigrationsAssembly(typeof(HostingExtensions).Assembly.GetName().Name)
            .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
            .LogTo(Log.Verbose, LogLevel.Trace)
            .EnableSensitiveDataLogging(EnableSensitiveLogging);

  private const string en_US = "en-US";
  private const string fr_CA = "fr-CA";
  private const string es_MX = "es-MX";

  /// <summary>
  /// Configures the pipeline.
  /// </summary>
  /// <param name="app">The application.</param>
  /// <returns>Microsoft.AspNetCore.Builder.WebApplication.</returns>
  public static WebApplication ConfigurePipeline(this WebApplication app)
  {
    ArgumentNullException.ThrowIfNull(app);

    app.UseSerilogRequestLogging();

#if !DEBUG //Disable this for local testing as it breaks things.. :)
    app.UseMiddleware<PublicFacingUrlMiddleware>();
#endif

    if (app.Environment.EnvironmentName.StartsWith("dev", StringComparison.OrdinalIgnoreCase))
    {
      app.UseDeveloperExceptionPage();
    }
    else
    {
      app.UseExceptionHandler("/Home/Error");
      app.UseStatusCodePages();
      app.UseHsts();
    }

    app.UseRequestLocalization();
    app.MapDefaultControllerRoute();

    app.UseStaticFiles(new StaticFileOptions
    {

    });
    app.UseRouting();
    app.UseIdentityServer()
       .UseIdentityServerSamlPlugin()
       .UseIdentityServerWsFederationPlugin();


    app.UseAuthorization();
    app.UseSession();
    app.MapRazorPages()
       .RequireAuthorization();

    app.MapHealthChecks("/health", new HealthCheckOptions
    {
      Predicate = _ => true,
      AllowCachingResponses = false,
      ResultStatusCodes =
      {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
      }
    });

    app.Use(async (httpContext, next) =>
    {
      httpContext.Response.Headers[HeaderNames.CacheControl] = "no-cache, no-store, must-revalidate, max-age=0";
      httpContext.Response.Headers[HeaderNames.Expires] = "-1";
      await next();
    });
    app.InitializeDatabase();

    return app;
  }

  private static void ConfigureAuthrorization(this IServiceCollection services)
  {
    services.AddAuthorizationBuilder()
      .AddPolicy("admin_ui_webhooks", policy =>
      {
        policy.AddAuthenticationSchemes("Bearer");
        //policy.RequireScope("adminui_ui_webhooks");
      })
      .AddPolicy("mfa_admin", policy =>
      {
        policy.AddAuthenticationSchemes("Bearer");
        //policy.RequireScope("identity");
      })
      ;
  }
}
