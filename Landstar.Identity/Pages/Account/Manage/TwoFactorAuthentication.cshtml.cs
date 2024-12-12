// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 08-20-2024
//
// Last Modified By : 
// Last Modified On : 09-27-2024
// ***********************************************************************
// <copyright file="TwoFactorAuthentication.cshtml.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using IdentityExpress.Identity;
using Landstar.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Landstar.Identity.Pages.Account.Manage;

/// <summary>
/// Class TwoFactorAuthenticationModel.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
public class TwoFactorAuthenticationModel(
    UserManager<IdentityExpressUser> userManager,
    ApplicationDbContext dbContext,
    SignInManager<IdentityExpressUser> signInManager
    ) : PageModel
{
  /// <summary>
  /// Gets or sets a value indicating whether this instance has authenticator.
  /// </summary>
  /// <value><see langword="true" /> if this instance has authenticator; otherwise, <see langword="false" />.</value>
  public bool HasAuthenticator { get; set; }

  /// <summary>
  /// Gets or sets the recovery codes left.
  /// </summary>
  /// <value>The recovery codes left.</value>
  public int RecoveryCodesLeft { get; set; }

  /// <summary>
  /// Gets or sets a value indicating whether [is2fa enabled].
  /// </summary>
  /// <value><see langword="true" /> if [is2fa enabled]; otherwise, <see langword="false" />.</value>
  [BindProperty]
  public bool Is2FaEnabled { get; set; }

  /// <summary>
  /// Gets or sets a value indicating whether this instance is machine remembered.
  /// </summary>
  /// <value><see langword="true" /> if this instance is machine remembered; otherwise, <see langword="false" />.</value>
  public bool IsMachineRemembered { get; set; }
  /// <summary>
  /// Gets or sets a value indicating whether this instance has phone configured.
  /// </summary>
  /// <value><see langword="true" /> if this instance has phone configured; otherwise, <see langword="false" />.</value>
  public bool HasPhoneConfigured { get; set; }
  /// <summary>
  /// Gets or sets a value indicating whether [prefer mfa over SMS].
  /// </summary>
  /// <value><see langword="true" /> if [prefer mfa over SMS]; otherwise, <see langword="false" />.</value>
  [BindProperty]
  public bool PreferMfaOverSms { get; set; }

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

    HasAuthenticator = await userManager.GetAuthenticatorKeyAsync(user).ConfigureAwait(false) != null;
    Is2FaEnabled = await userManager.GetTwoFactorEnabledAsync(user).ConfigureAwait(false);
    IsMachineRemembered = await signInManager.IsTwoFactorClientRememberedAsync(user).ConfigureAwait(false);
    RecoveryCodesLeft = await userManager.CountRecoveryCodesAsync(user).ConfigureAwait(false);
    HasPhoneConfigured = user.PhoneNumberConfirmed;
    PreferMfaOverSms = dbContext.CheckIfUserPrefersSmsOverTotp(user.Id);
    return Page();
  }

  /// <summary>
  /// On post as an asynchronous operation.
  /// </summary>
  /// <returns>A Task&lt;IActionResult&gt; representing the asynchronous operation.</returns>
  public async Task<IActionResult> OnPostAsync([FromQuery]string handler)
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
    if (handler.Equals("forget", StringComparison.OrdinalIgnoreCase))
    {
      await signInManager.ForgetTwoFactorClientAsync().ConfigureAwait(false);
      StatusMessage = "The current browser has been forgotten. When you login again from this browser you will be prompted for your 2fa code.";

    }
    else if (handler.Equals("update", StringComparison.OrdinalIgnoreCase))
    {
      
      await dbContext.UpdateUserSmsPreferenceAsync(user.Id, PreferMfaOverSms);
      StatusMessage = "Your Multi Factor SMS preferences have been updated....";
    }
      return RedirectToPage();
  }
  

  }