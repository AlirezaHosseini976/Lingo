using Lingo_VerticalSlice.Contracts.CardSet;
using Lingo_VerticalSlice.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Lingo_VerticalSlice.Features.CardSet.CreateCardSetFolder;

public interface ICreateCardSetFolderRepository
{
    Task<bool> IsFolderExistsAsync(string folderName,string userId, CancellationToken cancellationToken);
    Task<CreateCardSetFolderResponse> CreateCardSetFolderAsync(Entities.Folder folder, CancellationToken cancellationToken);
}

public class CreateCardSetFolderRepository : ICreateCardSetFolderRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public CreateCardSetFolderRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<bool> IsFolderExistsAsync(string folderName, string userId, CancellationToken cancellationToken)
    {
        var isFolderExists = await _applicationDbContext.Folder.AnyAsync(f => f.Name == folderName && f.UserId == userId,
            cancellationToken);
        return isFolderExists;

    }

    public async Task<CreateCardSetFolderResponse> CreateCardSetFolderAsync(Entities.Folder folder, CancellationToken cancellationToken)
    {
        _applicationDbContext.Folder.Add(folder);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
        var response = new CreateCardSetFolderResponse
        {
            FolderId = folder.Id,
            CardSetId = folder.CardSets.First().Id
        };
        return response;
    }
    
    
}