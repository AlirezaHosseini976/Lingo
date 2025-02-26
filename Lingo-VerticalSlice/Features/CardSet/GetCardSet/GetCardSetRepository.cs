using Lingo_VerticalSlice.Contracts.CardSetWord;
using Lingo_VerticalSlice.Contracts.CardSetWord.VocabStructure;
using Lingo_VerticalSlice.Database;
using Microsoft.EntityFrameworkCore;

namespace Lingo_VerticalSlice.Features.CardSet.GetCardSet;


public interface IGetCardSetRepository
{
    Task<List<Entities.CardSet>> GetCardSetByFolderIdAsync(int folderId, CancellationToken cancellationToken);
    
}


public class GetCardSetRepository : IGetCardSetRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public GetCardSetRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<List<Entities.CardSet>> GetCardSetByFolderIdAsync(int folderId, CancellationToken cancellationToken)
    {
        var cardSet = await _applicationDbContext.CardSet
            .Where(c => c.FolderId == folderId)
            .ToListAsync(cancellationToken);
        return cardSet;
    }
}