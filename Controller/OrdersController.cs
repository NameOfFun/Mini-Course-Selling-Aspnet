using Course_Selling_System.Dtos;
using Course_Selling_System.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Course_Selling_System.Controller;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orders;

    public OrdersController(IOrderService orders) => _orders = orders;

    private int UserId => int.Parse(
        User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)!);

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var (ok, err, order) = await _orders.CreateOrderAsync(UserId, request, ct);
        if (!ok)
            return BadRequest(new { message = err });

        return CreatedAtAction(nameof(GetById), new { id = order!.Id }, order);
    }

    [HttpPost("{id:int}/pay")]
    public async Task<IActionResult> Pay(int id, CancellationToken ct)
    {
        var (ok, err, order) = await _orders.CompletePaymentAsync(UserId, id, ct);
        if (!ok)
            return err?.Contains("Không tìm thấy") == true ? NotFound(new { message = err }) : BadRequest(new { message = err });

        return Ok(order);
    }

    [HttpGet]
    public async Task<IActionResult> Mine(CancellationToken ct)
    {
        var list = await _orders.GetMyOrderAsync(UserId, ct);
        return Ok(list);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var list = await _orders.GetMyOrderAsync(UserId, ct);
        var order = list.FirstOrDefault(o => o.Id == id);
        return order is null ? NotFound() : Ok(order);
    }
}