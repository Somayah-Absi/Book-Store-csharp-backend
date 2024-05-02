using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public DateTime? OrderDate { get; set; }

    public string OrderStatus { get; set; } = null!;

    public string? Payment { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual User? User { get; set; }
}
