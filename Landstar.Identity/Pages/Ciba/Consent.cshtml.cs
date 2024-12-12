// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-22-2024
//
// Last Modified By : 
// Last Modified On : 04-23-2024
// ***********************************************************************
// <copyright file="Consent.cshtml.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Duende.IdentityServer.Events;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Landstar.Identity.Pages.Ciba;

/// <summary>
/// Class Consent.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
[Authorize]
[SecurityHeaders]
public class Consent(
    IBackchannelAuthenticationInteractionService interaction,
    IEventService events,
    ILogger<Consent> logger) : PageModel
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
  /// <param name="id">The identifier.</param>
  /// <returns>A Task&lt;Microsoft.AspNetCore.Mvc.IActionResult&gt; representing the asynchronous operation.</returns>
  public async Task<IActionResult> OnGetAsync(string id)
  {
    if (!await SetViewModelAsync(id))
    {
      return RedirectToPage("/Home/Error/Index");
    }

    Input = new InputModel
    {
      Id = id
    };

    return Page();
  }

  /// <summary>
  /// On post as an asynchronous operation.
  /// </summary>
  /// <returns>A Task&lt;Microsoft.AspNetCore.Mvc.IActionResult&gt; representing the asynchronous operation.</returns>
  public async Task<IActionResult> OnPostAsync()
  {
    // validate return url is still valid
    var request = await interaction.GetLoginRequestByInternalIdAsync(Input.Id ?? throw new ArgumentException("Empty Value for Parameter: " + nameof(Input.Id)));
    if (request == null || request.Subject.GetSubjectId() != User.GetSubjectId())
    {
      logger.InvalidId(Input.Id);
      return RedirectToPage("/Home/Error/Index");
    }

    CompleteBackchannelLoginRequest result = null;

    // user clicked 'no' - send back the standard 'access_denied' response
    if (Input.Button == "no")
    {
      result = new CompleteBackchannelLoginRequest(Input.Id);

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
        var scopes = Input.ScopesConsented;
        if (!ConsentOptions.EnableOfflineAccess)
        {
          scopes = scopes.Where(x => x != Duende.IdentityServer.IdentityServerConstants.StandardScopes.OfflineAccess);
        }

        result = new CompleteBackchannelLoginRequest(Input.Id)
        {
          ScopesValuesConsented = scopes.ToArray(),
          Description = Input.Description
        };

        // emit event
        await events.RaiseAsync(new ConsentGrantedEvent(User.GetSubjectId(), request.Client.ClientId, request.ValidatedResources.RawScopeValues, result.ScopesValuesConsented, false));
        Telemetry.Metrics.ConsentGranted(request.Client.ClientId, result.ScopesValuesConsented, false);
        var denied = request.ValidatedResources.ParsedScopes.Select(s => s.ParsedName).Except(result.ScopesValuesConsented);
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

    if (result != null)
    {
      // communicate outcome of consent back to identityserver
      await interaction.CompleteLoginRequestAsync(result);

      return RedirectToPage("/Ciba/All");
    }

    // we need to redisplay the consent UI
    if (!await SetViewModelAsync(Input.Id))
    {
      return RedirectToPage("/Home/Error/Index");
    }
    return Page();
  }

  /// <summary>
  /// Set view model as an asynchronous operation.
  /// </summary>
  /// <param name="id">The identifier.</param>
  /// <returns>A Task&lt;bool&gt; representing the asynchronous operation.</returns>
  private Task<bool> SetViewModelAsync(string id)
  {
    ArgumentNullException.ThrowIfNull(id);
    return InternalSetViewModelAsync(id);
  }
  /// <summary>
  /// Internal set view model as an asynchronous operation.
  /// </summary>
  /// <param name="id">The identifier.</param>
  /// <returns>A Task&lt;System.Boolean&gt; representing the asynchronous operation.</returns>
  private async Task<bool> InternalSetViewModelAsync(string id)
  {


    var request = await interaction.GetLoginRequestByInternalIdAsync(id);
    if (request != null && request.Subject.GetSubjectId() == User.GetSubjectId())
    {
      View = CreateConsentViewModel(request);
      return true;
    }

    logger.NoMatchingBackchannelLoginRequest(id);
    return false;

  }

  /// <summary>
  /// Creates the consent view model.
  /// </summary>
  /// <param name="request">The request.</param>
  /// <returns>Landstar.Identity.Pages.Ciba.ViewModel.</returns>
  private ViewModel CreateConsentViewModel(BackchannelUserLoginRequest request)
  {
    var vm = new ViewModel
    {
      ClientName = request.Client.ClientName ?? request.Client.ClientId,
      ClientUrl = request.Client.ClientUri,
      ClientLogoUrl = request.Client.LogoUri,
      BindingMessage = request.BindingMessage,
      IdentityScopes = request.ValidatedResources.Resources.IdentityResources
        .Select(x => CreateScopeViewModel(x, Input == null || Input.ScopesConsented.Contains(x.Name)))
        .ToArray()
    };

    var resourceIndicators = request.RequestedResourceIndicators ?? [];
    var apiResources = request.ValidatedResources.Resources.ApiResources.Where(x => resourceIndicators.Contains(x.Name));

    var apiScopes = new List<ScopeViewModel>();
    foreach (var parsedScope in request.ValidatedResources.ParsedScopes)
    {
      var apiScope = request.ValidatedResources.Resources.FindApiScope(parsedScope.ParsedName);
      if (apiScope != null)
      {
        var scopeVm = CreateScopeViewModel(parsedScope, apiScope, Input == null || Input.ScopesConsented.Contains(parsedScope.RawValue));
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
      apiScopes.Add(GetOfflineAccessScope(Input == null || Input.ScopesConsented.Contains(Duende.IdentityServer.IdentityServerConstants.StandardScopes.OfflineAccess)));
    }
    vm.ApiScopes = apiScopes;

    return vm;
  }

  /// <summary>
  /// Creates the scope view model.
  /// </summary>
  /// <param name="identity">The identity.</param>
  /// <param name="check">The check.</param>
  /// <returns>Landstar.Identity.Pages.Ciba.ScopeViewModel.</returns>
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
  /// <param name="check">The check.</param>
  /// <returns>Landstar.Identity.Pages.Ciba.ScopeViewModel.</returns>
  private static ScopeViewModel CreateScopeViewModel(ParsedScopeValue parsedScopeValue, ApiScope apiScope, bool check)
  {
    var displayName = apiScope.DisplayName ?? apiScope.Name;
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
  /// Gets the offline access scope.
  /// </summary>
  /// <param name="check">The check.</param>
  /// <returns>Landstar.Identity.Pages.Ciba.ScopeViewModel.</returns>
  private static ScopeViewModel GetOfflineAccessScope(bool check)
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
