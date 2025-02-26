using Lingo_VerticalSlice.Entities;

namespace Lingo_VerticalSlice.Contracts.CardSetWord;

public class RequirmentForNextQuizResponse
{
    public int CardSetWordId { get; set; }
    public string VocabText { get; set; }
    public DateOnly QuizDate { get; set; }
    public WordState WordState { get; set; }
    
}
