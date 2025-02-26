using Lingo_VerticalSlice.Configurations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lingo_VerticalSlice.Features.User;

[ApiController]
[CustomResultFilter]
[Route("api/[controller]/[action]")]
public sealed partial class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }
}