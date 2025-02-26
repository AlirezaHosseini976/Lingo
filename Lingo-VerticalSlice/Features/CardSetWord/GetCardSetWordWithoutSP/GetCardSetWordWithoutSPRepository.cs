using Lingo_VerticalSlice.Contracts.CardSetWord.VocabStructureWithoutSP;
using Lingo_VerticalSlice.Database;
using Lingo_VerticalSlice.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lingo_VerticalSlice.Features.CardSetWord.GetCardSetWordWithoutSP;

public interface IGetCardSetWordWithoutSPRepository
{
    Task<List<Entities.CardSetWord>> GetCardSetAsync(int cardSetId, CancellationToken cancellationToken);
    Task<List<VocabularyStructureIdentifier>> GetWordStructureAsync(int cardSetId, string userId, CancellationToken cancellationToken);
    Task<bool> ValidateUserAsync(int cardSetId, string userId, CancellationToken cancellationToken); 
    Task<List<Word>> GetWordsAsync(int vocabularyId, CancellationToken cancellationToken);
}

public class GetCardSetWordWithoutSPRepository : IGetCardSetWordWithoutSPRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public GetCardSetWordWithoutSPRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }
    public async Task<List<Entities.CardSetWord>> GetCardSetAsync(int cardSetId, CancellationToken cancellationToken)
    {
        var cardSetWord = await _applicationDbContext.CardSetWord
            .Where(c => c.CardSetId == cardSetId)
            .ToListAsync(cancellationToken);
        return cardSetWord;
    }

    public async Task<List<VocabularyStructureIdentifier>> GetWordStructureAsync(int cardSetId, string userId, CancellationToken cancellationToken)
    {
        var vocabularyIds = await _applicationDbContext.CardSetWord
            .Where(csw => csw.CardSetId == cardSetId)
            .Select(csw => csw.VocabularyId)
            .ToListAsync(cancellationToken);

        var vocabularyDetails = await _applicationDbContext.VocabularyDetailsMaterializedView
            .Where(w => vocabularyIds.Contains(w.VocabularyId))
            .GroupBy(w => w.VocabularyId)
            .Select(g => new VocabularyStructureIdentifier
            {
                VocabularyId = g.Key,
                VocabularyStructure = new VocabularyStructureResponse
                {
                    Vocabulary = g.First().Vocabulary,
                    Structure = g.Select(w => new VocabularyDetailStructureResponse
                    {
                        Definition = w.Definition,
                        Translation = w.Translation,
                        Examples = w.Example.ToList(),
                        Synonyms = w.Synonym.ToList(),
                        PartOfSpeech = w.PartOfSpeech
                    }).ToList()
                }
            }).ToListAsync(cancellationToken);
        return vocabularyDetails;
    }

    public async Task<bool> ValidateUserAsync(int cardSetId, string userId, CancellationToken cancellationToken)
    {
        var userValidation =
            await _applicationDbContext.CardSet.AnyAsync(cw => cw.Id == cardSetId && cw.Folder.UserId == userId,
                cancellationToken);
        return userValidation;
    }

    public async Task<List<Word>> GetWordsAsync(int vocabularyId, CancellationToken cancellationToken)
    {
        var word = await _applicationDbContext.Word.Where(w => w.Id == vocabularyId).ToListAsync(cancellationToken);
        return word;
    }
}