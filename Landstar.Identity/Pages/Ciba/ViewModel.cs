// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-22-2024
//
// Last Modified By : 
// Last Modified On : 04-22-2024
// ***********************************************************************
// <copyright file="ViewModel.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Landstar.Identity.Pages.Ciba;

/// <summary>
/// Class ViewModel.
/// </summary>
public class ViewModel
{
  /// <summary>
  /// Gets or sets the name of the client.
  /// </summary>
  /// <value>The name of the client.</value>
  public string ClientName { get; set; }
  /// <summary>
  /// Gets or sets the client URL.
  /// </summary>
  /// <value>The client URL.</value>
  public string ClientUrl { get; set; }
  /// <summary>
  /// Gets or sets the client logo URL.
  /// </summary>
  /// <value>The client logo URL.</value>
  public string ClientLogoUrl { get; set; }

  /// <summary>
  /// Gets or sets the binding message.
  /// </summary>
  /// <value>The binding message.</value>
  public string BindingMessage { get; set; }

  /// <summary>
  /// Gets or sets the identity scopes.
  /// </summary>
  /// <value>The identity scopes.</value>
  public IEnumerable<ScopeViewModel> IdentityScopes { get; set; } = [];
  /// <summary>
  /// Gets or sets the API scopes.
  /// </summary>
  /// <value>The API scopes.</value>
  public IEnumerable<ScopeViewModel> ApiScopes { get; set; } = [];
}
