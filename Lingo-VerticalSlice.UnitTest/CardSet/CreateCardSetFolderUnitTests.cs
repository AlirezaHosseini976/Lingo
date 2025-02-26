using System.Security.Claims;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Lingo_VerticalSlice.Contracts.CardSet;
using Lingo_VerticalSlice.Contracts.Services;
using Lingo_VerticalSlice.Entities;
using Lingo_VerticalSlice.Features.CardSet.CreateCardSetFolder;
using Lingo_VerticalSlice.Shared;
using Microsoft.AspNetCore.Http;
using Moq;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace Lingo_VerticalSliceTest.CardSet;

public class CreateCardSetFolderUnitTests
{
    private readonly Mock<ICreateCardSetFolderRepository> _repositoryMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly Mock<IUserService> _mockUserServiceMock;
    private readonly Mock<IValidator<CreateCardSetFolder.Command>> _createCardSetFolderValidatorMock;
    private readonly CreateCardSetFolder.Handler _handler;

    public CreateCardSetFolderUnitTests()
    {
        // _createCardSetFolderValidatorMock =Mock.Of<IValidator<CreateCardSetFolder.Command>>();
        _createCardSetFolderValidatorMock = new Mock<IValidator<CreateCardSetFolder.Command>>();
        _repositoryMock = new Mock<ICreateCardSetFolderRepository>();
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _mockUserServiceMock = new Mock<IUserService>();
        _handler = new CreateCardSetFolder.Handler(_repositoryMock.Object,
            _createCardSetFolderValidatorMock.Object, _mockUserServiceMock.Object);
    }

    // [Theory, AutoData]
    // public async Task CreateCardSetFolder_AreValuesValid_ValuesAreNotValid()
    // {
    //     //Arrange
    //     var expectedResult = new ValidationResult()
    //     _createCardSetFolderValidatorMock
    //         .Setup(x => x.Validate(It.IsAny<CreateCardSetFolder.Command>()))
    //         .Returns()
    // }


    [Fact]
    public async Task CreateCardSetFolder_UserIsNotAuthorized_ReturnUserIsNotAuthenticatedError()
    {
        //Arrange
        // _httpContextAccessorMock
        //     .Setup(r => r.HttpContext.User)
        //     .Returns((ClaimsPrincipal?) null);
        _mockUserServiceMock.Setup(x => x.GetUserId()).Returns((string?)null);

        //Act
        var result = await _handler.Handle(new CreateCardSetFolder.Command(), CancellationToken.None);

        //Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("User.Error", result.Error.Code);
    }

