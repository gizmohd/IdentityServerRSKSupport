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
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Landstar.Identity.Pages.Grants;

/// <summary>
/// Class Index.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
[SecurityHeaders]
[Authorize]
public class Index(IIdentityServerInteractionService interaction,
    IClientStore clients,
    IResourceStore _resources,
    IEventService events) : PageModel
{

  /// <summary>
  /// Gets or sets the view.
  /// </summary>
  /// <value>The view.</value>
  public ViewModel View { get; set; } = default!;

  /// <summary>
  /// On get as an asynchronous operation.
  /// </summary>
  /// <returns>A Task representing the asynchronous operation.</returns>
  public async Task OnGetAsync()
  {
    var grants = await interaction.GetAllUserGrantsAsync();

    var list = new List<GrantViewModel>();
    foreach (var grant in grants)
    {
      var client = await clients.FindClientByIdAsync(grant.ClientId);
      if (client != null)
      {
        var resources = await _resources.FindResourcesByScopeAsync(grant.Scopes);

        var item = new GrantViewModel
        {
          ClientId = client.ClientId,
          ClientName = client.ClientName ?? client.ClientId,
          ClientLogoUrl = client.LogoUri,
          ClientUrl = client.ClientUri,
          Description = grant.Description,
          Created = grant.CreationTime,
          Expires = grant.Expiration,
          IdentityGrantNames = resources.IdentityResources.Select(x => x.DisplayName ?? x.Name).ToArray(),
          ApiGrantNames = resources.ApiScopes.Select(x => x.DisplayName ?? x.Name).ToArray()
        };

        list.Add(item);
      }
    }

    View = new ViewModel
    {
      Grants = list
    };
  }

  /// <summary>
  /// Gets or sets the client identifier.
  /// </summary>
  /// <value>The client identifier.</value>
  [BindProperty]
  public string ClientId { get; set; }

  /// <summary>
  /// On post as an asynchronous operation.
  /// </summary>
  /// <returns>A Task&lt;IActionResult&gt; representing the asynchronous operation.</returns>
  public async Task<IActionResult> OnPostAsync()
  {
    await interaction.RevokeUserConsentAsync(ClientId);
    await events.RaiseAsync(new GrantsRevokedEvent(User.GetSubjectId(), ClientId));
    Telemetry.Metrics.GrantsRevoked(ClientId);

    return RedirectToPage("/Grants/Index");
  }
}
