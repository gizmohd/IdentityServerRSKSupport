using Landstar.Identity.Models;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Landstar.Identity.Data;


/// <summary>
/// 
/// </summary>
/// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
/// <seealso cref="Microsoft.AspNetCore.DataProtection.EntityFrameworkCore.IDataProtectionKeyContext" />
/// <remarks>
/// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
/// </remarks>
/// <param name="options">The options.</param>
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IDataProtectionKeyContext
{

  
  /// <summary>
  /// Gets or sets the password history.
  /// </summary>
  /// <value>
  /// The password history.
  /// </value>
  public DbSet<PasswordHistory> PasswordHistory { get; set; }

  /// <summary>
  /// A collection of <see cref="T:Microsoft.AspNetCore.DataProtection.EntityFrameworkCore.DataProtectionKey" />
  /// </summary>
  public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

  
  /// <summary>
  /// Gets or sets the user audit records.
  /// </summary>
  /// <value>The user audit records.</value>
  public DbSet<AspNetUserAuditRecord> UserAuditRecords { get; set; }

  /// <summary>
  /// Gets the user audit web hook records asynchronous.
  /// </summary>
  /// <returns>Task&lt;List&lt;UserAuditWebHookRecord&gt;&gt;.</returns>
  public async Task<IList<UserAuditWebHookRecord>> GetUserAuditWebHookRecordsAsync(CancellationToken cancellationToken = default) =>
    await Database.SqlQueryRaw<UserAuditWebHookRecord>("exec dbo.GetUserChangeDataForWebHook").ToListAsync(cancellationToken);



  /// <summary>
  /// Stores the current user state for audit.
  /// </summary>
  /// <returns>System.Int32.</returns>
  public Task<int> StoreCurrentUserStateForAuditAsync(CancellationToken cancellationToken = default) =>
    Database.ExecuteSqlAsync($"exec dbo.StoreCurrentUserStateForAudit", cancellationToken);

  /// <summary>
  /// Marks the user audit record as webhook processed asynchronous.
  /// </summary>
  /// <param name="id">The identifier.</param>
  /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
  /// <returns>Task.</returns>
  public Task MarkUserAuditRecordAsWebhookProcessedAsync(long id, CancellationToken cancellationToken = default) =>
   this.Database.ExecuteSqlAsync($"exec dbo.SetUserAuditRecordWebHookSent @id={id}", cancellationToken);

  /// <summary>
  /// Checks if user prefers SMS over totp.
  /// </summary>
  /// <param name="userId">Name or Id of the user.</param>
  /// <returns><see langword="true" /> if XXXX, <see langword="false" /> otherwise.</returns>
  public bool CheckIfUserPrefersSmsOverTotp(string userId)
  {
    const string query = "SELECT TOP 1 TwoFactorPreferSMS as [Value] FROM dbo.AspNetUsers WHERE (Id = @userId OR UserName = @UserId)";
    SqlParameter parameter = new("@UserId", userId);
    return this.Database.SqlQueryRaw<bool>(query, parameter).FirstOrDefault();
  }

  /// <summary>
  /// Updates the user SMS preference.
  /// </summary>
  /// <param name="UserId">Name or Id of the user.</param>
  /// <param name="Preferred">if set to <see langword="true" /> [preferred].</param>
  /// <returns>Task&lt;System.Int32&gt;.</returns>
  public Task<int> UpdateUserSmsPreferenceAsync(string UserId, bool Preferred)
  {
    const string query = "UPDATE dbo.AspNetUsers SET TwoFactorPreferSMS = @Preferred WHERE (UserName = @UserId OR Id = @UserId)";
    SqlParameter p1 = new("@UserId", UserId);
    SqlParameter p2 = new("@Preferred", Preferred);
    return this.Database.ExecuteSqlRawAsync(query, p1, p2);
  }
}
