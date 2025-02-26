using Lingo_VerticalSlice.Database;

namespace Lingo_VerticalSlice.Features.AnonymousEmail.SendAnonymousEmail;

public interface ISendAnonymousEmailRepository
{
    Task SendAnonymousEmailAsync(Entities.AnonymousEmail emailMessage, CancellationToken cancellationToken);
}

public class SendAnonymousEmailRepository : ISendAnonymousEmailRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public SendAnonymousEmailRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task SendAnonymousEmailAsync(Entities.AnonymousEmail emailMessage, CancellationToken cancellationToken)
    {
        _applicationDbContext.AnonymousEmail.Add(emailMessage);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}