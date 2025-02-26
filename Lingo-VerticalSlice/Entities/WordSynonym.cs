using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lingo_VerticalSlice.Entities;
public class WordSynonym
{
    [Key]
    public int Id { get; set; }

    public int DefinitionId { get; set; }

    public string Synonym { get; set; }
    
    public WordDefinition WordDefinition { get; set; }
}