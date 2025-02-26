using Lingo_VerticalSlice.Contracts.CardSet;
using Lingo_VerticalSlice.Database;
using Lingo_VerticalSlice.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lingo_VerticalSlice.Features.CardSet.GetCardSet;

public static class GetCardSet
{
    public class Query : IRequest<Result<List<CardSetResponse>>>
    {
        public int FolderId { get; set; }   
    }
    
    internal sealed class Handler : IRequestHandler<Query,Result<List<CardSetResponse>>>
    {
        private readonly IGetCardSetRepository _getCardSetRepository;

        public Handler(IGetCardSetRepository getCardSetRepository)
        {
            _getCardSetRepository = getCardSetRepository;
        }

        public async Task<Result<List<CardSetResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var cardSet =
                await _getCardSetRepository.GetCardSetByFolderIdAsync(request.FolderId, cancellationToken);
            var cardSetResult = cardSet.Adapt<List<CardSetResponse>>();
            if (!cardSetResult.Any())
            {
                return Result.Failure<List<CardSetResponse>>(new Error(
                    "GetCardSet.Empty",
                    "No cardSet has been added to this folder"));
            }

            return cardSetResult;
        }
    }
}