using System.Security.Claims;
using Lingo_VerticalSlice.Contracts.CardSetWord;
using Lingo_VerticalSlice.Contracts.CardSetWord.SpacedRepitition;
using Lingo_VerticalSlice.Contracts.Services;
using Lingo_VerticalSlice.Entities;
using Lingo_VerticalSlice.Shared;
using MediatR;


namespace Lingo_VerticalSlice.Features.CardSetWord.CheckQuizAnswer;

public static class CheckQuizAnswer
{
    public class Command : IRequest<Result<List<NextStepResponseItem>>>
    {
        public List<NextQuizInfoRequest>NextStepInfo { get; set; }
    }
    
    public sealed class Handler : IRequestHandler<Command,Result<List<NextStepResponseItem>>>
    {
        private readonly CalculateNextStepService _calculateNextStepService;
        private readonly ICheckQuizAnswerRepository _checkQuizAnswer;
        private readonly IUserService _userService;

        public Handler(ICheckQuizAnswerRepository checkQuizAnswer, CalculateNextStepService calculateNextStepService, IUserService userService)
        {
            _checkQuizAnswer = checkQuizAnswer;
            _calculateNextStepService = calculateNextStepService;
            _userService = userService;
            _userService = userService;
        }

        public async Task<Result<List<NextStepResponseItem>>> Handle(Command command, CancellationToken cancellationToken)
        {
            var userId = _userService.GetUserId();
            if (userId == null)
            {
                return Result.Failure<List<NextStepResponseItem>>(new Error("User.Error",
                    "User is not authenticated."));
            }
            var nextStepResponses = new List<NextStepResponseItem>();
            var vocabulary = command.NextStepInfo.Select(info => info.Vocabulary).ToList();
            var vocabulariesForSpacedRepetition = await _checkQuizAnswer.GetSpaceRepetitionDetailAsync(vocabulary,userId, cancellationToken);
            foreach (var item in command.NextStepInfo)
            {
                var spacedRepetitionDetail = vocabulariesForSpacedRepetition.FirstOrDefault(r => r.Words.Vocabulary == item.Vocabulary);
                if (spacedRepetitionDetail == null)
                {
                    return Result.Failure<List<NextStepResponseItem>>(new Error($"Error.NotFound","Vocabulary not found!"));
                }
                if (spacedRepetitionDetail.WordState == WordState.Mastery)
                {   
                    continue;
                }

                var nextStepResult = _calculateNextStepService.CalculateNextStep(spacedRepetitionDetail, item.IsCorrectAnswer);
                if (nextStepResult.IsSuccess)
                {
                    spacedRepetitionDetail.NextDate = nextStepResult.Value.NextQuizDate;
                    spacedRepetitionDetail.WordState = nextStepResult.Value.NewState;
                    nextStepResponses.Add(new NextStepResponseItem
                    {
                        SpacedRepetitionId = spacedRepetitionDetail.Id,
                        NewState = nextStepResult.Value.NewState,
                        NextQuizDate = nextStepResult.Value.NextQuizDate.ToString("O")
                    });
                    await _checkQuizAnswer.UpdateSpaceRepetitionAsync(spacedRepetitionDetail, cancellationToken);
                }
                else
                {
                    return Result.Failure<List<NextStepResponseItem>>(nextStepResult.Error);
                }
            }
            return Result.Success(nextStepResponses);
        }
    }
}