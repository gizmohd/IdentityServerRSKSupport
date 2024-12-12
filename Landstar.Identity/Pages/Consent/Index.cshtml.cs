// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-22-2024
//
// Last Modified By : 
// Last Modified On : 04-22-2024
// ***********************************************************************
// <copyright file="Index.cshtml.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Duende.IdentityServer.Events;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Validation;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Landstar.Identity.Pages.Consent;

/// <summary>
/// Class Index.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
[Authorize]
[SecurityHeaders]
public class Index(
    IIdentityServerInteractionService interaction,
    IEventService events,
    ILogger<Index> logger) : PageModel
{
  /// <summary>
  /// Gets or sets the view.
  /// </summary>
  /// <value>The view.</value>
  public ViewModel View { get; set; } = default!;

  /// <summary>
  /// Gets or sets the input.
  /// </summary>
  /// <value>The input.</value>
  [BindProperty]
  public InputModel Input { get; set; } = default!;

  /// <summary>
  /// On get as an asynchronous operation.
  /// </summary>
  /// <param name="returnUrl">The return URL.</param>
  /// <returns>A Task&lt;IActionResult&gt; representing the asynchronous operation.</returns>
  public async Task<IActionResult> OnGetAsync(string returnUrl)
  {
    if (!await SetViewModelAsync(returnUrl))
    {
      return RedirectToPage("/Home/Error/Index");
    }

    Input = new InputModel
    {
      ReturnUrl = returnUrl,
    };

    return Page();
  }

  /// <summary>
  /// On post as an asynchronous operation.
  /// </summary>
  /// <returns>A Task&lt;IActionResult&gt; representing the asynchronous operation.</returns>
  /// <exception cref="System.ArgumentNullException">ReturnUrl</exception>
  public async Task<IActionResult> OnPostAsync()
  {
    // validate return url is still valid
    AuthorizationRequest request = await interaction.GetAuthorizationContextAsync(Input.ReturnUrl);
    if (request == null)
    {
      return RedirectToPage("/Home/Error/Index");
    }

    ConsentResponse grantedConsent = null;

    // user clicked 'no' - send back the standard 'access_denied' response
    if (Input.Button == "no")
    {
      grantedConsent = new() { Error = AuthorizationError.AccessDenied };

      // emit event
      await events.RaiseAsync(new ConsentDeniedEvent(User.GetSubjectId(), request.Client.ClientId, request.ValidatedResources.RawScopeValues));
      Telemetry.Metrics.ConsentDenied(request.Client.ClientId, request.ValidatedResources.ParsedScopes.Select(s => s.ParsedName));
    }
    // user clicked 'yes' - validate the data
    else if (Input.Button == "yes")
    {
      // if the user consented to some scope, build the response model
      if (Input.ScopesConsented.Any())
      {
        IEnumerable<string> scopes = Input.ScopesConsented;
        if (!ConsentOptions.EnableOfflineAccess)
        {
          scopes = scopes.Where(x => x != Duende.IdentityServer.IdentityServerConstants.StandardScopes.OfflineAccess);
        }

        grantedConsent = new ConsentResponse
        {
          RememberConsent = Input.RememberConsent,
          ScopesValuesConsented = scopes.ToArray(),
          Description = Input.Description
        };

        // emit event
        await events.RaiseAsync(new ConsentGrantedEvent(User.GetSubjectId(), request.Client.ClientId, request.ValidatedResources.RawScopeValues, grantedConsent.ScopesValuesConsented, grantedConsent.RememberConsent));
        Telemetry.Metrics.ConsentGranted(request.Client.ClientId, grantedConsent.ScopesValuesConsented, grantedConsent.RememberConsent);
        IEnumerable<string> denied = request.ValidatedResources.ParsedScopes.Select(s => s.ParsedName).Except(grantedConsent.ScopesValuesConsented);
        Telemetry.Metrics.ConsentDenied(request.Client.ClientId, denied);
      }
      else
      {
        ModelState.AddModelError("", ConsentOptions.MustChooseOneErrorMessage);
      }
    }
    else
    {
      ModelState.AddModelError("", ConsentOptions.InvalidSelectionErrorMessage);
    }

    if (grantedConsent != null)
    {
      ArgumentNullException.ThrowIfNull(Input.ReturnUrl, nameof(Input.ReturnUrl));

      // communicate outcome of consent back to identityserver
      await interaction.GrantConsentAsync(request, grantedConsent);

      // redirect back to authorization endpoint
      if (request.IsNativeClient())
      {
        // The client is native, so this change in how to
        // return the response is for better UX for the end user.
        return this.LoadingPage(Input.ReturnUrl);
      }

      return Redirect(Input.ReturnUrl);
    }

    // we need to redisplay the consent UI
    if (!await SetViewModelAsync(Input.ReturnUrl))
    {
      return RedirectToPage("/Home/Error/Index");
    }
    return Page();
  }

  /// <summary>
  /// Set view model as an asynchronous operation.
  /// </summary>
  /// <param name="returnUrl">The return URL.</param>
  /// <returns>A Task&lt;System.Boolean&gt; representing the asynchronous operation.</returns>
  /// <exception cref="System.ArgumentNullException"></exception>
  private Task<bool> SetViewModelAsync(string returnUrl)
  {
    ArgumentNullException.ThrowIfNull(returnUrl);

    return PrivateSetViewModelAsync(returnUrl);
  }

  private async Task<bool> PrivateSetViewModelAsync(string returnUrl)
  {


    AuthorizationRequest request = await interaction.GetAuthorizationContextAsync(returnUrl);
    if (request != null)
    {
      View = CreateConsentViewModel(request);
      return true;
    }

    logger.NoConsentMatchingRequest(returnUrl);
    return false;

  }
  /// <summary>
  /// Creates the consent view model.
  /// </summary>
  /// <param name="request">The request.</param>
  /// <returns>ViewModel.</returns>
  private ViewModel CreateConsentViewModel(AuthorizationRequest request)
  {
    ViewModel vm = new()
    {
      ClientName = request.Client.ClientName ?? request.Client.ClientId,
      ClientUrl = request.Client.ClientUri,
      ClientLogoUrl = request.Client.LogoUri,
      AllowRememberConsent = request.Client.AllowRememberConsent,
      IdentityScopes = request.ValidatedResources.Resources.IdentityResources
        .Select(x => CreateScopeViewModel(x, Input == null || Input.ScopesConsented.Contains(x.Name)))
        .ToArray()
    };

    IEnumerable<string> resourceIndicators = request.Parameters.GetValues(OidcConstants.AuthorizeRequest.Resource) ?? Enumerable.Empty<string>();
    IEnumerable<ApiResource> apiResources = request.ValidatedResources.Resources.ApiResources.Where(x => resourceIndicators.Contains(x.Name));

    List<ScopeViewModel> apiScopes = new();
    foreach (ParsedScopeValue parsedScope in request.ValidatedResources.ParsedScopes)
    {
      ApiScope apiScope = request.ValidatedResources.Resources.FindApiScope(parsedScope.ParsedName);
      if (apiScope != null)
      {
        ScopeViewModel scopeVm = CreateScopeViewModel(parsedScope, apiScope, Input == null || Input.ScopesConsented.Contains(parsedScope.RawValue));
        scopeVm.Resources = apiResources.Where(x => x.Scopes.Contains(parsedScope.ParsedName))
            .Select(x => new ResourceViewModel
            {
              Name = x.Name,
              DisplayName = x.DisplayName ?? x.Name,
            }).ToArray();
        apiScopes.Add(scopeVm);
      }
    }
    if (ConsentOptions.EnableOfflineAccess && request.ValidatedResources.Resources.OfflineAccess)
    {
      apiScopes.Add(CreateOfflineAccessScope(Input == null || Input.ScopesConsented.Contains(Duende.IdentityServer.IdentityServerConstants.StandardScopes.OfflineAccess)));
    }
    vm.ApiScopes = apiScopes;

    return vm;
  }

  /// <summary>
  /// Creates the scope view model.
  /// </summary>
  /// <param name="identity">The identity.</param>
  /// <param name="check">if set to <see langword="true" /> [check].</param>
  /// <returns>ScopeViewModel.</returns>
  private static ScopeViewModel CreateScopeViewModel(IdentityResource identity, bool check)
  {
    return new ScopeViewModel
    {
      Name = identity.Name,
      Value = identity.Name,
      DisplayName = identity.DisplayName ?? identity.Name,
      Description = identity.Description,
      Emphasize = identity.Emphasize,
      Required = identity.Required,
      Checked = check || identity.Required
    };
  }

  /// <summary>
  /// Creates the scope view model.
  /// </summary>
  /// <param name="parsedScopeValue">The parsed scope value.</param>
  /// <param name="apiScope">The API scope.</param>
  /// <param name="check">if set to <see langword="true" /> [check].</param>
  /// <returns>ScopeViewModel.</returns>
  private static ScopeViewModel CreateScopeViewModel(ParsedScopeValue parsedScopeValue, ApiScope apiScope, bool check)
  {
    string displayName = apiScope.DisplayName ?? apiScope.Name;
    if (!String.IsNullOrWhiteSpace(parsedScopeValue.ParsedParameter))
    {
      displayName += ":" + parsedScopeValue.ParsedParameter;
    }

    return new ScopeViewModel
    {
      Name = parsedScopeValue.ParsedName,
      Value = parsedScopeValue.RawValue,
      DisplayName = displayName,
      Description = apiScope.Description,
      Emphasize = apiScope.Emphasize,
      Required = apiScope.Required,
      Checked = check || apiScope.Required
    };
  }

  /// <summary>
  /// Creates the offline access scope.
  /// </summary>
  /// <param name="check">if set to <see langword="true" /> [check].</param>
  /// <returns>ScopeViewModel.</returns>
  private static ScopeViewModel CreateOfflineAccessScope(bool check)
  {
    return new ScopeViewModel
    {
      Value = Duende.IdentityServer.IdentityServerConstants.StandardScopes.OfflineAccess,
      DisplayName = ConsentOptions.OfflineAccessDisplayName,
      Description = ConsentOptions.OfflineAccessDescription,
      Emphasize = true,
      Checked = check
    };
  }
}
