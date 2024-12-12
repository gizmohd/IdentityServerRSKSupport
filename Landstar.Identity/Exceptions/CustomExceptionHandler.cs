using Microsoft.AspNetCore.Diagnostics;

namespace Landstar.Identity.Exceptions;

/// <summary>
/// CustomExceptionHandler
/// </summary>
/// <param name="logger"></param>
public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler
{
  /// <summary>
  /// TryHandleAsync
  /// </summary>
  /// <param name="httpContext"></param>
  /// <param name="exception"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  public ValueTask<bool> TryHandleAsync(
      HttpContext httpContext,
      Exception exception,
      CancellationToken cancellationToken)
  {
    if (exception != null)
    {
      var exceptionMessage = exception.Message;
      logger.LogError(exception, "Error Message: {ExceptionMessage}, Time of occurrence {ExceptionTime}", exceptionMessage, DateTime.UtcNow);
      // Return false to continue with the default behavior
      // - or - return true to signal that this exception is handled
    }

    return ValueTask.FromResult(false);
  }
}
