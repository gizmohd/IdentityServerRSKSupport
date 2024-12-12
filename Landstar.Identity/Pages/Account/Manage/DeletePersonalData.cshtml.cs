// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 08-20-2024
//
// Last Modified By : 
// Last Modified On : 08-20-2024
// ***********************************************************************
// <copyright file="DeletePersonalData.cshtml.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using IdentityExpress.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;



namespace Landstar.Identity.Pages.Account.Manage;

/// <summary>
/// Class DeletePersonalDataModel.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
public class DeletePersonalDataModel(
    UserManager<IdentityExpressUser> userManager,
    SignInManager<IdentityExpressUser> signInManager,
    ILogger<DeletePersonalDataModel> logger) : PageModel
{
  /// <summary>
  /// Gets or sets the input.
  /// </summary>
  /// <value>The input.</value>
  [BindProperty]
  public InputModel Input { get; set; }

  /// <summary>
  /// Class InputModel.
  /// </summary>
  public class InputModel
  {
    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    /// <value>The password.</value>
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
  }

  /// <summary>
  /// Gets or sets a value indicating whether [require password].
  /// </summary>
  /// <value><see langword="true" /> if [require password]; otherwise, <see langword="false" />.</value>
  public bool RequirePassword { get; set; }

  /// <summary>
  /// On get as an asynchronous operation.
  /// </summary>
  /// <returns>A Task&lt;IActionResult&gt; representing the asynchronous operation.</returns>
  public async Task<IActionResult> OnGetAsync()
  {
    IdentityExpressUser user ;
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

    RequirePassword = await userManager.HasPasswordAsync(user).ConfigureAwait(false);
    return Page();
  }

  /// <summary>
  /// On post as an asynchronous operation.
  /// </summary>
  /// <returns>A Task&lt;IActionResult&gt; representing the asynchronous operation.</returns>
  /// <exception cref="System.InvalidOperationException">Unexpected error occurred deleting user with ID '{userId}'.</exception>
  public async Task<IActionResult> OnPostAsync()
  {
    IdentityExpressUser user ;
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

    RequirePassword = await userManager.HasPasswordAsync(user).ConfigureAwait(false);

    if (RequirePassword && !await userManager.CheckPasswordAsync(user, Input.Password).ConfigureAwait(false))
    {
      ModelState.AddModelError(string.Empty, "Incorrect password.");
      return Page();
    }


    var result = await userManager.DeleteAsync(user).ConfigureAwait(false);

    var userId = await userManager.GetUserIdAsync(user).ConfigureAwait(false);
    
    if (!result.Succeeded)
    {
      throw new InvalidOperationException($"Unexpected error occurred deleting user with ID '{userId}'.");
    }

    await signInManager.SignOutAsync().ConfigureAwait(false);

    logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

    return Redirect("~/");
  }
}
