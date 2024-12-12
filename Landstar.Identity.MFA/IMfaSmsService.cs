// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 08-20-2024
//
// Last Modified By : 
// Last Modified On : 08-20-2024
// ***********************************************************************
// <copyright file="IMfaSmsService.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Landstar.Identity.Mfa;

/// <summary>
/// Interface IMfaSmsService
/// </summary>
public interface IMfaSmsService
{
  /// <summary>
  /// Generates the and store Mfa code asynchronous.
  /// </summary>
  /// <param name="userId">The user identifier.</param>
  /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
  /// <returns>Task&lt;System.String&gt;.</returns>
  Task<string> GenerateAndStoreMfaCodeAsync(string userId, CancellationToken cancellationToken);
  /// <summary>
  /// Generates the validation token.
  /// </summary>
  /// <param name="userId">The user identifier.</param>
  /// <returns>System.String.</returns>
  string GenerateValidationToken(string userId);
  /// <summary>
  /// Validates the token.
  /// </summary>
  /// <param name="userName">Name of the user.</param>
  /// <param name="token">The token.</param>
  /// <returns><see langword="true" /> if XXXX, <see langword="false" /> otherwise.</returns>
  bool ValidateToken(string userName, string token);
  /// <summary>
  /// Determines whether [is Mfa code valid asynchronous] [the specified user identifier].
  /// </summary>
  /// <param name="userId">The user identifier.</param>
  /// <param name="code">The code.</param>
  /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
  /// <returns>Task&lt;System.Boolean&gt;.</returns>
  Task<bool> IsMfaCodeValidAsync(string userId, string code, CancellationToken cancellationToken);

  /// <summary>
  /// Sends the SMS asynchronous.
  /// </summary>
  /// <param name="phoneNumber">The phone number.</param>
  /// <param name="message">The message.</param>
  /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
  /// <returns>Task&lt;System.Boolean&gt;.</returns>
  Task<bool> SendSmsAsync(string phoneNumber, string message, CancellationToken cancellationToken);

}