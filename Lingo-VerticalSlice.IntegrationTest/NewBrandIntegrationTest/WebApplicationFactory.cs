using System.Data.Common;
using Lingo_VerticalSlice.Database;
using Lingo_VerticalSlice.Entities;
using Lingo_VerticalSlice.Features.CardSet.CreateCardSetFolder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Lingo_VerticalSlice.IntegrationTest.NewBrandIntegrationTest;

public class TestWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly string _connectionString =
        "Data Source=KRJ-HOSSEINI;Initial Catalog=NewDictionary;Integrated Security=true;TrustServerCertificate=True";

    public async Task InitializeAsync()
    {
        using var scope = Services.CreateScope();

        var scopedServices = scope.ServiceProvider;
        var dbContext = scopedServices.GetRequiredService<ApplicationDbContext>();

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.MigrateAsync();
    }

    public new async Task DisposeAsync()
    {
        await base.DisposeAsync();
        await Task.CompletedTask;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // var descriptor = services.SingleOrDefault(
            //     d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            // if (descriptor != null)
            // {
            //     services.Remove(descriptor);
            // }

            services.RemoveAll<DbContextOptions<ApplicationDbContext>>();
            services.AddDbContext<ApplicationDbContext>(options => { options.UseSqlServer(_connectionString); });
            // services.AddHttpContextAccessor();
        });
        builder.UseEnvironment("Test");
    }
}