using Lingo_VerticalSlice.Database;
using Microsoft.EntityFrameworkCore;

namespace Lingo_VerticalSlice.Features.Folder.DeleteFolder;

public interface IDeleteFolderRepository
{
    Task DeleteFolderAsync(int folderId, string userId, CancellationToken cancellationToken);
    Task<bool> UserValidationAsync(int folderId, string userId, CancellationToken cancellationToken);
}


public class DeleteFolderRepository : IDeleteFolderRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public DeleteFolderRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task DeleteFolderAsync(int folderId,string userId, CancellationToken cancellationToken)
    {
        var folder = _applicationDbContext.Folder
            .FirstOrDefault(f => f.Id == folderId && f.UserId == userId);
        if (folder != null)
        {
            _applicationDbContext.Folder.Remove(folder);
        }

        await _applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> UserValidationAsync(int folderId, string userId, CancellationToken cancellationToken)
    {
        var userValidation =
            await _applicationDbContext.Folder.AnyAsync(f => f.Id == folderId && f.UserId == userId, cancellationToken);
        return userValidation;
    }
}