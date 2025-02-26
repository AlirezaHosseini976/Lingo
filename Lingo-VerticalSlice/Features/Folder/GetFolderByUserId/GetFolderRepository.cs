using Lingo_VerticalSlice.Database;
using Microsoft.EntityFrameworkCore;

namespace Lingo_VerticalSlice.Features.Folder.GetFolderByUserId;

public interface IGetFolderByUserIdRepository
{
    Task<List<Entities.Folder>> GetFolderByUserIdAsync(string userId, CancellationToken cancellationToken);
}

public class GetFolderRepository : IGetFolderByUserIdRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public GetFolderRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<List<Entities.Folder>> GetFolderByUserIdAsync(string userId, CancellationToken cancellationToken)
    {
        var folder = await _applicationDbContext.Folder
            .Where(f => f.UserId == userId)
            .ToListAsync(cancellationToken);
        return folder;
    }
}