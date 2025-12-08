using System.Runtime.CompilerServices;
using Backend.Api.DTOs;
using Backend.Domain.ValueObjects;
using Backend.Api.Services;
using Backend.Application;
using FluentAssertions;
using Microsoft.VisualBasic;
using Moq;
using Backend.Application.Repositories;
using Backend.Application.Commands;
using Backend.Domain.Entities;
using Backend.Domain.Roles;
using FluentAssertions.Common;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Backend.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity.Data;
using Backend.Api.DTOs.AuthDTOs;
using Microsoft.AspNetCore.Mvc;
using Backend.Api.Services.Auth;
using Microsoft.Extensions.Configuration;

namespace Api.Tests; 

public class AuthTests
{


    private readonly Mock<IAuthService> _authServiceMock;

    private readonly Mock<IUserRepository> _userRepositoryMock;

    private readonly IAuthService _authService;

    private readonly AuthController _controller;

    private readonly Mock<IConfiguration> _configMock;


    public AuthTests()
    {
        _authServiceMock = new Mock<IAuthService>();

        _controller = new AuthController(_authServiceMock.Object);

        _userRepositoryMock = new Mock<IUserRepository>();

        _configMock= new Mock<IConfiguration>();

        _authService = new AuthService(_userRepositoryMock.Object, _configMock.Object);

        


    }

    [Fact]

    public async Task ControllerLogin_ReturnsOkResult_WhenValid()
    {
        //Arrange
        var targetUser= new User( new UserName("Test"), PasswordHash.FromHash("hashed"), RoleRights.CreateBaseUser());
        var fakeRequestDto= new LoginRequestDto
        {
            Username= "Test",
            Password= "hashed"
        };

        var fakeResponseDto = new LoginResponseDto
        {
            Success= true,
            Token = "TestToken" 
        };

        _authServiceMock
        .Setup(auth => auth.LoginAsync(fakeRequestDto))
        .ReturnsAsync(fakeResponseDto);
    

        //Act
        var result= await _controller.Login(fakeRequestDto);

        //Assert
        var okResult= result as OkObjectResult;
        okResult.Should().NotBeNull();

        var returnedDto = okResult!.Value as LoginResponseDto;
        returnedDto!.Token.Should().Be("TestToken");
        returnedDto.Success.Should().BeTrue();
    
    }

    [Fact]

    public async Task AuthServiceLoginAsync_ReturnsToken_WhenValid()
    {
        //Arrange 
        var testUser = new User (new UserName("Test"), PasswordHash.FromPlainText("hashed"), RoleRights.CreateBaseUser());
        var fakeRequestDto= new LoginRequestDto
        {
            Username= "Test",
            Password= "hashed"
        };

        _configMock.Setup(c => c["Jwt:Key"]).Returns("Jqks5d4jl3qs22sqdkqsdm2545wxcq1234sqd");
        _configMock.Setup(c => c["Jwt:Issuer"]).Returns("RoleManager");
        _configMock.Setup(c => c["Jwt:Audience"]).Returns("Users");
        _configMock.Setup(c => c["Jwt:ExpireMinutes"]).Returns("60");

        _userRepositoryMock
        .Setup(repo => repo.GetByUsernameAsync(It.Is<UserName>(u => u.Value == "Test")))
        .ReturnsAsync(testUser);
        

        //Act

        var result= await _authService.LoginAsync(fakeRequestDto);

        //Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Token.Should().NotBeNull();

    }

}

