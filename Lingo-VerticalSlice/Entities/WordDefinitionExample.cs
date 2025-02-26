using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lingo_VerticalSlice.Entities;
public class WordDefinitionExample
{
    [Key]
    public int Id { get; set; }
    
    public int DefinitionId { get; set; }
    
    public string Example { get; set; }
    public string Proficiency { get; set; }
    
    public WordDefinition WordDefinition { get; set; }
}