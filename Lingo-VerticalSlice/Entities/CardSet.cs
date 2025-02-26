namespace Lingo_VerticalSlice.Entities;

public class CardSet
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int FolderId { get; set; }
    public Folder Folder { get; set; }
    public ICollection<CardSetWord> CardSetWords { get; set; }
}