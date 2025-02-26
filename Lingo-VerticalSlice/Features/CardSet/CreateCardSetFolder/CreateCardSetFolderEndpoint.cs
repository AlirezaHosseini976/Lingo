using Lingo_VerticalSlice.Contracts.CardSet;
using Lingo_VerticalSlice.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lingo_VerticalSlice.Features.CardSet.CreateCardSetFolder;

public sealed partial class CardSetController : ControllerBase
{
    private readonly ISender _sender;

    public CardSetController(ISender sender)
    {
        _sender = sender;
    }
    
    [Authorize]
    [HttpPost("create-cardset-folder")]
    [ProducesResponseType(typeof(CardSetFolderRequest), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Result>> AddCardSetWithFolderAsync([FromBody]CardSetFolderRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.Adapt<CreateCardSetFolder.Command>();
        var result = await _sender.Send(command, cancellationToken);
        if (result.IsFailure)
        {
           return BadRequest(result);
        }
        return Ok(result.Value);
    }
}