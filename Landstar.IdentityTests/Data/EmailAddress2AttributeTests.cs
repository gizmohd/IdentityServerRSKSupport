using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Landstar.Identity.Tests.Data;

/// <summary>
/// Tests for the EmailAddress2Attribute class
/// </summary>

public class EmailAddress2AttributeTests
{
  readonly EmailAddress2Attribute emailAddress2Attribute;

  /// <summary>
  /// EmailAddress2Attribute CTOR
  /// </summary>
  public EmailAddress2AttributeTests()
  {
    emailAddress2Attribute = new();
  }

  /// <summary>
  /// Validates the email test.
  /// </summary>
  [Fact(DisplayName = "Test if email address is valid.")]
  public void ValidateEmailTest()
  {
    Assert.True(emailAddress2Attribute.IsValid("joe@mail.com"));

  }

  /// <summary>
  /// Validates the landstar email test.
  /// </summary>
  [Fact(DisplayName = "Test if landstar email address is valid.")]
  public void ValidateLandstarEmailTest()
  {
    Assert.True(emailAddress2Attribute.IsValid("juser@landstar.com"));
  }

  /// <summary>
  /// Validates the landstar email test.
  /// </summary>
  [Fact(DisplayName = "Test if Landstar Blue email address is valid.")]
  public void ValidateLandstarBlueEmailTest()
  {
    Assert.True(emailAddress2Attribute.IsValid("juser@landstarblue.com"));
  }

  /// <summary>
  /// Validates the landstar email test.
  /// </summary>
  [Fact(DisplayName = "Test if Landstar Mail email address is valid.")]
  public void ValidateLandstarMailEmailTest()
  {
    Assert.True(emailAddress2Attribute.IsValid("juser@landstarmail.com"));
  }

  /// <summary>
  /// Validates the email test gmail plus.
  /// </summary>
  [Fact(DisplayName = "Test if gmail address with a + in the username is valid.")]
  public void ValidateEmailTestGmailPlus()
  {
    Assert.True(emailAddress2Attribute.IsValid("joe+user@gmail.com"));
  }

  /// <summary>
  /// Validates the email test non email.
  /// </summary>
  [Fact(DisplayName = "Ensure that a non-email address is not valid")]
  public void ValidateEmailTestNonEmail()
  {
    Assert.False(emailAddress2Attribute.IsValid("joeuser"));
    Assert.Equal(EmailAddress2Attribute.DefaultErrorMessage, emailAddress2Attribute.ErrorMessage);
  }

  /// <summary>
  /// Validates the email test non email plus.
  /// </summary>
  [Fact(DisplayName = "Ensure that a non-email address with a + is not valid")]
  public void ValidateEmailTestNonEmailPlus()
  {
    Assert.False(emailAddress2Attribute.IsValid("joe+user"));

    Assert.Equal(EmailAddress2Attribute.DefaultErrorMessage, emailAddress2Attribute.ErrorMessage);
  }
}