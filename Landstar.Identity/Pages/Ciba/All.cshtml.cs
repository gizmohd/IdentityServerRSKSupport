// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-22-2024
//
// Last Modified By : 
// Last Modified On : 04-22-2024
// ***********************************************************************
// <copyright file="All.cshtml.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Landstar.Identity.Pages.Ciba;

/// <summary>
/// Class AllModel.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
/// <remarks>
/// Initializes a new instance of the <see cref="AllModel"/> class.
/// </remarks>
/// <param name="backchannelAuthenticationInteractionService">The backchannel authentication interaction service.</param>
[SecurityHeaders]
[Authorize]
public class AllModel(IBackchannelAuthenticationInteractionService backchannelAuthenticationInteractionService) : PageModel
{
  /// <summary>
  /// Gets or sets the logins.
  /// </summary>
  /// <value>The logins.</value>
  public IEnumerable<BackchannelUserLoginRequest> Logins { get; set; } = default!;

  /// <summary>
  /// On get as an asynchronous operation.
  /// </summary>
  /// <returns>A Task representing the asynchronous operation.</returns>
  public async Task OnGetAsync()
  {
    Logins = await backchannelAuthenticationInteractionService.GetPendingLoginRequestsForCurrentUserAsync();
  }
}