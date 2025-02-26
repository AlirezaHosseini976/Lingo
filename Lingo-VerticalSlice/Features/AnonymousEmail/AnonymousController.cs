using Lingo_VerticalSlice.Configurations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lingo_VerticalSlice.Features.AnonymousEmail;

[ApiController]
[CustomResultFilter]
[Route("api/[controller]/[action]")]
public sealed partial class AnonymousController : ControllerBase
{
    private readonly IMediator _mediator;

    public AnonymousController(IMediator mediator)
    {
        _mediator = mediator;
    }
}