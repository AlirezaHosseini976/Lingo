using System.Data.Common;
using System.Net.Http.Headers;
using Lingo_VerticalSlice.Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Lingo_VerticalSlice.IntegrationTest;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected IServiceProvider _services;
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<ApplicationDbContext>));
            if (dbContextDescriptor != null)
            {
                services.Remove(dbContextDescriptor);
            }

            var dbConnectionDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbConnection));
            if (dbConnectionDescriptor != null)
            {
                services.Remove(dbConnectionDescriptor);    
            }

            services.AddSingleton<DbConnection>(container =>
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
            
        });

        builder.UseEnvironment("Test");
        // builder.ConfigureServices(services =>
        // {
        //     var context = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(ApplicationDbContext));
        //     if (context != null)
        //     {
        //         services.Remove(context);
        //         var options = services.Where(r => r.ServiceType == typeof(DbContextOptions)
        //                                           || r.ServiceType.IsGenericType &&
        //                                           r.ServiceType.GetGenericTypeDefinition() ==
        //                                           typeof(DbContextOptions<>)).ToArray();
        //
        //         foreach (var option in options)
        //         {
        //             services.Remove(option);
        //         }
        //     }
        // });
    }
}