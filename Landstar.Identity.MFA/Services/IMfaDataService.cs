// ***********************************************************************
// Assembly         : Landstar.Identity.Mfa
// Author           : 
// Created          : 09-20-2024
//
// Last Modified By : 
// Last Modified On : 09-20-2024
// ***********************************************************************
// <copyright file="MfaDataService.cs" company="Landstar.Identity.Mfa">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************


using Landstar.Identity.Mfa.Models;

namespace Landstar.Identity.Mfa.Services;

/// <summary>
/// Interface IMfaDataService
/// </summary>
public interface IMfaDataService
{
  /// <summary>
  /// Creates the asynchronous.
  /// </summary>
  /// <param name="MfaData">The Mfa data.</param>
  /// <returns>Task&lt;System.Int32&gt;.</returns>
  Task<int> CreateAsync(MfaData MfaData);
  /// <summary>
  /// Deletes the asynchronous.
  /// </summary>
  /// <param name="id">The identifier (either the Integer Id, or the UserId)</param>
  /// <returns>Task&lt;System.Boolean&gt;.</returns>
  Task<bool> DeleteAsync(string id);
  /// <summary>
  /// Gets all asynchronous.
  /// </summary>
  /// <returns>Task&lt;IEnumerable&lt;MfaData&gt;&gt;.</returns>
  Task<IList<MfaData>> GetAllAsync( ) ;
  /// <summary>
  /// Gets the by identifier asynchronous.
  /// </summary>
  /// <param name="id">The identifier.</param>
  /// <returns>Task&lt;MfaData&gt;.</returns>
  Task<MfaData> GetByIdAsync(int id);
  /// <summary>
  /// Gets the by user name asynchronous.
  /// </summary>
  /// <param name="userId">The user identifier.</param>
  /// <param name="includeStarNumbers">The include star numbers.</param>
  /// <returns>System.Threading.Tasks.Task&lt;System.Collections.Generic.IList&lt;Landstar.Identity.Mfa.Models.MfaData&gt;&gt;.</returns>
  Task<IList<MfaData>> GetByUserNameAsync(string userId, bool includeStarNumbers = false);
  /// <summary>
  /// Updates the asynchronous.
  /// </summary>
  /// <param name="MfaData">The Mfa data.</param>
  /// <returns>Task&lt;System.Boolean&gt;.</returns>
  Task<bool> UpdateAsync(MfaData MfaData);

  /// <summary>
  /// Validates if provided phone number is a mobile phone.
  /// </summary>
  /// <param name="phoneNumber">The phone number.</param>
  /// <returns>Task&lt;System.Boolean&gt;.</returns>
  Task<bool> ValidateIsMobilePhoneAsync(string phoneNumber);
}
