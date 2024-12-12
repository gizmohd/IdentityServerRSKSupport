// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 08-20-2024
//
// Last Modified By : 
// Last Modified On : 09-24-2024
// ***********************************************************************
// <copyright file="ManageNavPages.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Landstar.Identity.Pages.Account.Manage;

/// <summary>
/// Manage Nav Pages Helper Class
/// </summary>
public static class ManageNavPages
{
  /// <summary>
  /// The index
  /// </summary>
  public const string Index = "Index";

  /// <summary>
  /// The email
  /// </summary>
  public const string Email = "Email";

  /// <summary>
  /// The change password
  /// </summary>
  public const string ChangePassword = "ChangePassword";

  /// <summary>
  /// The link to lsol
  /// </summary>
  public const string LinkToLSOL = "LinkToLsol";

  /// <summary>
  /// The download personal data
  /// </summary>
  public const string DownloadPersonalData = "DownloadPersonalData";

  /// <summary>
  /// The delete personal data
  /// </summary>
  public const string DeletePersonalData = "DeletePersonalData";

  /// <summary>
  /// The external logins
  /// </summary>
  public const string ExternalLogins = "ExternalLogins";

  /// <summary>
  /// The personal data
  /// </summary>
  public const string PersonalData = "PersonalData";
  /// <summary>
  /// The diagnostics data
  /// </summary>
  public const string Diagnostics = "Diagnostics";
  /// <summary>
  /// The two factor authentication
  /// </summary>
  public const string TwoFactorAuthentication = "TwoFactorAuthentication";

  /// <summary>
  /// The phone number
  /// </summary>
  public const string UpdatePhoneNumber = "UpdatePhoneNumber";
  /// <summary>
  /// Indexes the nav class.
  /// </summary>
  /// <param name="viewContext">The view context.</param>
  /// <returns>System.String.</returns>
  public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

  /// <summary>
  /// Emails the nav class.
  /// </summary>
  /// <param name="viewContext">The view context.</param>
  /// <returns>System.String.</returns>
  public static string EmailNavClass(ViewContext viewContext) => PageNavClass(viewContext, Email);

  /// <summary>
  /// Changes the password nav class.
  /// </summary>
  /// <param name="viewContext">The view context.</param>
  /// <returns>System.String.</returns>
  public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);

  
  /// <summary>
  /// Downloads the personal data nav class.
  /// </summary>
  /// <param name="viewContext">The view context.</param>
  /// <returns>System.String.</returns>
  public static string DownloadPersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, DownloadPersonalData);

  /// <summary>
  /// Deletes the personal data nav class.
  /// </summary>
  /// <param name="viewContext">The view context.</param>
  /// <returns>System.String.</returns>
  public static string DeletePersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, DeletePersonalData);

  /// <summary>
  /// Externals the logins nav class.
  /// </summary>
  /// <param name="viewContext">The view context.</param>
  /// <returns>System.String.</returns>
  public static string ExternalLoginsNavClass(ViewContext viewContext) => PageNavClass(viewContext, ExternalLogins);

  /// <summary>
  /// Personals the data nav class.
  /// </summary>
  /// <param name="viewContext">The view context.</param>
  /// <returns>System.String.</returns>
  public static string PersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, PersonalData);

  /// <summary>
  /// Twoes the factor authentication nav class.
  /// </summary>
  /// <param name="viewContext">The view context.</param>
  /// <returns>System.String.</returns>
  public static string TwoFactorAuthenticationNavClass(ViewContext viewContext) => PageNavClass(viewContext, TwoFactorAuthentication);
  /// <summary>
  /// Diagnostics  nav class.
  /// </summary>
  /// <param name="viewContext">The view context.</param>
  /// <returns>System.String.</returns>
  public static string DiagnosticsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Diagnostics);


  /// <summary>
  /// Phones the numbers nav class.
  /// </summary>
  /// <param name="viewContext">The view context.</param>
  /// <returns>System.String.</returns>
  public static string PhoneNumbersNavClass(ViewContext viewContext) => PageNavClass(viewContext, UpdatePhoneNumber);

  /// <summary>
  /// Pages the nav class.
  /// </summary>
  /// <param name="viewContext">The view context.</param>
  /// <param name="page">The page.</param>
  /// <returns>System.String.</returns>
  private static string PageNavClass(ViewContext viewContext, string page)
  {
    var activePage = viewContext.ViewData["ActivePage"] as string
        ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
    return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
  }
}
