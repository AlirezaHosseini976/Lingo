using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lingo_VerticalSlice.Entities;

public class WordType
{
    [Key]
    public int Id { get; set; }

    public string Title { get; set; }
    
    public ICollection<WordDefinition> WordDefinitions { get; set; }
}