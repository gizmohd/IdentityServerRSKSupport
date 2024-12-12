// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-22-2024
//
// Last Modified By : 
// Last Modified On : 04-16-2024
// ***********************************************************************
// <copyright file="PhoneUpdatesInputModel.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;

namespace Landstar.Identity.Pages.Account.Login;

/// <summary>
/// Class PhoneUpdatesInputModel.
/// </summary>
public class InputModel
{
  /// <summary>
  /// Gets or sets the username.
  /// </summary>
  /// <value>The username.</value>
  [Required]
  public string Username { get; set; }
  /// <summary>
  /// Gets or sets the password.
  /// </summary>
  /// <value>The password.</value>
  [Required]
  public string Password { get; set; }
  /// <summary>
  /// Gets or sets a value indicating whether [remember login].
  /// </summary>
  /// <value><see langword="true" /> if [remember login]; otherwise, <see langword="false" />.</value>
  public bool RememberLogin { get; set; }
  /// <summary>
  /// Gets or sets the return URL.
  /// </summary>
  /// <value>The return URL.</value>
  public string ReturnUrl { get; set; }
  /// <summary>
  /// Gets or sets the button.
  /// </summary>
  /// <value>The button.</value>
  public string Button { get; set; }
  /// <summary>
  /// Gets or sets a value indicating whether [enable registration link].
  /// </summary>
  /// <value><see langword="true" /> if [enable registration link]; otherwise, <see langword="false" />.</value>
  public bool EnableRegistrationLink { get; set; }
}