﻿using Lingo_VerticalSlice.Contracts.CardSetWord.VocabStructure;
using Lingo_VerticalSlice.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lingo_VerticalSlice.Features.CardSetWord.GetCardSetWordWithoutSP;


    public sealed partial class CardSetWordController : ControllerBase
    
{
    private readonly ISender _sender;

    public CardSetWordController(ISender sender)
    {
        _sender = sender;
    }

    [Authorize]
    [HttpGet("get-words-cardset-without-sp")]
    [ProducesResponseType(typeof(VocabStructureResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Result>> GetCardSetWordAsync(GetCardSetWordWithoutSP.Query query, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(query,cancellationToken);
        if (result.IsFailure)
        {
            return NotFound(result);
        }

        return Ok(result.Value);
    }
}