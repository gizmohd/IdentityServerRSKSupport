// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace MvcClient
{
  static class Program
  {
    public static void Main(string[] args)
    {
      try
      {
        Console.Title = "MVC Client";

        BuildWebHost(args).Run();


      }
      catch (Exception e)
      {
        Console.WriteLine(e);

      }
    }

    public static IWebHost BuildWebHost(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .UseKestrel(options =>
            {
              options.Limits.MaxRequestHeadersTotalSize = 1024 * 1024;
            })
            .Build();
  }
}