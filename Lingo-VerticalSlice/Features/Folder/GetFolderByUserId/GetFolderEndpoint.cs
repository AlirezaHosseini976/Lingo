using Lingo_VerticalSlice.Contracts.CardSet;
using Lingo_VerticalSlice.Database;
using Lingo_VerticalSlice.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Lingo_VerticalSlice.Features.Folder.GetFolderByUserId.GetFolder;

namespace Lingo_VerticalSlice.Features.Folder.GetFolderByUserId;

public sealed partial class FolderController : ControllerBase
{
    private readonly ISender _sender;

    public FolderController(ISender sender)
    {
        _sender = sender;
    }
    
    [Authorize]
    [HttpGet("get-folders")]
    [ProducesResponseType(typeof(GetFolderRepository), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Result>> GetFolderAsync(Query query, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(query,cancellationToken);
        if (result.IsFailure)
        {
            return BadRequest(result);
        }

        return Ok(result.Value);
    }
}