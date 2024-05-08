using System.Text.Json.Serialization;
using Backend.Models;

namespace Backend.Dtos
{
        public class OrderDto
        {
                public int OrderId { get; set; }
                public DateTime? OrderDate { get; set; }
                public string OrderStatus { get; set; } = null!;
                public string? Payment { get; set; } = null!;
                public int? UserId { get; set; }
              
                public UserDto User { get; set; } = null!; // Include UserDTO to represent the associated User
            
                public ICollection<OrderProduct> OrderProducts { get; set; } = null!; // Include OrderProduct collection
        }

        public class OrderCreateDto
        {
                public int OrderId { get; set; }
                public string OrderStatus { get; set; } = null!;
                public string Payment { get; set; } = null!;


        }
}