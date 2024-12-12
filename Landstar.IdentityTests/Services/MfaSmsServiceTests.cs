// ***********************************************************************
// Assembly         : Landstar.Identity.Tests
// Author           : 
// Created          : 09-27-2024
//
// Last Modified By : 
// Last Modified On : 09-27-2024
// ***********************************************************************
// <copyright file="MfaSmsServiceTests.cs" company="Landstar.Identity.Tests">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Duende.IdentityServer.Models;
using IdentityExpress.Identity;
using Landstar.Identity.Mfa.Services;
using Landstar.Identity.Services;
using Landstar.Providers.CommunicationsHub;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using Xunit;

namespace Landstar.Identity.Tests.Services;

/// <summary>
/// Class MfaSmsServiceTests.
/// </summary>
public class MfaSmsServiceTests
{
  /// <summary>
  /// The SMS provider
  /// </summary>
  private readonly ISmsProvider _smsProvider;
  /// <summary>
  /// The configuration
  /// </summary>
  private readonly IConfiguration _configuration;
  /// <summary>
  /// The mfa data service
  /// </summary>
  private readonly IMfaDataService _mfaDataService;
  /// <summary>
  /// The cache
  /// </summary>
  private readonly IDistributedCache _cache;
  /// <summary>
  /// The user manager
  /// </summary>
  private readonly UserManager<IdentityExpressUser> _userManager;
  /// <summary>
  /// The mfa SMS service
  /// </summary>
  private readonly MfaSmsService _mfaSmsService;

  /// <summary>
  /// Initializes a new instance of the <see cref="MfaSmsServiceTests"/> class.
  /// </summary>
  public MfaSmsServiceTests()
  {
    _smsProvider = Substitute.For<ISmsProvider>();
    _configuration = Substitute.For<IConfiguration>();
    _mfaDataService = Substitute.For<IMfaDataService>();
    _cache = Substitute.For<IDistributedCache>();
    _userManager = Substitute.For<UserManager<IdentityExpressUser>>(Substitute.For<IUserStore<IdentityExpressUser>>(), null, null, null, null, null, null, null, null);
    _mfaSmsService = new MfaSmsService(_smsProvider, _configuration, _mfaDataService, _cache, _userManager);
  }


  /// <summary>
  /// Defines the test method SendSmsAsync_ValidRequest_ReturnsTrue.
  /// </summary>
  [Fact]
  public async Task SendSmsAsync_ValidRequest_ReturnsTrue()
  {
    // Arrange
    var phoneNumber = "1234567890";
    var message = "Test message";
    var cancellationToken = CancellationToken.None;
    _smsProvider.SendSmsAsync(Arg.Any<SendSmsRequest>()).Returns(true);

    // Act
    var result = await _mfaSmsService.SendSmsAsync(phoneNumber, message, cancellationToken);

    // Assert
    Assert.True(result);
  }

  /// <summary>
  /// Defines the test method GenerateAndStoreMfaCodeAsync_ValidUserId_ReturnsCode.
  /// </summary>
  [Fact]
  public async Task GenerateAndStoreMfaCodeAsync_ValidUserId_ReturnsCode()
  {
    // Arrange
    var userId = "testUser";
    var cancellationToken = CancellationToken.None;
    _configuration["Authentication:SMS:AllowableCharacters"].Returns("1234567890");
    _configuration["Authentication:SMS:CodeLength"].Returns("6");

    // Act
    var result = await _mfaSmsService.GenerateAndStoreMfaCodeAsync(userId, cancellationToken);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(6, result.Length);
  }
   
}