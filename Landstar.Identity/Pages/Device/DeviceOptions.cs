// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-22-2024
//
// Last Modified By : 
// Last Modified On : 04-16-2024
// ***********************************************************************
// <copyright file="DeviceOptions.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Landstar.Identity.Pages.Device;

/// <summary>
/// Class DeviceOptions.
/// </summary>
public static class DeviceOptions
{
  /// <summary>
  /// The enable offline access
  /// </summary>
  public static readonly bool EnableOfflineAccess = true;
  /// <summary>
  /// The offline access display name
  /// </summary>
  public static readonly string OfflineAccessDisplayName = "Offline Access";
  /// <summary>
  /// The offline access description
  /// </summary>
  public static readonly string OfflineAccessDescription = "Access to your applications and resources, even when you are offline";

  /// <summary>
  /// The invalid user code
  /// </summary>
  public static readonly string InvalidUserCode = "Invalid user code";
  /// <summary>
  /// The must choose one error message
  /// </summary>
  public static readonly string MustChooseOneErrorMessage = "You must pick at least one permission";
  /// <summary>
  /// The invalid selection error message
  /// </summary>
  public static readonly string InvalidSelectionErrorMessage = "Invalid selection";
}