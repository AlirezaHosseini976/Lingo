namespace Lingo_VerticalSlice.Contracts.CardSetWord;

public class AddWordToCardSetRequest
{
    public int CardSetId { get; set; }
    public string Vocabulary { get; set; }
}