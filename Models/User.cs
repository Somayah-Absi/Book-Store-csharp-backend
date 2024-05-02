using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class User
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Mobile { get; set; }

    public string Password { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public bool? IsAdmin { get; set; }

    public bool? IsBanned { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
