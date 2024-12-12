using Landstar.Identity;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
  WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
  builder.Host.UseSerilog();

#if !DEBUG  //Use localhost certificate when debugging locally... 
  builder.WebHost.LandstarSsl(Landstar.Identity.ConfigurationExtensions.Configuration);
#endif

  builder.WebHost.UseConfiguration(Landstar.Identity.ConfigurationExtensions.Configuration);


  WebApplication app = builder
      .ConfigureServices()
      .ConfigurePipeline();

  await app.RunAsync();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
  Log.Fatal(ex, "Unhandled exception");
}
finally
{
  Log.Information("Shut down complete");
  await Log.CloseAndFlushAsync();
}