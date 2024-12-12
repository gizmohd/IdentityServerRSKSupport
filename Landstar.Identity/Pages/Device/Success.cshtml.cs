// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-22-2024
//
// Last Modified By : 
// Last Modified On : 04-23-2024
// ***********************************************************************
// <copyright file="Success.cshtml.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Landstar.Identity.Pages.Device;

/// <summary>
/// Class SuccessModel.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
[SecurityHeaders]
[Authorize]
public class SuccessModel : PageModel
{
  /// <summary>
  /// Called when [get].
  /// </summary>
  public void OnGet()
  {
  //Empty Getter
  }
}
