using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Integration;

public class ConfigureWebApplicationFactory
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
        where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // set the environment for our test, which will correspond to the `appsettings.Testing.json` that we'll need in the `Api` project.
            // this allows us to use a configuration that is as similar to the real app as possible
            builder.UseEnvironment("Testing");
            builder.ConfigureAppConfiguration(
                (hostContext, configApp) =>
                {
                    var env = hostContext.HostingEnvironment;
                    configApp.AddJsonFile("appsettings.json");
                    configApp.AddJsonFile($"appsettings.{env.EnvironmentName}.json");
                    configApp.AddEnvironmentVariables();
                }
            );
        }
    }
}
