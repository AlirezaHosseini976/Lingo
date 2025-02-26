using Lingo_VerticalSlice.Entities;

namespace Lingo_VerticalSlice.Contracts.CardSetWord;

public class CalculateNextStepResponse
{
    public DateTime NextQuizDate { get; set; }
    public WordState NewState { get; set; }
}