    [Theory, AutoData]
    public async Task CreateCardSetFolder_IsFolderExist_FoldeShouldNotExists(CreateCardSetFolder.Command command)
    {
        //Arrange
        var userFixture = new Fixture();
        var userId = userFixture.Create<string>();
        _mockUserServiceMock.Setup(x => x.GetUserId()).Returns(userId);
        _createCardSetFolderValidatorMock.Setup(validator =>
                validator.ValidateAsync(It.IsAny<CreateCardSetFolder.Command>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _repositoryMock
            .Setup(r => r.IsFolderExistsAsync(command.FolderName, userId
                , CancellationToken.None))
            .ReturnsAsync(true);
        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Act
        Assert.False(result.IsSuccess);
        Assert.Equal("CardSetFolder.Error", result.Error.Code);
    }

    [Theory, AutoData]
    public async Task CreateCardSetFolder_TheCreateCardSetFolderMethod_CardSetFolderCreated(
        CreateCardSetFolder.Command command, CreateCardSetFolderResponse expectedResult)
    {
        //Arrange
        var userFixture = new Fixture();
        var userId = userFixture.Create<string>();
        _mockUserServiceMock.Setup(x => x.GetUserId()).Returns(userId);

        _repositoryMock
            .Setup(r => r.IsFolderExistsAsync(command.FolderName, userId
                , CancellationToken.None))
            .ReturnsAsync(false);
        _repositoryMock
            .Setup(r => r.CreateCardSetFolderAsync(It.IsAny<Folder>(), CancellationToken.None))
            .ReturnsAsync(expectedResult);

        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedResult, result.Value);
    }

    [Fact]
    public async Task Handle_ForInvalidInput_ReturnsError()
    {
        //Arrange
        var command = new CreateCardSetFolder.Command()
        {
            FolderName = "test-folder"
        };
        var response = new CreateCardSetFolderResponse()
        {
            FolderId = 1,
            CardSetId = 2
        };
        var userFixture = new Fixture();
        var userId = userFixture.Create<string>();

        _mockUserServiceMock.Setup(x => x.GetUserId()).Returns(userId);
        
        var a = new InlineValidator<CreateCardSetFolder.Command>();
        a.RuleFor(x => x.FolderName).Must(folderName => false);
        a.RuleFor(x => x.CardSetName).Must(cardSetName => false);
        var handler = new CreateCardSetFolder.Handler(_repositoryMock.Object,
            a, _mockUserServiceMock.Object);

        //Act
        var result = await handler.Handle(command);

        //Assert
        result.Should().BeOfType<Result<CreateCardSetFolderResponse>>();
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("CreateArticle.Validation");
    }

    [Fact]
    public async Task CreateCardSetFolder_TryCatch_UserAuthorization_ThrowsException()
    {
        //Arrange 
        var command = new CreateCardSetFolder.Command();
        _mockUserServiceMock
            .Setup(x=>x.GetUserId()).Returns((string?)null);

        //ACt & Assert
        await Assert.ThrowsAsync<Exception>(async () => { await _handler.Handle(command, CancellationToken.None); });
    }

    [Fact]
    public async Task CreateCardSetFolder_TryCatch_ValidationInputs_ThrowsException()
    {
        //Arrange
        var command = new CreateCardSetFolder.Command()
        {
            FolderName = "test-folder"
        };
        var userFixture = new Fixture();
        var userId = userFixture.Create<string>();
        var handler = new CreateCardSetFolder.Handler(_repositoryMock.Object,
            _createCardSetFolderValidatorMock.Object, _mockUserServiceMock.Object);

        _mockUserServiceMock.Setup(x => x.GetUserId()).Returns(userId);

        _createCardSetFolderValidatorMock.Setup(validator =>
                validator.ValidateAsync(It.IsAny<CreateCardSetFolder.Command>(),
                    It.IsAny<CancellationToken>()))
            .Throws<Exception>();
        //Act & Assert
        await Assert.ThrowsAsync<Exception>(async () => { await handler.Handle(command, CancellationToken.None); });
    }

    [Theory, AutoData]
    public async Task CreateCardSetFolder_TryCatch_IsFolderExists_ThrowsException(CreateCardSetFolder.Command command)
    {
        //Arrange
        var userFixture = new Fixture();
        var userId = userFixture.Create<string>();
        _mockUserServiceMock.Setup(x => x.GetUserId()).Returns(userId);

        _createCardSetFolderValidatorMock.Setup(validator =>
                validator.ValidateAsync(It.IsAny<CreateCardSetFolder.Command>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _repositoryMock
            .Setup(r => r.IsFolderExistsAsync(command.FolderName, userId
                , CancellationToken.None))
            .Throws<Exception>();

        //Act & Assert
        await Assert.ThrowsAsync<Exception>(async () => { await _handler.Handle(command, CancellationToken.None); });
    }

    [Theory, AutoData]
    public async Task CreateCardSetFolder_TryCatch_CreateCardSetFolderAsync_ThrowsException(
        CreateCardSetFolder.Command command)
    {
        var userFixture = new Fixture();
        var userId = userFixture.Create<string>();
        _mockUserServiceMock.Setup(x => x.GetUserId()).Returns(userId);

        _createCardSetFolderValidatorMock.Setup(validator =>
                validator.ValidateAsync(It.IsAny<CreateCardSetFolder.Command>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _repositoryMock
            .Setup(r => r.IsFolderExistsAsync(command.FolderName, userId
                , CancellationToken.None))
            .ReturnsAsync(false);
        _repositoryMock
            .Setup(r => r.CreateCardSetFolderAsync(It.IsAny<Folder>(), CancellationToken.None))
            .Throws<Exception>();
        await Assert.ThrowsAsync<Exception>(async () => { await _handler.Handle(command, CancellationToken.None); });
    }
}