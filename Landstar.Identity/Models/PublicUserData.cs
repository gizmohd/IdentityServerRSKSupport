// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-16-2024
//
// Last Modified By : 
// Last Modified On : 04-22-2024
// ***********************************************************************
// <copyright file="PublicUserData.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;

namespace Landstar.Identity.Models;


/// <summary>
/// Class PublicUserData.
/// Implements the <see cref="IEquatable{PublicUserData}" />
/// </summary>
/// <seealso cref="IEquatable{PublicUserData}" />
public record PublicUserData
{

  /// <summary>
  /// Gets or sets the name.
  /// </summary>
  /// <value>The name.</value>
  public string Name { get; set; }
  /// <summary>
  /// Gets the user identifier.
  /// </summary>
  /// <value>The user identifier.</value>
  public string UserId { get; init; }
  /// <summary>
  /// Gets the name of the user.
  /// </summary>
  /// <value>The name of the user.</value>
  public string UserName { get; init; }
  /// <summary>
  /// Gets the full name.
  /// </summary>
  /// <value>The full name.</value>
  public string FullName { get; init; }
  /// <summary>
  /// Gets or sets the email address.
  /// </summary>
  /// <value>The email address.</value>
  public string EmailAddress { get; set; }
  /// <summary>
  /// Gets the ls unique identifier.
  /// </summary>
  /// <value>The ls unique identifier.</value>
  public string LsUniqueId { get; init; }
  /// <summary>
  /// Gets or sets the injected roles.
  /// </summary>
  /// <value>The injected roles.</value>
  public IList<string> InjectedRoles { get; set; }
}
