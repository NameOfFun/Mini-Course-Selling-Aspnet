using Course_Selling_System.Dtos;
using Course_Selling_System.Models;
using Course_Selling_System.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace Course_Selling_System.Service.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly CourseSellingDbContext _context;
        public OrderService(CourseSellingDbContext context)
        {
            _context = context;
        }

        public async Task<(bool Ok, string? Error, OrderDto? Order)> CreateOrderAsync(int userId, CreateOrderRequest request, CancellationToken ct = default)
        {
            var ids = request.CourseIds.Distinct().ToList();
            if (ids.Count == 0)
            {
                return (false, "Giỏ hàng trống", null);
            }

            var courses = await _context.Courses.AsNoTracking()
                .Where(c => ids.Contains(c.Id) && c.IsPublished)
                .ToDictionaryAsync(c => c.Id, ct);

            if (courses.Count != ids.Count)
            {
                return (false, "Có khóa học không tồn tại hoặc chưa được xuất bản", null);
            }

            var enrroled = await _context.Enrollments.AsNoTracking()
                .Where(e => e.UserId == userId && ids.Contains(e.CourseId))
                .Select(e => e.CourseId)
                .ToListAsync(ct);

            if (enrroled.Count > 0)
            {
                return (false, $"Bạn đã đăng ký khóa học {string.Join(", ", enrroled)}", null);
            }

            await using var tx = await _context.Database.BeginTransactionAsync(ct);
            try
            {
                var order = new Order
                {
                    UserId = userId,
                    Status = "Pending",
                    TotalAmount = 0
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync(ct);

                decimal total = 0;
                foreach (var courseId in ids)
                {
                    var price = courses[courseId].Price;
                    total += price;
                    _context.OrderItems.Add(new OrderItem
                    {
                        OrderId = order.Id,
                        CourseId = courseId,
                        PriceAtPurchase = price
                    });
                }

                order.TotalAmount = total;
                await _context.SaveChangesAsync(ct);
                await tx.CommitAsync(ct);

                return (true, null, await MapOrderAsync(order.Id, ct));
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync(ct);
                return (false, "Đã có lỗi xảy ra khi tạo đơn hàng", null);
            }
        }

        private async Task<OrderDto> MapOrderAsync(int orderId, CancellationToken ct)
        {
            var order = await _context.Orders.AsNoTracking().FirstAsync(o => o.Id == orderId, ct);

            var items = await _context.OrderItems.AsNoTracking()
                .Where(i => i.OrderId == orderId)
                .Join(_context.Courses.AsNoTracking(),
                    i => i.CourseId,
                    c => c.Id,
                    (i, c) => new OrderItemDto
                    {
                        CourseId = i.CourseId,
                        CourseTitle = c.Title,
                        PriceAtPurchase = i.PriceAtPurchase
                    }).ToListAsync();

            return new OrderDto
            {
                Id = order.Id,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                CreatedAt = order.CreatedAt,
                Items = items
            };
        }
        public async Task<(bool Ok, string? Error, OrderDto? Order)> CompletePaymentAsync(int userId, int orderId, CancellationToken ct = default)
        {
            await using var tx = await _context.Database.BeginTransactionAsync(ct);
            try
            {
                var order = await _context.Orders.Include(o => o.OrderItems)
                    .FirstOrDefaultAsync(o => o.Id == orderId, ct);

                if(order is null)
                {
                    return (false, "Đơn hàng không tồn tại", null);
                }
                if(order.UserId != userId)
                {
                    return (false, "Bạn không có quyền thanh toán đơn hàng này", null);
                }
                if(order.Status == "Paid")
                {
                    await tx.CommitAsync(ct);
                    return (true, null, await MapOrderAsync(order.Id, ct));
                }
                if(order.Status != "Pending")
                {
                    return (false, "Đơn hàng không cho phép thanh toán", null);
                }

                order.Status = "Paid";

                foreach (var item in order.OrderItems)
                {
                    var exists = await _context.Enrollments.AnyAsync(
                        e => e.UserId == userId && e.CourseId == item.CourseId, ct);

                    if (!exists)
                    {
                        _context.Enrollments.Add(new Enrollment
                        {
                            UserId = userId,
                            CourseId = item.CourseId,
                            EnrolledAt = DateTime.UtcNow
                        });
                    }
                }

                await _context.SaveChangesAsync(ct);
                await tx.CommitAsync(ct);
                return (true, null, await MapOrderAsync(order.Id, ct));
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync(ct);
                throw;
            }
        }

        public async Task<IReadOnlyList<OrderDto>> GetMyOrderAsync(int userId, CancellationToken ct = default)
        {
            var ids = await _context.Orders.AsNoTracking()
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .Select(o => o.Id)
                .ToListAsync(ct);

            var list = new List<OrderDto>();
            foreach (var id in ids)
            {
                list.Add(await MapOrderAsync(id, ct));
            }
            return list;
        }
    }
}
