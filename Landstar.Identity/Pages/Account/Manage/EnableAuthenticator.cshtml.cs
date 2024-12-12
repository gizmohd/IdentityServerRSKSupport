// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 08-20-2024
//
// Last Modified By : 
// Last Modified On : 09-27-2024
// ***********************************************************************
// <copyright file="EnableAuthenticator.cshtml.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using IdentityExpress.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;

namespace Landstar.Identity.Pages.Account.Manage;


/// <summary>
/// Class EnableAuthenticatorModel.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
public class EnableAuthenticatorModel(
        UserManager<IdentityExpressUser> userManager,
        ILogger<EnableAuthenticatorModel> logger,
        IConfiguration configuration,
        UrlEncoder urlEncoder) : PageModel
{
#pragma warning disable S1075 // URIs should not be hardcoded
  /// <summary>
  /// The authenticator URI format
  /// </summary>
  private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

#pragma warning restore S1075 // URIs should not be hardcoded

  /// <summary>
  /// Gets or sets the shared key.
  /// </summary>
  /// <value>The shared key.</value>
  public string SharedKey { get; set; }

  /// <summary>
  /// Gets or sets the authenticator URI.
  /// </summary>
  /// <value>The authenticator URI.</value>
  public string AuthenticatorUri { get; set; }

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
  /// Gets or sets the input.
  /// </summary>
  /// <value>The input.</value>
  [BindProperty]
  public InputModel Input { get; set; }

  /// <summary>
  /// Class InputModel.
  /// </summary>
  public class InputModel
  {
    /// <summary>
    /// Gets or sets the code.
    /// </summary>
    /// <value>The code.</value>
    [Required]
    [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Text)]
    [Display(Name = "Verification Code")]
    public string Code { get; set; }
  }

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

    await LoadSharedKeyAndQrCodeUriAsync(user).ConfigureAwait(false);

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

    if (!ModelState.IsValid)
    {
      await LoadSharedKeyAndQrCodeUriAsync(user).ConfigureAwait(false);
      return Page();
    }

    // Strip spaces and hypens
    var verificationCode = Input.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

    var is2faTokenValid = await userManager.VerifyTwoFactorTokenAsync(
        user, userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode).ConfigureAwait(false);

    if (!is2faTokenValid)
    {
      ModelState.AddModelError("Input.Code", "Verification code is invalid.");
      await LoadSharedKeyAndQrCodeUriAsync(user).ConfigureAwait(false);
      return Page();
    }

    await userManager.SetTwoFactorEnabledAsync(user, true).ConfigureAwait(false);
    var userId = await userManager.GetUserIdAsync(user).ConfigureAwait(false);
    logger.LogInformation("User with ID '{UserId}' has enabled 2FA with an authenticator app.", userId);

    StatusMessage = "Your authenticator app has been verified.";

    if (await userManager.CountRecoveryCodesAsync(user).ConfigureAwait(false) == 0)
    {
      var recoveryCodes = await userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10).ConfigureAwait(false);
      RecoveryCodes = recoveryCodes!.ToArray();
      return RedirectToPage("./ShowRecoveryCodes");
    }
  
    return RedirectToPage("./TwoFactorAuthentication");
  }

  /// <summary>
  /// Load shared key and qr code URI as an asynchronous operation.
  /// </summary>
  /// <param name="user">The user.</param>
  /// <returns>A Task representing the asynchronous operation.</returns>
  private async Task LoadSharedKeyAndQrCodeUriAsync(IdentityExpressUser user)
  {
    // Load the authenticator key & QR code URI to display on the form
    var unformattedKey = await userManager.GetAuthenticatorKeyAsync(user).ConfigureAwait(false);
    if (string.IsNullOrEmpty(unformattedKey))
    {
      await userManager.ResetAuthenticatorKeyAsync(user).ConfigureAwait(false);
      unformattedKey = await userManager.GetAuthenticatorKeyAsync(user).ConfigureAwait(false);
    }

    SharedKey = FormatKey(unformattedKey);

    var email = await userManager.GetEmailAsync(user).ConfigureAwait(false);
    AuthenticatorUri = GenerateQrCodeUri(email, unformattedKey);
  }

  /// <summary>
  /// Formats the key.
  /// </summary>
  /// <param name="unformattedKey">The unformatted key.</param>
  /// <returns>System.String.</returns>
  private static string FormatKey(string unformattedKey)
  {
    var result = new StringBuilder();
    int currentPosition = 0;

    while (currentPosition + 4 < unformattedKey.Length)
    {
      result.Append(unformattedKey, currentPosition, 4).Append(' ');
      currentPosition += 4;
    }

    if (currentPosition < unformattedKey.Length)
    {
      result.Append(unformattedKey[currentPosition..]);
    }

    return result.ToString().ToLowerInvariant();
  }

  /// <summary>
  /// Generates the qr code URI.
  /// </summary>
  /// <param name="email">The email.</param>
  /// <param name="unformattedKey">The unformatted key.</param>
  /// <returns>System.String.</returns>
  private string GenerateQrCodeUri(string email, string unformattedKey)
  {
    var siteName = "Landstar";
    var env = configuration["ASPNETCORE_ENVIRONMENT"];
    
    if (env?.StartsWith("PROD", System.StringComparison.InvariantCultureIgnoreCase) == false)
    {
      siteName = $"{siteName} {env}".Trim();
    }

    //"otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
    return string.Format(
        AuthenticatorUriFormat,
        urlEncoder.Encode(siteName),
        urlEncoder.Encode(email),
        unformattedKey);
  }
}
