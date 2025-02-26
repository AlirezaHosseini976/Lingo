using System.Security.Claims;
using Lingo_VerticalSlice.Contracts.Services;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Lingo_VerticalSliceTest.Contracts.Services;

public class UserServiceUnitTest
{
    private readonly Mock<IUserService> _userServiceInterFace;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly UserService _userService;

    public UserServiceUnitTest()
    {
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _userServiceInterFace = new Mock<IUserService>();
    }

    [Fact]
    public void UserService_FailToCatchUser_ThrowsException()
    {
        //Arrange
        var httpContext = new DefaultHttpContext { User = null };
        _httpContextAccessorMock.Setup(x=>x.HttpContext).Returns(httpContext);
        var userService = new UserService(_httpContextAccessorMock.Object);
        
        //Act
        var result = userService.GetUserId();
        
        //Assert
        Assert.Null(result);
    }
}