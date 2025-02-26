using System.Security.Claims;
using FluentValidation;
using Lingo_VerticalSlice.Contracts.CardSetWord;
using Lingo_VerticalSlice.Contracts.CardSetWord.VocabStructure;
using Lingo_VerticalSlice.Database;
using Lingo_VerticalSlice.Entities;
using Lingo_VerticalSlice.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lingo_VerticalSlice.Features.CardSetWord.AddWordToCardSet;

public static class AddWordToCardSet
{
    public class Command : IRequest<Result<VocabStructureResponse>>
    {
        public int CardSetId { get; set; }
        public string  Vocabulary { get; set; }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.CardSetId).NotEmpty();
            RuleFor(c => c.Vocabulary).NotEmpty();
            
        }
    }

    public sealed class Handler : IRequestHandler<Command, Result<VocabStructureResponse>>
    {
        private readonly IAddWordToCardSetRepository _addWordToCardSetRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IValidator<Command> _validator;
        
        public Handler( IValidator<Command> validator, IAddWordToCardSetRepository addWordToCardSetRepository, IHttpContextAccessor httpContextAccessor)
        {
            _validator = validator;
            _addWordToCardSetRepository = addWordToCardSetRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        
        public async Task<Result<VocabStructureResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Result.Failure<VocabStructureResponse>(new Error("User.Error",
                    "User is not authenticated."));
            }
            var vocabularyId =
                await _addWordToCardSetRepository.FindVocabularyIdAsync(request.Vocabulary, cancellationToken);
            if (vocabularyId == null)
            {
                return Result.Failure<VocabStructureResponse>(new Error("Error.VocabularyNotFound",
                    "Sorry! This word is not in present in our database"));
            }
            var cardSetWord = new Entities.CardSetWord
            {
                VocabularyId = vocabularyId,
                CardSetId = request.CardSetId,
            };
            var isWordExistsInCardSet =
                await _addWordToCardSetRepository.IfWordExistsInCardSetAsync(cardSetWord, cancellationToken);
            if (isWordExistsInCardSet)
            {
                return Result.Failure<VocabStructureResponse>(new Error("Error.Duplication",
                    "You have already added this word"));
            }
            var spacedRepetition = new SpacedRepetition
            {
                NextDate = DateTime.Now.ToUniversalTime(),
                WordState = WordState.Encounter,
                UserId = userId,
                VocabularyId = vocabularyId
            };
            await _addWordToCardSetRepository.AddWordToCardSetAsync(cardSetWord, spacedRepetition,cancellationToken);
            var vocabStruct =
                await _addWordToCardSetRepository.FindVocabStructureAsync(vocabularyId, userId,cancellationToken);
            
                
            return Result.Success<VocabStructureResponse>(vocabStruct);
        }
    }
}