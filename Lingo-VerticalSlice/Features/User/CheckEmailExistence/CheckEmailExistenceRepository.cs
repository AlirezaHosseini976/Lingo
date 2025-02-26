using Lingo_VerticalSlice.Database;
using Microsoft.EntityFrameworkCore;

namespace Lingo_VerticalSlice.Features.User.CheckEmailExistence;

public interface ICheckEmailExistenceRepository
{
    Task<bool> IfEmailExists(string email, CancellationToken cancellationToken);
}

public class CheckEmailExistenceRepository : ICheckEmailExistenceRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public CheckEmailExistenceRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<bool> IfEmailExists(string email, CancellationToken cancellationToken)
    {
        var emailExists = await _applicationDbContext.Users.AnyAsync(e => e.Email == email,cancellationToken);
        return emailExists;
    }
}