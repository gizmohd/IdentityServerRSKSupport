// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-22-2024
//
// Last Modified By : 
// Last Modified On : 04-16-2024
// ***********************************************************************
// <copyright file="ViewModel.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using System.Text;
using System.Text.Json;

namespace Landstar.Identity.Pages.Diagnostics;

/// <summary>
/// Class ViewModel.
/// </summary>
public class ViewModel
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ViewModel"/> class.
  /// </summary>
  /// <param name="result">The result.</param>
  public ViewModel(AuthenticateResult result)
  {
    AuthenticateResult = result;

    if (result?.Properties?.Items.TryGetValue("client_list", out var encoded) == true && encoded != null)
    {
      var bytes = Base64Url.Decode(encoded);
      var value = Encoding.UTF8.GetString(bytes);
      Clients = JsonSerializer.Deserialize<string[]>(value) ?? Enumerable.Empty<string>();
      return;
    }
    Clients = [];
  }

  /// <summary>
  /// Gets the authenticate result.
  /// </summary>
  /// <value>The authenticate result.</value>
  public AuthenticateResult AuthenticateResult { get; }
  /// <summary>
  /// Gets the clients.
  /// </summary>
  /// <value>The clients.</value>
  public IEnumerable<string> Clients { get; }
}