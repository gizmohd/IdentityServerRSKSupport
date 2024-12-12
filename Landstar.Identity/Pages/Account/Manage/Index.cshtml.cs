// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 08-20-2024
//
// Last Modified By : 
// Last Modified On : 09-27-2024
// ***********************************************************************
// <copyright file="Index.cshtml.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using IdentityExpress.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;



namespace Landstar.Identity.Pages.Account.Manage;

/// <summary>
/// Class IndexModel.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
[Authorize]
public class IndexModel(
    UserManager<IdentityExpressUser> userManager,
    IConfiguration configuration,
    IHttpContextAccessor httpContextAccessor,
    SignInManager<IdentityExpressUser> signInManager) : PageModel
{

  /// <summary>
  /// Gets or sets the username.
  /// </summary>
  /// <value>The username.</value>
  public string Username { get; set; }
  /// <summary>
  /// Gets or sets the phone number.
  /// </summary>
  /// <value>The phone number.</value>
  [Phone]
  [Display(Name = "Phone number")]
  public string PhoneNumber { get; set; }

  /// <summary>
  /// Gets or sets the status message.
  /// </summary>
  /// <value>The status message.</value>
  [TempData]
  public string StatusMessage { get; set; }

  /// <summary>
  /// Gets or sets the input.
  /// </summary>
  /// <value>The input.</value>
  [BindProperty]
  public InputModel Input { get; set; }

  /// <summary>
  /// Load as an asynchronous operation.
  /// </summary>
  /// <param name="user">The user.</param>
  /// <returns>A Task representing the asynchronous operation.</returns>
  private async Task LoadAsync(IdentityExpressUser user)
  {
    Input ??= new();

    var userName = await userManager.GetUserNameAsync(user).ConfigureAwait(false);

    PhoneNumber = user.PhoneNumber;
    Input.FirstName = user.FirstName;
    Input.LastName = user.LastName;
    Username = userName;

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
    user ??= await userManager.FindByEmailAsync(User.Identity.Name).ConfigureAwait(false);
    if (user != null)
    {
      await LoadAsync(user).ConfigureAwait(false);
    }
    else
    {
      //Redirect user?
      var lUser = httpContextAccessor.HttpContext?.User;

      var clientId = lUser?.FindFirst("idp")?.Value;
      if (clientId == "oidc")
      {
        var sm = httpContextAccessor.HttpContext?.Request.Cookies["SMIDENTITY"];
        if (sm == null)
        {
          return Redirect("/Account/LogoffWithRedirect?post_logout_redirect_uri=%2F");
        }
        var redirectUrl = configuration["Authentication:SiteMinder:LPW"];
        return Redirect(redirectUrl);
      }
    }
    return Page();
  }

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

    if (!ModelState.IsValid)
    {
      await LoadAsync(user).ConfigureAwait(false);
      return Page();
    }

    user.FirstName = Input.FirstName;

    user.LastName = Input.LastName;

    await userManager.UpdateAsync(user).ConfigureAwait(false);


    await signInManager.RefreshSignInAsync(user).ConfigureAwait(false);

    StatusMessage = "Your profile has been updated";

    return RedirectToPage();
  }

  /// <summary>
  /// Class InputModel.
  /// </summary>
  public class InputModel
  {
    /// <summary>
    /// Gets or sets the first name.
    /// </summary>
    /// <value>The first name.</value>
    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name.
    /// </summary>
    /// <value>The last name.</value>
    [Display(Name = "Last Name")]
    public string LastName { get; set; }

  }
}
