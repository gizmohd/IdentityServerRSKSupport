// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-22-2024
//
// Last Modified By : 
// Last Modified On : 04-16-2024
// ***********************************************************************
// <copyright file="IdentityExpressDbContextFactory.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using IdentityExpress.Identity;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Landstar.Identity.Data.Factories
{
  /// <summary>
  /// Class IdentityExpressDbContextFactory.
  /// Implements the <see cref="IDesignTimeDbContextFactory{IdentityExpressDbContext}" />
  /// </summary>
  /// <seealso cref="IDesignTimeDbContextFactory{IdentityExpressDbContext}" />
  public class IdentityExpressDbContextFactory : IDesignTimeDbContextFactory<IdentityExpressDbContext>
  {

    /// <summary>
    /// Creates the database context.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <returns>IdentityExpressDbContext.</returns>
    public IdentityExpressDbContext CreateDbContext(string[] args)
    {
      var optionsBuilder = new DbContextOptionsBuilder<IdentityExpressDbContext>();
      optionsBuilder.UseSqlServer(Landstar.Identity.ConfigurationExtensions.Configuration.GetConnectionString("DefaultConnection"),
      b => b.MigrationsAssembly("Landstar.Identity"));
      return new IdentityExpressDbContext(optionsBuilder.Options);

    }
  }
}

