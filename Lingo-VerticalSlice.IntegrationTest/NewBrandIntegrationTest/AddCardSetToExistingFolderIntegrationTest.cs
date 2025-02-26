namespace Lingo_VerticalSlice.IntegrationTest.NewBrandIntegrationTest;

public class AddCardSetToExistingFolderIntegrationTest : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    public AddCardSetToExistingFolderIntegrationTest(TestWebApplicationFactory<Program> factory)
        => _client = factory.CreateClient();
    //time for user
}