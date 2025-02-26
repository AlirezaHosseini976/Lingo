namespace Lingo_VerticalSlice.Entities;

public class CardSetWord
{
    public int Id { get; set; }
    public int CardSetId { get; set; }
    public int? VocabularyId { get; set; }
    
    public  CardSet CardSet { get; set; }
    public Word Words { get; set; }
}
