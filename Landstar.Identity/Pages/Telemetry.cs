// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-22-2024
//
// Last Modified By : 
// Last Modified On : 04-23-2024
// ***********************************************************************
// <copyright file="Telemetry.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Diagnostics.Metrics;

namespace Landstar.Identity.Pages;



/// <summary>
/// Telemetry helpers for the UI
/// </summary>
public static class Telemetry
{
  /// <summary>
  /// The service version
  /// </summary>
  private static readonly string ServiceVersion = typeof(Telemetry).Assembly.GetName().Version!.ToString();

  /// <summary>
  /// Service name for telemetry.
  /// </summary>
  public static readonly string ServiceName = typeof(Telemetry).Assembly.GetName().Name!;

  /// <summary>
  /// Metrics configuration
  /// </summary>
  public static class Metrics
  {

    /// <summary>
    /// Name of Counters
    /// </summary>
    public static class Counters
    {
      /// <summary>
      /// The consent
      /// </summary>
      public const string Consent = "tokenservice.consent";
      /// <summary>
      /// The grants revoked
      /// </summary>
      public const string Grants_Revoked = "tokenservice.grants_revoked";
      /// <summary>
      /// The user login
      /// </summary>
      public const string User_Login = "tokenservice.user_login";
      /// <summary>
      /// The user logout
      /// </summary>
      public const string User_Logout = "tokenservice.user_logout";
    }

    /// <summary>
    /// Name of tags
    /// </summary>
    public static class Tags
    {
      /// <summary>
      /// The client
      /// </summary>
      public const string Client = "client";
      /// <summary>
      /// The error
      /// </summary>
      public const string Error = "error";
      /// <summary>
      /// The idp
      /// </summary>
      public const string Idp = "idp";
      /// <summary>
      /// The remember
      /// </summary>
      public const string Remember = "remember";
      /// <summary>
      /// The scope
      /// </summary>
      public const string Scope = "scope";
      /// <summary>
      /// The consent
      /// </summary>
      public const string Consent = "consent";
    }

    /// <summary>
    /// Values of tags
    /// </summary>
    public static class TagValues
    {
      /// <summary>
      /// The granted
      /// </summary>
      public const string Granted = "granted";
      /// <summary>
      /// The denied
      /// </summary>
      public const string Denied = "denied";
    }


    /// <summary>
    /// Meter for the IdentityServer host project
    /// </summary>
    private static readonly Meter Meter = new(ServiceName, ServiceVersion);

    /// <summary>
    /// The consent counter
    /// </summary>
    private static readonly Counter<long> ConsentCounter = Meter.CreateCounter<long>(Counters.Consent);

    /// <summary>
    /// Helper method to increase <see cref="Counters.Consent" /> counter. The scopes
    /// are expanded and called one by one to not cause a combinatory explosion of scopes.
    /// </summary>
    /// <param name="clientId">Client id</param>
    /// <param name="scopes">Scope names. Each element is added on it's own to the counter</param>
    /// <param name="remember">if set to <see langword="true" /> [remember].</param>
    /// <exception cref="System.ArgumentNullException"></exception>
    public static void ConsentGranted(string clientId, IEnumerable<string> scopes, bool remember)
    {
      ArgumentNullException.ThrowIfNull(scopes);

      foreach (var scope in scopes)
      {
        ConsentCounter.Add(1,
            new(Tags.Client, clientId),
            new(Tags.Scope, scope),
            new(Tags.Remember, remember),
            new(Tags.Consent, TagValues.Granted));
      }
    }

    /// <summary>
    /// Consents the denied.
    /// </summary>
    /// <param name="clientId">The client identifier.</param>
    /// <param name="scopes">The scopes.</param>
    /// <exception cref="System.ArgumentNullException"></exception>
    public static void ConsentDenied(string clientId, IEnumerable<string> scopes)
    {
      ArgumentNullException.ThrowIfNull(scopes);
      foreach (var scope in scopes)
      {
        ConsentCounter.Add(1, new(Tags.Client, clientId), new(Tags.Scope, scope), new(Tags.Consent, TagValues.Denied));
      }
    }

    /// <summary>
    /// The grants revoked counter
    /// </summary>
    private static readonly Counter<long> GrantsRevokedCounter = Meter.CreateCounter<long>(Counters.Grants_Revoked);

    /// <summary>
    /// Helper method to increase the <see cref="Counters.Grants_Revoked" /> counter.
    /// </summary>
    /// <param name="clientId">Client id to revoke for, or null for all.</param>
    public static void GrantsRevoked(string clientId)
        => GrantsRevokedCounter.Add(1, tag: new(Tags.Client, clientId));

    /// <summary>
    /// The user login counter
    /// </summary>
    private static readonly Counter<long> UserLoginCounter = Meter.CreateCounter<long>(Counters.User_Login);

    /// <summary>
    /// Helper method to increase <see cref="Counters.User_Login" /> counter.
    /// </summary>
    /// <param name="clientId">Client Id, if available</param>
    /// <param name="idp">The idp.</param>
    public static void UserLogin(string clientId, string idp)
        => UserLoginCounter.Add(1, new(Tags.Client, clientId), new(Tags.Idp, idp));

    /// <summary>
    /// Users the login failure.
    /// </summary>
    /// <param name="clientId">The client identifier.</param>
    /// <param name="idp">The idp.</param>
    /// <param name="error">The error.</param>
    /// <font color="red">Badly formed XML comment.</font>
    public static void UserLoginFailure(string clientId, string idp, string error)
        => UserLoginCounter.Add(1, new(Tags.Client, clientId), new(Tags.Idp, idp), new(Tags.Error, error));

    /// <summary>
    /// The user logout counter
    /// </summary>
    private static readonly Counter<long> UserLogoutCounter = Meter.CreateCounter<long>(Counters.User_Logout);

    /// <summary>
    /// Helper method to increase the <see cref="Counters.User_Logout" /> counter.
    /// </summary>
    /// <param name="idp">Idp/authentication scheme for external authentication, or "local" for built in.</param>
    public static void UserLogout(string idp)
        => UserLogoutCounter.Add(1, tag: new(Tags.Idp, idp));
  }
}
