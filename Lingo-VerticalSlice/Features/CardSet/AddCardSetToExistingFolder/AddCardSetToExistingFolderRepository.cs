using Lingo_VerticalSlice.Database;
using Lingo_VerticalSlice.Shared;
using Microsoft.EntityFrameworkCore;
using static Lingo_VerticalSlice.Features.CardSet.AddCardSetToExistingFolder.AddCardSetToExistingFolder;

namespace Lingo_VerticalSlice.Features.CardSet.AddCardSetToExistingFolder;

public interface IAddCardSetToExistingFolderRepository
{
    Task<bool> IsCardSetExistsAsync(int folderId,string cardSetName, CancellationToken cancellationToken);
    Task<int> CardSetInFolderCountAsync(int folderId, CancellationToken cancellationToken);
    Task<int> AddCardSetToExistingFolderAsync(Entities.CardSet cardSet, CancellationToken cancellationToken);
}


public class AddCardSetToExistingFolderRepository : IAddCardSetToExistingFolderRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public AddCardSetToExistingFolderRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<bool> IsCardSetExistsAsync(int folderId, string cardSetName , CancellationToken cancellationToken)
    {
        var isCardSetExists =await _applicationDbContext.CardSet.AnyAsync(c => c.Name == cardSetName && c.FolderId == folderId, cancellationToken);
        return isCardSetExists;
    }

    public async Task<int> CardSetInFolderCountAsync(int folderId, CancellationToken cancellationToken)
    {
        var cardSetCount =
            await _applicationDbContext.CardSet.CountAsync(c => c.FolderId == folderId, cancellationToken);
        return cardSetCount;
    }

    public async Task<int> AddCardSetToExistingFolderAsync(Entities.CardSet cardSet, CancellationToken cancellationToken)
    {
        _applicationDbContext.Add(cardSet);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
        int id = cardSet.Id;
        return id;
    }
}