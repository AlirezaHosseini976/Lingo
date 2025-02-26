using System.Security.Claims;
using Lingo_VerticalSlice.Contracts.CardSetWord.VocabStructure;
using Lingo_VerticalSlice.Shared;
using MediatR;

namespace Lingo_VerticalSlice.Features.CardSetWord.GetCardSetWord;

public class GetCardSetWord
{
    public class Query : IRequest<Result<List<VocabStructureResponse>>>
    {
        public int CardSetId { get; set; }
    }
    
    
    internal sealed class Handler : IRequestHandler<Query,Result<List<VocabStructureResponse>>>
    {
        private readonly IGetCardSetWordRepository _getCardSetWordRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Handler(IGetCardSetWordRepository getCardSetWordRepository, IHttpContextAccessor httpContextAccessor)
        {
            _getCardSetWordRepository = getCardSetWordRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Result<List<VocabStructureResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Result.Failure<List<VocabStructureResponse>>(new Error("User.Error",
                    "User is not authenticated."));
            }

            var userValidation =
                await _getCardSetWordRepository.ValidateUserAsync(request.CardSetId, userId, cancellationToken);
            if (!userValidation)
            {
                return Result.Failure<List<VocabStructureResponse>>(new Error ("User.Error",
                    "You don't have permission to access these information!"));
            }
            var cardSetWordList = await _getCardSetWordRepository.GetCardSetAsync(request.CardSetId, cancellationToken);
            var tomorrowQuizList = new List<VocabStructureResponse>();
            var todayQuizList = new List<VocabStructureResponse>();
            var otherDayQuizList = new List<VocabStructureResponse>();
            var today = DateTime.Now;
            var tomorrow = today.AddDays(1);
            var vocabStructure =
                await _getCardSetWordRepository.GetWordStructureAsync(request.CardSetId, userId,cancellationToken);
            foreach (var item in vocabStructure)
            {
                if (item.VocabStructure.NextQuizDate == today || item.VocabStructure.NextQuizDate < today)
                {
                    todayQuizList.Add(item.VocabStructure);
                }
                else if (item.VocabStructure.NextQuizDate == tomorrow)
                {
                    tomorrowQuizList.Add(item.VocabStructure);
                }
                else
                {
                    otherDayQuizList.Add(item.VocabStructure);
                }
            }
            var responseList = cardSetWordList.Select(item =>
            {
                var vocabStructForEachItems = vocabStructure.FirstOrDefault(v => v.VocabularyId == item.VocabularyId);
                return new VocabStructureResponse
                {
                    Vocabulary = vocabStructForEachItems?.VocabStructure.Vocabulary ?? string.Empty,
                    WordState = vocabStructForEachItems.VocabStructure.WordState,
                    NextQuizDate = vocabStructForEachItems.VocabStructure.NextQuizDate,
                    Structure = vocabStructForEachItems?.VocabStructure.Structure
                };

            }).ToList();
            
                
            return Result.Success(responseList);
        }
    }


    
}