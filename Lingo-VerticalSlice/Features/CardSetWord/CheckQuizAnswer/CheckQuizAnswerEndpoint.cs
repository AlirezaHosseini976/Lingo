    using Lingo_VerticalSlice.Contracts.CardSetWord;
    using Lingo_VerticalSlice.Contracts.CardSetWord.SpacedRepitition;
    using Lingo_VerticalSlice.Shared;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    namespace Lingo_VerticalSlice.Features.CardSetWord.CheckQuizAnswer;

    public sealed partial class CardSetWordController : ControllerBase
    {
        private readonly ISender _sender;

        public CardSetWordController(ISender sender)
        {
            _sender = sender; 
        }

        [Authorize]
        [HttpPut("check-quiz-answer")]
        [ProducesResponseType(typeof(NextStepResponseItem), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result>> CheckQuizAnswerAsync([FromBody] CheckQuizAnswerRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CheckQuizAnswer.Command
            {
                NextStepInfo = request.listCheckQuizAnswerRequest.Select(r=> new NextQuizInfoRequest
                {
                    IsCorrectAnswer = r.IsCorrectAnswer,
                    Vocabulary = r.Vocabulary
                }).ToList()
            };
            var result = await _sender.Send(command, cancellationToken);
            if (result.IsFailure)
            {
                return BadRequest(result);
            }

            return Ok(result.Value);
        }

    }