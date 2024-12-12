// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 05-14-2024
//
// Last Modified By : 
// Last Modified On : 05-14-2024
// ***********************************************************************
// <copyright file="ConfirmEmailChange.cshtml.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using IdentityExpress.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Landstar.Identity.Pages.Account;

/// <summary>
/// Class ConfirmEmailChangeModel.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
[AllowAnonymous]
public class ConfirmEmailChangeModel(UserManager<IdentityExpressUser> userManager, SignInManager<IdentityExpressUser> signInManager) : PageModel
{
  /// <summary>
  /// Gets or sets the status message.
  /// </summary>
  /// <value>The status message.</value>
  [TempData]
  public string StatusMessage { get; set; }
  /// <exclude />
  public string RedirectUri { get; set; }
  /// <summary>
  /// Called when [get asynchronous].
  /// </summary>
  /// <param name="userId">The user identifier.</param>
  /// <param name="email">The email.</param>
  /// <param name="code">The code.</param>
  /// <param name="redirectUrl">The redirect URL.</param>
  /// <returns>Task&lt;IActionResult&gt;.</returns>
  /// <exception cref="System.ArgumentNullException"></exception>
  public Task<IActionResult> OnGetAsync(string userId, string email, string code, string redirectUrl = null)
  {
    ArgumentNullException.ThrowIfNull(userManager);
    return InternalOnGetAsync(userId, email, code, redirectUrl);
  }
  /// <summary>
  /// Internal on get as an asynchronous operation.
  /// </summary>
  /// <param name="userId">The user identifier.</param>
  /// <param name="email">The email.</param>
  /// <param name="code">The code.</param>
  /// <param name="redirectUrl">The redirect URL.</param>
  /// <returns>A Task&lt;IActionResult&gt; representing the asynchronous operation.</returns>
  async Task<IActionResult> InternalOnGetAsync(string userId, string email, string code, string redirectUrl = null)
  {
    RedirectUri = redirectUrl;
    if (userId == null || email == null || code == null)
    {
      if (redirectUrl == null)
      {
        return RedirectToPage("/Index");
      }
      return LocalRedirect(redirectUrl);

    }

    var user = await userManager.FindByIdAsync(userId).ConfigureAwait(false);
    if (user == null)
    {
      return NotFound($"Unable to load user with ID '{userId}'.");
    }

    code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
    var result = await userManager.ChangeEmailAsync(user, email, code).ConfigureAwait(false);
    if (!result.Succeeded)
    {
      StatusMessage = "Error changing email.";
      return Page();
    }

    // In our UI email and user name are one and the same, so when we update the email
    // we need to update the user name.
    var setUserNameResult = await userManager.SetUserNameAsync(user, email).ConfigureAwait(false);
    if (!setUserNameResult.Succeeded)
    {
      StatusMessage = "Error changing user name.";
      return Page();
    }

    await signInManager.RefreshSignInAsync(user).ConfigureAwait(false);
    StatusMessage = "Thank you for confirming your email change.";
    return Page();
  }
}
