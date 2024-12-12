// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-22-2024
//
// Last Modified By : 
// Last Modified On : 04-16-2024
// ***********************************************************************
// <copyright file="Index.cshtml.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Landstar.Identity.Pages.Redirect;

/// <summary>
/// Class IndexModel.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
[AllowAnonymous]
public class IndexModel : PageModel
{
  /// <summary>
  /// Gets or sets the redirect URI.
  /// </summary>
  /// <value>The redirect URI.</value>
  public string RedirectUri { get; set; }

  /// <summary>
  /// Called when [get].
  /// </summary>
  /// <param name="redirectUri">The redirect URI.</param>
  /// <returns>IActionResult.</returns>
  public IActionResult OnGet(string redirectUri)
  {
    if (!Url.IsLocalUrl(redirectUri))
    {
      return RedirectToPage("/Home/Error/Index");
    }

    RedirectUri = redirectUri;
    return Page();
  }
}
