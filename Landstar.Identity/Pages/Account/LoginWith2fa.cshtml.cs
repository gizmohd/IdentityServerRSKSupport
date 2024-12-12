// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 09-25-2024
//
// Last Modified By : 
// Last Modified On : 09-25-2024
// ***********************************************************************
// <copyright file="LoginWith2fa.cshtml.cs" company="Landstar.Identity">
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
/// Class LoginWith2faModel.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
/// <remarks>
/// Initializes a new instance of the <see cref="LoginWith2FaModel"/> class.
/// </remarks>
/// <param name="signInManager">The sign in manager.</param>
/// <param name="logger">The logger.</param>
[AllowAnonymous]
public class LoginWith2FaModel(SignInManager<IdentityExpressUser> signInManager, ILogger<LoginWith2FaModel> logger) : PageModel
{

  /// <summary>
  /// Gets or sets the input.
  /// </summary>
  /// <value>The input.</value>
  [BindProperty]
  public InputModel Input { get; set; }

  /// <summary>
  /// Gets or sets a value indicating whether [remember me].
  /// </summary>
  /// <value><see langword="true" /> if [remember me]; otherwise, <see langword="false" />.</value>
  public bool RememberMe { get; set; }

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
    /// Gets or sets the two factor code.
    /// </summary>
    /// <value>The two factor code.</value>
    [Required]
    [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Text)]
    [Display(Name = "Authenticator code")]
    public string TwoFactorCode { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [remember machine].
    /// </summary>
    /// <value><see langword="true" /> if [remember machine]; otherwise, <see langword="false" />.</value>
    [Display(Name = "Remember this machine")]
    public bool RememberMachine { get; set; }
  }

  /// <summary>
  /// On get as an asynchronous operation.
  /// </summary>
  /// <param name="rememberMe">if set to <see langword="true" /> [remember me].</param>
  /// <param name="returnUrl">The return URL.</param>
  /// <returns>A Task&lt;IActionResult&gt; representing the asynchronous operation.</returns>
  /// <exception cref="System.InvalidOperationException">Unable to load two-factor authentication user.</exception>
  public async Task<IActionResult> OnGetAsync(bool rememberMe, string returnUrl = null)
  {
    // Ensure the user has gone through the username & password screen first
    var user = await signInManager.GetTwoFactorAuthenticationUserAsync().ConfigureAwait(false);

    if (user == null)
    {
      throw new InvalidOperationException($"Unable to load two-factor authentication user.");
    }

    ReturnUrl = returnUrl;
    RememberMe = rememberMe;

    return Page();
  }

  /// <summary>
  /// On post as an asynchronous operation.
  /// </summary>
  /// <param name="rememberMe">if set to <see langword="true" /> [remember me].</param>
  /// <param name="returnUrl">The return URL.</param>
  /// <returns>A Task&lt;IActionResult&gt; representing the asynchronous operation.</returns>
  /// <exception cref="System.InvalidOperationException">Unable to load two-factor authentication user.</exception>
  public async Task<IActionResult> OnPostAsync(bool rememberMe, string returnUrl = null)
  {
    if (!ModelState.IsValid)
    {
      return Page();
    }

    returnUrl ??= Url.Content("~/");

    var user = await signInManager.GetTwoFactorAuthenticationUserAsync().ConfigureAwait(false) ?? throw new InvalidOperationException($"Unable to load two-factor authentication user.");
   
    var authenticatorCode = Input.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

    var result = await signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, Input.RememberMachine).ConfigureAwait(false);

    if (result.Succeeded)
    {
      logger.LogInformation("User with ID '{UserId}' logged in with 2fa.", user.Id);
      return Redirect(returnUrl);
    }

    if (result.IsLockedOut)
    {
      logger.LogWarning("User with ID '{UserId}' account locked out.", user.Id);
      return RedirectToPage("./Lockout");
    }

    logger.LogWarning("Invalid authenticator code entered for user with ID '{UserId}'.", user.Id);
    ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
    return Page();

  }
}


