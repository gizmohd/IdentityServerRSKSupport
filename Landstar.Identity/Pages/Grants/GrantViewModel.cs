// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-23-2024
//
// Last Modified By : 
// Last Modified On : 04-23-2024
// ***********************************************************************
// <copyright file="GrantViewModel.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Landstar.Identity.Pages.Grants;

/// <summary>
/// Class GrantViewModel.
/// </summary>
public class GrantViewModel
{
  /// <summary>
  /// Gets or sets the client identifier.
  /// </summary>
  /// <value>The client identifier.</value>
  public string ClientId { get; set; }
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
  /// Gets or sets the description.
  /// </summary>
  /// <value>The description.</value>
  public string Description { get; set; }
  /// <summary>
  /// Gets or sets the created.
  /// </summary>
  /// <value>The created.</value>
  public DateTime Created { get; set; }
  /// <summary>
  /// Gets or sets the expires.
  /// </summary>
  /// <value>The expires.</value>
  public DateTime? Expires { get; set; }
  /// <summary>
  /// Gets or sets the identity grant names.
  /// </summary>
  /// <value>The identity grant names.</value>
  public IEnumerable<string> IdentityGrantNames { get; set; } = [];
  /// <summary>
  /// Gets or sets the API grant names.
  /// </summary>
  /// <value>The API grant names.</value>
  public IEnumerable<string> ApiGrantNames { get; set; } = [];
}
