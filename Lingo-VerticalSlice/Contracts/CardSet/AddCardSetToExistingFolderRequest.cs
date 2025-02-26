namespace Lingo_VerticalSlice.Contracts.CardSet;

public class AddCardSetToExistingFolderRequest
{
    public int FolderId { get; set; }
    public string CardSetName { get; set; }
}