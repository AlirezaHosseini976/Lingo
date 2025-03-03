using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Lingo_VerticalSlice.Contracts.Services;
using Lingo_VerticalSlice.Database;
using Lingo_VerticalSlice.Entities;
using Lingo_VerticalSlice.Features.CardSet.CreateCardSetFolder;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http;
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
        var password1 = "QAxlsx_123";

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

        // using (var scope = _base.Services.CreateScope())
        //  {
        // //     var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        // //     var result = await userManager.CreateAsync(user, password1);
        //
        //     
        // }


        var client = _base.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        var signUp = await client.PostAsync("https://localhost:5001/register",
            new StringContent(
                JsonSerializer.Serialize(new
                {
                    email = "admin@gmail.com",
                    password = password1,
                }), Encoding.UTF8, mediaType: "application/json"));

        var loginResult = await client.PostAsync("https://localhost:5001/login?useCookies=false",
            new StringContent(
                JsonSerializer.Serialize(new
                {
                    email = user.Email,
                    password = password1,
                    twoFactorCode = "string",
                    twoFactorRecoveryCode = "string"
                }), Encoding.UTF8, mediaType: "application/json"));
        var responseMessage = await loginResult.Content.ReadAsStringAsync();
        var token = JsonSerializer.Deserialize<AccessTokenResponse>(responseMessage, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        var dbContext = _base.Services.GetRequiredService<ApplicationDbContext>();
        var dbUser = await dbContext.Users.FirstOrDefaultAsync();

        var folder = new Folder
        {
            UserId = dbUser.Id,
            Name = "First Folder",
            CardSets = new List<CardSet> { defaultCardSet },
        };
        var repository = _base.Services.GetRequiredService<ICreateCardSetFolderRepository>();
        await repository.CreateCardSetFolderAsync(folder, CancellationToken.None);

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
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}