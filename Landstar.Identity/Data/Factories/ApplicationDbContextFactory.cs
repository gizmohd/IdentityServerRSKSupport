// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-22-2024
//
// Last Modified By : 
// Last Modified On : 04-16-2024
// ***********************************************************************
// <copyright file="ApplicationDbContextFactory.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Landstar.Identity.Data.Factories
{
  /// <summary>
  /// Class ApplicationDbContextFactory.
  /// Implements the <see cref="IDesignTimeDbContextFactory{ApplicationDbContext}" />
  /// </summary>
  /// <seealso cref="IDesignTimeDbContextFactory{ApplicationDbContext}" />
  public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
  {
    /// <summary>
    /// Creates the database context.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <returns>ApplicationDbContext.</returns>
    public ApplicationDbContext CreateDbContext(string[] args)
    {
      DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new();
      optionsBuilder.UseSqlServer(Landstar.Identity.ConfigurationExtensions.Configuration.GetConnectionString("DefaultConnection"),
      b => b.MigrationsAssembly("Landstar.Identity")
            .EnableRetryOnFailure());

      return new ApplicationDbContext(optionsBuilder.Options);
    }
  }
}
