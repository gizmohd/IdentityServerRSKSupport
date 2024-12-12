// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 10-03-2024
//
// Last Modified By : 
// Last Modified On : 10-03-2024
// ***********************************************************************
// <copyright file="AspNetUserAuditRecord.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Landstar.Identity.Models;


/// <summary>
/// Class AspNetUserAuditRecord.
/// </summary>
[Table("AspNetUserAudit", Schema = "dbo")]
[Index(nameof(UserId),Name ="IX_AspNetUserAudit_UserId")]
public class AspNetUserAuditRecord
{
  /// <summary>
  /// Gets or sets the identifier.
  /// </summary>
  /// <value>The identifier.</value>
  [Key]
  public long Id { get; set; }

  /// <summary>
  /// Gets or sets the user identifier.
  /// </summary>
  /// <value>The user identifier.</value>
  [Required]
  [StringLength(450)]
  public string UserId { get; set; }

  /// <summary>
  /// Gets or sets the change date.
  /// </summary>
  /// <value>The change date.</value>
  public DateTime ChangeDate { get; set; }

  /// <summary>
  /// Gets or sets the user data.
  /// </summary>
  /// <value>The user data.</value>
  [Required]
  public string UserData { get; set; }

  /// <summary>
  /// Gets or sets the hash.
  /// </summary>
  /// <value>The hash.</value>
  [StringLength(40)]
  public string Hash { get; set; }

  /// <summary>
  /// Gets or sets the web hook sent DateTime.65.
  /// </summary>
  /// <value>The web hook sent DTM.</value>
  public DateTime? WebHookSentDtm { get; set; }

}
