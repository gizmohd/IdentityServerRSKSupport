// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-22-2024
//
// Last Modified By : 
// Last Modified On : 04-22-2024
// ***********************************************************************
// <copyright file="LoggedOut.cshtml.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Duende.IdentityServer.Services;
using Flurl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.OutputCaching;

namespace Landstar.Identity.Pages.Account.Logout;

/// <summary>
/// Class LoggedOut.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
[SecurityHeaders]
[AllowAnonymous]
public class LoggedOut(IIdentityServerInteractionService interactionService, IConfiguration configuration) : PageModel
{
  /// <summary>
  /// Gets or sets the view.
  /// </summary>
  /// <value>The view.</value>
  public LoggedOutViewModel View { get; set; } = default!;

  /// <summary>
  /// Called when [get].
  /// </summary>
  /// <param name="logoutId">The logout identifier.</param>
  [OutputCache(Duration = 0, NoStore = true)]
  public async Task OnGetAsync(string logoutId)
  {
    // get context information (client name, post logout redirect URI and iframe for federated signout)
    var logout = await interactionService.GetLogoutContextAsync(logoutId);

    View = new LoggedOutViewModel
    {
      AutomaticRedirectAfterSignOut = LogoutOptions.AutomaticRedirectAfterSignOut,

      PostLogoutRedirectUri = logout?.PostLogoutRedirectUri ?? configuration["Authentication:SiteMinder:LandstarLoggedOutUrl"].SetQueryParam("redirect_uri", configuration["IssuerUri"]),
      //PostLogoutRedirectUri =  configuration["Authentication:SiteMinder:LandstarLoggedOutUrl"],
      ClientName = logout?.ClientName ?? logout?.ClientId,

      SignOutIframeUrl = logout?.SignOutIFrameUrl ?? configuration["Authentication:SiteMinder:LandstarLoggedOutUrl"].SetQueryParam("redirect_uri", configuration["IssuerUri"])
      //SignOutIframeUrl = logout?.SignOutIFrameUrl ?? configuration["Authentication:SiteMinder:LandstarLoggedOutUrl"]
    };
  }
}
