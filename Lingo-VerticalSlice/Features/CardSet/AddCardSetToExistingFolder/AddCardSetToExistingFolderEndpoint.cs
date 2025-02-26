using Lingo_VerticalSlice.Contracts.CardSet;
using Lingo_VerticalSlice.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Lingo_VerticalSlice.Features.CardSet.AddCardSetToExistingFolder.AddCardSetToExistingFolder;

namespace Lingo_VerticalSlice.Features.CardSet.AddCardSetToExistingFolder;

public sealed partial class CardSetController : ControllerBase
{
    private readonly ISender _sender;

    public CardSetController(ISender sender)
    {
        _sender = sender;
    }

    
    [Authorize]
    [HttpPost("add-cardset-to-folder")]
    [ProducesResponseType(typeof(AddCardSetToExistingFolderRequest), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Result>> AddingCardSetToExistingFolderAsync([FromBody]AddCardSetToExistingFolderRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.Adapt<Command>();
        var result = await _sender.Send(command,cancellationToken);
        if (result.IsFailure)
        {
            return BadRequest(result);
        }
        return Ok(result.Value);
    }
}