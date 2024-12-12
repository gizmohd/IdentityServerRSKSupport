// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-19-2024
//
// Last Modified By : 
// Last Modified On : 04-19-2024
// ***********************************************************************
// <copyright file="DataExtensions.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Duende.IdentityServer.EntityFramework.DbContexts;
using IdentityExpress.Identity;
using Microsoft.EntityFrameworkCore;
using Rsk.Saml.EntityFramework.DbContexts;
using Rsk.Saml.IdentityProvider.Storage.EntityFramework.DbContexts;
using System.Reflection;

namespace Landstar.Identity.Data;

/// <summary>
/// Class DataExtensions.
/// </summary>
public static class DataExtensions
{
  /// <summary>
  /// Initializes the database.
  /// </summary>
  /// <param name="app">The application.</param>
  public static void InitializeDatabase(this IApplicationBuilder app)
  {
    if (app == null)
    {
      return;
    }

    using IServiceScope serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

    serviceScope.RunPremigrationScripts();
    serviceScope.MigrateDatabase<ApplicationDbContext>();
    serviceScope.MigrateDatabase<IdentityExpressDbContext>();
    serviceScope.MigrateDatabase<PersistedGrantDbContext>();
    serviceScope.MigrateDatabase<ConfigurationDbContext>();

    serviceScope.MigrateDatabase<SamlArtifactDbContext>();
    serviceScope.MigrateDatabase<SamlConfigurationDbContext>();

    serviceScope.MigrateDatabase<WsFederationConfigurationDbContext>();
  }

  /// <summary>
  /// Migrates the database.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="serviceScope">The service scope.</param>
  private static void MigrateDatabase<T>(this IServiceScope serviceScope) where T : DbContext
  {
    T dbContext = serviceScope.ServiceProvider.GetService<T>();

    dbContext?.Database.Migrate();
  }

  private static void RunPremigrationScripts(this IServiceScope serviceScope)
  {
    ApplicationDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    Assembly assembly = Assembly.GetAssembly(typeof(DataExtensions));

    const string script = "Landstar.Identity.Scripts.PreMigration.sql";
    using Stream stream = assembly!.GetManifestResourceStream(script);

    if (stream != null)
    {
      using StreamReader reader = new(stream);
      string sql = reader.ReadToEnd();
      dbContext.Database.ExecuteSqlRaw(sql);
    }

  }
  /// <summary>
  /// Gets the lsol roles for user.
  /// </summary>
  /// <param name="dbContext">The database context.</param>
  /// <param name="userName">Name of the user.</param>
  /// <returns>IList&lt;System.String&gt;.</returns>
  public static IList<string> GetLsolRolesForUser(this IdentityExpressDbContext dbContext, string userName)
  {

    IdentityExpressUser user = dbContext.Users.Include(r => r.Roles.Where(b => b.Role.Description != null &&
                                                                               b.Role.Reserved &&
                                                                               b.Role.Description.Contains("[LSOL]")))
                              .ThenInclude(a => a.Role)
                              .FirstOrDefault(a => a.UserName == userName);
    if (user == null)
    {
      return [];
    }
    return user.Roles.Select(a => a.Role.Name)
                     .ToList();
  }
  /// <summary>
  /// Check add claim type as an asynchronous operation.
  /// </summary>
  /// <param name="dbContext">The database context.</param>
  /// <param name="Name">The name.</param>
  /// <param name="Description">The description.</param>
  /// <param name="Reserved">if set to <see langword="true" /> [reserved].</param>
  /// <param name="Required">if set to <see langword="true" /> [required].</param>
  /// <param name="userEditable">if set to <see langword="true" /> [user editable].</param>
  /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
  /// <returns>A Task representing the asynchronous operation.</returns>
  public static async Task CheckAddClaimTypeAsync(this IdentityExpressDbContext dbContext, string Name, string Description, bool Reserved = false, bool Required = false, bool userEditable=false, CancellationToken cancellationToken= default)
  {
    IdentityExpressClaimType claimType = await dbContext.ClaimTypes.FirstOrDefaultAsync(a => a.Name == Name, cancellationToken).ConfigureAwait(false);
    if (claimType == null)
    {
      claimType = new IdentityExpressClaimType
      {
        Name = Name,
        NormalizedName = Name.ToUpper(),
        Reserved = Reserved,
        Required = Required,
        Description = Description,
        DisplayName = Name,
        UserEditable = userEditable
      };
      dbContext.ClaimTypes.Add(claimType);

    }
    else
    {
      claimType.Reserved = Reserved;
      claimType.Required = Required;
      claimType.DisplayName = Name;
      claimType.Description = Description;
      claimType.UserEditable = userEditable;
      dbContext.Update(claimType);
    }
    await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
  }

}
