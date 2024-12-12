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

using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Landstar.Identity.Pages.ServerSideSessions;

/// <summary>
/// Class IndexModel.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
/// <remarks>
/// Initializes a new instance of the <see cref="IndexModel"/> class.
/// </remarks>
/// <param name="sessionManagementService">The session management service.</param>
public class IndexModel(ISessionManagementService sessionManagementService = null) : PageModel
{

  /// <summary>
  /// Gets or sets the user sessions.
  /// </summary>
  /// <value>The user sessions.</value>
  public QueryResult<UserSession> UserSessions { get; set; }

  /// <summary>
  /// Gets or sets the display name filter.
  /// </summary>
  /// <value>The display name filter.</value>
  [BindProperty(SupportsGet = true)]
  public string DisplayNameFilter { get; set; }

  /// <summary>
  /// Gets or sets the session identifier filter.
  /// </summary>
  /// <value>The session identifier filter.</value>
  [BindProperty(SupportsGet = true)]
  public string SessionIdFilter { get; set; }

  /// <summary>
  /// Gets or sets the subject identifier filter.
  /// </summary>
  /// <value>The subject identifier filter.</value>
  [BindProperty(SupportsGet = true)]
  public string SubjectIdFilter { get; set; }

  /// <summary>
  /// Gets or sets the token.
  /// </summary>
  /// <value>The token.</value>
  [BindProperty(SupportsGet = true)]
  public string Token { get; set; }

  /// <summary>
  /// Gets or sets the previous.
  /// </summary>
  /// <value>The previous.</value>
  [BindProperty(SupportsGet = true)]
  public string Prev { get; set; }

  /// <summary>
  /// On get as an asynchronous operation.
  /// </summary>
  /// <returns>A Task representing the asynchronous operation.</returns>
  public async Task OnGetAsync(CancellationToken cancellationToken)
  {
    if (sessionManagementService != null)
    {
      UserSessions = await sessionManagementService.QuerySessionsAsync(new SessionQuery
      {
        ResultsToken = Token,
        RequestPriorResults = Prev == "true",
        DisplayName = DisplayNameFilter,
        SessionId = SessionIdFilter,
        SubjectId = SubjectIdFilter
      }, cancellationToken:cancellationToken);
    }
  }

  /// <summary>
  /// Gets or sets the session identifier.
  /// </summary>
  /// <value>The session identifier.</value>
  [BindProperty]
  public string SessionId { get; set; }

  /// <summary>
  /// On post as an asynchronous operation.
  /// </summary>
  /// <returns>A Task&lt;IActionResult&gt; representing the asynchronous operation.</returns>
  /// <exception cref="System.ArgumentNullException"></exception>
  public Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
  {
    ArgumentNullException.ThrowIfNull(sessionManagementService);
    return InternalOnPostAsync(cancellationToken);
  }
  /// <summary>
  /// Internal on post as an asynchronous operation.
  /// </summary>
  /// <returns>A Task&lt;IActionResult&gt; representing the asynchronous operation.</returns>
  async Task<IActionResult> InternalOnPostAsync( CancellationToken cancellationToken)
  {


    await sessionManagementService!.RemoveSessionsAsync(new RemoveSessionsContext
    {
      SessionId = SessionId,
    }, cancellationToken);
    return RedirectToPage("/ServerSideSessions/Index", new { Token, DisplayNameFilter, SessionIdFilter, SubjectIdFilter, Prev });
  }
}

