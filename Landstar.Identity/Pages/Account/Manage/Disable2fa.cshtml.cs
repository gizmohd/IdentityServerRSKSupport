// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 09-25-2024
//
// Last Modified By : 
// Last Modified On : 09-25-2024
// ***********************************************************************
// <copyright file="Disable2fa.cshtml.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using IdentityExpress.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Landstar.Identity.Pages.Account.Manage;

/// <summary>
/// Class Disable2FaModel.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
public class Disable2FaModel(
    UserManager<IdentityExpressUser> userManager,
    ILogger<Disable2FaModel> logger) : PageModel
{
  /// <summary>
  /// Gets or sets the status message.
  /// </summary>
  /// <value>The status message.</value>
  [TempData]
  public string StatusMessage { get; set; }

  /// <summary>
  /// On get as an asynchronous operation.
  /// </summary>
  /// <returns>A Task&lt;IActionResult&gt; representing the asynchronous operation.</returns>
  /// <exception cref="System.InvalidOperationException">Cannot disable 2FA for user with ID '{userManager.GetUserId(User)}' as it's not currently enabled.</exception>
  public async Task<IActionResult> OnGetAsync()
  {
    IdentityExpressUser user;
    try
    {
      user = await userManager.GetUserAsync(User).ConfigureAwait(false);
    }
    catch
    {
      //Some lsol users dont have proper guid...
      user = await userManager.FindByNameAsync(User.Identity.Name).ConfigureAwait(false);
    }


    if (user == null)
    {
      user = await userManager.FindByEmailAsync(User.Identity.Name).ConfigureAwait(false);
      if (user == null)
      {
        return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
      }
    }

    if (!await userManager.GetTwoFactorEnabledAsync(user).ConfigureAwait(false))
    {
      throw new InvalidOperationException($"Cannot disable 2FA for user with ID '{userManager.GetUserId(User)}' as it's not currently enabled.");
    }

    return Page();
  }

  /// <summary>
  /// On post as an asynchronous operation.
  /// </summary>
  /// <returns>A Task&lt;IActionResult&gt; representing the asynchronous operation.</returns>
  /// <exception cref="System.InvalidOperationException">Unexpected error occurred disabling 2FA for user with ID '{userManager.GetUserId(User)}'.</exception>
  public async Task<IActionResult> OnPostAsync()
  {
    IdentityExpressUser user;
    try
    {
      user = await userManager.GetUserAsync(User).ConfigureAwait(false);
    }
    catch
    {
      //Some lsol users dont have proper guid...
      user = await userManager.FindByNameAsync(User.Identity.Name).ConfigureAwait(false);
    }


    if (user == null)
    {
      user = await userManager.FindByEmailAsync(User.Identity.Name).ConfigureAwait(false);
      if (user == null)
      {
        return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
      }
    }

    var disable2faResult = await userManager.SetTwoFactorEnabledAsync(user, false).ConfigureAwait(false);
    if (!disable2faResult.Succeeded)
    {
      throw new InvalidOperationException($"Unexpected error occurred disabling 2FA for user with ID '{userManager.GetUserId(User)}'.");
    }

    logger.LogInformation("User with ID '{UserId}' has disabled 2fa.", userManager.GetUserId(User));
    StatusMessage = "2fa has been disabled. You can reenable 2fa when you setup an authenticator app";
    return RedirectToPage("./TwoFactorAuthentication");
  }
}