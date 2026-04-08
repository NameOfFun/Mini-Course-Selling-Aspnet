using System;
using System.Collections.Generic;

namespace Course_Selling_System.Models;

public partial class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int CourseId { get; set; }

    public decimal PriceAtPurchase { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
