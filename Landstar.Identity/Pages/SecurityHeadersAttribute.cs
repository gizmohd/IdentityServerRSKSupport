// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-22-2024
//
// Last Modified By : 
// Last Modified On : 04-16-2024
// ***********************************************************************
// <copyright file="SecurityHeadersAttribute.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Landstar.Identity.Pages;

/// <summary>
/// Class SecurityHeadersAttribute. This class cannot be inherited.
/// Implements the <see cref="ActionFilterAttribute" />
/// </summary>
/// <seealso cref="ActionFilterAttribute" />
[AttributeUsage(AttributeTargets.Class)]
public sealed class SecurityHeadersAttribute : ActionFilterAttribute
{

  /// <summary>
  /// Called when [result executing].
  /// </summary>
  /// <param name="context">The context.</param>
  /// <exception cref="System.ArgumentNullException">context</exception>
  /// <inheritdoc />
  public override void OnResultExecuting(ResultExecutingContext context)
  {
    _ = bool.TryParse(Landstar.Identity.ConfigurationExtensions.Configuration["SecHeadersEnabled"], out bool enabled);
    if (enabled) //This seems to be breaking current ui when enabled... need to look into this more when there is time..
    {
      //Disable for now to test something
      ArgumentNullException.ThrowIfNull(context);

      Microsoft.AspNetCore.Mvc.IActionResult result = context.Result;
      if (result is PageResult)
      {
        // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Content-Type-Options
        if (!context.HttpContext.Response.Headers.ContainsKey("X-Content-Type-Options"))
        {
          context.HttpContext.Response.Headers.Append("X-Content-Type-Options", "nosniff");
        }

        // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Frame-Options
        if (!context.HttpContext.Response.Headers.ContainsKey("X-Frame-Options"))
        {
          context.HttpContext.Response.Headers.Append("X-Frame-Options", "DENY");
        }

        // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy
        string csp = "default-src 'self'; object-src 'none'; frame-ancestors 'none'; sandbox allow-forms allow-same-origin allow-scripts; base-uri 'self';";
        // also consider adding upgrade-insecure-requests once you have HTTPS in place for production
        //csp += "upgrade-insecure-requests;";
        // also an example if you need client images to be displayed from twitter
        // csp += "img-src 'self' https://pbs.twimg.com;";
        csp += "script-src 'self' https://unpkg.com https://code.jquery.com https://cdn.jsdelivr.net; https://kendo.cdn.telerik.com";
        csp += "style-src 'self' https://code.jquery.com https://kendo.cdn.telerik.com https://cdn.jsdelivr.net; ";
        csp += "img-src 'self' https://code.jquery.com https://kendo.cdn.telerik.com https://cdn.jsdelivr.net; ";
        csp += "font-src 'self' https://code.jquery.com https://kendo.cdn.telerik.com https://cdn.jsdelivr.net;";
        // once for standards compliant browsers
        if (!context.HttpContext.Response.Headers.ContainsKey("Content-Security-Policy"))
        {
          context.HttpContext.Response.Headers.Append("Content-Security-Policy", csp);
        }
        // and once again for IE
        if (!context.HttpContext.Response.Headers.ContainsKey("X-Content-Security-Policy"))
        {
          context.HttpContext.Response.Headers.Append("X-Content-Security-Policy", csp);
        }

        // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Referrer-Policy
        const string referrer_policy = "no-referrer";
        if (!context.HttpContext.Response.Headers.ContainsKey("Referrer-Policy"))
        {
          context.HttpContext.Response.Headers.Append("Referrer-Policy", referrer_policy);
        }
      }
    }
  }
}