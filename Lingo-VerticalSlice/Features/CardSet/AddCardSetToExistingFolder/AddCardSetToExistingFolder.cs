using FluentValidation;
using Lingo_VerticalSlice.Contracts.CardSet;
using Lingo_VerticalSlice.Database;
using Lingo_VerticalSlice.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lingo_VerticalSlice.Features.CardSet.AddCardSetToExistingFolder;

public static class AddCardSetToExistingFolder
{
    public class Command : IRequest<Result<AddResponse>>
    {
        public int FolderId { get; set; }
        public string CardSetName { get; set; }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.CardSetName).NotEmpty();
        }
    }

    public sealed class Handler : IRequestHandler<Command, Result<AddResponse>>
    {
        private readonly IAddCardSetToExistingFolderRepository _addCardSetToExistingFolderRepository;
        private readonly IValidator<Command> _validator;

        public Handler( IValidator<Command> validator, IAddCardSetToExistingFolderRepository addCardSetToExistingFolderRepository)
        {
            _validator = validator;
            _addCardSetToExistingFolderRepository = addCardSetToExistingFolderRepository;
        }
    
        public async Task<Result<AddResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            var isCardSetExists =await _addCardSetToExistingFolderRepository.IsCardSetExistsAsync(request.FolderId, request.CardSetName,cancellationToken);
            if (isCardSetExists)
            {
                return Result.Failure<AddResponse>(new Error("CardSetExists",
                    "This CardSetName is already exists in this folder"));
            }
            
            var cardSetCount =
                await _addCardSetToExistingFolderRepository.CardSetInFolderCountAsync(request.FolderId, cancellationToken);
            if (cardSetCount >= 20)
            {
                return Result.Failure<AddResponse>(new Error("CardSetLimitIsReached",
                    "Folder limit is reached, please build a new folder!"));
            }

            var cardSet = new Entities.CardSet
            {
                FolderId = request.FolderId,
                Name = request.CardSetName
            };
            var id= await _addCardSetToExistingFolderRepository.AddCardSetToExistingFolderAsync(cardSet,cancellationToken);
            var response = new AddResponse
            {
                Id = id,
                Message = "CardSet has been build!"
            };
            return Result.Success(response);
        }
    }
}