using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Lingo_VerticalSlice.Entities;

public class SpacedRepetition
{
    public int Id { get; set; }
    [ForeignKey("VocabularyId")]
    public int? VocabularyId { get; set; }
    [ForeignKey("UserId")]
    public string UserId { get; set; }
    public WordState WordState { get; set; }
    public DateTime NextDate { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public IdentityUser User { get;set; }
    [ForeignKey(nameof(VocabularyId))]
    public Word Words { get; set; }
}
public enum WordState
{
    Encounter = 1,
    Recognition = 2,
    Recall = 3,
    Familiarity = 4, 
    Proficiency = 5,
    Fluency  = 6, 
    Mastery = 7
}