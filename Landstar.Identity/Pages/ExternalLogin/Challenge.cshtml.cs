// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-22-2024
//
// Last Modified By : 
// Last Modified On : 04-22-2024
// ***********************************************************************
// <copyright file="Challenge.cshtml.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Landstar.Identity.Pages.ExternalLogin;

/// <summary>
/// Class Challenge.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
[AllowAnonymous]
[SecurityHeaders]
public class Challenge(IIdentityServerInteractionService interactionService) : PageModel
{
  /// <summary>
  /// Called when [get].
  /// </summary>
  /// <param name="scheme">The scheme.</param>
  /// <param name="returnUrl">The return URL.</param>
  /// <returns>IActionResult.</returns>
  /// <exception cref="System.ArgumentException">invalid return URL</exception>
  public IActionResult OnGet(string scheme, string returnUrl)
  {
    if (string.IsNullOrEmpty(returnUrl))
    {
      returnUrl = "~/";
    }

    // validate returnUrl - either it is a valid OIDC URL or back to a local page
    if (!Url.IsLocalUrl(returnUrl) && !interactionService.IsValidReturnUrl(returnUrl))
    {
      // user might have clicked on a malicious link - should be logged
      throw new ArgumentException("invalid return URL");
    }

    // start challenge and roundtrip the return URL and scheme 
    var props = new AuthenticationProperties
    {
      RedirectUri = Url.Page("/externallogin/callback"),

      Items =
            {
                { "returnUrl", returnUrl },
                { "scheme", scheme },
            }
    };

    return Challenge(props, scheme);
  }
}
