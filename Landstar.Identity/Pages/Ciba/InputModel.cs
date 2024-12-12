// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-22-2024
//
// Last Modified By : 
// Last Modified On : 04-22-2024
// ***********************************************************************
// <copyright file="PhoneUpdatesInputModel.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Landstar.Identity.Pages.Ciba;

/// <summary>
/// Class PhoneUpdatesInputModel.
/// </summary>
public class InputModel
{
  /// <summary>
  /// Gets or sets the button.
  /// </summary>
  /// <value>The button.</value>
  public string Button { get; set; }
  /// <summary>
  /// Gets or sets the scopes consented.
  /// </summary>
  /// <value>The scopes consented.</value>
  public IEnumerable<string> ScopesConsented { get; set; } = [];
  /// <summary>
  /// Gets or sets the identifier.
  /// </summary>
  /// <value>The identifier.</value>
  public string Id { get; set; }
  /// <summary>
  /// Gets or sets the description.
  /// </summary>
  /// <value>The description.</value>
  public string Description { get; set; }
}