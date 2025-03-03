using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.IdentityModel.Tokens;

namespace Lingo_VerticalSlice.IntegrationTest;

public class AddCardSetToExistingFolderIntegrationTests: IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _base;
    private readonly RepositoryTests _repositoryTests;

    public AddCardSetToExistingFolderIntegrationTests(CustomWebApplicationFactory<Program> @base )
    {
        _base = @base;
    }
    
    [Fact]
    public async Task AddCardSetToExistingFolder_EverythingIsOk_CardSetShouldBeAddedToFolderInDatabase()
    {
        // Arrange
        var client = _base.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        // Act
        var response = await client.PostAsync("https://localhost:5001/add-cardset-to-folder", 
                new StringContent(
                    JsonSerializer.Serialize(new {
                        folderId = 4, 
                        cardSetName = "alli"
                    }), Encoding.UTF8, "application/json"))
            ;
        var results = await response.Content.ReadAsStringAsync();
        

        // Assert
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}