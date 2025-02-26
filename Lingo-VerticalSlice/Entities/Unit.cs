namespace Lingo_VerticalSlice.Entities;

public class Unit
{
    
    public int Id { get; set; }
    public string Name { get; set; }
    public int LessonId { get; set; }
    public Lesson Lesson { get; set; }
}