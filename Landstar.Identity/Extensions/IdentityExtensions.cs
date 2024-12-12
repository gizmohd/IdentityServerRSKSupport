// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-19-2024
//
// Last Modified By : 
// Last Modified On : 04-19-2024
// ***********************************************************************
// <copyright file="IdentityExtensions.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Security.Claims;
using Flurl;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Duende.IdentityServer.Extensions;
using IdentityModel;


namespace Landstar.Identity.Extensions;



/// <summary>
/// Class IdentityExtensions.
/// </summary>
public static class IdentityExtensions
{

  /// <summary>
  /// Checks if the redirect URI is for a native client.
  /// </summary>
  /// <param name="context">The context.</param>
  /// <returns>bool.</returns>
  public static bool IsNativeClient(this AuthorizationRequest context)
  {
    return !context.RedirectUri.StartsWith("https", StringComparison.Ordinal)
       && !context.RedirectUri.StartsWith("http", StringComparison.Ordinal);
  }
  /// <summary>
  /// Determines whether the specified value is present.
  /// </summary>
  /// <param name="value">The value.</param>
  /// <returns>bool.</returns>
  public static bool IsPresent(this string value)
  {
    return !string.IsNullOrWhiteSpace(value);
  }

  /// <summary>
  /// Gets the name.
  /// </summary>
  /// <param name="principal">The principal.</param>
  /// <returns></returns>
  public static string GetDisplayName2(this ClaimsPrincipal principal) {
    string loginId = principal.GetDisplayName();
    if (loginId.Contains('/') || loginId.Contains('\\'))
    {      
      loginId = principal.Claims.FirstOrDefault(a => a.Type.Equals(JwtClaimTypes.PreferredUserName))?.Value;
    }
    return loginId;
  }
  /// <summary>
  /// Converts to spaceseparatedstring.
  /// </summary>
  /// <param name="input">The input.</param>
  /// <returns>string.</returns>
  public static string ToSpaceSeparatedString(this IEnumerable<string> input)
  {
    return string.Join(' ', input);
  }
  /// <summary>
  /// Converts to dictionary.
  /// </summary>
  /// <param name="claims">The claims.</param>
  /// <returns>System.Collections.Generic.Dictionary&lt;string, object&gt;.</returns>
  public static Dictionary<string, object> ToDictionary(this IList<Claim> claims)
  {
    Dictionary<string, object> result = [];
    var typesGroups = claims.GroupBy(a => a.Type);

    foreach (var t in typesGroups)
    {
      var keyClaims = t.Select(a => a.Value);
      result.Add(t.Key, keyClaims.Count() == 1 ? keyClaims.First() : keyClaims);
    }

    return result;
  }


  /// <summary>
  /// Loadings the page.
  /// </summary>
  /// <param name="page">The page.</param>
  /// <param name="redirectUri">The redirect URI.</param>
  /// <returns>Microsoft.AspNetCore.Mvc.IActionResult.</returns>
  public static IActionResult LoadingPage(this PageModel page, string redirectUri)
  {
    page.HttpContext.Response.StatusCode = 200;
    page.HttpContext.Response.Headers.Location = "";

    return new RedirectResult("~/redirect".SetQueryParam("redirectUri", redirectUri));
  }

  /// <summary>
  /// Converts to guid.
  /// </summary>
  /// <param name="value">The value.</param>
  /// <param name="prefix">The prefix.</param>
  /// <returns>System.Guid.</returns>
  public static Guid ToGuid(this int value, int prefix = 0)
  {
    byte[] bytes = new byte[16];
    BitConverter.GetBytes(value + ((long)prefix * 1000000000)).CopyTo(bytes, 0);
    return new Guid(bytes);
  }

  /// <summary>
  /// Converts to claim.
  /// </summary>
  /// <param name="claim">The claim.</param>
  /// <returns>System.Security.Claims.Claim.</returns>
  public static Claim ToClaim(this Duende.IdentityServer.Models.ClientClaim claim)
  {
    return new Claim(claim.Type, claim.Value, claim.ValueType);
  }


  /// <summary>
  /// Gets the siteminder public user overrides.
  /// </summary>
  /// <param name="configuration">The configuration.</param>
  /// <returns>System.Collections.Generic.IList&lt;Landstar.Identity.Models.PublicUserData&gt;.</returns>
  public static IList<Models.PublicUserData> GetSiteminderPublicUserOverrides(this IConfiguration configuration)
  {
    return configuration.GetValue<List<Models.PublicUserData>>("Authentication:PublicUser") ?? [];

  }
}
