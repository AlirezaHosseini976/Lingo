using System.Diagnostics.CodeAnalysis;
using Lingo_VerticalSlice.Contracts.CardSetWord.VocabStructure;
using Lingo_VerticalSlice.Database;
using Lingo_VerticalSlice.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Lingo_VerticalSlice.Features.CardSetWord.GetCardSetWord;

public interface IGetCardSetWordRepository
{
    Task<List<Entities.CardSetWord>> GetCardSetAsync(int cardSetId, CancellationToken cancellationToken);
    Task<List<VocabStructId>> GetWordStructureAsync(int cardSetId, string userId, CancellationToken cancellationToken);
    Task<bool> ValidateUserAsync(int cardSetId, string userId, CancellationToken cancellationToken); 
    Task<List<Word>> GetWordsAsync(int vocabularyId, CancellationToken cancellationToken);
}

public class GetCardSetWordRepository : IGetCardSetWordRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public GetCardSetWordRepository(ApplicationDbContext applicationDbContext)
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

    
    public async Task<List<VocabStructId>> GetWordStructureAsync(int cardSetId, string userId,
        CancellationToken cancellationToken)
    {
        var query = _applicationDbContext.Word
            .Include(x => x.WordDefinitions).ThenInclude(x => x.WordMeaning)
            .Include(x => x.WordDefinitions).ThenInclude(x => x.WordDefinitionExamples)
            .Include(x => x.WordDefinitions).ThenInclude(x => x.Synonyms)
            .Include(x => x.WordDefinitions).ThenInclude(x => x.WordType).Select(
                x=>new
                {
                    id= x.Id,
                    vocabulary = x.Vocabulary,
                    structure = x.WordDefinitions.Select(y=>new DefinitionStructureResponse
                    {
                        Definition = y.DefinitionText,
                        Translation = y.WordMeaning.FirstOrDefault().Translation,
                        Examples = y.WordDefinitionExamples.Select(s=>s.Example).ToList(),
                        Synonyms = y.Synonyms.Select(sy =>sy.Synonym).ToList(),
                        PartOfSpeech = y.WordType.Title
                    }).ToList()
                });
        var vocabularyDetails =await _applicationDbContext.CardSetWord
            .Where(csw => csw.CardSetId == cardSetId)
            .Join(query,
                csw => csw.VocabularyId,
                v => v.id,
                (csw, v) => new { CardSetWord = csw, Word = v })
            .Join(_applicationDbContext.SpacedRepetition,
                combined => new { combined.CardSetWord.VocabularyId, UserId = userId },
                sr => new { sr.VocabularyId, sr.UserId },
                (combined, sr) => new VocabStructId
                {
                    VocabularyId = combined.Word.id,
                    VocabStructure = new VocabStructureResponse
                    {
                        NextQuizDate = sr.NextDate,
                        WordState = sr.WordState,
                        Vocabulary = combined.Word.vocabulary,
                        Structure = combined.Word.structure
                    }
                }).ToListAsync(cancellationToken);
        return vocabularyDetails;
    }


    public async Task<List<Word>> GetWordsAsync(int vocabularyId, CancellationToken cancellationToken)
    {
        var word = await _applicationDbContext.Word.Where(w => w.Id == vocabularyId).ToListAsync(cancellationToken);
        return word;
    }

    public async Task<bool> ValidateUserAsync(int cardSetId, string userId, CancellationToken cancellationToken)
    {
        var userValidation =
            await _applicationDbContext.CardSet.AnyAsync(cw => cw.Id == cardSetId && cw.Folder.UserId == userId,
                cancellationToken);
        return userValidation;
    }
}