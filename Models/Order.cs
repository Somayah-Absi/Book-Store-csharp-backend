using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public partial class Order
{
    [Key]
    [Required(ErrorMessage = "OrderId is required.")]
    public int OrderId { get; set; }

    public DateTime? OrderDate { get; set; }

    [Required(ErrorMessage = "OrderStatus is required.")]
    public string OrderStatus { get; set; } = null!;

    [Required(ErrorMessage = "Payment method is required.")]
    public string? Payment { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual User? User { get; set; }
}

