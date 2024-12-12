// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-22-2024
//
// Last Modified By : 
// Last Modified On : 04-19-2024
// ***********************************************************************
// <copyright file="WsFederationConfigurationDbContextFactory.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Rsk.WsFederation.EntityFramework.DbContexts;

namespace Landstar.Identity.Data.Factories;

/// <summary>
/// Class WsFederationConfigurationDbContextFactory.
/// Implements the <see cref="IDesignTimeDbContextFactory{WsFederationConfigurationDbContext}" />
/// </summary>
/// <seealso cref="IDesignTimeDbContextFactory{WsFederationConfigurationDbContext}" />
public class WsFederationConfigurationDbContextFactory : IDesignTimeDbContextFactory<WsFederationConfigurationDbContext>
{

  /// <inheritdoc />
  public WsFederationConfigurationDbContext CreateDbContext(string[] args)
  {
    var optionsBuilder = new DbContextOptionsBuilder<WsFederationConfigurationDbContext>();
    optionsBuilder.UseSqlServer(Landstar.Identity.ConfigurationExtensions.Configuration.GetConnectionString("DefaultConnection"),
 b => b.MigrationsAssembly("Landstar.Identity"));
    return new WsFederationConfigurationDbContext(optionsBuilder.Options);

  }
}

