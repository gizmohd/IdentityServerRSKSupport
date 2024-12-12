// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-22-2024
//
// Last Modified By : 
// Last Modified On : 04-22-2024
// ***********************************************************************
// <copyright file="ForgotPassword.cshtml.cs" company="Landstar.Identity">
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


namespace Landstar.Identity.Pages.Account;

/// <summary>
/// Class ForgotPasswordModel.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
[AllowAnonymous]
public class ForgotPasswordModel : PageModel
{
  /// <summary>
  /// The user manager
  /// </summary>
  private readonly UserManager<IdentityExpressUser> _userManager;
  
  /// <summary>
  /// The configuration
  /// </summary>
  private readonly IConfiguration _configuration;
  /// <summary>
  /// The allow send unconfirmed email
  /// </summary>
  private readonly bool _allowSendUnconfirmedEmail;
  /// <summary>
  /// The logger
  /// </summary>
  private readonly ILogger<ForgotPasswordModel> _logger;
  /// <summary>
  /// Initializes a new instance of the <see cref="ForgotPasswordModel"/> class.
  /// </summary>
  /// <param name="userManager">The user manager.</param>
  /// <param name="configuration">The configuration.</param>
  /// <param name="logger">The logger.</param>
  public ForgotPasswordModel(UserManager<IdentityExpressUser> userManager, IConfiguration configuration, ILogger<ForgotPasswordModel> logger)
  {
    _userManager = userManager;
    _logger = logger;
    _configuration = configuration;
    _ = bool.TryParse(configuration["AllowSendUnconfirmedEmail"], out _allowSendUnconfirmedEmail);
  }

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
    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    /// <value>The email.</value>
    [Required]
    [EmailAddress2]
    public string Email { get; set; }

  }

  /// <summary>
  /// On post as an asynchronous operation.
  /// </summary>
  /// <param name="redirectUri">The redirect URI.</param>
  /// <returns>A Task&lt;Microsoft.AspNetCore.Mvc.IActionResult&gt; representing the asynchronous operation.</returns>
  public async Task<IActionResult> OnPostAsync(string redirectUri = null)
  {
    if (ModelState.IsValid)
    {
      try
      {

        //Temporary Measure since email addresses could get duplicated due to current lsol accounts...
        //Will need to revisit this to consolidate the accounts with dupliate email addresses.

        //var user = await UsrManager.FindByEmailAsync(Input.Email)
        IdentityExpressUser user = await _userManager.FindByNameAsync(Input.Email).ConfigureAwait(false);

        bool userIsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user).ConfigureAwait(false);

        if (user == null || (!userIsEmailConfirmed && !_allowSendUnconfirmedEmail))
        {
          // Don't reveal that the user does not exist or is not confirmed
          return RedirectToPage("./ForgotPasswordConfirmation");
        }

        // For more information on how to enable account confirmation and password reset please 
        // visit https://go.microsoft.com/fwlink/?LinkID=532713
        string code = await _userManager.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        const string urlParts = "/Account/ResetPassword";

        string callbackUrl = Url.Page(urlParts,
                                   protocol: Request.Scheme,
                                   values: new { code, redirectUri },
                                   pageHandler: null);
 

         


      }
      catch (System.Exception e)
      {
        _logger.LogError(e, "Error Sending Forgot Password Email");
        //return BadRequest("Error Sending Forgot Password Email");
      }
      return RedirectToPage("./ForgotPasswordConfirmation");
    }
    return Page();

  }
}
