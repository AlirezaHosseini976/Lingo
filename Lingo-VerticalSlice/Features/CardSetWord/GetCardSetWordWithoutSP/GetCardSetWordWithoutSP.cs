using System.Security.Claims;
using Lingo_VerticalSlice.Contracts.CardSetWord.VocabStructure;
using Lingo_VerticalSlice.Contracts.CardSetWord.VocabStructureWithoutSP;
using Lingo_VerticalSlice.Shared;
using MediatR;

namespace Lingo_VerticalSlice.Features.CardSetWord.GetCardSetWordWithoutSP;

public class GetCardSetWordWithoutSP
{
    public class Query : IRequest<Result<List<VocabularyStructureResponse>>>
    {
        public int CardSetId { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, Result<List<VocabularyStructureResponse>>>
    {
        private readonly IGetCardSetWordWithoutSPRepository _getCardSetWordWithoutSpRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Handler(IGetCardSetWordWithoutSPRepository getCardSetWordRepositoryWithoutSpRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _getCardSetWordWithoutSpRepository = getCardSetWordRepositoryWithoutSpRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<List<VocabularyStructureResponse>>> Handle(Query request,
            CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Result.Failure<List<VocabularyStructureResponse>>(new Error("User.Error",
                    "User is not authenticated."));
            }

            var userValidation =
                await _getCardSetWordWithoutSpRepository.ValidateUserAsync(request.CardSetId, userId,
                    cancellationToken);
            if (!userValidation)
            {
                return Result.Failure<List<VocabularyStructureResponse>>(new Error("User.Error",
                    "You don't have permission to access these information!"));
            }

            var cardSetWordList =
                await _getCardSetWordWithoutSpRepository.GetCardSetAsync(request.CardSetId, cancellationToken);
            var vocabStructure =
                await _getCardSetWordWithoutSpRepository.GetWordStructureAsync(request.CardSetId, userId,
                    cancellationToken);

            var responseList = cardSetWordList.Select(item =>
            {
                var vocabStructForEachItems = vocabStructure
                    .FirstOrDefault(v => v.VocabularyId == item.VocabularyId);
                return new VocabularyStructureResponse
                {
                    Vocabulary = vocabStructForEachItems?.VocabularyStructure.Vocabulary ?? string.Empty,
                    Structure = vocabStructForEachItems?.VocabularyStructure.Structure
                };
            }).ToList();


            return Result.Success(responseList);
        }
    }
}