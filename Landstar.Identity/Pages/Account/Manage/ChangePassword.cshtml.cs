// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 08-20-2024
//
// Last Modified By : 
// Last Modified On : 08-20-2024
// ***********************************************************************
// <copyright file="ChangePassword.cshtml.cs" company="Landstar.Identity">
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
/// Class ChangePasswordModel.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
/// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
/// <param name="userManager">The user manager.</param>
/// <param name="signInManager">The sign in manager.</param>
/// <param name="logger">The logger.</param>
/// <remarks>Initializes a new instance of the <see cref="ChangePasswordModel" /> class.</remarks>
public class ChangePasswordModel(
    UserManager<IdentityExpressUser> userManager,
    SignInManager<IdentityExpressUser> signInManager,
    ILogger<ChangePasswordModel> logger) : PageModel
{
  /// <summary>
  /// Gets or sets the input.
  /// </summary>
  /// <value>The input.</value>
  [BindProperty]
  public InputModel Input { get; set; }

  /// <summary>
  /// Gets or sets the status message.
  /// </summary>
  /// <value>The status message.</value>
  [TempData]
  public string StatusMessage { get; set; }
  /// <summary>
  /// INput Model
  /// </summary>
  public class InputModel
  {
    /// <summary>
    /// Gets or sets the old password.
    /// </summary>
    /// <value>The old password.</value>
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Current password")]
    public string OldPassword { get; set; }

    /// <summary>
    /// Creates new password.
    /// </summary>
    /// <value>The new password.</value>
    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "New password")]
    public string NewPassword { get; set; }

    /// <summary>
    /// Gets or sets the confirm password.
    /// </summary>
    /// <value>The confirm password.</value>
    [DataType(DataType.Password)]
    [Display(Name = "Confirm new password")]
    [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
  }
  /// <summary>
  /// Called when [get asynchronous].
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

    var hasPassword = await userManager.HasPasswordAsync(user).ConfigureAwait(false);
    if (!hasPassword)
    {
      return RedirectToPage("./SetPassword");
    }

    return Page();
  }
  /// <summary>
  /// Called when [post asynchronous].
  /// </summary>
  /// <param name="returnUrl">The return URL.</param>
  /// <returns>A Task&lt;IActionResult&gt; representing the asynchronous operation.</returns>
  public async Task<IActionResult> OnPostAsync(string returnUrl = null)
  {
    
    if (!ModelState.IsValid)
    {
      return Page();
    }

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

    var changePasswordResult = await userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword).ConfigureAwait(false);
    if (!changePasswordResult.Succeeded)
    {
      foreach (var error in changePasswordResult.Errors)
      {
        ModelState.AddModelError(string.Empty, error.Description);
      }
      return Page();
    }

    await signInManager.RefreshSignInAsync(user).ConfigureAwait(false);
    logger.LogInformation("User changed their password successfully.");
    StatusMessage = "Your password has been changed.";
    if (returnUrl != null) {
      return Redirect(returnUrl);
    }
    return RedirectToPage();
  }
}
