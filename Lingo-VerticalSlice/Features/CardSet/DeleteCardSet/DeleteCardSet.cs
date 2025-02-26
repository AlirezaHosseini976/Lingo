using System.Security.Claims;
using FluentValidation;
using Lingo_VerticalSlice.Contracts.CardSetWord.VocabStructure;
using Lingo_VerticalSlice.Database;
using Lingo_VerticalSlice.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lingo_VerticalSlice.Features.CardSet.DeleteCardSet;

public static class DeleteCardSet
{
    public class Command : IRequest<Result<string>>
    {
        public int CardSetId { get; set; }
    }
    
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.CardSetId).NotEmpty();
        }
    }

    public sealed class Handler : IRequestHandler<Command ,Result<string>>
    {
        private readonly IDeleteCardSetRepository _deleteCardSetRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IValidator<Command> _validator;

        public Handler( IValidator<Command> validator, IDeleteCardSetRepository deleteCardSetRepository, IHttpContextAccessor httpContextAccessor)
        {
            _validator = validator;
            _deleteCardSetRepository = deleteCardSetRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Result.Failure<string>(new Error("User.Error",
                    "User is not authenticated."));
            }

            var userValidation =
                await _deleteCardSetRepository.UserValidation(request.CardSetId, userId, cancellationToken);
            if (!userValidation)
            {
                return Result.Failure<string>(new Error("User.Error",
                    "You don't have permission to access these information!"));
            }
            await _deleteCardSetRepository.DeleteCardSetAsync(request.CardSetId,cancellationToken);
            return Result.Success<string>("The cardSet is deleted successfully!");
        }
    }
}