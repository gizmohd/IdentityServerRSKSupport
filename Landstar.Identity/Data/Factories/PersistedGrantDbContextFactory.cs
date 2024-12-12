// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-22-2024
//
// Last Modified By : 
// Last Modified On : 04-16-2024
// ***********************************************************************
// <copyright file="PersistedGrantDbContextFactory.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Duende.IdentityServer.EntityFramework.DbContexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Landstar.Identity.Data.Factories
{
  /// <summary>
  /// Class PersistedGrantDbContextFactory.
  /// Implements the <see cref="IDesignTimeDbContextFactory{PersistedGrantDbContext}" />
  /// </summary>
  /// <seealso cref="IDesignTimeDbContextFactory{PersistedGrantDbContext}" />
  public class PersistedGrantDbContextFactory : IDesignTimeDbContextFactory<PersistedGrantDbContext>
  {

    /// <summary>
    /// Creates the database context.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <returns>PersistedGrantDbContext.</returns>
    public PersistedGrantDbContext CreateDbContext(string[] args)
    {
      var optionsBuilder = new DbContextOptionsBuilder<PersistedGrantDbContext>();
      optionsBuilder.UseSqlServer(Landstar.Identity.ConfigurationExtensions.Configuration.GetConnectionString("DefaultConnection"),
     b => b.MigrationsAssembly("Landstar.Identity"));

      var dbContext = new PersistedGrantDbContext(optionsBuilder.Options)
      {
        StoreOptions = new Duende.IdentityServer.EntityFramework.Options.OperationalStoreOptions()
      };
      return dbContext;
    }
  }
}

