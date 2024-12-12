﻿// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-16-2024
//
// Last Modified By : 
// Last Modified On : 04-22-2024
// ***********************************************************************
// <copyright file="InvalidUrlException.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Landstar.Identity.Exceptions;

/// <summary>
/// Class InvalidUrlException.
/// Implements the <see cref="Exception" />
/// </summary>
/// <seealso cref="Exception" />
[Serializable]
public class InvalidUrlException : Exception
{
  /// <summary>
  /// Initializes a new instance of the <see cref="InvalidUrlException"/> class.
  /// </summary>
  public InvalidUrlException()
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="InvalidUrlException"/> class.
  /// </summary>
  /// <param name="message">The message that describes the error.</param>
  public InvalidUrlException(string message) : base(message)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="InvalidUrlException"/> class.
  /// </summary>
  /// <param name="message">The error message that explains the reason for the exception.</param>
  /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
  public InvalidUrlException(string message, Exception innerException) : base(message, innerException)
  {
  }


}