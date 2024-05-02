using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string CategorySlug { get; set; } = null!;

    public string? CategoryDescription { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
