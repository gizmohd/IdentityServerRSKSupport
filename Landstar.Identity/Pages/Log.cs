// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-23-2024
//
// Last Modified By : 
// Last Modified On : 04-23-2024
// ***********************************************************************
// <copyright file="Log.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Landstar.Identity.Pages;

/// <summary>
/// Class Log.
/// </summary>
internal static class Log
{
  /// <summary>
  /// The invalid identifier
  /// </summary>
  private static readonly Action<ILogger, string, Exception> _invalidId = LoggerMessage.Define<string>(
      LogLevel.Error,
      EventIds.InvalidId,
      "Invalid id {Id}");

  /// <summary>
  /// Invalids the identifier.
  /// </summary>
  /// <param name="logger">The logger.</param>
  /// <param name="id">The identifier.</param>
  public static void InvalidId(this ILogger logger, string id)
  {
    _invalidId(logger, id, null);
  }

  /// <summary>
  /// The invalid backchannel login identifier
  /// </summary>
  private static readonly Action<ILogger, string, Exception> _invalidBackchannelLoginId = LoggerMessage.Define<string>(
  LogLevel.Warning,
  EventIds.InvalidBackchannelLoginId,
  "Invalid backchannel login id {Id}");

  /// <summary>
  /// Invalids the backchannel login identifier.
  /// </summary>
  /// <param name="logger">The logger.</param>
  /// <param name="id">The identifier.</param>
  public static void InvalidBackchannelLoginId(this ILogger logger, string id)
  {
    _invalidBackchannelLoginId(logger, id, null);
  }

  /// <summary>
  /// The external claims
  /// </summary>
  private static readonly Action<ILogger, IEnumerable<string>, Exception> _externalClaims = LoggerMessage.Define<IEnumerable<string>>(
    LogLevel.Debug,
    EventIds.ExternalClaims,
    "External claims: {Claims}");

  /// <summary>
  /// Externals the claims.
  /// </summary>
  /// <param name="logger">The logger.</param>
  /// <param name="claims">The claims.</param>
  public static void ExternalClaims(this ILogger logger, IEnumerable<string> claims)
  {
    _externalClaims(logger, claims, null);
  }

  /// <summary>
  /// The no matching backchannel login request
  /// </summary>
  private static readonly Action<ILogger, string, Exception> _noMatchingBackchannelLoginRequest = LoggerMessage.Define<string>(
    LogLevel.Error,
    EventIds.NoMatchingBackchannelLoginRequest,
    "No backchannel login request matching id: {Id}");

  /// <summary>
  /// Noes the matching backchannel login request.
  /// </summary>
  /// <param name="logger">The logger.</param>
  /// <param name="id">The identifier.</param>
  public static void NoMatchingBackchannelLoginRequest(this ILogger logger, string id)
  {
    _noMatchingBackchannelLoginRequest(logger, id, null);
  }

  /// <summary>
  /// The no consent matching request
  /// </summary>
  private static readonly Action<ILogger, string, Exception> _noConsentMatchingRequest = LoggerMessage.Define<string>(
    LogLevel.Error,
    EventIds.NoConsentMatchingRequest,
    "No consent request matching request: {ReturnUrl}");

  /// <summary>
  /// Noes the consent matching request.
  /// </summary>
  /// <param name="logger">The logger.</param>
  /// <param name="returnUrl">The return URL.</param>
  public static void NoConsentMatchingRequest(this ILogger logger, string returnUrl)
  {
    _noConsentMatchingRequest(logger, returnUrl, null);
  }


}

/// <summary>
/// Class EventIds.
/// </summary>
internal static class EventIds
{
  /// <summary>
  /// The UI events start
  /// </summary>
  private const int UIEventsStart = 10000;

  //////////////////////////////
  // Consent
  //////////////////////////////
  /// <summary>
  /// The consent events start
  /// </summary>
  private const int ConsentEventsStart = UIEventsStart + 1000;
  /// <summary>
  /// The invalid identifier
  /// </summary>
  public const int InvalidId = ConsentEventsStart + 0;
  /// <summary>
  /// The no consent matching request
  /// </summary>
  public const int NoConsentMatchingRequest = ConsentEventsStart + 1;

  //////////////////////////////
  // External Login
  //////////////////////////////
  /// <summary>
  /// The external login events start
  /// </summary>
  private const int ExternalLoginEventsStart = UIEventsStart + 2000;
  /// <summary>
  /// The external claims
  /// </summary>
  public const int ExternalClaims = ExternalLoginEventsStart + 0;

  //////////////////////////////
  // CIBA
  //////////////////////////////
  /// <summary>
  /// The ciba events start
  /// </summary>
  private const int CibaEventsStart = UIEventsStart + 3000;
  /// <summary>
  /// The invalid backchannel login identifier
  /// </summary>
  public const int InvalidBackchannelLoginId = CibaEventsStart + 0;
  /// <summary>
  /// The no matching backchannel login request
  /// </summary>
  public const int NoMatchingBackchannelLoginRequest = CibaEventsStart + 1;



}
