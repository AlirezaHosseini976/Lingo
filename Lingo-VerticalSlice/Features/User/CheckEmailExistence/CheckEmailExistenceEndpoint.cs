using Lingo_VerticalSlice.Contracts.User;
using Lingo_VerticalSlice.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lingo_VerticalSlice.Features.User.CheckEmailExistence;

public partial class UserController : ControllerBase
{
    private readonly ISender _sender ;

    public UserController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("email-verification")]
    [ProducesResponseType(typeof(CheckEmailExistenceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Result
    >> CheckEmailExistenceAsync(CheckEmailExistence.Query query, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(query,cancellationToken);
        if (result.IsFailure)
        {
            return BadRequest(result);
        }

        return Ok(result.Value);
    }
    
}

