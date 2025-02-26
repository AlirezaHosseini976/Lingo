namespace Lingo_VerticalSlice.Entities;

public class UnitWord
{
    public int Id { get; set; }
    public int UnitId { get; set; }
    public int VocabularyId { get; set; }
    public Unit Unit { get; set; }
    public Word Word { get; set; }
}