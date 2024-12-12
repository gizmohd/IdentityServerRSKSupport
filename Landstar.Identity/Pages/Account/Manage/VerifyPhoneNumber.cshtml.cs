using IdentityExpress.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Landstar.Identity.Pages.Account.Manage;

/// <summary>
/// Class VerifyPhoneNumberModel.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
public class VerifyPhoneNumberModel(
  UserManager<IdentityExpressUser> userManager,
  SignInManager<IdentityExpressUser> signInManager
  ) : PageModel

{
  /// <summary>
  /// Gets or sets the phone number.
  /// </summary>
  /// <value>The phone number.</value>
  [Phone]
  [Display(Name = "Phone number")]
  public string PhoneNumber { get; set; }


  /// <summary>
  /// Gets or sets the code.
  /// </summary>
  /// <value>The code.</value>
  [BindProperty]
  [Required]
  [StringLength(6,MinimumLength =6)]
  public string Code { get; set; }


  /// <summary>
  /// Gets or sets the status message.
  /// </summary>
  /// <value>The status message.</value>
  [TempData]
  public string StatusMessage { get; set; }

  /// <summary>
  /// Called when [get].
  /// </summary>
  /// <returns>IActionResult.</returns>
  public IActionResult OnGet()
  {
    return Page();
  }
  /// <summary>
  /// On post as an asynchronous operation.
  /// </summary>
  /// <returns>A Task&lt;IActionResult&gt; representing the asynchronous operation.</returns>
  public async Task<IActionResult> OnPostAsync(string data)
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
        NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
      }
    }
    var result = await userManager.ChangePhoneNumberAsync(user, PhoneNumber, Code);
    if (result.Succeeded)
    {
      await signInManager.SignInAsync(user, isPersistent: false);
      StatusMessage = "Phone number updated successfully...";
      return this.RedirectToPage("/Account/Manage");
    }
    return Page();
  }
  

}
