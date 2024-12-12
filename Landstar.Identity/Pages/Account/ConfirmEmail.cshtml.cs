// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 05-14-2024
//
// Last Modified By : 
// Last Modified On : 05-14-2024
// ***********************************************************************
// <copyright file="ConfirmEmail.cshtml.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using IdentityExpress.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using SerilogTimings;
using System.Text;



namespace Landstar.Identity.Pages.Account;

/// <summary>
/// Class ConfirmEmailModel.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
[AllowAnonymous]
public class ConfirmEmailModel(UserManager<IdentityExpressUser> userManager) : PageModel
{
  /// <summary>
  /// Gets or sets the status message.
  /// </summary>
  /// <value>The status message.</value>
  [TempData]
  public string StatusMessage { get; set; }
  /// <summary>
  /// Gets or sets the redirect URI.
  /// </summary>
  /// <value>The redirect URI.</value>
  public string RedirectUri { get; set; }
  /// <summary>
  /// Called when [get asynchronous].
  /// </summary>
  /// <param name="userId">The user identifier.</param>
  /// <param name="code">The code.</param>
  /// <param name="redirectUrl">The redirect URL.</param>
  /// <param name="data">compressed object data</param>
  /// <returns>A Task&lt;IActionResult&gt; representing the asynchronous operation.</returns>
  public async Task<IActionResult> OnGetAsync(string userId = null, string code = null, string redirectUrl = null, string data = null)
  {
    Operation operation = Operation.At(Serilog.Events.LogEventLevel.Information, Serilog.Events.LogEventLevel.Warning)
                             .Begin("Confirm Email for user");
    try
    {


      string _code;
      string _userid = userId;
      if (!string.IsNullOrEmpty(data))
      {
        data = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(data));
        //var json = data.DecompressString();
        //dynamic obj = JsonConvert.DeserializeObject<dynamic>(json);
        //_userid = obj.userId;
        //_code = obj.code;
        //operation.EnrichWith("UserName", _userid);
        //RedirectUri = obj.redirectUrl;
      }
      else
      {
        operation.EnrichWith("UserName", _userid);
        if (string.IsNullOrWhiteSpace(_userid) || string.IsNullOrWhiteSpace(code))
        {
          StatusMessage = "Invalid Validation Link";
          return Page();
        }

        _code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        RedirectUri = redirectUrl ?? "/";

      }

      IdentityExpressUser user = await userManager.FindByIdAsync(_userid).ConfigureAwait(false);

      if (user == null)
      {
        StatusMessage = $"Unable to load user with ID: '{userId}'.";
        return Page();

      }



      IdentityResult result = await userManager.ConfirmEmailAsync(user, "").ConfigureAwait(false);

      StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
      operation.EnrichWith("Succeeded", result.Succeeded);
      if (!result.Succeeded)
      {
        operation.EnrichWith("Errors", result.Errors, true);
        operation.Abandon();
      }
      else
      {
        operation.Complete();
      }

      return Page();
    }
    catch (System.Exception e)
    {
      operation.Abandon(e);
      throw;
    }
  }
}
