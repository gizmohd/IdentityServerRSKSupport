// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-22-2024
//
// Last Modified By : 
// Last Modified On : 04-16-2024
// ***********************************************************************
// <copyright file="SamlConfigurationDbContextFactory.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Rsk.Saml.IdentityProvider.Storage.EntityFramework.DbContexts;

namespace Landstar.Identity.Data.Factories
{
  /// <summary>
  /// Class SamlConfigurationDbContextFactory.
  /// Implements the <see cref="IDesignTimeDbContextFactory{SamlConfigurationDbContext}" />
  /// </summary>
  /// <seealso cref="IDesignTimeDbContextFactory{SamlConfigurationDbContext}" />
  public class SamlConfigurationDbContextFactory : IDesignTimeDbContextFactory<SamlConfigurationDbContext>
  {

    /// <summary>
    /// Creates the database context.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <returns>SamlConfigurationDbContext.</returns>
    public SamlConfigurationDbContext CreateDbContext(string[] args)
    {
      var optionsBuilder = new DbContextOptionsBuilder<SamlConfigurationDbContext>();
      optionsBuilder.UseSqlServer(Landstar.Identity.ConfigurationExtensions.Configuration.GetConnectionString("DefaultConnection"),
      b => b.MigrationsAssembly("Landstar.Identity"));
      return new SamlConfigurationDbContext(optionsBuilder.Options);

    }
  }
}

