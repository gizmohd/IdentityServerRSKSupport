// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-22-2024
//
// Last Modified By : 
// Last Modified On : 04-16-2024
// ***********************************************************************
// <copyright file="LoggedOutViewModel.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

namespace Landstar.Identity.Pages.Account.Logout;

/// <summary>
/// Class LoggedOutViewModel.
/// </summary>
public class LoggedOutViewModel
{
  /// <summary>
  /// Gets or sets the post logout redirect URI.
  /// </summary>
  /// <value>The post logout redirect URI.</value>
  public string PostLogoutRedirectUri { get; set; }
  /// <summary>
  /// Gets or sets the name of the client.
  /// </summary>
  /// <value>The name of the client.</value>
  public string ClientName { get; set; }
  /// <summary>
  /// Gets or sets the sign out iframe URL.
  /// </summary>
  /// <value>The sign out iframe URL.</value>
  public string SignOutIframeUrl { get; set; }
  /// <summary>
  /// Gets or sets a value indicating whether [automatic redirect after sign out].
  /// </summary>
  /// <value><see langword="true" /> if [automatic redirect after sign out]; otherwise, <see langword="false" />.</value>
  public bool AutomaticRedirectAfterSignOut { get; set; }
}
