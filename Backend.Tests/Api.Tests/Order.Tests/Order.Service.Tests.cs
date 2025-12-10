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
using Backend.Api.Services.Order;

namespace Api.Tests; 

public class OrderServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;

    private readonly Mock<ICreateOrderCommandHandler> _createOrderHandlerMock;

    private readonly Mock<IEditOrderCommandHandler> _editOrderHandlerMock;

    private readonly Mock<IDeleteOrderCommandHandler> _deleteOrderHandlerMock;

    private readonly IOrderService _orderService;

    public OrderServiceTests() {

        _userRepositoryMock = new Mock<IUserRepository>();

        _createOrderHandlerMock = new Mock<ICreateOrderCommandHandler>();

        _editOrderHandlerMock = new Mock<IEditOrderCommandHandler>();

        _deleteOrderHandlerMock = new Mock<IDeleteOrderCommandHandler>();

        _orderService = new OrderService (_createOrderHandlerMock.Object, _editOrderHandlerMock.Object, _deleteOrderHandlerMock.Object);

    }

    [Fact]

    public async Task CreateOrderService_ReturnsCreateOrderResponse_WhenValid()
    {
        //Arrange
        var fakeUser= new User(new UserName("test"), PasswordHash.FromHash("hashed"), RoleRights.CreateBaseUser());
        var fakeUserId= fakeUser.Id;

        var fakeTargetUser= new User(new UserName("test"), PasswordHash.FromHash("hashed"), RoleRights.CreateAdmin());
        var fakeTargetUserId= fakeTargetUser.Id;

        var fakeRequestDto = new CreateOrderDto
        {
            Name = "Test",
            Date= DateTime.UtcNow,
            Price= 100
        };

        var fakeOrderId= Guid.NewGuid();
        var fakeResponse= new CreateOrderResponse(fakeOrderId, fakeRequestDto.Name!, fakeRequestDto.Price, fakeRequestDto.Date);


        _userRepositoryMock
        .Setup(repo => repo.GetByIdAsync(fakeTargetUserId))
        .ReturnsAsync(fakeTargetUser);

        _userRepositoryMock
        .Setup(repo => repo.GetByIdAsync(fakeUserId))
        .ReturnsAsync(fakeUser);

        _createOrderHandlerMock
        .Setup(h => h.Handle(It.IsAny<CreateOrderCommand>(), fakeUserId, fakeTargetUserId))
        .ReturnsAsync(fakeResponse);


        //Act
        var result= await _orderService.CreateOrderAsync(fakeRequestDto, fakeUserId, fakeTargetUserId);


        //Assert
        result.OrderId.Should().Be(fakeResponse.OrderId);
        result.Name.Should().Be(fakeResponse.Name);
        result.Price.Should().Be(fakeResponse.Price);
        result.Date.Should().Be(fakeResponse.Date);
    
    }

     [Fact]

    public async Task CreateOrderService_Throws_WhenNameIsNull()
    {
        //Arrange
        var fakeUser= new User(new UserName("test"), PasswordHash.FromHash("hashed"), RoleRights.CreateBaseUser());
        var fakeUserId= fakeUser.Id;

        var fakeTargetUser= new User(new UserName("test"), PasswordHash.FromHash("hashed"), RoleRights.CreateAdmin());
        var fakeTargetUserId= fakeTargetUser.Id;

        var fakeRequestDto = new CreateOrderDto
        {
            Name = null,
            Date= DateTime.UtcNow,
            Price= 100
        };


        //Act
        Func<Task> act= async() => await _orderService.CreateOrderAsync(fakeRequestDto, fakeUserId, fakeTargetUserId);

        //Assert
        await act.Should().ThrowAsync<Exception>()
        .WithMessage("Name cannot be empty");



    }

    [Fact]

    public async Task EditOrderService_ReturnsEditOrderResponse_WhenValid()
    {
        //Arrange
        var fakeUser= new User(new UserName("test"), PasswordHash.FromHash("hashed"), RoleRights.CreateBaseUser());
        var fakeUserId= fakeUser.Id;

        var fakeTargetUser= new User(new UserName("test"), PasswordHash.FromHash("hashed"), RoleRights.CreateAdmin());
        var fakeTargetUserId= fakeTargetUser.Id;


        var fakeOrderId= Guid.NewGuid();

        var fakeRequestDto = new EditOrderDto
        {
            Id = fakeOrderId,
            Name = "Test",
            Date= DateTime.UtcNow,
            Price= 100
        };

        var fakeResponse= new EditOrderResponse(fakeOrderId, fakeRequestDto.Name!, fakeRequestDto.Price, fakeRequestDto.Date);


        _userRepositoryMock
        .Setup(repo => repo.GetByOrderIdAsync(fakeOrderId))
        .ReturnsAsync(fakeTargetUser);

        _userRepositoryMock
        .Setup(repo => repo.GetByIdAsync(fakeUserId))
        .ReturnsAsync(fakeUser);

        _editOrderHandlerMock
        .Setup(h => h.Handle(It.IsAny<EditOrderCommand>(), fakeUserId, fakeTargetUserId))
        .ReturnsAsync(fakeResponse);


        //Act
        var result= await _orderService.EditOrderAsync(fakeRequestDto, fakeUserId, fakeTargetUserId);


        //Assert
        result.OrderId.Should().Be(fakeResponse.OrderId);
        result.Name.Should().Be(fakeResponse.Name);
        result.Price.Should().Be(fakeResponse.Price);
        result.Date.Should().Be(fakeResponse.Date);
    
    }

    [Fact]

    public async Task DeleteOrderService_ReturnsEditOrderResponse_WhenValid()
    {
        //Arrange
        var fakeUser= new User(new UserName("test"), PasswordHash.FromHash("hashed"), RoleRights.CreateBaseUser());
        var fakeUserId= fakeUser.Id;

        var fakeTargetUser= new User(new UserName("test"), PasswordHash.FromHash("hashed"), RoleRights.CreateAdmin());
        var fakeTargetUserId= fakeTargetUser.Id;


        var fakeOrderId= Guid.NewGuid();

        var fakeResponse= new DeleteOrderResponse("Order deleted", true);


        _userRepositoryMock
        .Setup(repo => repo.GetByOrderIdAsync(fakeOrderId))
        .ReturnsAsync(fakeTargetUser);

        _userRepositoryMock
        .Setup(repo => repo.GetByIdAsync(fakeUserId))
        .ReturnsAsync(fakeUser);

        _deleteOrderHandlerMock
        .Setup(h => h.Handle(It.IsAny<DeleteOrderCommand>(), fakeUserId))
        .ReturnsAsync(fakeResponse);


        //Act
        var result= await _orderService.DeleteOrderAsync(fakeOrderId, fakeUserId);


        //Assert
        result.Message.Should().Be("Order deleted");
        result.Success.Should().BeTrue();
    
    }

}