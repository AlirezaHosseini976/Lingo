using Lingo_VerticalSlice.Entities;

namespace Lingo_VerticalSlice.Contracts.CardSetWord.SpacedRepitition;

public class NextStepResponseItem
{
    public int SpacedRepetitionId { get; set; }
    public WordState NewState { get; set; }
    public string NextQuizDate { get; set; }
}