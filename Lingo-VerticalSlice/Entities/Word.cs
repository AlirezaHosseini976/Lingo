using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lingo_VerticalSlice.Entities;

public class Word
{
    [Key]
    public int Id { get; set; }
    
    public string Vocabulary { get; set; }
    public ICollection<CardSetWord> CardSetWords { get; set; }
    public ICollection<WordDefinition> WordDefinitions { get; set; }
    public ICollection<SpacedRepetition> SpacedRepetitions { get; set; }
}