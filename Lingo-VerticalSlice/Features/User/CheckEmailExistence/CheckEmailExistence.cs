using Lingo_VerticalSlice.Configurations;
using Lingo_VerticalSlice.Contracts.User;
using Lingo_VerticalSlice.Shared;
using MediatR;

namespace Lingo_VerticalSlice.Features.User.CheckEmailExistence;

public static class CheckEmailExistence
{
    public class Query : IRequest<Result<bool>>
    {
        public string Email { get; set; }
    }

    public sealed class Handler : IRequestHandler<Query, Result<bool>>
    {
        private readonly ICheckEmailExistenceRepository _checkEmailExistenceRepository;

        public Handler(ICheckEmailExistenceRepository checkEmailExistenceRepository)
        {
            _checkEmailExistenceRepository = checkEmailExistenceRepository;
        }

        public async Task<Result<bool>> Handle(Query request,
            CancellationToken cancellationToken)
        {
            var isValidEmail = EmailVerification.IsValidEmail(request.Email);
            if (!isValidEmail)
            {
                return Result.Failure<bool>(new Error("FormatValidationError",
                    "The email is not in valid format"));
            }

            var emailVerificationResponse =await 
                _checkEmailExistenceRepository.IfEmailExists(request.Email, cancellationToken);
            return Result.Success(emailVerificationResponse);
        }
    }
}