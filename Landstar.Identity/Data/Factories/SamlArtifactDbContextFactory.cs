// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-22-2024
//
// Last Modified By : 
// Last Modified On : 04-16-2024
// ***********************************************************************
// <copyright file="SamlArtifactDbContextFactory.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Rsk.Saml.EntityFramework.DbContexts;

namespace Landstar.Identity.Data.Factories
{
  /// <summary>
  /// Class SamlArtifactDbContextFactory.
  /// Implements the <see cref="IDesignTimeDbContextFactory{SamlArtifactDbContext}" />
  /// </summary>
  /// <seealso cref="IDesignTimeDbContextFactory{SamlArtifactDbContext}" />
  public class SamlArtifactDbContextFactory : IDesignTimeDbContextFactory<SamlArtifactDbContext>
  {

    /// <summary>
    /// Creates the database context.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <returns>SamlArtifactDbContext.</returns>
    public SamlArtifactDbContext CreateDbContext(string[] args)
    {
      var optionsBuilder = new DbContextOptionsBuilder<SamlArtifactDbContext>();
      optionsBuilder.UseSqlServer(Landstar.Identity.ConfigurationExtensions.Configuration.GetConnectionString("DefaultConnection"),
   b => b.MigrationsAssembly("Landstar.Identity"));
      return new SamlArtifactDbContext(optionsBuilder.Options);

    }
  }
}

