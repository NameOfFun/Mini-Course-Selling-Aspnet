using Course_Selling_System.Dtos;

namespace Course_Selling_System.Service.Interface
{
    public interface IOrderService
    {
        Task<(bool Ok, string? Error, OrderDto? Order)> CreateOrderAsync(int userId, CreateOrderRequest request, CancellationToken ct = default);
        Task<(bool Ok, string? Error, OrderDto? Order)> CompletePaymentAsync(int userId, int orderId, CancellationToken ct = default);
        Task<IReadOnlyList<OrderDto>> GetMyOrderAsync(int userId, CancellationToken ct =default);
    }
}
