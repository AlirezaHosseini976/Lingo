using Microsoft.AspNetCore.Identity;

namespace Lingo_VerticalSlice.Entities;

public class Folder
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string UserId { get; set; }
    public ICollection<CardSet> CardSets { get; set; }
    public IdentityUser User { get; set; }
}