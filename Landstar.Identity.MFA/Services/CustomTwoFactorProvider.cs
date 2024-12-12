// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 09-20-2024
//
// Last Modified By : 
// Last Modified On : 09-20-2024
// ***********************************************************************
// <copyright file="Class.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using IdentityExpress.Identity;
using Microsoft.AspNetCore.Identity;

namespace Landstar.Identity.Mfa.Services;

/// <summary>
/// Class CustomTwoFactorProvider.
/// Implements the <see cref="IUserTwoFactorTokenProvider&lt;IdentityExpressUser&gt;" />
/// </summary>
/// <seealso cref="IUserTwoFactorTokenProvider&lt;IdentityExpressUser&gt;" />
public class CustomTwoFactorProvider(IMfaSmsService MfaSmsService, IMfaDataService mfaDataService) : IUserTwoFactorTokenProvider<IdentityExpressUser>
{
  /// <inheritdoc />
  public async Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<IdentityExpressUser> manager, IdentityExpressUser user)
  {
    bool hasUserName = !string.IsNullOrWhiteSpace(user?.UserName);
    if (!hasUserName)
    {
      return false;
    }

    return (await mfaDataService.GetByUserNameAsync(user!.UserName)).Any(a => !string.IsNullOrWhiteSpace(a.PhoneNumber));
    
    
  }

  /// <inheritdoc />
  public Task<string> GenerateAsync(string purpose, UserManager<IdentityExpressUser> manager, IdentityExpressUser user)
  {
    ArgumentNullException.ThrowIfNull(user);
    ArgumentNullException.ThrowIfNull(user.UserName);

    return MfaSmsService.GenerateAndStoreMfaCodeAsync(user.UserName, default);
  }

  /// <inheritdoc />
  public async Task<bool> ValidateAsync(string purpose, string token, UserManager<IdentityExpressUser> manager, IdentityExpressUser user)
  {
    if (string.IsNullOrWhiteSpace(user?.UserName))
    {
      return false;
    }
    return await MfaSmsService.IsMfaCodeValidAsync(user.UserName, token, cancellationToken: default);

  }
}
