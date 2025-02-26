using Lingo_VerticalSlice.Configurations;
using Lingo_VerticalSlice.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lingo_VerticalSlice.Features.CardSet;

[Authorize]
[ApiController]
[CustomResultFilter]
[Route("api/[controller]/[action]")]
public sealed partial class CardSetController : ControllerBase
{
    private readonly IMediator _mediator;

    public CardSetController(IMediator mediator)
    {
        _mediator = mediator;
    }
}