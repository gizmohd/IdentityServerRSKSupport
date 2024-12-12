// ***********************************************************************
// Assembly         : Landstar.Identity.Tests
// Author           : 
// Created          : 09-27-2024
//
// Last Modified By : 
// Last Modified On : 09-27-2024
// ***********************************************************************
// <copyright file="DistributedCacheTicketStoreTests.cs" company="Landstar.Identity.Tests">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Landstar.Identity.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Distributed;
using NSubstitute;
using Xunit;

namespace Landstar.Identity.Tests.Services;

/// <summary>
/// Class DistributedCacheTicketStoreTests.
/// </summary>
public class DistributedCacheTicketStoreTests
{
  /// <summary>
  /// The cache
  /// </summary>
  private readonly IDistributedCache _cache;
  /// <summary>
  /// The ticket store
  /// </summary>
  private readonly DistributedCacheTicketStore _ticketStore;

  /// <summary>
  /// Initializes a new instance of the <see cref="DistributedCacheTicketStoreTests"/> class.
  /// </summary>
  public DistributedCacheTicketStoreTests()
  {
    _cache = Substitute.For<IDistributedCache>();
    _ticketStore = new DistributedCacheTicketStore(_cache);
  }

  /// <summary>
  /// Defines the test method StoreAsync_ShouldStoreTicketAndReturnKey.
  /// </summary>
  [Fact]
  public async Task StoreAsync_ShouldStoreTicketAndReturnKey()
  {
    // Arrange
    var ticket = new AuthenticationTicket(new System.Security.Claims.ClaimsPrincipal(), new AuthenticationProperties(), "TestScheme");

    // Act
    var key = await _ticketStore.StoreAsync(ticket);

    // Assert
    Assert.StartsWith("AuthSessionStore-", key);
    await _cache.Received(1).SetAsync(Arg.Is<string>(k => k == key), Arg.Any<byte[]>(), Arg.Any<DistributedCacheEntryOptions>(), default);
  }

  /// <summary>
  /// Defines the test method RenewAsync_ShouldUpdateCacheWithTicket.
  /// </summary>
  [Fact]
  public async Task RenewAsync_ShouldUpdateCacheWithTicket()
  {
    // Arrange
    var key = "AuthSessionStore-TestKey";
    var ticket = new AuthenticationTicket(new System.Security.Claims.ClaimsPrincipal(), new AuthenticationProperties(), "TestScheme");

    // Act
    await _ticketStore.RenewAsync(key, ticket);

    // Assert
    await _cache.Received(1).SetAsync(Arg.Is<string>(k => k == key), Arg.Any<byte[]>(), Arg.Any<DistributedCacheEntryOptions>(), default);
  }

  /// <summary>
  /// Defines the test method RetrieveAsync_ShouldReturnTicketFromCache.
  /// </summary>
  [Fact]
  public async Task RetrieveAsync_ShouldReturnTicketFromCache()
  {
    // Arrange
    var key = "AuthSessionStore-TestKey";
    var ticket = new AuthenticationTicket(new System.Security.Claims.ClaimsPrincipal(), new AuthenticationProperties(), "TestScheme");
    var ticketBytes = TicketSerializer.Default.Serialize(ticket);
    _cache.GetAsync(key, default).Returns(ticketBytes);

    // Act
    var result = await _ticketStore.RetrieveAsync(key);

    // Assert
    Assert.NotNull(result);
  }

  /// <summary>
  /// Defines the test method RemoveAsync_ShouldRemoveTicketFromCache.
  /// </summary>
  [Fact]
  public async Task RemoveAsync_ShouldRemoveTicketFromCache()
  {
    // Arrange
    var key = "AuthSessionStore-TestKey";

    // Act
    await _ticketStore.RemoveAsync(key);

    // Assert
    await _cache.Received(1).RemoveAsync(key, default);
  }
}