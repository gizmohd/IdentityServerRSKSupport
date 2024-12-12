// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 08-20-2024
//
// Last Modified By : 
// Last Modified On : 09-27-2024
// ***********************************************************************
// <copyright file="DownloadPersonalData.cshtml.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using IdentityExpress.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;


namespace Landstar.Identity.Pages.Account.Manage;

/// <summary>
/// Class DownloadPersonalDataModel.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
public class DownloadPersonalDataModel(
    UserManager<IdentityExpressUser> userManager,
    ILogger<DownloadPersonalDataModel> logger) : PageModel
{
  /// <summary>
  /// On post as an asynchronous operation.
  /// </summary>
  /// <returns>A Task&lt;IActionResult&gt; representing the asynchronous operation.</returns>
  public async Task<IActionResult> OnPostAsync()
  {
    IdentityExpressUser user;
    try
    {
      user = await userManager.GetUserAsync(User).ConfigureAwait(false);
    }
    catch
    {
      //Some lsol users dont have proper guid...
      user = await userManager.FindByNameAsync(User.Identity.Name).ConfigureAwait(false);
    }


    if (user == null)
    {
      user = await userManager.FindByEmailAsync(User.Identity.Name).ConfigureAwait(false);
      if (user == null)
      {
        return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
      }
    }

    logger.LogInformation("User with ID '{UserId}' asked for their personal data.", userManager.GetUserId(User));

    // Only include personal data for download
    var personalData = new Dictionary<string, object>();
    var personalDataProps = typeof(IdentityExpressUser).GetProperties().Where(
                    prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));
 
    foreach (var p in personalDataProps)
    {
      try
      {
        personalData.Add(p.Name, p.GetValue(user) ?? "null");
      }
      catch (Exception pe)
      {
        logger.LogWarning(pe, "Error Pulling Personal Data");
        
      }
      
    }

    var logins = await userManager.GetLoginsAsync(user).ConfigureAwait(false);
   
    foreach (var l in logins)
    {
      personalData.Add($"{l.LoginProvider} external login provider key", l.ProviderKey);
    }

    Response.Headers.Append("Content-Disposition", "attachment; filename=PersonalData.json");
    return new FileContentResult(JsonSerializer.SerializeToUtf8Bytes(personalData), "application/json");
  }
}
