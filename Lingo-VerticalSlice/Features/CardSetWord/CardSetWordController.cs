using Lingo_VerticalSlice.Configurations;
using Lingo_VerticalSlice.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lingo_VerticalSlice.Features.CardSetWord;

[Authorize]
[ApiController]
[CustomResultFilter]
[Route("api/[controller]/[action]")]

public sealed partial class CardSetWordController : ControllerBase
{
    private readonly IMediator _mediator;

    public CardSetWordController(IMediator mediator)
    {
        _mediator = mediator;
    }
}