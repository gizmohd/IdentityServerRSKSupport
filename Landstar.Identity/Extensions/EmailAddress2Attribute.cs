// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-16-2024
//
// Last Modified By : 
// Last Modified On : 04-19-2024
// ***********************************************************************
// <copyright file="EmailAddress2Attribute.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Text.RegularExpressions;

namespace System.ComponentModel.DataAnnotations;

/// <summary>
/// Alternate to EmailAddressAttribute that supports using the + symbol for gmail addresses.
/// </summary>
/// <seealso cref="System.ComponentModel.DataAnnotations.DataTypeAttribute" />
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed partial class EmailAddress2Attribute : DataTypeAttribute
{
  /// <summary>
  /// The default error message
  /// </summary>
  public const string DefaultErrorMessage = "Username must be in the form of a valid email address";
  /// <summary>
  /// Initializes a new instance of the <see cref="EmailAddress2Attribute"/> class.
  /// </summary>
  public EmailAddress2Attribute()
      : base(DataType.EmailAddress)
  {
    // Set DefaultErrorMessage not ErrorMessage, allowing user to set
    // ErrorMessageResourceType and ErrorMessageResourceName to use localized messages.
    ErrorMessage = DefaultErrorMessage;
  }

  /// <inheritdoc />
  public override bool IsValid(object value)
  {
    if (value == null)
    {
      return true;
    }

    if (value is not string valueAsString)
    {
      return false;
    }

    var result = EmailValidatorRegex().Match(valueAsString).Success;
    return result;
  }

  /// <summary>
  /// Emails the validator regex.
  /// </summary>
  /// <returns>Regex.</returns>
  [GeneratedRegex("^\\w+([\\+\\.-]?\\w+)*@\\w+([\\.-]?\\w+)*(\\.\\w{2,3})+$", RegexOptions.Compiled, 1000)]
  private static partial Regex EmailValidatorRegex();
}

