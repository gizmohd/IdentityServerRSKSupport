// ***********************************************************************
// Assembly         : Landstar.Identity.Tests
// Author           : 
// Created          : 09-27-2024
//
// Last Modified By : 
// Last Modified On : 09-27-2024
// ***********************************************************************
// <copyright file="LandstarAuthorizationServiceTests.cs" company="Landstar.Identity.Tests">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Landstar.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Landstar.Identity.Tests.Services;

/// <summary>
/// Class LandstarAuthorizationServiceTests.
/// </summary>
public class LandstarAuthorizationServiceTests
{
  /// <summary>
  /// The policy provider
  /// </summary>
  private readonly IAuthorizationPolicyProvider _policyProvider;
  /// <summary>
  /// The handlers
  /// </summary>
  private readonly IAuthorizationHandlerProvider _handlers;
  /// <summary>
  /// The logger
  /// </summary>
  private readonly ILogger<DefaultAuthorizationService> _logger;
  /// <summary>
  /// The context factory
  /// </summary>
  private readonly IAuthorizationHandlerContextFactory _contextFactory;
  /// <summary>
  /// The evaluator
  /// </summary>
  private readonly IAuthorizationEvaluator _evaluator;
  /// <summary>
  /// The options
  /// </summary>
  private readonly IOptions<AuthorizationOptions> _options;
  /// <summary>
  /// The authorization service
  /// </summary>
  private readonly LandstarAuthorizationService _authorizationService;

  /// <summary>
  /// Initializes a new instance of the <see cref="LandstarAuthorizationServiceTests"/> class.
  /// </summary>
  public LandstarAuthorizationServiceTests()
  {
    _policyProvider = Substitute.For<IAuthorizationPolicyProvider>();
    _handlers = Substitute.For<IAuthorizationHandlerProvider>();
    _logger = Substitute.For<ILogger<DefaultAuthorizationService>>();
    _contextFactory = Substitute.For<IAuthorizationHandlerContextFactory>();
    _evaluator = Substitute.For<IAuthorizationEvaluator>();
    _options = Substitute.For<IOptions<AuthorizationOptions>>();
    _options.Value.Returns(new AuthorizationOptions());

    _authorizationService = new LandstarAuthorizationService(
        _policyProvider, _handlers, _logger, _contextFactory, _evaluator, _options);
  }

  /// <summary>
  /// Defines the test method AuthorizeAsync_ShouldThrowArgumentNullException_WhenRequirementsIsNull.
  /// </summary>
  [Fact]
  public async Task AuthorizeAsync_ShouldThrowArgumentNullException_WhenRequirementsIsNull()
  {
    // Arrange
    var user = new ClaimsPrincipal();
    var resource = new object();
    IEnumerable<IAuthorizationRequirement> requirements = null;

    // Act & Assert
    await Assert.ThrowsAsync<ArgumentNullException>(() => _authorizationService.AuthorizeAsync(user, resource, requirements));
  }

  /// <summary>
  /// Defines the test method AuthorizeAsync_ShouldReturnSuccess_WhenAuthorizationSucceeds.
  /// </summary>
  [Fact]
  public async Task AuthorizeAsync_ShouldReturnSuccess_WhenAuthorizationSucceeds()
  {
    // Arrange
    var user = new ClaimsPrincipal();
    var resource = new object();
    var requirements = new List<IAuthorizationRequirement> { Substitute.For<IAuthorizationRequirement>() };
    var context = Substitute.For<AuthorizationHandlerContext>(requirements, user, resource);
    var result = AuthorizationResult.Success();

    _contextFactory.CreateContext(requirements, user, resource).Returns(context);
    _handlers.GetHandlersAsync(context).Returns(new List<IAuthorizationHandler> { Substitute.For<IAuthorizationHandler>() });
    _evaluator.Evaluate(context).Returns(result);

    // Act
    var authorizationResult = await _authorizationService.AuthorizeAsync(user, resource, requirements);

    // Assert
    Assert.True(authorizationResult.Succeeded);
  }

  /// <summary>
  /// Defines the test method AuthorizeAsync_ShouldReturnFailure_WhenAuthorizationFails.
  /// </summary>
  [Fact]
  public async Task AuthorizeAsync_ShouldReturnFailure_WhenAuthorizationFails()
  {
    // Arrange
    var user = new ClaimsPrincipal();
    var resource = new object();
    var requirements = new List<IAuthorizationRequirement> { Substitute.For<IAuthorizationRequirement>() };
    var context = Substitute.For<AuthorizationHandlerContext>(requirements, user, resource);
    var result = AuthorizationResult.Failed();

    _contextFactory.CreateContext(requirements, user, resource).Returns(context);
    _handlers.GetHandlersAsync(context).Returns(new List<IAuthorizationHandler> { Substitute.For<IAuthorizationHandler>() });
    _evaluator.Evaluate(context).Returns(result);

    // Act
    var authorizationResult = await _authorizationService.AuthorizeAsync(user, resource, requirements);

    // Assert
    Assert.False(authorizationResult.Succeeded);
  }
}
