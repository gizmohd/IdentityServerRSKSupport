// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 08-20-2024
//
// Last Modified By : 
// Last Modified On : 09-27-2024
// ***********************************************************************
// <copyright file="SetPassword.cshtml.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using IdentityExpress.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Landstar.Identity.Pages.Account.Manage
{
  /// <summary>
  /// Class SetPasswordModel.
  /// Implements the <see cref="PageModel" />
  /// </summary>
  /// <seealso cref="PageModel" />
  public class SetPasswordModel(
      UserManager<IdentityExpressUser> userManager,
      SignInManager<IdentityExpressUser> signInManager) : PageModel
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
    /// Class InputModel.
    /// </summary>
    public class InputModel
    {
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

      var hasPassword = await userManager.HasPasswordAsync(user).ConfigureAwait(false);

      if (hasPassword)
      {
        return RedirectToPage("./ChangePassword");
      }

      return Page();
    }

    /// <summary>
    /// On post as an asynchronous operation.
    /// </summary>
    /// <returns>A Task&lt;IActionResult&gt; representing the asynchronous operation.</returns>
    public async Task<IActionResult> OnPostAsync()
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

      var addPasswordResult = await userManager.AddPasswordAsync(user, Input.NewPassword).ConfigureAwait(false);
      if (!addPasswordResult.Succeeded)
      {
        foreach (var error in addPasswordResult.Errors)
        {
          ModelState.AddModelError(string.Empty, error.Description);
        }
        return Page();
      }

      await signInManager.RefreshSignInAsync(user).ConfigureAwait(false);
      StatusMessage = "Your password has been set.";

      return RedirectToPage();
    }
  }
}
