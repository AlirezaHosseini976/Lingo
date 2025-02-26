namespace Lingo_VerticalSlice.Contracts.CardSetWord.VocabStructureWithoutSP;

public class VocabularyDetailStructureResponse
{
    public string Definition { get; set; }
    public string PartOfSpeech { get; set; }
    public List<string> Examples { get; set; }
    public List<string> Synonyms { get; set; }
    public string Translation { get; set; }
}