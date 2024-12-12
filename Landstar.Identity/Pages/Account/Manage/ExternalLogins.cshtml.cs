// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 08-20-2024
//
// Last Modified By : 
// Last Modified On : 09-27-2024
// ***********************************************************************
// <copyright file="ExternalLogins.cshtml.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using IdentityExpress.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Landstar.Identity.Pages.Account.Manage
{
  /// <summary>
  /// Class ExternalLoginsModel.
  /// Implements the <see cref="PageModel" />
  /// </summary>
  /// <seealso cref="PageModel" />
  public class ExternalLoginsModel(
      UserManager<IdentityExpressUser> userManager,
      IUserStore<IdentityExpressUser> userStore,
      SignInManager<IdentityExpressUser> signInManager) : PageModel
  {
    /// <summary>
    /// Gets or sets the current logins.
    /// </summary>
    /// <value>The current logins.</value>
    public IList<UserLoginInfo> CurrentLogins { get; set; }

    /// <summary>
    /// Gets or sets the other logins.
    /// </summary>
    /// <value>The other logins.</value>
    public IList<AuthenticationScheme> OtherLogins { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [show remove button].
    /// </summary>
    /// <value><see langword="true" /> if [show remove button]; otherwise, <see langword="false" />.</value>
    public bool ShowRemoveButton { get; set; }

    /// <summary>
    /// Gets or sets the status message.
    /// </summary>
    /// <value>The status message.</value>
    [TempData]
    public string StatusMessage { get; set; }

    /// <summary>
    /// Get current user as an asynchronous operation.
    /// </summary>
    /// <returns>A Task&lt;IdentityExpressUser&gt; representing the asynchronous operation.</returns>
    async Task<IdentityExpressUser> GetCurrentUserAsync() {
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

      user ??= await userManager.FindByEmailAsync(User.Identity.Name).ConfigureAwait(false);
      return user;
    }

    /// <summary>
    /// On get as an asynchronous operation.
    /// </summary>
    /// <returns>A Task&lt;IActionResult&gt; representing the asynchronous operation.</returns>
    public async Task<IActionResult> OnGetAsync()
    {
      IdentityExpressUser user = await GetCurrentUserAsync();
      if (user == null)
      {
        return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
      }

      CurrentLogins = await userManager.GetLoginsAsync(user).ConfigureAwait(false);
      OtherLogins = (await signInManager.GetExternalAuthenticationSchemesAsync().ConfigureAwait(false))
          .Where(auth => CurrentLogins.All(ul => !auth.Name.Equals(ul.LoginProvider)))
          .ToList();
      
      
      string passwordHash = null;
      if (userStore is IUserPasswordStore<IdentityExpressUser> userPasswordStore)
      {
        passwordHash = await userPasswordStore.GetPasswordHashAsync(user, HttpContext.RequestAborted);
      }
      
      ShowRemoveButton = passwordHash != null || CurrentLogins.Count > 1;
      
      return Page();
    }

    /// <summary>
    /// On post remove login as an asynchronous operation.
    /// </summary>
    /// <param name="loginProvider">The login provider.</param>
    /// <param name="providerKey">The provider key.</param>
    /// <returns>A Task&lt;IActionResult&gt; representing the asynchronous operation.</returns>
    public async Task<IActionResult> OnPostRemoveLoginAsync(string loginProvider, string providerKey)
    {
      IdentityExpressUser user = await GetCurrentUserAsync();
      if (user == null)
      {
        return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
      }

      var result = await userManager.RemoveLoginAsync(user, loginProvider, providerKey).ConfigureAwait(false);
      if (!result.Succeeded)
      {
        StatusMessage = "The external login was not removed.";
        return RedirectToPage();
      }

      await signInManager.RefreshSignInAsync(user).ConfigureAwait(false);
      StatusMessage = "The external login was removed.";
      return RedirectToPage();
    }

    /// <summary>
    /// On post link login as an asynchronous operation.
    /// </summary>
    /// <param name="provider">The provider.</param>
    /// <returns>A Task&lt;IActionResult&gt; representing the asynchronous operation.</returns>
    public async Task<IActionResult> OnPostLinkLoginAsync(string provider)
    {
      // Clear the existing external cookie to ensure a clean login process
      await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme).ConfigureAwait(false);

      // Request a redirect to the external login provider to link a login for the current user
      var redirectUrl = Url.Page("./ExternalLogins", pageHandler: "LinkLoginCallback");
      var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, userManager.GetUserId(User));
      return new ChallengeResult(provider, properties);
    }

    /// <summary>
    /// On get link login callback as an asynchronous operation.
    /// </summary>
    /// <returns>A Task&lt;IActionResult&gt; representing the asynchronous operation.</returns>
    /// <exception cref="System.InvalidOperationException">Unexpected error occurred loading external login info for user with ID " + user.Id</exception>
    public async Task<IActionResult> OnGetLinkLoginCallbackAsync()
    {
      IdentityExpressUser user = await GetCurrentUserAsync();
      if (user == null)
      {
        return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
      }
      
      var userId = await userManager.GetUserIdAsync(user);
      var info = await signInManager.GetExternalLoginInfoAsync(userId) ?? throw new InvalidOperationException("Unexpected error occurred loading external login info for user with ID " + user.Id);
      var result = await userManager.AddLoginAsync(user, info).ConfigureAwait(false);
      if (!result.Succeeded)
      {
        StatusMessage = "The external login was not added. External logins can only be associated with one account.";
        return RedirectToPage();
      }

      // Clear the existing external cookie to ensure a clean login process
      await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme).ConfigureAwait(false);

      StatusMessage = "The external login was added.";
      return RedirectToPage();
    }
  }
}
