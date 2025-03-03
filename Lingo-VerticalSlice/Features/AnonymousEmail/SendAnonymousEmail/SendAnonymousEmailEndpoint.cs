using System.Security.Claims;
using Lingo_VerticalSlice.Contracts.AnonymousEmail;
using Lingo_VerticalSlice.Contracts.CardSetWord;
using Lingo_VerticalSlice.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using static Lingo_VerticalSlice.Features.AnonymousEmail.SendAnonymousEmail.SendAnonymousEmail;

namespace Lingo_VerticalSlice.Features.AnonymousEmail.SendAnonymousEmail;

public sealed partial class AnonymousController : ControllerBase
{
    private readonly ISender _sender;

    public AnonymousController(ISender sender)
    {
        _sender = sender;
    }


    [HttpPost("sending-anonymous-email")]
    [ProducesResponseType(typeof(AddWordToCardSetRequest), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<Result>> SendAnonymousEmail([FromBody]SendRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.Adapt<Command>();
        var result = await _sender.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            return BadRequest(result);
        }

        return Ok(result.Value);
    }
}