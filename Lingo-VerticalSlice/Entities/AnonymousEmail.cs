using System.Net.Mime;

namespace Lingo_VerticalSlice.Entities;

public class AnonymousEmail
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Message { get; set; }
}