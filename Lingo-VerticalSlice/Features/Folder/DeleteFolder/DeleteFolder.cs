using System.Security.Claims;
using FluentValidation;
using Lingo_VerticalSlice.Database;
using Lingo_VerticalSlice.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lingo_VerticalSlice.Features.Folder.DeleteFolder;

public static class DeleteFolder
{
    public class Command : IRequest<Result<string>>
    {
        public int FolderId { get; set; }      
    }
    
    public class Validation : AbstractValidator<Command>
    {
        public Validation()
        {
            RuleFor(f => f.FolderId).NotEmpty();
        }
    }
    
    
    public sealed class Handler : IRequestHandler<Command,Result<string>>
    {
        private readonly IDeleteFolderRepository _deleteFolderRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IValidator<Command> _validator;

        public Handler( IValidator<Command> validator, IDeleteFolderRepository deleteFolderRepository, IHttpContextAccessor httpContextAccessor)
        {
            _validator = validator;
            _deleteFolderRepository = deleteFolderRepository;
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
                await _deleteFolderRepository.UserValidationAsync(request.FolderId, userId, cancellationToken);
            if (!userValidation)
            {
                return Result.Failure<string>(new Error("User.Error",
                    "You don't have permission to access these information!"));
            }
            await _deleteFolderRepository.DeleteFolderAsync(request.FolderId, userId, cancellationToken);
            return Result.Success<string>("Your folder has been deleted successfully!");
        }
    }
}