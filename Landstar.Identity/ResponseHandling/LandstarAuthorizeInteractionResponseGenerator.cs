// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Duende.IdentityServer;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.ResponseHandling;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Validation;
using IdentityModel;
using Landstar.Identity.Extensions;




namespace Landstar.Identity.ResponseHandling;

/// <summary>
/// Default logic for determining if user must login or consent when making requests to the authorization endpoint.
/// </summary>
/// <seealso cref="Duende.IdentityServer.ResponseHandling.IAuthorizeInteractionResponseGenerator" />
/// <remarks>
/// Initializes a new instance of the <see cref="AuthorizeInteractionResponseGenerator"/> class.
/// </remarks>
/// <param name="clock">The clock.</param>
/// <param name="logger">The logger.</param>
/// <param name="consent">The consent.</param>
/// <param name="profile">The profile.</param>
public class AuthorizeInteractionResponseGenerator2(
    IClock clock,
    ILogger<AuthorizeInteractionResponseGenerator> logger,
    IConsentService consent,
    IProfileService profile) : IAuthorizeInteractionResponseGenerator
{
  /// <summary>
  /// The logger.
  /// </summary>
  protected readonly ILogger Logger = logger;

  /// <summary>
  /// The consent service.
  /// </summary>
  protected readonly IConsentService Consent = consent;

  /// <summary>
  /// The profile service.
  /// </summary>
  protected readonly IProfileService Profile = profile;

  /// <summary>
  /// The clock
  /// </summary>
  protected readonly IClock Clock = clock;

  /// <summary>
  /// Processes the interaction logic.
  /// </summary>
  /// <param name="request">The request.</param>
  /// <param name="consent">The consent.</param>
  /// <returns></returns>
  public virtual async Task<InteractionResponse> ProcessInteractionAsync(ValidatedAuthorizeRequest request, ConsentResponse consent = null)
  {
    Logger.LogTrace("ProcessInteractionAsync");

    if (consent != null && !consent.Granted && consent.Error.HasValue && !request.Subject.IsAuthenticated())
    {
      // special case when anonymous user has issued an error prior to authenticating
      Logger.LogInformation("Error: User consent result: {Error}", consent.Error);

      var error = consent.Error switch
      {
        AuthorizationError.AccountSelectionRequired => OidcConstants.AuthorizeErrors.AccountSelectionRequired,
        AuthorizationError.ConsentRequired => OidcConstants.AuthorizeErrors.ConsentRequired,
        AuthorizationError.InteractionRequired => OidcConstants.AuthorizeErrors.InteractionRequired,
        AuthorizationError.LoginRequired => OidcConstants.AuthorizeErrors.LoginRequired,
        _ => OidcConstants.AuthorizeErrors.AccessDenied
      };

      return new InteractionResponse
      {
        Error = error,
        ErrorDescription = consent.ErrorDescription
      };
    }

    var result = await ProcessLoginAsync(request).ConfigureAwait(false);

    if (!result.IsLogin && !result.IsError && !result.IsRedirect)
    {
      result = await ProcessConsentAsync(request, consent).ConfigureAwait(false);
    }

    if ((result.IsLogin || result.IsConsent || result.IsRedirect) && request.PromptModes.Contains(OidcConstants.PromptModes.None))
    {
      // prompt=none means do not show the UI
      Logger.LogInformation("Changing response to LoginRequired: prompt=none was requested");

      var isConsent = result.IsConsent ?
                              OidcConstants.AuthorizeErrors.ConsentRequired :
                              OidcConstants.AuthorizeErrors.InteractionRequired;

      result = new InteractionResponse
      {
        Error = result.IsLogin ? OidcConstants.AuthorizeErrors.LoginRequired : isConsent
      };
    }

    return result;
  }

  /// <summary>
  /// Processes the login logic.
  /// </summary>
  /// <param name="request">The request.</param>
  /// <returns></returns>
  protected internal virtual async Task<InteractionResponse> ProcessLoginAsync(ValidatedAuthorizeRequest request)
  {
    try
    {
      // unauthenticated user
      var isAuthenticated = request.Subject.IsAuthenticated();

      // check current idp
      const string currentIdp = "oidc";

      // user de-activated
      bool isActive = await CheckIsActiveAsync(request, isAuthenticated).ConfigureAwait(false);

      var authTimeEpoch = request.Subject.GetAuthenticationTimeEpoch();
      // check authentication freshness
      var authTime = request.Subject.GetAuthenticationTime();

      var nowEpoch = Clock.UtcNow.ToUnixTimeSeconds();

      var diff = nowEpoch - authTimeEpoch;

      // check if idp login hint matches current provider
      var idp = request.GetIdP();

      if (request.PromptModes.Contains(OidcConstants.PromptModes.Login) ||
          request.PromptModes.Contains(OidcConstants.PromptModes.SelectAccount))
      {
        Logger.LogInformation("Showing login: request contains prompt={Prompt}", request.PromptModes.ToSpaceSeparatedString());

        // remove prompt so when we redirect back in from login page
        // we won't think we need to force a prompt again
        request.RemovePrompt();

        return new InteractionResponse { IsLogin = true };
      }

      bool mustLogin = CheckLoginStatus(request, isAuthenticated, currentIdp, isActive, authTime, diff, idp);
      return new InteractionResponse { IsLogin = mustLogin };

    }
    catch (Exception e)
    {
      Logger.LogError(exception: e,"Error Processing Logon: {Message}", e.Message);

    }
    return new InteractionResponse();
  }

  private bool CheckLoginStatus(ValidatedAuthorizeRequest request, bool isAuthenticated, string currentIdp, bool isActive, DateTime authTime, long diff, string idp)
  {

    if (!isAuthenticated || !isActive)
    {
      Logger.LogInformation("Showing login: {Status}", isAuthenticated ? "User is not active" : "User is not authenticated");
      return true;

    }


    if (idp.IsPresent() && !idp.Equals(currentIdp))
    {
      Logger.LogInformation("Showing login: Current IdP ({CurrentIdp}) is not the requested IdP ({Idp})", currentIdp, idp);
      return true;
    }


    if (request.MaxAge.HasValue &&
        Clock.UtcNow > authTime.AddSeconds(request.MaxAge.Value))
    {
      Logger.LogInformation("Showing login: Requested MaxAge exceeded.");

      return true;
    }

    // check local idp restrictions
    if (currentIdp.Equals(IdentityServerConstants.LocalIdentityProvider) && !request.Client.EnableLocalLogin)
    {
      Logger.LogInformation("Showing login: User logged in locally, but client does not allow local logins");
      return true;
    }

    // check external idp restrictions if user not using local idp
    if (request.Client.IdentityProviderRestrictions != null &&
        request.Client.IdentityProviderRestrictions.Count>0 &&
       !request.Client.IdentityProviderRestrictions.Contains(currentIdp))
    {
      Logger.LogInformation("Showing login: User is logged in with idp: {Idp}, but idp not in client restriction list.", currentIdp);
      return true;
    }

    // check client's user SSO timeout
    // Only check if UserSsoLifetime is > 300
    if (request.Client.UserSsoLifetime.HasValue &&
        request.Client.UserSsoLifetime > 300 &&
        diff > request.Client.UserSsoLifetime.Value)
    {

      Logger.LogInformation("Showing login: User's auth session duration: {SessionDuration} exceeds client's user SSO lifetime: {UserSsoLifetime}.", diff, request.Client.UserSsoLifetime);
      return true;
    }

    return false;
  }

  private async Task<bool> CheckIsActiveAsync(ValidatedAuthorizeRequest request, bool isAuthenticated)
  {
    bool isActive = false;
    if (isAuthenticated)
    {
      var isActiveCtx = new IsActiveContext(request.Subject, request.Client, IdentityServerConstants.ProfileIsActiveCallers.AuthorizeEndpoint);
      await Profile.IsActiveAsync(isActiveCtx).ConfigureAwait(false);

      isActive = isActiveCtx.IsActive;
    }

    return isActive;
  }

  /// <summary>
  /// Processes the consent logic.
  /// </summary>
  /// <param name="request">The request.</param>
  /// <param name="consent">The consent.</param>
  /// <returns></returns>
  /// <exception cref="ArgumentNullException"></exception>
  /// <exception cref="ArgumentException">Invalid PromptMode</exception>
  protected internal virtual Task<InteractionResponse> ProcessConsentAsync(ValidatedAuthorizeRequest request, ConsentResponse consent = null)
  {
    ArgumentNullException.ThrowIfNull(request);

    if (request.PromptModes.Any() &&
    !request.PromptModes.Contains(OidcConstants.PromptModes.None) &&
    !request.PromptModes.Contains(OidcConstants.PromptModes.Consent))
    {
      Logger.LogError("Invalid prompt mode: {PromptMode}", request.PromptModes.ToSpaceSeparatedString());
      throw new ArgumentException("Invalid PromptMode Parameter", nameof(request));
    }

    return ProcessConsentInternalAsync(request, consent);
  }

  private async Task<InteractionResponse> ProcessConsentInternalAsync(ValidatedAuthorizeRequest request, ConsentResponse consent = null)
  {


    var consentRequired = await Consent.RequiresConsentAsync(request.Subject, request.Client, request.ValidatedResources.ParsedScopes).ConfigureAwait(false);

    if (consentRequired && request.PromptModes.Contains(OidcConstants.PromptModes.None))
    {
      Logger.LogInformation("Error: prompt=none requested, but consent is required.");

      return new InteractionResponse
      {
        Error = OidcConstants.AuthorizeErrors.ConsentRequired
      };
    }

    if (request.PromptModes.Contains(OidcConstants.PromptModes.Consent) || consentRequired)
    {
      InteractionResponse response = await GenerateInteractionResponseAsync(request, consent).ConfigureAwait(false);

      return response;
    }

    return new InteractionResponse();
  }

  private async Task<InteractionResponse> GenerateInteractionResponseAsync(ValidatedAuthorizeRequest request, ConsentResponse consent)
  {
    var response = new InteractionResponse();

    // did user provide consent
    if (consent == null)
    {
      // user was not yet shown conset screen
      response.IsConsent = true;
      Logger.LogInformation("Showing consent: User has not yet consented");
    }
    else
    {
      request.WasConsentShown = true;
      Logger.LogTrace("Consent was shown to user");

      // user was shown consent -- did they say yes or no
      if (consent.Granted)
      {
        // double check that required scopes are in the list of consented scopes
        var requiredScopes = request.ValidatedResources.GetRequiredScopeValues();
        var valid = requiredScopes.All(x => consent.ScopesValuesConsented.Contains(x));
        if (valid)
        {
          await UpdateConsentAsync(request, consent).ConfigureAwait(false);
        }
        else
        {
          response.Error = OidcConstants.AuthorizeErrors.AccessDenied;
          Logger.LogInformation("Error: User denied consent to required scopes");
        }
      }
      else
      {
        // no need to show consent screen again
        // build error to return to client
        Logger.LogInformation("Error: User consent result: {Error}", consent.Error);

        var error = consent.Error switch
        {
          AuthorizationError.AccountSelectionRequired => OidcConstants.AuthorizeErrors.AccountSelectionRequired,
          AuthorizationError.ConsentRequired => OidcConstants.AuthorizeErrors.ConsentRequired,
          AuthorizationError.InteractionRequired => OidcConstants.AuthorizeErrors.InteractionRequired,
          AuthorizationError.LoginRequired => OidcConstants.AuthorizeErrors.LoginRequired,
          _ => OidcConstants.AuthorizeErrors.AccessDenied
        };

        response.Error = error;
        response.ErrorDescription = consent.ErrorDescription;
      }
    }

    return response;
  }

  private async Task UpdateConsentAsync(ValidatedAuthorizeRequest request, ConsentResponse consent)
  {
    // they said yes, set scopes they chose
    request.Description = consent.Description;
    request.ValidatedResources = request.ValidatedResources.Filter(consent.ScopesValuesConsented);
    Logger.LogInformation("User consented to scopes: {Scopes}", consent.ScopesValuesConsented);

    if (request.Client.AllowRememberConsent)
    {
      // remember consent
      var parsedScopes = Enumerable.Empty<ParsedScopeValue>();
      if (consent.RememberConsent)
      {
        // remember what user actually selected
        parsedScopes = request.ValidatedResources.ParsedScopes;
        Logger.LogDebug("User indicated to remember consent for scopes: {Scopes}", request.ValidatedResources.RawScopeValues);
      }

      await Consent.UpdateConsentAsync(request.Subject, request.Client, parsedScopes).ConfigureAwait(false);
    }
  }
}