using Lingo_VerticalSlice.Contracts.CardSetWord;
using Lingo_VerticalSlice.Contracts.CardSetWord.SpacedRepitition;
using Lingo_VerticalSlice.Entities;

namespace Lingo_VerticalSlice.Shared
{
    public class CalculateNextStepService
    {
        public Result<CalculateNextStepResponse> CalculateNextStep(SpacedRepetition spacedRepetition, bool isCorrectAnswer)
        {
            DateTime currentDate = DateTime.Now.ToUniversalTime();
            if (spacedRepetition.NextDate > currentDate)
            {
                return Result.Success(new CalculateNextStepResponse
                {
                    NewState = spacedRepetition.WordState,
                    NextQuizDate = spacedRepetition.NextDate
                });
            }
            DateTime nextStepDate;
            if (isCorrectAnswer)
            {
                nextStepDate = spacedRepetition.WordState switch
                {
                    WordState.Encounter => currentDate.AddHours(4),
                    WordState.Recognition => currentDate.AddHours(12),
                    WordState.Recall => currentDate.AddDays(1),
                    WordState.Familiarity => currentDate.AddDays(2),
                    WordState.Proficiency => currentDate.AddDays(3),
                    WordState.Fluency => currentDate.AddDays(4),
                    WordState.Mastery => currentDate,
                    _ => default
                };
                spacedRepetition.WordState = spacedRepetition.WordState < WordState.Mastery
                    ? spacedRepetition.WordState + 1
                    : WordState.Mastery;
            }
            else
            {
                nextStepDate = currentDate.AddMinutes(1);

                spacedRepetition.WordState = WordState.Encounter;
            }

            spacedRepetition.NextDate = nextStepDate;
            return Result.Success(new CalculateNextStepResponse
            {
                NewState = spacedRepetition.WordState,
                NextQuizDate = spacedRepetition.NextDate
            });
        }
    }
}