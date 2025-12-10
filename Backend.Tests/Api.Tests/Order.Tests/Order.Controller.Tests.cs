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
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Api.Tests;

public class OrderControllerTests {

    private readonly Mock<IOrderService> _orderServiceMock;

    private readonly OrderController _orderController; 

    public OrderControllerTests ()
{
    _orderServiceMock = new Mock<IOrderService>();

    _orderController = new OrderController(_orderServiceMock.Object);
}
    
    
    [Fact]
    
    public async Task Controller_CreateOrder_ReturnsOk_WhenValid()
    {
        //Arrange
        var userId= Guid.NewGuid();
        var targetUserId= Guid.NewGuid();
        var fakeOrderId= Guid.NewGuid();

        var claimsIdentity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        }, "TestAuth");

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        _orderController.ControllerContext= new ControllerContext
        {
            HttpContext= new DefaultHttpContext
            {
                User= claimsPrincipal
            }
        };

        var fakeRequestDto= new CreateOrderDto
        {
            Name= "Test",

            Date= DateTime.UtcNow,

            Price= 100
        };

        var fakeResponseDto = new CreateOrderResponse(fakeOrderId, fakeRequestDto.Name!, fakeRequestDto.Price, fakeRequestDto.Date);


        _orderServiceMock
        .Setup(s => s.CreateOrderAsync(fakeRequestDto, userId, targetUserId))
        .ReturnsAsync(fakeResponseDto);



        //Act
        
        var result = await _orderController.CreateOrder(fakeRequestDto, targetUserId);


        //Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();

        var returnedDto = okResult!.Value as CreateOrderResponse;
        returnedDto!.Name.Should().Be(fakeResponseDto.Name);



    }

    [Fact]
    
    public async Task Controller_CreateOrder_Throw_WhenNameIsNull()
    {
        //Arrange
        var userId= Guid.NewGuid();
        var targetUserId= Guid.NewGuid();
        var fakeOrderId= Guid.NewGuid();

        var claimsIdentity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        }, "TestAuth");

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        _orderController.ControllerContext= new ControllerContext
        {
            HttpContext= new DefaultHttpContext
            {
                User= claimsPrincipal
            }
        };

        var fakeRequestDto= new CreateOrderDto
        {
            Name= null,

            Date= DateTime.UtcNow,

            Price= 100
        };

        var fakeResponseDto = new CreateOrderResponse(fakeOrderId, fakeRequestDto.Name!, fakeRequestDto.Price, fakeRequestDto.Date);


        _orderServiceMock
        .Setup(s => s.CreateOrderAsync(fakeRequestDto, userId, targetUserId))
        .ThrowsAsync(new Exception("Name cannot be empty"));



        //Act
        
        Func<Task> act= async () => await _orderController.CreateOrder(fakeRequestDto, targetUserId);


        //Assert
        await act.Should().ThrowAsync<Exception>()
        .WithMessage("Name cannot be empty");
       
    }

    [Fact]

    public async Task Controller_EditOrder_ReturnsOk_WhenValid()
    {
        //Arrange
        var userId= Guid.NewGuid();
        var targetUserId= Guid.NewGuid();
        var fakeOrderId= Guid.NewGuid();

        var claimsIdentity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        }, "TestAuth");

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        _orderController.ControllerContext= new ControllerContext
        {
            HttpContext= new DefaultHttpContext
            {
                User= claimsPrincipal
            }
        };

        var fakeRequestDto= new EditOrderDto
        {
            Id= fakeOrderId,

            Name= "Test",

            Date= DateTime.UtcNow,

            Price= 100
        };

        var fakeResponseDto = new EditOrderResponse(fakeOrderId, fakeRequestDto.Name!, fakeRequestDto.Price, fakeRequestDto.Date);


        _orderServiceMock
        .Setup(s => s.EditOrderAsync(fakeRequestDto, fakeOrderId, userId))
        .ReturnsAsync(fakeResponseDto);



        //Act
        
        var result = await _orderController.EditOrder(fakeRequestDto, fakeOrderId);


        //Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();

        var returnedDto = okResult!.Value as EditOrderResponse;
        returnedDto!.Name.Should().Be(fakeResponseDto.Name);
        returnedDto!.OrderId.Should().Be(fakeResponseDto.OrderId);

    }

    [Fact]
    public async Task Controller_DeleteOder_ReturnsOk_WhenValid()
    {
        
    {
        //Arrange
        var userId= Guid.NewGuid();
        var targetUserId= Guid.NewGuid();
        var fakeOrderId= Guid.NewGuid();

        var claimsIdentity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        }, "TestAuth");

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        _orderController.ControllerContext= new ControllerContext
        {
            HttpContext= new DefaultHttpContext
            {
                User= claimsPrincipal
            }
        };

        var fakeResponseDto = new DeleteOrderResponse("Order deleted", true);


        _orderServiceMock
        .Setup(s => s.DeleteOrderAsync(fakeOrderId, userId))
        .ReturnsAsync(fakeResponseDto);



        //Act
        
        var result = await _orderController.DeleteOrder(fakeOrderId);


        //Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();

        var returnedDto = okResult!.Value as DeleteOrderResponse;
        returnedDto!.Message.Should().Be("Order deleted");
        returnedDto!.Success.Should().Be(true);
    }
    }

}