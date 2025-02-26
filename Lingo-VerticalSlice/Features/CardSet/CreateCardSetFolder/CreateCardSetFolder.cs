using System.Diagnostics;
using System.Security.Claims;
using System.Transactions;
using FluentValidation;
using Lingo_VerticalSlice.Contracts.CardSet;
using Lingo_VerticalSlice.Contracts.Services;
using Lingo_VerticalSlice.Shared;
using MediatR;

namespace Lingo_VerticalSlice.Features.CardSet.CreateCardSetFolder;

public static class CreateCardSetFolder
{
    public class Command : IRequest<Result<CreateCardSetFolderResponse>>
    {
        public string FolderName { get; set; }
        public string CardSetName { get; set; }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.FolderName).NotEmpty();
            RuleFor(c => c.CardSetName).NotEmpty();
            
        }
    }
    public sealed class Handler : IRequestHandler<Command, Result<CreateCardSetFolderResponse>>
    {
        private readonly ICreateCardSetFolderRepository _createCardSetFolderRepository;
        private readonly IValidator<Command> _validator;
        private readonly IUserService _userService;

        public Handler(ICreateCardSetFolderRepository createCardSetFolderRepository,
             IValidator<Command> validator, IUserService userService)
        {
            _createCardSetFolderRepository = createCardSetFolderRepository;
            _validator = validator;
            _userService = userService;
        }

        public async Task<Result<CreateCardSetFolderResponse>> Handle(Command request,
            CancellationToken cancellationToken = default)
        {
            var transactionOption = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.Serializable,
                Timeout = TimeSpan.FromMinutes(1)
            };
            
            using var scope = new TransactionScope(TransactionScopeOption.Required, transactionOption,
                TransactionScopeAsyncFlowOption.Enabled);
            
            try
            {
                var userId = _userService.GetUserId();
                if (userId == null)
                {
                    return Result.Failure<CreateCardSetFolderResponse>(new Error("User.Error",
                        "User is not authenticated."));
                }

                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    return Result.Failure<CreateCardSetFolderResponse>(
                        new Error("CreateArticle.Validation", validationResult.ToString()));
                }

                var isFolderExist =
                    await _createCardSetFolderRepository.IsFolderExistsAsync(request.FolderName, userId,
                        cancellationToken);
                if (isFolderExist)
                {
                    return Result.Failure<CreateCardSetFolderResponse>(new Error("CardSetFolder.Error",
                        "This folder is already built!"));
                }

                var cardSetFolder = new Entities.Folder
                {
                    UserId = userId,
                    Name = request.FolderName,
                    CardSets = new List<Entities.CardSet>
                    {
                        new Entities.CardSet()
                        {
                            Name = request.CardSetName
                        }
                    }
                };
                var response =
                    await _createCardSetFolderRepository.CreateCardSetFolderAsync(cardSetFolder, cancellationToken);
                scope.Complete();
                return Result.Success(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }
    }
}