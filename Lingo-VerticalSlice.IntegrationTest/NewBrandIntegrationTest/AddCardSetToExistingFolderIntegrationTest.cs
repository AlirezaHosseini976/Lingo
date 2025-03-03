using System.Net;
using System.Text;
using Lingo_VerticalSlice.Database;
using Lingo_VerticalSlice.Entities;
using Lingo_VerticalSlice.Features.CardSet.CreateCardSetFolder;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit.Abstractions;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Lingo_VerticalSlice.IntegrationTest.NewBrandIntegrationTest;

public class AddCardSetToExistingFolderIntegrationTest : IClassFixture<TestWebApplicationFactory>
{
    private readonly TestWebApplicationFactory _base;

    public AddCardSetToExistingFolderIntegrationTest(TestWebApplicationFactory @base)
    {
        _base = @base;
    }

    //time for user
    [Fact]
    public async Task AddCardSetToExistingFolder_EverythingIsOk_CardSetShouldBeAddedToFolderInDatabase()
    {
        //Arrange
        var user = new IdentityUser
        {
            UserName = "Admin",
            Email = "admin@gmail.com"
        };
        var defaultCardSet = new CardSet
        {
            Name = "default",
        };
        var folder = new Folder
        {
            UserId = user.Id,
            Name = "First Folder",
            CardSets = new List<CardSet> { defaultCardSet },
        };
        using (var scope = _base.Services.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var result = await userManager.CreateAsync(user, "QAZwsx_123");

            var repository = scope.ServiceProvider.GetRequiredService<ICreateCardSetFolderRepository>();
            await repository.CreateCardSetFolderAsync(folder, CancellationToken.None);
        }


        var client = _base.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        var loginResult = await client.PostAsync("https://localhost:5001/login",
            new StringContent(
                JsonSerializer.Serialize(new 
                {
                    Email = user.Email,
                    Password = "QAZwsx_123"
                }), Encoding.UTF8, mediaType: "application/json"));
        var token = JsonSerializer.Deserialize<AccessTokenResponse>(await loginResult.Content.ReadAsStringAsync());


        //Act

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token!.AccessToken}");
        var response = await client.PostAsync("https://localhost:5001/add-cardset-to-folder",
            new StringContent(
                JsonSerializer.Serialize(new
                {
                    folderId = folder.Id,
                    cardSetName = "SecondCardSet"
                }), Encoding.UTF8, "application/json"));

        //Assert 
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}