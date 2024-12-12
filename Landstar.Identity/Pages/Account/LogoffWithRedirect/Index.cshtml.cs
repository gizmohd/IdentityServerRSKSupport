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

using Flurl;
using IdentityExpress.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Distributed;

namespace Landstar.Identity.Pages.Account.LogoffWithRedirect;

/// <summary>
/// Class Index.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
[SecurityHeaders]
[AllowAnonymous]
public class Index(IConfiguration _config, ILogger<Index> _logger, SignInManager<IdentityExpressUser> _signInManager,
                   IDistributedCache cache) : PageModel
{

  /// <summary>
  /// On get as an asynchronous operation.
  /// </summary>
  /// <param name="post_logout_redirect_uri">The post logout redirect URI.</param>
  /// <returns>A Task&lt;Microsoft.AspNetCore.Mvc.IActionResult&gt; representing the asynchronous operation.</returns>
  public async Task<IActionResult> OnGetAsync(string post_logout_redirect_uri = null)
  {
    _logger.LogDebug("LogoffWithRedirect: {Query}", HttpContext.Request.Query);
    
    var currentUserName = HttpContext?.User?.Identity?.Name;
  
    Dictionary<string, string> queryParams = [];

    HttpContext.Request.Query.Keys.ToList().ForEach(key =>
    {
      queryParams[key] = HttpContext.Request.Query[key];
    });

    await _signInManager.SignOutAsync().ConfigureAwait(false);
    await HttpContext.SignOutAsync().ConfigureAwait(false);
    // Clear the existing external cookie to ensure a clean login process
    await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme).ConfigureAwait(false);
    // Clear the existing external cookie to ensure a clean login process
    await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme).ConfigureAwait(false);

    foreach (var cookie in Request.Cookies.Keys)
    {
      Response.Cookies.Delete(cookie);
    }

    var newUrl2 = post_logout_redirect_uri?.SetQueryParams(queryParams);
    var newUrl = _config["Authentication:SiteMinder:LandstarLoggedOutUrl"].SetQueryParam("redirect_uri", _config["IssuerUri"]);
    return Redirect(newUrl2 ?? newUrl);
  }
   
}
