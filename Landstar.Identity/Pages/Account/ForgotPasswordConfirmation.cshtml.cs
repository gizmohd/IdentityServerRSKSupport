// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-22-2024
//
// Last Modified By : 
// Last Modified On : 04-19-2024
// ***********************************************************************
// <copyright file="ForgotPasswordConfirmation.cshtml.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Landstar.Identity.Pages.Account;

/// <summary>
/// Class ForgotPasswordConfirmation.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
[AllowAnonymous]
public class ForgotPasswordConfirmation : PageModel
{
  /// <summary>
  /// Called when [get].
  /// </summary>
  public void OnGet()
  {
    //Default Getter
  }
}
