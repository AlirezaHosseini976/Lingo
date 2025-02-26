using Lingo_VerticalSlice.Contracts.CardSet;
using Lingo_VerticalSlice.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lingo_VerticalSlice.Features.CardSet.GetCardSet;

public sealed partial class CardSetController : ControllerBase
{
    private readonly ISender _sender;

    public CardSetController(ISender sender)
    {
        _sender = sender;
    }

    [Authorize]
    [HttpGet("get-cardSet")]
    [ProducesResponseType(typeof(CardSetResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Result>> GetCardSetAsync(GetCardSet.Query query, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(query,cancellationToken);
        if (result.IsFailure)
        {
            return NotFound(result);
        }

        return Ok(result.Value);
    }
}