// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 05-14-2024
//
// Last Modified By : 
// Last Modified On : 05-14-2024
// ***********************************************************************
// <copyright file="ResetPassword.cshtml.cs" company="Landstar.Identity">
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
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;


namespace Landstar.Identity.Pages.Account;

/// <summary>
/// Class ResetPasswordModel.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
[AllowAnonymous]
public class ResetPasswordModel(UserManager<IdentityExpressUser> userManager) : PageModel
{
  /// <summary>
  /// Gets or sets the input.
  /// </summary>
  /// <value>The input.</value>
  [BindProperty]
  public InputModel Input { get; set; }

  /// <summary>
  /// Class PhoneUpdatesInputModel.
  /// </summary>
  public class InputModel
  {
    private const int ContentLength = 100;

    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    /// <value>The email.</value>
    [Required]
    [EmailAddress2]
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    /// <value>The password.</value>
    [Required]
    [StringLength(ContentLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets the confirm password.
    /// </summary>
    /// <value>The confirm password.</value>
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }

    /// <summary>
    /// Gets or sets the code.
    /// </summary>
    /// <value>The code.</value>
    public string Code { get; set; }

    /// <summary>
    /// Gets or sets the redirect URI.
    /// </summary>
    /// <value>The redirect URI.</value>
    public string RedirectUri { get; set; }
  }

  /// <summary>
  /// Called when [get].
  /// </summary>
  /// <param name="code">The code.</param>
  /// <param name="redirectUri">The redirect URI.</param>
  /// <returns>IActionResult.</returns>
  public IActionResult OnGet(string code = null, string redirectUri = null)
  {
    if (code == null)
    {
      return BadRequest("A code must be supplied for password reset.");
    }
    
    Input = new InputModel
    {
      Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code)),
      RedirectUri = redirectUri
    };
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

    var user = await userManager.FindByNameAsync(Input.Email).ConfigureAwait(false);
    const string urlParts = "./ResetPasswordConfirmation";

    if (user == null)
    {
      // Don't reveal that the user does not exist
      return RedirectToPage(urlParts, new { Input.RedirectUri });
    }

    var result = await userManager.ResetPasswordAsync(user, Input.Code, Input.Password).ConfigureAwait(false);

    if (result.Succeeded)
    {
      return RedirectToPage(urlParts,new { redirectUrl = Input.RedirectUri });
    }

    foreach (var error in result.Errors)
    {
      ModelState.AddModelError(string.Empty, error.Description);
    }
    return Page();
  }
}
