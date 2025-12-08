using Backend.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]

public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController (IOrderService orderService)
    {
        _orderService = orderService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
    {
        var userId = GetUserIdFromToken();
        var result = await _orderService.CreateOrderAsync(dto, userId);
        return Ok(result);
    }

    [Authorize]
    [HttpPut("{id}")]

    public async Task<IActionResult> EditOrder(EditOrderDto dto, Guid id)
    {
        var userId = GetUserIdFromToken();
        var result = await _orderService.EditOrderAsync(dto, id, userId);
        return Ok(result);
    }

    [Authorize]
    [HttpDelete("{id}")]

    public async Task<IActionResult> DeleteOrderAsync (Guid id)
    {
        var userId = GetUserIdFromToken();
        var result = await _orderService.DeleteOrderAsync(id, userId);
        return Ok(result);
    }

      private Guid GetUserIdFromToken()
    {
        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdStr == null )
        throw new UnauthorizedAccessException("No UserId");
        
        return Guid.Parse(userIdStr) ;
    }
}