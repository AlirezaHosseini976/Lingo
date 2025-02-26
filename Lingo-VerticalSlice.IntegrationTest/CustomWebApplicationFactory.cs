using System.Net.Http.Headers;
using Lingo_VerticalSlice.Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Lingo_VerticalSlice.IntegrationTest;

public class CustomWebApplicationFactory<TProgram> :WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // var application = new WebApplicationFactory<Program>()
        //     .WithWebHostBuilder(builder =>
        //     {
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll(typeof(AuthenticationHandler<AuthenticationSchemeOptions>));
                    services.AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", null);
                });
            // });
        // var client = application.CreateClient();
        // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        builder.UseEnvironment("Test");
        builder.ConfigureServices(services =>
        {
            var context = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(ApplicationDbContext));
            if (context != null)
            {
                services.Remove(context);
                var options = services.Where(r => r.ServiceType == typeof(DbContextOptions)
                                                  || r.ServiceType.IsGenericType &&
                                                  r.ServiceType.GetGenericTypeDefinition() ==
                                                  typeof(DbContextOptions<>)).ToArray();

                foreach (var option in options)
                {
                    services.Remove(option);
                }
            }

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDatabase");
            });
        });
    }
}