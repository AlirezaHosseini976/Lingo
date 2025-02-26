using Lingo_VerticalSlice.Contracts.CardSet;
using Lingo_VerticalSlice.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Lingo_VerticalSlice.Features.CardSet.DeleteCardSet.DeleteCardSet;

namespace Lingo_VerticalSlice.Features.CardSet.DeleteCardSet;

public sealed partial class CardSetController : ControllerBase
{
    private readonly ISender _sender;

    public CardSetController(ISender sender)
    {
        _sender = sender;
    }

    [Authorize]
    [HttpDelete("delete-cardset")]
    [ProducesResponseType(typeof(DeleteCardSetRequest), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<Result>> DeleteCardSetAsync(Command request, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(request, cancellationToken);
        if (result.IsFailure)
        {
            return BadRequest(result);
        }

        return Ok(result.Value);
    }
}