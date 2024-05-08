using Backend.Models;

namespace Backend.Dtos
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public string Payment { get; set; }
        public int? UserId { get; set; }
        public UserDto User { get; set; } // Include UserDTO to represent the associated User

        public ICollection<OrderProduct> OrderProducts { get; set; } // Include OrderProduct collection
    }
    public class OrderCreateDto
    {
        public int OrderId { get; set; }
        public string OrderStatus { get; set; }
        public string Payment { get; set; }


    }

}