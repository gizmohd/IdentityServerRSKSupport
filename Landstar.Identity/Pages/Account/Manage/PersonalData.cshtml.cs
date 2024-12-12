// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 08-20-2024
//
// Last Modified By : 
// Last Modified On : 09-27-2024
// ***********************************************************************
// <copyright file="PersonalData.cshtml.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using IdentityExpress.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Landstar.Identity.Pages.Account.Manage;



/// <summary>
/// Class PersonalDataModel.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
public class PersonalDataModel(UserManager<IdentityExpressUser> userManager) : PageModel
{
  /// <summary>
  /// On get as an asynchronous operation.
  /// </summary>
  /// <returns>A Task&lt;IActionResult&gt; representing the asynchronous operation.</returns>
  public async Task<IActionResult> OnGetAsync()
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

    return Page();
  }
}