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

using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Landstar.Identity.Pages.Ciba;


/// <summary>
/// Class IndexModel.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
/// <remarks>
/// Initializes a new instance of the <see cref="IndexModel"/> class.
/// </remarks>
/// <param name="backchannelAuthenticationInteractionService">The backchannel authentication interaction service.</param>
/// <param name="logger">The logger.</param>
[AllowAnonymous]
[SecurityHeaders]
public class IndexModel(IBackchannelAuthenticationInteractionService backchannelAuthenticationInteractionService, ILogger<IndexModel> logger) : PageModel
{
  /// <summary>
  /// Gets or sets the login request.
  /// </summary>
  /// <value>The login request.</value>
  public BackchannelUserLoginRequest LoginRequest { get; set; } = default!;

  /// <summary>
  /// On get as an asynchronous operation.
  /// </summary>
  /// <param name="id">The identifier.</param>
  /// <returns>A Task&lt;IActionResult&gt; representing the asynchronous operation.</returns>
  public Task<IActionResult> OnGetAsync(string id)
  {
    ArgumentNullException.ThrowIfNull(id);
    return InternalOnGetAsync(id);
  }

  async Task<IActionResult> InternalOnGetAsync(string id)
  {
    BackchannelUserLoginRequest result = await backchannelAuthenticationInteractionService.GetLoginRequestByInternalIdAsync(id);
    if (result == null)
    {
      logger.InvalidBackchannelLoginId(id);
      return RedirectToPage("/Home/Error/Index");
    }

    LoginRequest = result;


    return Page();
  }
}