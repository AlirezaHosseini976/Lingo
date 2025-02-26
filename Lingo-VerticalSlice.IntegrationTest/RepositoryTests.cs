using FluentAssertions;
using Lingo_VerticalSlice.Database;
using Lingo_VerticalSlice.Entities;
using Lingo_VerticalSlice.Features.CardSet.AddCardSetToExistingFolder;
using Lingo_VerticalSlice.Features.CardSet.CreateCardSetFolder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Lingo_VerticalSlice.IntegrationTest;

public class RepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _dbContext;
    private readonly AddCardSetToExistingFolderRepository _folderRepository;
    private readonly CreateCardSetFolderRepository _cardSetFolderRepository;
    public RepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer("Data Source=KRJ-HOSSEINI;Initial Catalog=NewDictionary;Integrated Security=true;TrustServerCertificate=True;")
            .Options;
        _dbContext = new ApplicationDbContext(options);
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.Migrate();
        _folderRepository = new AddCardSetToExistingFolderRepository(_dbContext);
        _cardSetFolderRepository = new CreateCardSetFolderRepository(_dbContext);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
    

    [Fact]
    public async Task AddCardSetToExistingFolder_EverythingIsOk_CardSetShouldBeAddedToFolderInDatabase()
    {
        var user = new User
        {
            Id = "qwerty"
        };
        var defaultCardSet = new CardSet
        {
            FolderId = 4,
            Name = "default",

        };
        var folder = new Folder
        {
            UserId = "qwerty",
            Name = "folderName",
            CardSets = new List<CardSet>
            {
                defaultCardSet
            }
        };
        await _cardSetFolderRepository.CreateCardSetFolderAsync(folder, CancellationToken.None);
        var createdFolder = await _dbContext.Folder.FirstOrDefaultAsync(f=>f.Name == "qwerty");
        var firstCardSet = new CardSet
        {
            FolderId = createdFolder.Id,
            Name = "firstCardSetName"
        };
        var newCardSet = new CardSet
        {
            FolderId = 4,
            Name = "New CardSet"
        };
        await _folderRepository.AddCardSetToExistingFolderAsync(newCardSet,CancellationToken.None);
        await _dbContext.SaveChangesAsync();
        var cardSet =  _dbContext.CardSet.FirstOrDefault(c=>c.Name == "New CardSet" && c.FolderId == 4);

        cardSet.Name.Should().Be("New CardSet");
    }
}