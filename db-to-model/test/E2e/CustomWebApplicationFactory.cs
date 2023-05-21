using System.Data.Common;
using FoodDiary.Data.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace E2e;

public class ConfigureWebApplicationFactory
{
    public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<FoodDiaryDbContext>));

                services.Remove(dbContextDescriptor);

                var dbConnectionDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbConnection));

                services.Remove(dbConnectionDescriptor);

                // Create a PostgreSQL Connection, this is required so that
                // this connection can be used by Entity Framework without getting the connection closed.
                services.AddSingleton<DbConnection>(container =>
                {
                    var connection = new NpgsqlConnection("Server=127.0.0.1;Port=5432;Database=test_db;User Id=user_demo;Password=pg_strong_password;Include Error Detail=true");
                    connection.Open();

                    return connection;
                });

                // Add the required connection string to override the one in use in the "real" Program.cs.
                services.AddDbContext<FoodDiaryDbContext>((container, options) =>
                {
                    var connection = container.GetRequiredService<DbConnection>();
                    options.UseNpgsql("Server=127.0.0.1;Port=5432;Database=test_db;User Id=user_demo;Password=pg_strong_password;Include Error Detail=true");
                });
            });

            // set the environment for our test, which will correspond to the `appsettings.Testing.json` that we'll need in the `Api` project.
            builder.UseEnvironment("Testing");
        }
    }
}
