using System.Data.Common;
using Lingo_VerticalSlice.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Lingo_VerticalSlice.IntegrationTest.NewBrandIntegrationTest;

public class TestWebApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : Program
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if(descriptor != null)
                services.Remove(descriptor);
            services.AddScoped<DbConnection>(container =>
            {
                var connection = new SqlConnection(
                    "Data Source=KRJ-HOSSEINI;Initial Catalog=NewDictionary;Integrated Security=true;TrustServerCertificate=True;");
                connection.Open();
                return connection;
            });
            services.AddDbContext<ApplicationDbContext>((container, options) =>
            {
                var connection = container.GetRequiredService<DbConnection>();
                options.UseSqlServer(connection);
            });

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            using (var appContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
            {
                try
                {
                    appContext.Database.Migrate();
                }
                catch (Exception ex)
                {
                    throw new Exception("Database migration failed", ex);
                }
            }
        });
        
    }
    
}