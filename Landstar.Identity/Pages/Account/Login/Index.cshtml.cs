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

using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using IdentityExpress.Identity;

using Landstar.Identity.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Landstar.Identity.Pages.Account.Login;

/// <summary>
/// Class Index.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
[SecurityHeaders]
[AllowAnonymous]
public class Index(
    IIdentityServerInteractionService interaction,
    IAuthenticationSchemeProvider schemeProvider,
    IIdentityProviderStore identityProviderStore,
    IEventService events,
    ApplicationDbContext applicationDbContext,
    UserManager<IdentityExpressUser> userManager,
    SignInManager<IdentityExpressUser> signInManager) : PageModel
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
  /// <returns>A Task&lt;Microsoft.AspNetCore.Mvc.IActionResult&gt; representing the asynchronous operation.</returns>
  public async Task<IActionResult> OnGetAsync(string returnUrl)
  {
    await BuildModelAsync(returnUrl);
    
    Input ??= new();

    _ = bool.TryParse(Landstar.Identity.ConfigurationExtensions.Configuration["EnableRegisterFromLogin"], out bool EnableRegistrationLink);

    Input.EnableRegistrationLink = EnableRegistrationLink;

    if (View.IsExternalLoginOnly)
    {
      // we only have one option for logging in and it's an external provider
      return RedirectToPage("/ExternalLogin/Challenge", new { scheme = View.ExternalLoginScheme, returnUrl });
    }

    return Page();
  }

  /// <summary>
  /// On post as an asynchronous operation.
  /// </summary>
  /// <returns>A Task&lt;Microsoft.AspNetCore.Mvc.IActionResult&gt; representing the asynchronous operation.</returns>
  /// <exception cref="ArgumentException">invalid return URL</exception>
  public async Task<IActionResult> OnPostAsync()
  {
    
    // check if we are in the context of an authorization request
    AuthorizationRequest context = await interaction.GetAuthorizationContextAsync(Input.ReturnUrl);

    // the user clicked the "cancel" button
    if (Input.Button != "login")
    {
      if (context != null)
      {
        // This "can't happen", because if the ReturnUrl was null, then the context would be null
        ArgumentNullException.ThrowIfNull(Input.ReturnUrl);

        // if the user cancels, send a result back into IdentityServer as if they 
        // denied the consent (even if this client does not require consent).
        // this will send back an access denied OIDC error response to the client.
        await interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

        // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
        if (context.IsNativeClient())
        {
          // The client is native, so this change in how to
          // return the response is for better UX for the end user.
          return this.LoadingPage(Input.ReturnUrl);
        }

        return Redirect(Input.ReturnUrl ?? "~/");
      }

      // since we don't have a valid context, then we just go back to the home page
      return Redirect("~/");

    }

    if (ModelState.IsValid)
    {
      Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(Input.Username!, Input.Password!, Input.RememberLogin, lockoutOnFailure: true);
      bool userPrefersSms = applicationDbContext.CheckIfUserPrefersSmsOverTotp(Input.Username);

      if (result.RequiresTwoFactor )
      {

       
          return RedirectToPage("../LoginWith2fa", new
          {
            Input.ReturnUrl,
            RememberMe = Input.RememberLogin
          });
       
      }

      else if (result.Succeeded)
      {
        IdentityExpressUser user = await userManager.FindByNameAsync(Input.Username!);
        await events.RaiseAsync(new UserLoginSuccessEvent(user!.UserName, user.Id, user.UserName, clientId: context?.Client.ClientId));
        Telemetry.Metrics.UserLogin(context?.Client.ClientId, IdentityServerConstants.LocalIdentityProvider);

        if (context != null)
        {
          // This "can't happen", because if the ReturnUrl was null, then the context would be null
          ArgumentNullException.ThrowIfNull(Input.ReturnUrl);

          if (context.IsNativeClient())
          {
            // The client is native, so this change in how to }
            if (result.RequiresTwoFactor)
            {
              return RedirectToPage("./LoginWith2fa", new
              {
                Input.ReturnUrl,
                RememberMe = Input.RememberLogin
              });
            }
            // return the response is for better UX for the end user.
            return this.LoadingPage(Input.ReturnUrl);
          }

          // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
          return Redirect(Input.ReturnUrl ?? "~/");
        }

        // request for a local page
        if (Url.IsLocalUrl(Input.ReturnUrl))
        {
          return Redirect(Input.ReturnUrl);
        }

        if (string.IsNullOrEmpty(Input.ReturnUrl))
        {
          return Redirect("~/");
        }

        // user might have clicked on a malicious link - should be logged
        throw new ArgumentException("invalid return URL");

      }

      const string error = "invalid credentials";
      await events.RaiseAsync(new UserLoginFailureEvent(Input.Username, error, clientId: context?.Client.ClientId));
      Telemetry.Metrics.UserLoginFailure(context?.Client.ClientId, IdentityServerConstants.LocalIdentityProvider, error);
      ModelState.AddModelError(string.Empty, LoginOptions.InvalidCredentialsErrorMessage);
    }

    // something went wrong, show form with error
    await BuildModelAsync(Input.ReturnUrl);
    return Page();
  }

  /// <summary>
  /// Build model as an asynchronous operation.
  /// </summary>
  /// <param name="returnUrl">The return URL.</param>
  /// <returns>A Task&lt;System.Threading.Tasks.Task&gt; representing the asynchronous operation.</returns>
  private async Task BuildModelAsync(string returnUrl)
  {
    Input = new InputModel
    {
      ReturnUrl = returnUrl
    };

    AuthorizationRequest context = await interaction.GetAuthorizationContextAsync(returnUrl);
    if (context?.IdP != null && await schemeProvider.GetSchemeAsync(context.IdP) != null)
    {
      bool local = context.IdP == IdentityServerConstants.LocalIdentityProvider;

      // this is meant to short circuit the UI and only trigger the one external IdP
      View = new ViewModel
      {
        EnableLocalLogin = local,
      };

      Input.Username = context.LoginHint;

      if (!local)
      {
        View.ExternalProviders = [new ExternalProvider(authenticationScheme: context.IdP)];
      }

      return;
    }

    IEnumerable<AuthenticationScheme> schemes = await schemeProvider.GetAllSchemesAsync();

    List<ExternalProvider> providers = schemes
        .Where(x => x.DisplayName != null)
        .Select(x => new ExternalProvider
        (
            authenticationScheme: x.Name,
            displayName: x.DisplayName ?? x.Name
        )).ToList();

    IEnumerable<ExternalProvider> dynamicSchemes = (await identityProviderStore.GetAllSchemeNamesAsync())
        .Where(x => x.Enabled)
        .Select(x => new ExternalProvider
        (
            authenticationScheme: x.Scheme,
            displayName: x.DisplayName ?? x.Scheme
        ));
    providers.AddRange(dynamicSchemes);


    bool allowLocal = true;
    Client client = context?.Client;
    if (client != null)
    {
      allowLocal = client.EnableLocalLogin;
      if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Count != 0)
      {
        providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
      }
    }

    View = new ViewModel
    {
      AllowRememberLogin = LoginOptions.AllowRememberLogin,
      EnableLocalLogin = allowLocal && LoginOptions.AllowLocalLogin,
      ExternalProviders = [.. providers]
    };
  }
}
