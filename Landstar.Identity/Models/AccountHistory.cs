using System;
using System.ComponentModel.DataAnnotations;

namespace Landstar.Identity.Models;

/// <summary>
/// Password History
/// </summary>

public record PasswordHistory
{
  /// <summary>
  /// Gets or sets the identifier.
  /// </summary>
  /// <value>
  /// The identifier.
  /// </value>
  [Key]
  public int Id { get; set; }

  /// <summary>
  /// Gets or sets the user identifier.
  /// </summary>
  /// <value>
  /// The user identifier.
  /// </value>
  [StringLength(450)]
  public string UserId { get; set; }

  /// <summary>
  /// Gets or sets the password hash.
  /// </summary>
  /// <value>
  /// The password hash.
  /// </value>
  public string PasswordHash { get; set; }

  /// <summary>
  /// Gets or sets the password date.
  /// </summary>
  /// <value>
  /// The password date.
  /// </value>
  public DateTime PasswordDate { get; set; }
}
