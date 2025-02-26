using Microsoft.EntityFrameworkCore;

namespace Lingo_VerticalSlice.Entities;
public class VocabularyDetailsMaterializedView
{
    public int UniqueId { get; set; }
    public int VocabularyId { get; set; }
    public string Vocabulary { get; set; }
    public string Definition { get; set; }
    public string? Translation { get; set; }
    public string PartOfSpeech { get; set; }
    public string[] Example { get; set; }
    public string[] Synonym { get; set; }
}