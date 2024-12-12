// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 05-14-2024
//
// Last Modified By : 
// Last Modified On : 05-14-2024
// ***********************************************************************
// <copyright file="RegisterConfirmation.cshtml.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using IdentityExpress.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;



namespace Landstar.Identity.Pages.Account;

/// <summary>
/// Class RegisterConfirmationModel.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
[AllowAnonymous]
public class RegisterConfirmationModel(UserManager<IdentityExpressUser> userManager) : PageModel
{
  /// <summary>
  /// Gets or sets the email.
  /// </summary>
  /// <value>The email.</value>
  public string Email { get; set; }

  /// <summary>
  /// Gets or sets a value indicating whether [display confirm account link].
  /// </summary>
  /// <value><see langword="true" /> if [display confirm account link]; otherwise, <see langword="false" />.</value>
  public bool DisplayConfirmAccountLink { get; set; }

  /// <summary>
  /// Gets or sets the email confirmation URL.
  /// </summary>
  /// <value>The email confirmation URL.</value>
  public string EmailConfirmationUrl { get; set; }
  /// <summary>
  /// Gets or sets the redirect URI.
  /// </summary>
  /// <value>The redirect URI.</value>
  public string RedirectUri { get; set; }

  /// <summary>
  /// Called when [get asynchronous].
  /// </summary>
  /// <param name="email">The email.</param>
  /// <param name="returnUrl">The return URL.</param>
  /// <returns>Task&lt;IActionResult&gt;.</returns>
  /// <exception cref="System.ArgumentNullException"></exception>
  public Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
  {
    ArgumentNullException.ThrowIfNull(userManager);
    return InternalOnGetAsync(email, returnUrl);
  }


  /// <summary>
  /// Internal on get as an asynchronous operation.
  /// </summary>
  /// <param name="email">The email.</param>
  /// <param name="returnUrl">The return URL.</param>
  /// <returns>A Task&lt;IActionResult&gt; representing the asynchronous operation.</returns>
  async Task<IActionResult> InternalOnGetAsync(string email, string returnUrl = null)
  {
    if (email == null)
    {
      return RedirectToPage("/Index");
    }
    RedirectUri = returnUrl;
    //Temporary Measure since email addresses could get duplicated due to current lsol accounts...
    //Will need to revisit this to consolidate the accounts with dupliate email addresses.

    //var user = await UsrManager.FindByEmailAsync(email);
    var user = await userManager.FindByNameAsync(email).ConfigureAwait(false);

    if (user == null)
    {
      return NotFound($"Unable to load user with email '{email}'.");
    }

    Email = email;

    return Page();

  }
}
