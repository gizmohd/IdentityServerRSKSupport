﻿// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-23-2024
//
// Last Modified By : 
// Last Modified On : 04-23-2024
// ***********************************************************************
// <copyright file="ScopeViewModel.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Landstar.Identity.Pages.Consent;

/// <summary>
/// Class ScopeViewModel.
/// </summary>
public class ScopeViewModel
{
  /// <summary>
  /// Gets or sets the name.
  /// </summary>
  /// <value>The name.</value>
  public string Name { get; set; }
  /// <summary>
  /// Gets or sets the value.
  /// </summary>
  /// <value>The value.</value>
  public string Value { get; set; }
  /// <summary>
  /// Gets or sets the display name.
  /// </summary>
  /// <value>The display name.</value>
  public string DisplayName { get; set; }
  /// <summary>
  /// Gets or sets the description.
  /// </summary>
  /// <value>The description.</value>
  public string Description { get; set; }
  /// <summary>
  /// Gets or sets the emphasize.
  /// </summary>
  /// <value>The emphasize.</value>
  public bool Emphasize { get; set; }
  /// <summary>
  /// Gets or sets the required.
  /// </summary>
  /// <value>The required.</value>
  public bool Required { get; set; }
  /// <summary>
  /// Gets or sets the checked.
  /// </summary>
  /// <value>The checked.</value>
  public bool Checked { get; set; }
  /// <summary>
  /// Gets or sets the resources.
  /// </summary>
  /// <value>The resources.</value>
  public IEnumerable<ResourceViewModel> Resources { get; set; } = [];
}