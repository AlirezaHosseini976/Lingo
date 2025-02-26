using Lingo_VerticalSlice.Entities;

namespace Lingo_VerticalSlice.Contracts.CardSetWord.VocabStructure;

public class VocabStructureResponse
{
    public string Vocabulary { get; set; }
    public DateTime NextQuizDate { get; set; }
    public WordState WordState { get; set; }
    public List<DefinitionStructureResponse> Structure { get; set; }
}