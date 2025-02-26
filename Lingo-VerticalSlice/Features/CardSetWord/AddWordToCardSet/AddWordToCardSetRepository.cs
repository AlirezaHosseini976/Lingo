using Lingo_VerticalSlice.Contracts.CardSetWord;
using Lingo_VerticalSlice.Contracts.CardSetWord.VocabStructure;
using Lingo_VerticalSlice.Database;
using Lingo_VerticalSlice.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lingo_VerticalSlice.Features.CardSetWord.AddWordToCardSet;

public interface IAddWordToCardSetRepository
{
    Task<AddWordToCardSetResponse> AddWordToCardSetAsync(Entities.CardSetWord cardSetWord, SpacedRepetition spaceRepetition,
        CancellationToken cancellationToken);

    Task<bool> IfWordExistsInCardSetAsync(Entities.CardSetWord cardSetWord, CancellationToken cancellationToken);
    Task<int?> FindVocabularyIdAsync(string vocabulary, CancellationToken cancellationToken);

    Task<VocabStructureResponse> FindVocabStructureAsync(int? vocabularyId, string userId,
        CancellationToken cancellationToken);
}

public class AddWordToCardSetRepository : IAddWordToCardSetRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public AddWordToCardSetRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<bool> IfWordExistsInCardSetAsync(Entities.CardSetWord cardSetWord,
        CancellationToken cancellationToken)
    {
        var ifWordExistsInCardSet = await _applicationDbContext.CardSetWord.AnyAsync(w =>
            w.VocabularyId == cardSetWord.VocabularyId
            && w.Id == cardSetWord.Id, cancellationToken);
        return ifWordExistsInCardSet;
    }

    public async Task<AddWordToCardSetResponse> AddWordToCardSetAsync(Entities.CardSetWord cardSetWord,
        SpacedRepetition spaceRepetition,
        CancellationToken cancellationToken)
    {
        _applicationDbContext.Add(cardSetWord);
        var isSpaceRepetitionExists = await _applicationDbContext.SpacedRepetition.AnyAsync(u =>
            u.UserId == spaceRepetition.UserId
            && u.VocabularyId == spaceRepetition.VocabularyId, cancellationToken);
        if (!isSpaceRepetitionExists)
        {
            _applicationDbContext.Add(spaceRepetition);
        }

        await _applicationDbContext.SaveChangesAsync(cancellationToken);
        var response = new AddWordToCardSetResponse
        {
            SpacedRepetitionId = spaceRepetition.Id,
            CardSetWordId = cardSetWord.Id
        };
        return (response);
    }


    public async Task<int?> FindVocabularyIdAsync(string vocabularyName, CancellationToken cancellationToken)
    {
        var vocabulary =
            await _applicationDbContext.Word.FirstOrDefaultAsync(w => w.Vocabulary == vocabularyName,
                cancellationToken);
        if (vocabulary == null) return null;
        var vocabularyId = vocabulary.Id;
        return vocabularyId;
    }

    public async Task<VocabStructureResponse> FindVocabStructureAsync(int? vocabularyId, string userId,
        CancellationToken cancellationToken)
    {
        var query = _applicationDbContext.Word
            .Include(x => x.WordDefinitions).ThenInclude(x => x.WordMeaning)
            .Include(x => x.WordDefinitions).ThenInclude(x => x.WordDefinitionExamples)
            .Include(x => x.WordDefinitions).ThenInclude(x => x.Synonyms)
            .Include(x => x.WordDefinitions).ThenInclude(x => x.WordType).Select(
                x => new
                {
                    id = x.Id,
                    vocabulary = x.Vocabulary,
                    structure = x.WordDefinitions.Select(y => new DefinitionStructureResponse
                    {
                        Definition = y.DefinitionText,
                        Translation = y.WordMeaning.FirstOrDefault().Translation,
                        Examples = y.WordDefinitionExamples.Select(s => s.Example).ToList(),
                        Synonyms = y.Synonyms.Select(sy => sy.Synonym).ToList(),
                        PartOfSpeech = y.WordType.Title
                    }).ToList()
                });
        var vocabStruct = await query
            .Where(v => v.id == vocabularyId)
            .Select(v => new
            {
                word = v,
                SpacedRepetition = _applicationDbContext.SpacedRepetition
                    .FirstOrDefault(sr => sr.VocabularyId == v.id && sr.UserId == userId)
            }).Select(result => new VocabStructureResponse
            {
                Vocabulary = result.word.vocabulary,
                NextQuizDate = result.SpacedRepetition.NextDate,
                WordState = result.SpacedRepetition.WordState,
                Structure = result.word.structure
            }).FirstOrDefaultAsync(cancellationToken);
        return vocabStruct;
    }
}