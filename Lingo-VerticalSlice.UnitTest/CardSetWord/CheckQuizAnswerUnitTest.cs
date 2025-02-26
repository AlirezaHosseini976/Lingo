using AutoFixture;
using AutoFixture.Xunit2;
using Lingo_VerticalSlice.Contracts.CardSetWord;
using Lingo_VerticalSlice.Contracts.CardSetWord.SpacedRepitition;
using Lingo_VerticalSlice.Contracts.Services;
using Lingo_VerticalSlice.Entities;
using Lingo_VerticalSlice.Features.CardSetWord.CheckQuizAnswer;
using Lingo_VerticalSlice.Shared;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Lingo_VerticalSliceTest.CardSetWord;

public class CheckQuizAnswerUnitTest
{
    private readonly Mock<CalculateNextStepService> _calculateNextStepService;
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Mock<ICheckQuizAnswerRepository> _checkQuizAnswerRepositoryMock;


    public CheckQuizAnswerUnitTest()
    {
        _calculateNextStepService = new Mock<CalculateNextStepService>();
        _userServiceMock = new Mock<IUserService>();
        _checkQuizAnswerRepositoryMock = new Mock<ICheckQuizAnswerRepository>();
    }

    [Fact]
    public async Task CheckUserAuthorization_UserIsNotAuthorized_ReturnsFalse()
    {
        //Arrange
        var handle = new CheckQuizAnswer.Handler(_checkQuizAnswerRepositoryMock.Object,_calculateNextStepService.Object ,_userServiceMock.Object );
        _userServiceMock.Setup(x=>x.GetUserId()).Returns((string?)null);
        
        //Act
        var result = await handle.Handle(new CheckQuizAnswer.Command(),CancellationToken.None);
        
        //Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("User.Error", result.Error.Code);
    }

    [Fact]
    public async Task GetSpaceRepetitionDetail_SpaceRepetitionNull_ReturnsNullError()
    {
        //Arrange
        var fixture = new Fixture();
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b=>fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        
        var command = new CheckQuizAnswer.Command
        {
            NextStepInfo = new List<NextQuizInfoRequest>
            {
                new NextQuizInfoRequest { Vocabulary = "test", IsCorrectAnswer = true }
            }
        };
        var word = fixture.Build<Word>().With(w=>w.Vocabulary,"test2");
        var spaceRepetition = fixture.Build<SpacedRepetition>().With(w=>w.Words,word).CreateMany().ToList();
        {

        };
        _userServiceMock.Setup(x => x.GetUserId()).Returns("userId");
        var handle = new CheckQuizAnswer.Handler(_checkQuizAnswerRepositoryMock.Object,_calculateNextStepService.Object ,_userServiceMock.Object );
        
        _checkQuizAnswerRepositoryMock.Setup(x=>x.GetSpaceRepetitionDetailAsync(It.IsAny<List<string>>(),It.IsAny<string>(),CancellationToken.None))
            .ReturnsAsync(spaceRepetition);
        
        //Act
        var result = await handle.Handle(command,CancellationToken.None);
        
        //Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Error.NotFound", result.Error.Code);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenVocabularyNotFound()
    {
        // Arrange
        var fixture = new Fixture();
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b=>fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        
        
        _userServiceMock.Setup(u => u.GetUserId()).Returns("user123");
        
        var word = fixture.Build<Word>().With(w=>w.Vocabulary,"test");
        
        var spacedRepetition = fixture.Build<SpacedRepetition>().With(w => w.Words,word).CreateMany().ToList();
        
        _checkQuizAnswerRepositoryMock.Setup(r => r.GetSpaceRepetitionDetailAsync(It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(spacedRepetition);
        
        var command = new CheckQuizAnswer.Command
        {
            NextStepInfo = new List<NextQuizInfoRequest>
            {
                new NextQuizInfoRequest { Vocabulary = "test", IsCorrectAnswer = true }
            }
        };
        SpacedRepetition singleSr = spacedRepetition.First();

        // var calculateNextStepService = fixture.Build<Result<CalculateNextStepResponse>>()
        //     .With(w=>w.IsSuccess,false).Create();
        var calculateNextStepService = fixture.Build<Result<CalculateNextStepResponse>>().With(e=>e.IsFailure,true).Create();
        

        _calculateNextStepService.Setup(x=>x.CalculateNextStep(singleSr,false))
            .Returns(calculateNextStepService);
        
        
        var handle = new CheckQuizAnswer.Handler(_checkQuizAnswerRepositoryMock.Object,_calculateNextStepService.Object ,_userServiceMock.Object );
        // Act
        var result = await handle.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Error.NotFound", result.Error.Code);
    }
}
