// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 09-24-2024
//
// Last Modified By : 
// Last Modified On : 09-24-2024
// ***********************************************************************
// <copyright file="LoginWithRecoveryCode.cshtml.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using IdentityExpress.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;


namespace Landstar.Identity.Pages.Account;

/// <summary>
/// Class LoginWithRecoveryCodeModel.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
/// <remarks>
/// Initializes a new instance of the <see cref="LoginWithRecoveryCodeModel"/> class.
/// </remarks>
/// <param name="signInManager">The sign in manager.</param>
/// <param name="logger">The logger.</param>
[AllowAnonymous]
public class LoginWithRecoveryCodeModel(SignInManager<IdentityExpressUser> signInManager, ILogger<LoginWithRecoveryCodeModel> logger) : PageModel
{

  /// <summary>
  /// Gets or sets the input.
  /// </summary>
  /// <value>The input.</value>
  [BindProperty]
  public InputModel Input { get; set; }

  /// <summary>
  /// Gets or sets the return URL.
  /// </summary>
  /// <value>The return URL.</value>
  public string ReturnUrl { get; set; }

  /// <summary>
  /// Class InputModel.
  /// </summary>
  public class InputModel
  {
    /// <summary>
    /// Gets or sets the recovery code.
    /// </summary>
    /// <value>The recovery code.</value>
    [BindProperty]
    [Required]
    [DataType(DataType.Text)]
    [Display(Name = "Recovery Code")]
    public string RecoveryCode { get; set; }
  }

  /// <summary>
  /// On get as an asynchronous operation.
  /// </summary>
  /// <param name="returnUrl">The return URL.</param>
  /// <returns>A Task&lt;IActionResult&gt; representing the asynchronous operation.</returns>
  /// <exception cref="System.InvalidOperationException">Unable to load two-factor authentication user.</exception>
  public async Task<IActionResult> OnGetAsync(string returnUrl = null)
  {
    // Ensure the user has gone through the username & password screen first
    _ = await signInManager.GetTwoFactorAuthenticationUserAsync().ConfigureAwait(false) ?? throw new InvalidOperationException($"Unable to load two-factor authentication user.");
    ReturnUrl = returnUrl;

    return Page();
  }

  /// <summary>
  /// On post as an asynchronous operation.
  /// </summary>
  /// <param name="returnUrl">The return URL.</param>
  /// <returns>A Task&lt;IActionResult&gt; representing the asynchronous operation.</returns>
  /// <exception cref="System.InvalidOperationException">Unable to load two-factor authentication user.</exception>
  public async Task<IActionResult> OnPostAsync(string returnUrl = null)
  {
    if (!ModelState.IsValid)
    {
      return Page();
    }

    var user = await signInManager.GetTwoFactorAuthenticationUserAsync().ConfigureAwait(false);
    if (user == null)
    {
      throw new InvalidOperationException($"Unable to load two-factor authentication user.");
    }

    var recoveryCode = Input.RecoveryCode.Replace(" ", string.Empty);

    var result = await signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode).ConfigureAwait(false);

    if (result.Succeeded)
    {
      logger.LogInformation("User with ID '{UserId}' logged in with a recovery code.", user.Id);
      return Redirect(returnUrl ?? Url.Content("~/"));
    }

    if (result.IsLockedOut)
    {
      logger.LogWarning("User with ID '{UserId}' account locked out.", user.Id);
      return RedirectToPage("./Lockout");
    }

    logger.LogWarning("Invalid recovery code entered for user with ID '{UserId}' ", user.Id);
    ModelState.AddModelError(string.Empty, "Invalid recovery code entered.");
    return Page();

  }
}
