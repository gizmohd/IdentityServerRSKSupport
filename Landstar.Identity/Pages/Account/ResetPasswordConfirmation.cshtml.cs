// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 05-14-2024
//
// Last Modified By : 
// Last Modified On : 05-14-2024
// ***********************************************************************
// <copyright file="ResetPasswordConfirmation.cshtml.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

 
namespace Landstar.Identity.Pages.Account;

/// <summary>
/// Class ResetPasswordConfirmationModel.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
[AllowAnonymous]
public class ResetPasswordConfirmationModel : PageModel
{
  /// <summary>
  /// Gets or sets the redirect URI.
  /// </summary>
  /// <value>The redirect URI.</value>
  public string RedirectUri { get; set; }
  /// <summary>
  /// Called when [get].
  /// </summary>
  /// <param name="redirectUrl">The redirect URL.</param>
  public void OnGet(string redirectUrl)
  {
    RedirectUri = redirectUrl;
  }
}
