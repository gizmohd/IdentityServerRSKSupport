// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-22-2024
//
// Last Modified By : 
// Last Modified On : 04-16-2024
// ***********************************************************************
// <copyright file="ConfigurationDbContextFactory.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Landstar.Identity.Data.Factories
{
  /// <summary>
  /// Class ConfigurationDbContextFactory.
  /// Implements the <see cref="IDesignTimeDbContextFactory{ConfigurationDbContext}" />
  /// </summary>
  /// <seealso cref="IDesignTimeDbContextFactory{ConfigurationDbContext}" />
  public class ConfigurationDbContextFactory : IDesignTimeDbContextFactory<ConfigurationDbContext>
  {


    /// <summary>
    /// Creates the database context.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <returns>ConfigurationDbContext.</returns>
    public ConfigurationDbContext CreateDbContext(string[] args)
    {



      DbContextOptionsBuilder<ConfigurationDbContext> optionsBuilder = new DbContextOptionsBuilder<ConfigurationDbContext>();
      optionsBuilder.UseSqlServer(Landstar.Identity.ConfigurationExtensions.Configuration.GetConnectionString("DefaultConnection"),
      b => b.MigrationsAssembly("Landstar.Identity"));
      ConfigurationStoreOptions options = new ConfigurationStoreOptions
      {

      };
      ConfigurationDbContext dbCOntext = new ConfigurationDbContext(optionsBuilder.Options)
      {
        StoreOptions = options
      };
      return dbCOntext;
    }
  }
}
