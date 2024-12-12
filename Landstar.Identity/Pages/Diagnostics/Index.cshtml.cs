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

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages; 

namespace Landstar.Identity.Pages.Diagnostics;

/// <summary>
/// Class Index.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
[SecurityHeaders]
[Authorize]
public class Index(IConfiguration configuration) : PageModel
{
  /// <summary>
  /// Gets or sets the view.
  /// </summary>
  /// <value>The view.</value>
  public ViewModel View { get; set; } = default!;

  /// <summary>
  /// On get as an asynchronous operation.
  /// </summary>
  /// <returns>A Task&lt;IActionResult&gt; representing the asynchronous operation.</returns>
  public async Task<IActionResult> OnGetAsync()
  {

    _ = bool.TryParse(configuration["Diagnostics:LocalOnly"], out var localOnly);

    if (localOnly)
    {
      return NotFound();
    }

    View = new ViewModel(await HttpContext.AuthenticateAsync());

    return Page();
  }
}
