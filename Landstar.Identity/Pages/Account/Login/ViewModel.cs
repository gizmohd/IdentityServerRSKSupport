// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-22-2024
//
// Last Modified By : 
// Last Modified On : 04-19-2024
// ***********************************************************************
// <copyright file="ViewModel.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Landstar.Identity.Pages.Account.Login;

/// <summary>
/// Class ViewModel.
/// </summary>
public class ViewModel
{
  /// <summary>
  /// Gets or sets the allow remember login.
  /// </summary>
  /// <value>The allow remember login.</value>
  public bool AllowRememberLogin { get; set; } = true;

  /// <summary>
  /// Gets or sets a value indicating whether [allow forgot password].
  /// </summary>
  /// <value><see langword="true" /> if [allow forgot password]; otherwise, <see langword="false" />.</value>
  public bool AllowForgotPassword { get; set; } = true;
  /// <summary>
  /// Gets or sets the enable local login.
  /// </summary>
  /// <value>The enable local login.</value>
  public bool EnableLocalLogin { get; set; } = true;

  /// <summary>
  /// Gets or sets the external providers.
  /// </summary>
  /// <value>The external providers.</value>
  public IEnumerable<ExternalProvider> ExternalProviders { get; set; } = [];
  /// <summary>
  /// Gets the visible external providers.
  /// </summary>
  /// <value>The visible external providers.</value>
  public IEnumerable<ExternalProvider> VisibleExternalProviders => ExternalProviders.Where(x => !string.IsNullOrWhiteSpace(x.DisplayName));

  /// <summary>
  /// Gets the is external login only.
  /// </summary>
  /// <value>The is external login only.</value>
  public bool IsExternalLoginOnly => !EnableLocalLogin && ExternalProviders?.Count() == 1;
  /// <summary>
  /// Gets the external login scheme.
  /// </summary>
  /// <value>The external login scheme.</value>
  public string ExternalLoginScheme => IsExternalLoginOnly ? ExternalProviders?.SingleOrDefault()?.AuthenticationScheme : null;


}
/// <summary>
/// Class ExternalProvider.
/// </summary>
public class ExternalProvider(string authenticationScheme, string displayName = null)
{
  /// <summary>
  /// Gets or sets the display name.
  /// </summary>
  /// <value>The display name.</value>
  public string DisplayName { get; set; } = displayName;
  /// <summary>
  /// Gets or sets the authentication scheme.
  /// </summary>
  /// <value>The authentication scheme.</value>
  public string AuthenticationScheme { get; set; } = authenticationScheme;
}