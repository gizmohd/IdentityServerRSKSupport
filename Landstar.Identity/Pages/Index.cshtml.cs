// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-22-2024
//
// Last Modified By : 
// Last Modified On : 04-22-2024
// ***********************************************************************
// <copyright file="Index.cshtml.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Duende.IdentityServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;

namespace Landstar.Identity.Pages;

/// <summary>
/// Class Index.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
[AllowAnonymous]
public class Index(IdentityServerLicense license = null) : PageModel
{
  /// <summary>
  /// Gets the version.
  /// </summary>
  /// <value>The version.</value>
  public string Version
  {
    get => typeof(Duende.IdentityServer.Hosting.IdentityServerMiddleware).Assembly
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
        ?.InformationalVersion.Split('+')[0]
        ?? "unavailable";
  }
  /// <summary>
  /// Gets the license.
  /// </summary>
  /// <value>The license.</value>
  public IdentityServerLicense License { get; } = license;

  /// <summary>
  /// Called when [get].
  /// </summary>
  /// <returns>IActionResult.</returns>
  public IActionResult OnGet()
  {
    //if (configuration["ASPNETCORE_ENVIRONMENT"]?.StartsWith("Dev", StringComparison.OrdinalIgnoreCase) == true ||
    //   (configuration["ASPNETCORE_ENVIRONMENT"]?.StartsWith("Qa", StringComparison.OrdinalIgnoreCase) == true))
    //{
    //  return Redirect("diagnostics");
    //}
    return Redirect("/Account/Manage");
    //return new OkResult();
  }
}
