using System.Net;
using FluentValidation;
using Lingo_VerticalSlice.Configurations;
using Lingo_VerticalSlice.Contracts.AnonymousEmail;
using Lingo_VerticalSlice.Exceptions;
using Lingo_VerticalSlice.MiddleWare;
using Lingo_VerticalSlice.Shared;
using MediatR;

namespace Lingo_VerticalSlice.Features.AnonymousEmail.SendAnonymousEmail;

public static class SendAnonymousEmail 
{
    public class Command : IRequest<Result<string>>
    {
        public string Email { get; set; }
        public string  Message { get; set; }
    }
    
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c =>c.Email ).NotEmpty();
            RuleFor(c =>c.Message ).NotEmpty();
        }
    }
    
    public sealed class Handler : IRequestHandler<Command,Result<string>>
    {
        private readonly ISendAnonymousEmailRepository _sendAnonymousEmailRepository;
        private readonly IValidator<Command> _validator;

        public Handler(ISendAnonymousEmailRepository sendAnonymousEmailRepository, IValidator<Command> validator)
        {
            _sendAnonymousEmailRepository = sendAnonymousEmailRepository;
            _validator = validator;
        }

        public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            
            var email = new Entities.AnonymousEmail
            {
                Email = request.Email,
                Message = request.Message
            };
            var isEmailValid =EmailVerification.IsValidEmail(request.Email);
            if (!isEmailValid)
            {
                return Result.Failure<string>(new Error("Validation error",  "Email.Validation"));
            }
            await _sendAnonymousEmailRepository.SendAnonymousEmailAsync(email, cancellationToken);
            return Result.Success("Your email has been sent!");
        }
    }
}