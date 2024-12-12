// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-22-2024
//
// Last Modified By : 
// Last Modified On : 04-22-2024
// ***********************************************************************
// <copyright file="Index.cshtml.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Duende.IdentityServer.Events;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Services;
using IdentityExpress.Identity;
using IdentityModel;
using Landstar.Identity.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Distributed;

namespace Landstar.Identity.Pages.Account.Logout;

/// <summary>
/// Class Index.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
[SecurityHeaders]
[AllowAnonymous]
public class Index(SignInManager<IdentityExpressUser> signInManager, IIdentityServerInteractionService interaction,
                   IEventService events, IDistributedCache cache) : PageModel
{
  /// <summary>
  /// Gets or sets the logout identifier.
  /// </summary>
  /// <value>The logout identifier.</value>
  [BindProperty]
  public string LogoutId { get; set; }

  /// <summary>
  /// On get as an asynchronous operation.
  /// </summary>
  /// <param name="logoutId">The logout identifier.</param>
  /// <returns>A Task&lt;IActionResult&gt; representing the asynchronous operation.</returns>
  public async Task<IActionResult> OnGetAsync(string logoutId)
  {
    LogoutId = logoutId;
    string currentUserName = HttpContext?.User?.Identity?.Name;
    
    bool showLogoutPrompt = LogoutOptions.ShowLogoutPrompt;

    if (User.Identity?.IsAuthenticated != true)
    {
      // if the user is not authenticated, then just show logged out page
      showLogoutPrompt = false;
    }
    else
    {
      Duende.IdentityServer.Models.LogoutRequest context = await interaction.GetLogoutContextAsync(LogoutId);
      if (context?.ShowSignoutPrompt == false)
      {
        // it's safe to automatically sign-out
        showLogoutPrompt = false;
      }
    }

    if (!showLogoutPrompt)
    {
      // if the request for logout was properly authenticated from IdentityServer, then
      // we don't need to show the prompt and can just log the user out directly.
      return await OnPostAsync();
    }

    return Page();
  }

  /// <summary>
  /// On post as an asynchronous operation.
  /// </summary>
  /// <returns>A Task&lt;IActionResult&gt; representing the asynchronous operation.</returns>
  public async Task<IActionResult> OnPostAsync()
  {
    if (User.Identity?.IsAuthenticated == true)
    {
      // if there's no current logout context, we need to create one
      // this captures necessary info from the current logged in user
      // this can still return null if there is no context needed
      LogoutId ??= await interaction.CreateLogoutContextAsync();

      // delete local authentication cookie
      await signInManager.SignOutAsync();

      // see if we need to trigger federated logout
      string idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;

      // raise the logout event
      await events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName2()));
      Telemetry.Metrics.UserLogout(idp);

      // if it's a local login we can ignore this workflow
      if (idp != null && idp != Duende.IdentityServer.IdentityServerConstants.LocalIdentityProvider &&
          await HttpContext.GetSchemeSupportsSignOutAsync(idp))
      {
        // build a return URL so the upstream provider will redirect back
        // to us after the user has logged out. this allows us to then
        // complete our single sign-out processing.
        string url = Url.Page("/Account/Logout/Loggedout", new { logoutId = LogoutId });

        // this triggers a redirect to the external provider for sign-out
        return SignOut(new AuthenticationProperties { RedirectUri = url }, idp);
      }
    }

    return RedirectToPage("/Account/Logout/LoggedOut", new { logoutId = LogoutId });
  }
}
