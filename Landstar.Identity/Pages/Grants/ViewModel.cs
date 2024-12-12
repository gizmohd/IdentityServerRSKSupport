// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-22-2024
//
// Last Modified By : 
// Last Modified On : 04-23-2024
// ***********************************************************************
// <copyright file="ViewModel.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Landstar.Identity.Pages.Grants;

/// <summary>
/// Class ViewModel.
/// </summary>
public class ViewModel
{
  /// <summary>
  /// Gets or sets the grants.
  /// </summary>
  /// <value>The grants.</value>
  public IEnumerable<GrantViewModel> Grants { get; set; } = [];
}
