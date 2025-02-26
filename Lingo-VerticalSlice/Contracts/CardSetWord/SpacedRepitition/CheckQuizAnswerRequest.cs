namespace Lingo_VerticalSlice.Contracts.CardSetWord;

public class CheckQuizAnswerRequest
{
    public int CardSetId { get; set; }
    public List<ListCheckQuizAnswerRequest> listCheckQuizAnswerRequest { get; set; }
}

public class ListCheckQuizAnswerRequest
{
    public bool IsCorrectAnswer { get; set; }
    public string Vocabulary { get; set; }
}