// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 08-20-2024
//
// Last Modified By : 
// Last Modified On : 09-27-2024
// ***********************************************************************
// <copyright file="GenerateRecoveryCodes.cshtml.cs" company="Landstar.Identity">
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
/// Class GenerateRecoveryCodesModel.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
public class GenerateRecoveryCodesModel(
    UserManager<IdentityExpressUser> userManager,
    ILogger<GenerateRecoveryCodesModel> logger) : PageModel
{
  /// <summary>
  /// Gets or sets the recovery codes.
  /// </summary>
  /// <value>The recovery codes.</value>
  [TempData]
  public string[] RecoveryCodes { get; set; }

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
  /// <exception cref="System.InvalidOperationException">Cannot generate recovery codes for user with ID '{userId}' because they do not have 2FA enabled.</exception>
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

    var isTwoFactorEnabled = await userManager.GetTwoFactorEnabledAsync(user).ConfigureAwait(false);
    if (!isTwoFactorEnabled)
    {
      var userId = await userManager.GetUserIdAsync(user).ConfigureAwait(false);
      throw new InvalidOperationException($"Cannot generate recovery codes for user with ID '{userId}' because they do not have 2FA enabled.");
    }

    return Page();
  }

  /// <summary>
  /// On post as an asynchronous operation.
  /// </summary>
  /// <returns>A Task&lt;IActionResult&gt; representing the asynchronous operation.</returns>
  /// <exception cref="System.InvalidOperationException">Cannot generate recovery codes for user with ID '{userId}' as they do not have 2FA enabled.</exception>
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

    var isTwoFactorEnabled = await userManager.GetTwoFactorEnabledAsync(user).ConfigureAwait(false);
    var userId = await userManager.GetUserIdAsync(user).ConfigureAwait(false);
    if (!isTwoFactorEnabled)
    {
      throw new InvalidOperationException($"Cannot generate recovery codes for user with ID '{userId}' as they do not have 2FA enabled.");
    }

    var recoveryCodes = await userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10).ConfigureAwait(false);
    RecoveryCodes = recoveryCodes.ToArray();

    logger.LogInformation("User with ID '{UserId}' has generated new 2FA recovery codes.", userId);
    StatusMessage = "You have generated new recovery codes.";
    return RedirectToPage("./ShowRecoveryCodes");
  }
}