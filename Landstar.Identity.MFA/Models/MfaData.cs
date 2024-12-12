// ***********************************************************************
// Assembly         : Landstar.Identity.Mfa
// Author           : 
// Created          : 09-20-2024
//
// Last Modified By : 
// Last Modified On : 09-20-2024
// ***********************************************************************
// <copyright file="MfaData.cs" company="Landstar.Identity.Mfa">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Landstar.Identity.Mfa.Models;

/// <summary>
/// Class MfaData.
/// </summary>
[Table("MfaData", Schema = "mfa")]
[Index(nameof(UserId), IsUnique = false, Name = "idx_UserId")]
public class MfaData
{
  /// <summary>
  /// Gets or sets the identifier.
  /// </summary>
  /// <value>The identifier.</value>
  [Key]
  public int? Id { get; set; }
  /// <summary>
  /// Gets or sets the user identifier.
  /// </summary>
  /// <value>The user identifier.</value>

  public string UserId { get; set; }
  /// <summary>
  /// Gets or sets the phone number.
  /// </summary>
  /// <value>The phone number.</value>
  [StringLength(15, MinimumLength =10)]
  
  public string PhoneNumber { get; set; }

  /// <summary>
  /// Gets the masked phone number.
  /// </summary>
  /// <value>The masked phone number.</value>
  public string MaskedPhoneNumber => MaskPhoneNumber(PhoneNumber);

  /// <summary>
  /// Gets or sets the created date.
  /// </summary>
  /// <value>The created date.</value>
  public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
  /// <summary>
  /// Gets or sets the last updated.
  /// </summary>
  /// <value>The last updated.</value>
  public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

  private static string MaskPhoneNumber(string phoneNumber)
  {
    if (string.IsNullOrEmpty(phoneNumber) || phoneNumber.Length <= 4)
    {
      return phoneNumber;
    }

    return $"***-***-{phoneNumber[^4..]}";
  }
}
