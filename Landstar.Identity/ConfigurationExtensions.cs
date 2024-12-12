// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-22-2024
//
// Last Modified By : 
// Last Modified On : 04-22-2024
// ***********************************************************************
// <copyright file="HostingExtensions.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Serilog;
using System.Reflection;




namespace Landstar.Identity;
 

//
// Summary:
//     Configuration Extensions
public static class ConfigurationExtensions
{
  //
  // Summary:
  //     Static Configuration Endpoint Note: Azure Key Vault secrets are not available
  //     using this method, must use IConfiguration di container.
  //
  // Value:
  //     The configuration.
  public static IConfiguration Configuration { get; private set; } = SetConfigurationDefaults();


  //
  // Summary:
  //     Gets the name of the API engine.
  //
  // Value:
  //     The name of the API engine.
  public static string API_ENGINE_NAME => Assembly.GetEntryAssembly()?.GetName().Name;

  //
  // Summary:
  //     Gets the API engine version.
  //
  // Value:
  //     The API engine version.
  public static string API_ENGINE_VERSION => Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "0.1";

  //
  // Summary:
  //     Sets the configuration defaults.
  //
  // Parameters:
  //   AdditionalConfigurationFiles:
  //     The additional configuration files.
  public static IConfiguration SetConfigurationDefaults(string[] AdditionalConfigurationFiles = null)
  {
    IConfigurationBuilder configurationBuilder = CreateBaseConfig(AdditionalConfigurationFiles);
    configurationBuilder = configurationBuilder.AddEnvironmentVariables();
    IConfigurationRoot configurationRoot = configurationBuilder.Build();
    

    try
    {
      configurationRoot = configurationBuilder.Build();
    }
    catch (Exception exception2)
    {
      Log.Error(exception2, "Error Attaching to Azure KeyVault Configuration");
    }

    Configuration = configurationRoot;
    return configurationRoot;
  }

  private static IConfigurationBuilder CreateBaseConfig(string[] AdditionalConfigurationFiles)
  {
    IConfigurationBuilder configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).AddJsonFile("appsettings." + Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") + ".json", optional: true, reloadOnChange: true).AddJsonFile("settings/appsettings.docker.json", optional: true, reloadOnChange: true);
    if (AdditionalConfigurationFiles != null)
    {
      foreach (string path in AdditionalConfigurationFiles)
      {
        configurationBuilder.AddJsonFile(path);
      }
    }



    return configurationBuilder;
  }
}
