using Lingo_VerticalSlice.Database;
using Microsoft.EntityFrameworkCore;

namespace Lingo_VerticalSlice.Features.CardSet.DeleteCardSet;


public interface IDeleteCardSetRepository
{
    Task DeleteCardSetAsync(int cardSetId, CancellationToken cancellationToken);
    Task<bool> UserValidation(int cardSetId, string userId, CancellationToken cancellationToken);
}


public class DeleteCardSetRepository : IDeleteCardSetRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public DeleteCardSetRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task DeleteCardSetAsync(int cardSetId, CancellationToken cancellationToken)
    {
        var cardSet = _applicationDbContext.CardSet.FirstOrDefault(x => x.Id == cardSetId);
        if (cardSet != null)
        {
            _applicationDbContext.CardSet.Remove(cardSet);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> UserValidation(int cardSetId, string userId, CancellationToken cancellationToken)
    {
        var userValidation =
            await _applicationDbContext.CardSet.AnyAsync(c => c.Id == cardSetId && c.Folder.UserId == userId,
                cancellationToken);
        return userValidation;
    }
}