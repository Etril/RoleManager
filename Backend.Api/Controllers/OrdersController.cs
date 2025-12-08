

using Backend.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    [HttpPost("orders")]
    public async Task<IActionResult> CreateOrder(CreateOrderDto dto, Guid userId)
    {
        var result = await _orderService.CreateOrderAsync(dto, userId);
        return Ok(result);
    }

    [Authorize]
    [HttpPut("orders")]

    public async Task<IActionResult> EditOrder(EditOrderDto dto, Guid userId)
    {
        var result = await _orderService.EditOrderAsync(dto, userId);
        return Ok(result);
    }

    [Authorize]
    [HttpDelete("order/{id}")]

    public async Task<IActionResult> DeleteOrderAsync (Guid orderId, Guid userId)
    {
        var result = await _orderService.DeleteOrderAsync(orderId, userId);
        return Ok(result);
    }
}