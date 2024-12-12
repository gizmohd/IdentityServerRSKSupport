namespace Landstar.Identity;

/// <summary>
/// Class Constants.
/// </summary>
public static class FeatureFlagConstants
{
  /// <summary>
  /// The enable mfa constant used with Launch Darkly.
  /// </summary>
  public const string EnableMFA = "enable-mfa";
  /// <summary>
  /// The disable rules as roles
  /// </summary>
  public const string DisableRulesAsRoles = "identity-server-disable-rules-as-roles";

  /// <summary>
  /// The enable account registration
  /// </summary>
  public const string EnableAccountRegistration = "identity-server-enable-account-registration";
  /// <summary>
  /// The enable test phone numbers
  /// </summary>
  public const string EnableTestPhoneNumbers = "identity-server-mfa-enable-test-numbers";
  /// <summary>
  /// The allow inline mfa enrollment
  /// </summary>
  public const string AllowInlineMFAEnrollment = "identity-server-mfa-allow-inline-enrollment";
  /// <summary>
  /// The enable localization
  /// </summary>
  public const string EnableLocalization = "identity-server-enable-localization";

  /// <summary>
  /// The synchronize corporate users
  /// </summary>
  public const string SyncCorporateUsers = "identity-server-sync-lsol-landstar-corporate-users";
}
