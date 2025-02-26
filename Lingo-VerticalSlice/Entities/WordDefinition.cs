using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lingo_VerticalSlice.Entities;

public class WordDefinition
{

    [Key]
    public int Id { get; set; }

    public int WordTypeId { get; set; }
    
    public int VocabularyId { get; set; }
    
    public string DefinitionText { get; set; }
    
    
    public ICollection<WordDefinitionExample> WordDefinitionExamples { get; set; }
    public ICollection<WordSynonym> Synonyms { get; set; }
    public ICollection<WordMeaning> WordMeaning { get; set; }
    public Word Word { get; set; }
    public WordType WordType { get; set; }
}