// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 08-20-2024
//
// Last Modified By : 
// Last Modified On : 08-20-2024
// ***********************************************************************
// <copyright file="ShowRecoveryCodes.cshtml.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Landstar.Identity.Pages.Account.Manage;

/// <summary>
/// Class ShowRecoveryCodesModel.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
public class ShowRecoveryCodesModel : PageModel
{
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
  /// Called when [get].
  /// </summary>
  /// <returns>IActionResult.</returns>
  public IActionResult OnGet()
  {
    if (RecoveryCodes == null || RecoveryCodes.Length == 0)
    {
      return RedirectToPage("./TwoFactorAuthentication");
    }

    return Page();
  }
}
