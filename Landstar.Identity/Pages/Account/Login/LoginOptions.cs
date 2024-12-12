// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-22-2024
//
// Last Modified By : 
// Last Modified On : 04-16-2024
// ***********************************************************************
// <copyright file="LoginOptions.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Landstar.Identity.Pages.Account.Login;

/// <summary>
/// Class LoginOptions.
/// </summary>
public static class LoginOptions
{
  /// <summary>
  /// The allow local login
  /// </summary>
  public static readonly bool AllowLocalLogin = true;
  /// <summary>
  /// The allow remember login
  /// </summary>
  public static readonly bool AllowRememberLogin = true;
  /// <summary>
  /// The remember me login duration
  /// </summary>
  public static readonly TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);
  /// <summary>
  /// The invalid credentials error message
  /// </summary>
  public static readonly string InvalidCredentialsErrorMessage = "Invalid username or password";
}