// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 08-20-2024
//
// Last Modified By : 
// Last Modified On : 09-27-2024
// ***********************************************************************
// <copyright file="ResetAuthenticator.cshtml.cs" company="Landstar.Identity">
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
/// Class ResetAuthenticatorModel.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
public class ResetAuthenticatorModel(
    UserManager<IdentityExpressUser> userManager,
    SignInManager<IdentityExpressUser> signInManager,
    ILogger<ResetAuthenticatorModel> logger) : PageModel
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

    return Page();
  }

  /// <summary>
  /// On post as an asynchronous operation.
  /// </summary>
  /// <returns>A Task&lt;IActionResult&gt; representing the asynchronous operation.</returns>
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

    await userManager.SetTwoFactorEnabledAsync(user, false).ConfigureAwait(false);
    await userManager.ResetAuthenticatorKeyAsync(user).ConfigureAwait(false);
    logger.LogInformation("User with ID '{UserId}' has reset their authentication app key.", user.Id);

    await signInManager.RefreshSignInAsync(user).ConfigureAwait(false);
    StatusMessage = "Your authenticator app key has been reset, you will need to configure your authenticator app using the new key.";

    return RedirectToPage("./EnableAuthenticator");
  }
}