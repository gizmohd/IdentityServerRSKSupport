// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-22-2024
//
// Last Modified By : 
// Last Modified On : 04-22-2024
// ***********************************************************************
// <copyright file="Callback.cshtml.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Services;
using IdentityExpress.Identity;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Distributed;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Claim = System.Security.Claims.Claim;


namespace Landstar.Identity.Pages.ExternalLogin;

/// <summary>
/// Class Callback.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
[AllowAnonymous]
[SecurityHeaders]
public class Callback(
    IIdentityServerInteractionService interaction,
    IEventService events,
    ILogger<Callback> logger,
    UserManager<IdentityExpressUser> userManager,
    IDistributedCache _cache,
    SignInManager<IdentityExpressUser> signInManager) : PageModel
{
  /// <summary>
  /// On get as an asynchronous operation.
  /// </summary>
  /// <returns>A Task&lt;Microsoft.AspNetCore.Mvc.IActionResult&gt; representing the asynchronous operation.</returns>
  /// <exception cref="InvalidOperationException">$"External authentication error: {result.Failure}</exception>
  public async Task<IActionResult> OnGetAsync(string data = null)
  {
    

    // read external identity from the temporary cookie
    AuthenticateResult result = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
    ClaimsPrincipal externalUser = result.Principal ??
       throw new InvalidOperationException("External authentication produced a null Principal");

    Claim userIdClaim =
                  externalUser.FindFirst(JwtClaimTypes.PreferredUserName) ?? //This line 'may' need to go away when rolled out
                  externalUser.FindFirst(JwtClaimTypes.Subject) ??
                  externalUser.FindFirst(ClaimTypes.NameIdentifier) ??
                  throw new InvalidOperationException("Unknown userid");

    string userId = userIdClaim.Value;

    if (!result.Succeeded)
    {
      throw new InvalidOperationException($"External authentication error: {result.Failure}");
    }



    if (logger.IsEnabled(LogLevel.Debug))
    {
      IEnumerable<string> externalClaims = externalUser.Claims.Select(c => $"{c.Type}: {c.Value}");
      logger.ExternalClaims(externalClaims);
    }

    // lookup our user and external provider info
    // try to determine the unique id of the external user (issued by the provider)
    // the most common claim type for that are the sub claim and the NameIdentifier
    // depending on the external provider, some other claim type might be used
    userIdClaim = externalUser.FindFirst(JwtClaimTypes.Subject) ??
                  externalUser.FindFirst(ClaimTypes.NameIdentifier) ??
                  throw new InvalidOperationException("Unknown userid");

    string provider = result.Properties.Items["scheme"] ?? throw new InvalidOperationException("Null scheme in authentiation properties");
    string providerUserId = userIdClaim.Value;
    string preferredUserName = externalUser.FindFirst(JwtClaimTypes.PreferredUserName)?.Value;


    // find external user
    IdentityExpressUser user = await userManager.FindByLoginAsync(provider, providerUserId);
    if (user == null && (providerUserId.Contains('@') || preferredUserName?.Contains('@') == true)) //Dont want to auto provision LSOL users.
    {
      // this might be where you might initiate a custom workflow for user registration
      // in this sample we don't show how that would be done, as our sample implementation
      // simply auto-provisions new external user
      user = await AutoProvisionUserAsync(provider, providerUserId, preferredUserName, externalUser.Claims);
    }

    if (user == null && !providerUserId.Contains('@'))
    {
      List<Claim> filtered = GetFilteredClaims(externalUser.Claims);

      string emailAddress = GetUserEmailAddress(externalUser.Claims, filtered);


      user = MapUserProvisioning(providerUserId, emailAddress, externalUser.Claims);
    }
    // this allows us to collect any additional claims or properties
    // for the specific protocols used and store them in the local auth cookie.
    // this is typically used to store data needed for signout from those protocols.
    List<Claim> additionalLocalClaims = new List<Claim>();
    AuthenticationProperties localSignInProps = new AuthenticationProperties();
    CaptureExternalLoginContext(result, additionalLocalClaims, localSignInProps);

    // issue authentication cookie for user
    await signInManager.SignInWithClaimsAsync(user, localSignInProps, additionalLocalClaims);

    // delete temporary cookie used during external authentication
    await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

    // retrieve return URL
    string returnUrl = result.Properties.Items["returnUrl"] ?? "/";

    // check if external login is in the context of an OIDC request
    Duende.IdentityServer.Models.AuthorizationRequest context = await interaction.GetAuthorizationContextAsync(returnUrl);
    await events.RaiseAsync(new UserLoginSuccessEvent(provider, providerUserId, user!.Id, user!.UserName, true, context?.Client.ClientId));

    Telemetry.Metrics.UserLogin(context?.Client.ClientId, provider!);
    CacheUserClaims(user, additionalLocalClaims);


    if (context != null && context.IsNativeClient())
    {
      // The client is native, so this change in how to
      // return the response is for better UX for the end user.
      return this.LoadingPage(returnUrl);
    }

    return Redirect(returnUrl);
  }


  /// <summary>
  /// Maps the user provisioning.
  /// </summary>
  /// <param name="providerUserId">The provider user identifier.</param>
  /// <param name="emailAddress">The email address.</param>
  /// <param name="claims">The claims.</param>
  /// <returns>IdentityExpress.Identity.IdentityExpressUser.</returns>
  public static IdentityExpressUser MapUserProvisioning(string providerUserId, string emailAddress, IEnumerable<Claim> claims)
  {
    IdentityExpressUser user = new(providerUserId)
    {
      Id = claims.FirstOrDefault(a => a.Type.Equals(JwtClaimTypes.Subject, StringComparison.OrdinalIgnoreCase))?.Value ?? providerUserId.ToSha256(),
      Email = emailAddress,
      UserName = providerUserId,
      FirstName = claims.FirstOrDefault(a => a.Type.Equals(JwtClaimTypes.GivenName) && !string.IsNullOrEmpty(a.Value))?.Value,
      LastName = claims.FirstOrDefault(a => a.Type.Equals(JwtClaimTypes.FamilyName) && !string.IsNullOrEmpty(a.Value))?.Value,
      NormalizedUserName = providerUserId?.ToUpper()
    };
    user.NormalizedFirstName = user.FirstName?.ToUpper();
    user.NormalizedLastName = user.LastName?.ToUpper();
    user.NormalizedEmail = emailAddress?.ToUpper();

    claims.Select(a => new IdentityExpressClaim { ClaimValue = a.Value, ClaimType = a.Type })
            .ToList()
            .ForEach(c => user.Claims.Add(c));

    return user;
  }
  /// <summary>
  /// Gets the user email address.
  /// </summary>
  /// <param name="claims">The claims.</param>
  /// <param name="filtered">The filtered.</param>
  /// <returns>string.</returns>
  private static string GetUserEmailAddress(IEnumerable<Claim> claims, List<Claim> filtered)
  {
    string emailAddress = null;
    if (!filtered.Exists(a => a.Type.Equals(JwtClaimTypes.Email)))
    {
      emailAddress = claims.FirstOrDefault(x => x.Type.Equals(JwtClaimTypes.Email) ||
                                                x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"))?.Value;
      if (emailAddress != null)
      {
        filtered.Add(new Claim(JwtClaimTypes.Email, emailAddress));
      }
    }


    emailAddress = filtered.Find(a => a.Type.Equals(JwtClaimTypes.Email) && !string.IsNullOrEmpty(a.Value))?.Value;
    return emailAddress;
  }
  /// <summary>
  /// Gets the filtered claims.
  /// </summary>
  /// <param name="claims">The claims.</param>
  /// <returns>System.Collections.Generic.List&lt;System.Security.Claims.Claim&gt;.</returns>
  private static List<Claim> GetFilteredClaims(IEnumerable<Claim> claims)
  {
    List<Claim> filtered = new List<Claim>();
    // create new user


    foreach (Claim claim in claims)
    {
      // if the external system sends a display name - translate that to the standard OIDC name claim
      if (claim.Type.Equals(ClaimTypes.Name))
      {
        filtered.Add(new Claim(JwtClaimTypes.Name, claim.Value));
      }
      // if the JWT handler has an outbound mapping to an OIDC claim use that
      else if (JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.TryGetValue(claim.Type, out string value))
      {
        filtered.Add(new Claim(value, claim.Value));
      }
      // copy the claim as-is
      else
      {
        filtered.Add(claim);
      }
    }
    // if no display name was provided, try to construct by first and/or last name
    if (!filtered.Exists(x => x.Type.Equals(JwtClaimTypes.Name)))
    {
      string first = filtered.Find(x => x.Type.Equals(JwtClaimTypes.GivenName))?.Value;
      string last = filtered.Find(x => x.Type.Equals(JwtClaimTypes.FamilyName))?.Value;

      if (first != null && last != null)
      {
        filtered.Add(new Claim(JwtClaimTypes.Name, first + " " + last));
      }
      else if (first != null)
      {
        filtered.Add(new Claim(JwtClaimTypes.Name, first));
      }
      else if (last != null)
      {
        filtered.Add(new Claim(JwtClaimTypes.Name, last));
      }
    }
    return filtered;
  }


  /// <summary>
  /// Automatic provision user as an asynchronous operation.
  /// </summary>
  /// <param name="provider">The provider.</param>
  /// <param name="providerUserId">The provider user identifier.</param>
  /// <param name="preferredUserName">Name of the preferred user.</param>
  /// <param name="claims">The claims.</param>
  /// <returns>A Task&lt;IdentityExpressUser&gt; representing the asynchronous operation.</returns>
  /// <exception cref="System.InvalidOperationException"></exception>
  private async Task<IdentityExpressUser> AutoProvisionUserAsync(string provider, string providerUserId, string preferredUserName, IEnumerable<Claim> claims)
  {
    string sub = Guid.NewGuid().ToString();
    string emailLookup = providerUserId;
    if (!providerUserId.Contains('@') && preferredUserName.Contains('@'))
    {
      emailLookup = preferredUserName;
    }
    IdentityExpressUser user = await userManager.FindByEmailAsync(emailLookup);
    if (user == null)
    {
      user = new IdentityExpressUser
      {
        Id = sub,
        UserName = emailLookup, // don't need a username, since the user will be using an external provider to login
      };

      // email
      string email = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Email)?.Value ??
                  claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
      if (email != null)
      {
        user.Email = email;
      }

      // create a list of claims that we want to transfer into our store
      List<Claim> filtered = new List<Claim>();

      // user's display name
      string name = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Name)?.Value ??
                 claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
      if (name != null)
      {
        filtered.Add(new Claim(JwtClaimTypes.Name, name));
      }
      else
      {
        string first = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.GivenName)?.Value ??
                    claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
        string last = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.FamilyName)?.Value ??
                   claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value;
        if (first != null && last != null)
        {
          filtered.Add(new Claim(JwtClaimTypes.Name, first + " " + last));
        }
        else if (first != null)
        {
          filtered.Add(new Claim(JwtClaimTypes.Name, first));
        }
        else if (last != null)
        {
          filtered.Add(new Claim(JwtClaimTypes.Name, last));
        }
      }

      IdentityResult identityResult = await userManager.CreateAsync(user);
      if (!identityResult.Succeeded)
      {
        throw new InvalidOperationException(identityResult.Errors.First().Description);
      }

      if (filtered.Count != 0)
      {
        identityResult = await userManager.AddClaimsAsync(user, filtered);
        if (!identityResult.Succeeded)
        {
          throw new InvalidOperationException(identityResult.Errors.First().Description);
        }
      }
    }

    //Set external description for AD
    string externalDescription = provider == "oidc-adfs" ? "Landstar Corporate" : provider;

    IdentityResult identityResult2 = await userManager.AddLoginAsync(user, new UserLoginInfo(provider, providerUserId, externalDescription));
    if (!identityResult2.Succeeded)
    {
      throw new InvalidOperationException(identityResult2.Errors.First().Description);
    }

    return user;
  }
  /// <summary>
  /// Caches the user claims.
  /// </summary>
  /// <param name="user">The user.</param>
  /// <param name="additionalLocalClaims">The additional local claims.</param>
  private void CacheUserClaims(IdentityExpressUser user, List<Claim> additionalLocalClaims)
  {
    List<(string, string)> issuedClaims = [];

    issuedClaims.AddRange(
      [
        ( JwtClaimTypes.GivenName, user.FirstName ),
        ( JwtClaimTypes.FamilyName, user.LastName ),
        ( JwtClaimTypes.Email, user.Email )
      ]);

    IEnumerable<Claim> groups = additionalLocalClaims.Where(a =>
          a.Type.Equals("groups", StringComparison.OrdinalIgnoreCase) &&
          a.Value.StartsWith("G", StringComparison.OrdinalIgnoreCase)
    );

    if (groups.Any())
    {
      issuedClaims.AddRange(groups.Select(a => (a.Type, a.Value)));
    }

    string json = Newtonsoft.Json.JsonConvert.SerializeObject(issuedClaims);

    _cache.SetString($"{user.UserName}__claims", json, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1) });
  }
  // if the external login is OIDC-based, there are certain things we need to preserve to make logout work
  // this will be different for WS-Fed, SAML2p or other protocols
  /// <summary>
  /// Captures the external login context.
  /// </summary>
  /// <param name="externalResult">The external result.</param>
  /// <param name="localClaims">The local claims.</param>
  /// <param name="localSignInProps">The local sign in props.</param>
  private static void CaptureExternalLoginContext(AuthenticateResult externalResult, List<Claim> localClaims, AuthenticationProperties localSignInProps)
  {
    ArgumentNullException.ThrowIfNull(externalResult.Principal);

    // capture the idp used to login, so the session knows where the user came from
    localClaims.Add(new Claim(JwtClaimTypes.IdentityProvider, externalResult.Properties?.Items["scheme"] ?? "unknown identity provider"));
    List<string> groups = externalResult.Principal.FindAll("groups")
                                    .Select(c => c.Value)
                                    //.Where(a => a.StartsWith("G", StringComparison.OrdinalIgnoreCase)) //Only adding AD Groups otherwise the token is too large.
                                    .ToList();

    localClaims.AddRange(groups.Select(a => new Claim("groups", a)));
    // if the external system sent a session id claim, copy it over
    // so we can use it for single sign-out
    Claim sid = externalResult.Principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.SessionId);
    if (sid != null)
    {
      localClaims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));
    }

    // if the external provider issued an id_token, we'll keep it for signout
    string idToken = externalResult.Properties?.GetTokenValue("id_token");
    if (idToken != null)
    {
      localSignInProps.StoreTokens([new AuthenticationToken { Name = "id_token", Value = idToken }]);
    }
  }
}
