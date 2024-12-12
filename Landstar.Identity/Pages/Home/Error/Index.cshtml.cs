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

using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Landstar.Identity.Pages.Home.Error;

/// <summary>
/// Class Index.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
[AllowAnonymous]
[SecurityHeaders]
public class Index(IIdentityServerInteractionService interaction, IConfiguration configuration) : PageModel
{
  /// <summary>
  /// Gets or sets the view.
  /// </summary>
  /// <value>The view.</value>
  public ViewModel View { get; set; } = new();

  /// <summary>
  /// Called when [get].
  /// </summary>
  /// <param name="errorId">The error identifier.</param>
  public async Task OnGetAsync(string errorId)
  {
    // retrieve error details from identityserver
    var message = await interaction.GetErrorContextAsync(errorId);
    if (message != null)
    {
      View.Error = message;

      if ((configuration["ASPNETCORE_ENVIRONMENT"] ?? "prod").StartsWith("prod", StringComparison.OrdinalIgnoreCase))
      {
        // only show in development
        message.ErrorDescription = null;
      }
    }
  }
}
