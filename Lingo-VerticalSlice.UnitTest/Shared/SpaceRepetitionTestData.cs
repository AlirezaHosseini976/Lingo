using System.Collections;
using Lingo_VerticalSlice.Entities;

namespace Lingo_VerticalSliceTest.Shared;

public class SpaceRepetitionTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        DateTime currentDate = DateTime.UtcNow;
        yield return new object[]
        {
            new SpacedRepetition
            {
                WordState = WordState.Encounter,
                NextDate = currentDate.AddDays(-1),
            },
            true,
            WordState.Recognition,
            currentDate.AddHours(4),
        };
        yield return new object[]
        {
            new SpacedRepetition
            {
                WordState = WordState.Recall,
                NextDate = currentDate.AddDays(-1),
            },
            true,
            WordState.Familiarity,
            currentDate.AddDays(1),
        };
        yield return new object[]
        {
            new SpacedRepetition
            {
                WordState = WordState.Proficiency,
                NextDate = currentDate.AddDays(-1),
            },
            false,
            WordState.Encounter,
            currentDate.AddMinutes(1),
        };
        
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}