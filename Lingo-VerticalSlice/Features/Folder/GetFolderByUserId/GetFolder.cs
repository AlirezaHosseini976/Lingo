using System.Security.Claims;
using Lingo_VerticalSlice.Contracts.Folder;
using Lingo_VerticalSlice.Shared;
using Mapster;
using MediatR;

namespace Lingo_VerticalSlice.Features.Folder.GetFolderByUserId;

public static class GetFolder
{
    public class Query : IRequest<Result<List<GetFolderResponse>>>
    {
    }
    internal sealed class Handler : IRequestHandler<Query, Result<List<GetFolderResponse>>>
    {
        private readonly IGetFolderByUserIdRepository _getFolderRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Handler(IGetFolderByUserIdRepository getFolderByUserIdRepository, IHttpContextAccessor httpContextAccessor)
        {
            _getFolderRepository = getFolderByUserIdRepository;
            _httpContextAccessor = httpContextAccessor; 
        }

        public async Task<Result<List<GetFolderResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Result.Failure<List<GetFolderResponse>>(new Error("UnAuthorizedUser", "You are not authorized"));
                
            }
            var folder = await _getFolderRepository.GetFolderByUserIdAsync(userId, cancellationToken);
            var folderResult = folder.Adapt<List<GetFolderResponse>>();
            if (!folderResult.Any())
            {
                return Result.Failure<List<GetFolderResponse>>(new Error(
                    "GetFolder.Empty",
                    "You don't have any folder yet!"));
            }

            return folderResult;
        }
    }
}