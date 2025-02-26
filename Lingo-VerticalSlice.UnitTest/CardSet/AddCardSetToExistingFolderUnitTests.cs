using AutoFixture;
using AutoFixture.Xunit2;
using FluentValidation;
using Lingo_VerticalSlice.Features.CardSet.AddCardSetToExistingFolder;
using Moq;

namespace Lingo_VerticalSliceTest.CardSet;

public class AddCardSetToExistingFolderUnitTests
{
    public AddCardSetToExistingFolderUnitTests()
    {
    }
    
    [Fact]
    public async Task AddCardSetToExistingFolder_CardSetExists_ReturnsCardSetExistsError()
    {
        
        // Arrange
        var fixture = new Fixture();
        var command = fixture.Create<AddCardSetToExistingFolder.Command>();
        
        var validatorMock = new Mock<IValidator<AddCardSetToExistingFolder.Command>>();
        var repositoryMock = new Mock<IAddCardSetToExistingFolderRepository>();
        repositoryMock
            .Setup(r => r.IsCardSetExistsAsync(command.FolderId, command.CardSetName, CancellationToken.None))
            .ReturnsAsync(true);
        var handler = new AddCardSetToExistingFolder.Handler(validatorMock.Object, repositoryMock.Object);
        
        //Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("CardSetExists", result.Error.Code);
        
    }
    
    [Theory, AutoData]
    public async Task AddCardSetToExistingFolder_CardSetInFolderCountExceeded_ReturnsCardSetLimitIsReachedError(AddCardSetToExistingFolder.Command command)
    {
        // Arrange
        var validatorMock = new Mock<IValidator<AddCardSetToExistingFolder.Command>>();
        var repositoryMock = new Mock<IAddCardSetToExistingFolderRepository>();
        repositoryMock
            .Setup(r => r.IsCardSetExistsAsync(command.FolderId, command.CardSetName, CancellationToken.None))
            .ReturnsAsync(false);

        repositoryMock
            .Setup(r => r.CardSetInFolderCountAsync(command.FolderId, CancellationToken.None))
            .ReturnsAsync(21);
        
        var handler = new AddCardSetToExistingFolder.Handler(validatorMock.Object, repositoryMock.Object);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("CardSetLimitIsReached", result.Error.Code);
        
    }

    [Theory, AutoData]
    public async Task AddCardSetToExistingFolder_CardSetGotCreated_ReturnsCardSetCreatedApproval(
        AddCardSetToExistingFolder.Command command)
    {
        //Arange
        var createdCardSetId = 1;
        
        var validatorMock = new Mock<IValidator<AddCardSetToExistingFolder.Command>>();
        var repositoryMock = new Mock<IAddCardSetToExistingFolderRepository>();
        repositoryMock
            .Setup(r => r.IsCardSetExistsAsync(command.FolderId, command.CardSetName, CancellationToken.None))
            .ReturnsAsync(false);

        repositoryMock
            .Setup(r => r.CardSetInFolderCountAsync(command.FolderId, CancellationToken.None))
            .ReturnsAsync(19);
        repositoryMock
            .Setup(r => r.AddCardSetToExistingFolderAsync(It.IsAny<Lingo_VerticalSlice.Entities.CardSet>(), CancellationToken.None))
            .ReturnsAsync(createdCardSetId);
        var handler = new AddCardSetToExistingFolder.Handler(validatorMock.Object, repositoryMock.Object);
        
        //Act 
        var result = await handler.Handle(command, CancellationToken.None);
        
        //Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(createdCardSetId, result.Value.Id);
        Assert.Equal("CardSet has been build!", result.Value.Message);
    }
}