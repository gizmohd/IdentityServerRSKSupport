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

namespace Landstar.Identity.Pages.Consent;

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
  /// Gets or sets the remember consent.
  /// </summary>
  /// <value>The remember consent.</value>
  public bool RememberConsent { get; set; } = true;
  /// <summary>
  /// Gets or sets the return URL.
  /// </summary>
  /// <value>The return URL.</value>
  public string ReturnUrl { get; set; }
  /// <summary>
  /// Gets or sets the description.
  /// </summary>
  /// <value>The description.</value>
  public string Description { get; set; }
}