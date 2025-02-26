using Lingo_VerticalSlice.Contracts.Folder;
using Lingo_VerticalSlice.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Lingo_VerticalSlice.Features.Folder.DeleteFolder.DeleteFolder;

namespace Lingo_VerticalSlice.Features.Folder.DeleteFolder;

public sealed partial class FolderController : ControllerBase
{
    private readonly ISender _sender;

    public FolderController(ISender sender)
    {
        _sender = sender;
    }

    [Authorize]
    [HttpDelete("delete-folder")]
    [ProducesResponseType(typeof(DeleteFolderRequest), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<Result>> DeleteFolderAsync(Command request, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(request,cancellationToken);
        if (result.IsFailure)
        {
            return NotFound(result);
        }

        return Ok(result.Value);
    }
}