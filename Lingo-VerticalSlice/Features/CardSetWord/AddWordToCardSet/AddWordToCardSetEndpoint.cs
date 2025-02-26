using Lingo_VerticalSlice.Contracts.CardSetWord;
using Lingo_VerticalSlice.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Lingo_VerticalSlice.Features.CardSetWord.AddWordToCardSet.AddWordToCardSet;

namespace Lingo_VerticalSlice.Features.CardSetWord.AddWordToCardSet;

public sealed partial class CardSetWordController : ControllerBase
{
    private readonly ISender _sender;

    public CardSetWordController(ISender sender)
    {
        _sender = sender;
    }

    [Authorize]
    [HttpPost("add-word-to-cardset")]
    [ProducesResponseType(typeof(AddWordToCardSetRequest), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Result>> AddWordToCardSetAsync([FromBody]AddWordToCardSetRequest request,
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