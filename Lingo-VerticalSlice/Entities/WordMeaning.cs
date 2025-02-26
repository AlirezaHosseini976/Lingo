using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lingo_VerticalSlice.Entities;
public class WordMeaning
{
    [Key]
    public int Id { get; set; }
    
    public string Translation { get; set; }

    public string LanguageCode { get; set; }
    
    public int DefinitionId { get; set; }
    
    public WordDefinition WordDefinition { get; set; }
}