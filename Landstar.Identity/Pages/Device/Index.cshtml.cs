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
using Landstar.Identity.Pages.Consent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;



namespace Landstar.Identity.Pages.Device;

/// <summary>
/// Class Index.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
[SecurityHeaders]
[Authorize]
public class Index(
    IDeviceFlowInteractionService interaction,
    IEventService eventService
      ) : PageModel
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
  /// <param name="userCode">The user code.</param>
  /// <returns>A Task&lt;IActionResult&gt; representing the asynchronous operation.</returns>
  public async Task<IActionResult> OnGetAsync(string userCode)
  {
    if (String.IsNullOrWhiteSpace(userCode))
    {
      return Page();
    }

    if (!await SetViewModelAsync(userCode))
    {
      ModelState.AddModelError("", DeviceOptions.InvalidUserCode);
      return Page();
    }

    Input = new InputModel
    {
      UserCode = userCode,
    };

    return Page();
  }

  /// <summary>
  /// Called when [post].
  /// </summary>
  /// <returns>IActionResult.</returns>
  /// <exception cref="System.ArgumentNullException">UserCode</exception>
  public Task<IActionResult> OnPostAsync()
  {
    ArgumentNullException.ThrowIfNull(Input.UserCode);
    return InternalOnPostAsync();
  }

  async Task<IActionResult> InternalOnPostAsync()
  {

    var request = await interaction.GetAuthorizationContextAsync(Input.UserCode!);
    if (request == null)
    {
      return RedirectToPage("/Home/Error/Index");
    }
    ConsentResponse grantedConsent = null;

    // user clicked 'no' - send back the standard 'access_denied' response
    if (Input.Button == "no")
    {
      grantedConsent = new ConsentResponse
      {
        Error = AuthorizationError.AccessDenied
      };

      // emit event
      await eventService.RaiseAsync(new ConsentDeniedEvent(User.GetSubjectId(), request.Client.ClientId, request.ValidatedResources.RawScopeValues));
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

        grantedConsent = new ConsentResponse
        {
          RememberConsent = Input.RememberConsent,
          ScopesValuesConsented = scopes.ToArray(),
          Description = Input.Description
        };

        // emit event
        await eventService.RaiseAsync(new ConsentGrantedEvent(User.GetSubjectId(), request.Client.ClientId, request.ValidatedResources.RawScopeValues, grantedConsent.ScopesValuesConsented, grantedConsent.RememberConsent));
        Telemetry.Metrics.ConsentGranted(request.Client.ClientId, grantedConsent.ScopesValuesConsented, grantedConsent.RememberConsent);
        var denied = request.ValidatedResources.ParsedScopes.Select(s => s.ParsedName).Except(grantedConsent.ScopesValuesConsented);
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
      // communicate outcome of consent back to identityserver
      await interaction.HandleRequestAsync(Input.UserCode, grantedConsent);

      // indicate that's it ok to redirect back to authorization endpoint
      return RedirectToPage("/Device/Success");
    }

    // we need to redisplay the consent UI
    if (!await SetViewModelAsync(Input.UserCode))
    {
      return RedirectToPage("/Home/Error/Index");
    }
    return Page();
  }


  /// <summary>
  /// Set view model as an asynchronous operation.
  /// </summary>
  /// <param name="userCode">The user code.</param>
  /// <returns>A Task&lt;System.Boolean&gt; representing the asynchronous operation.</returns>
  private async Task<bool> SetViewModelAsync(string userCode)
  {
    var request = await interaction.GetAuthorizationContextAsync(userCode);
    if (request != null)
    {
      View = CreateConsentViewModel(request);
      return true;
    }
    else
    {
      View = new ViewModel();
      return false;
    }
  }

  /// <summary>
  /// Creates the consent view model.
  /// </summary>
  /// <param name="request">The request.</param>
  /// <returns>ViewModel.</returns>
  private ViewModel CreateConsentViewModel(DeviceFlowAuthorizationRequest request)
  {
    var vm = new ViewModel
    {
      ClientName = request.Client.ClientName ?? request.Client.ClientId,
      ClientUrl = request.Client.ClientUri,
      ClientLogoUrl = request.Client.LogoUri,
      AllowRememberConsent = request.Client.AllowRememberConsent,
      IdentityScopes = request.ValidatedResources.Resources.IdentityResources.Select(x => CreateScopeViewModel(x, Input == null || Input.ScopesConsented.Contains(x.Name))).ToArray()
    };

    var apiScopes = new List<ScopeViewModel>();
    foreach (var parsedScope in request.ValidatedResources.ParsedScopes)
    {
      var apiScope = request.ValidatedResources.Resources.FindApiScope(parsedScope.ParsedName);
      if (apiScope != null)
      {
        var scopeVm = CreateScopeViewModel(parsedScope, apiScope, Input == null || Input.ScopesConsented.Contains(parsedScope.RawValue));
        apiScopes.Add(scopeVm);
      }
    }
    if (DeviceOptions.EnableOfflineAccess && request.ValidatedResources.Resources.OfflineAccess)
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
  /// <param name="check">if set to <see langword="true" /> [check].</param>
  /// <returns>ScopeViewModel.</returns>
  private static ScopeViewModel CreateScopeViewModel(IdentityResource identity, bool check)
  {
    return new ScopeViewModel
    {
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
    return new ScopeViewModel
    {
      Value = parsedScopeValue.RawValue,
      // todo: use the parsed scope value in the display?
      DisplayName = apiScope.DisplayName ?? apiScope.Name,
      Description = apiScope.Description,
      Emphasize = apiScope.Emphasize,
      Required = apiScope.Required,
      Checked = check || apiScope.Required
    };
  }

  /// <summary>
  /// Gets the offline access scope.
  /// </summary>
  /// <param name="check">if set to <see langword="true" /> [check].</param>
  /// <returns>ScopeViewModel.</returns>
  private static ScopeViewModel GetOfflineAccessScope(bool check)
  {
    return new ScopeViewModel
    {
      Value = Duende.IdentityServer.IdentityServerConstants.StandardScopes.OfflineAccess,
      DisplayName = DeviceOptions.OfflineAccessDisplayName,
      Description = DeviceOptions.OfflineAccessDescription,
      Emphasize = true,
      Checked = check
    };
  }
}
