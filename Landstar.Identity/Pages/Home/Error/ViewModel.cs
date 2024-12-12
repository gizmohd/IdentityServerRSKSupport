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

using Duende.IdentityServer.Models;

namespace Landstar.Identity.Pages.Home.Error
{
  /// <summary>
  /// Class ViewModel.
  /// </summary>
  public class ViewModel
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ViewModel"/> class.
    /// </summary>
    public ViewModel()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ViewModel"/> class.
    /// </summary>
    /// <param name="error">The error.</param>
    public ViewModel(string error)
    {
      Error = new ErrorMessage { Error = error };
    }

    /// <summary>
    /// Gets or sets the error.
    /// </summary>
    /// <value>The error.</value>
    public ErrorMessage Error { get; set; }
  }
}