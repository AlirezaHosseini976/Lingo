using Lingo_VerticalSlice.Contracts.CardSetWord;
using Lingo_VerticalSlice.Database;
using Lingo_VerticalSlice.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lingo_VerticalSlice.Features.CardSetWord.CheckQuizAnswer;

public interface ICheckQuizAnswerRepository
{
    Task<List<SpacedRepetition>> GetSpaceRepetitionDetailAsync(List<string> vocabularies,string userId, CancellationToken cancellationToken);
    Task UpdateSpaceRepetitionAsync(SpacedRepetition spacedRepetition, CancellationToken cancellationToken);
}

public class CheckQuizAnswerRepository : ICheckQuizAnswerRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public CheckQuizAnswerRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<List<SpacedRepetition>> GetSpaceRepetitionDetailAsync( List<string> vocabularies,string userId, CancellationToken cancellationToken)
    {
        var spacedRepetitionDetail = await _applicationDbContext.SpacedRepetition
            .Where(sp =>sp.UserId == userId && vocabularies.Contains(sp.Words.Vocabulary))
            .Include(sp=>sp.Words)
            .ToListAsync(cancellationToken);
        return spacedRepetitionDetail;
    }

    public async Task UpdateSpaceRepetitionAsync(SpacedRepetition spacedRepetition, CancellationToken cancellationToken)
    {
        _applicationDbContext.SpacedRepetition.Update(spacedRepetition);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}