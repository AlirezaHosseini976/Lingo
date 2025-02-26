using AutoFixture;
using FluentAssertions;
using Lingo_VerticalSlice.Entities;
using Lingo_VerticalSlice.Shared;
using Moq;

namespace Lingo_VerticalSliceTest.Shared;

public class CalculateNextStepUnitTest
{
    [Theory]
    [ClassData(typeof(SpaceRepetitionTestData))]
    public void CalculateNextStep_EveryThingIsFine_ShouldReturnExpectedResult(SpacedRepetition spacedRepetition , bool isCorrectAnswer,
        WordState expectedState, DateTime expectedDate)
    {
        //Arrange
        var service = new CalculateNextStepService();
        
        //Act
        var result = service.CalculateNextStep(spacedRepetition, isCorrectAnswer);
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        var response = result.Value;
        response.Should().NotBeNull();
        
        response.NewState.Should().Be(expectedState);
        response.NextQuizDate.Should().BeCloseTo(expectedDate, TimeSpan.FromMinutes(5));
    }
}