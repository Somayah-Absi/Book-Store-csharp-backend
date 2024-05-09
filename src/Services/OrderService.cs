using System.Text.Json;
using Backend.Dtos;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class OrderService
    {

        private readonly EcommerceSdaContext _dbContext;
        // private readonly IMapper _mapper;

        public OrderService(EcommerceSdaContext appDbContext)
        {
            _dbContext = appDbContext;
            // _mapper = mapper;
        }
        public async Task<IEnumerable<OrderDto>> GetAllOrdersService()
        {
            try
            {
                var orderEntities = await _dbContext.Orders.Include(o => o.User)
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .ToListAsync();

                var orderDtos = orderEntities.Select(o => new OrderDto
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate,
                    OrderStatus = o.OrderStatus,
                    Payment = o.Payment.ValueKind == JsonValueKind.String ? o.Payment.GetString() : null,
                    UserId = o.UserId,
                    User = new UserDto // Assuming UserDto is your DTO for User
                    {
                        UserId = o.User.UserId,
                        FirstName = o.User.FirstName,
                        LastName = o.User.LastName,
                        Email = o.User.Email,
                        Mobile = o.User.Mobile,


                    },
                    OrderProducts = o.OrderProducts.Select(op => new OrderProductDto
                    {
                        OrderProductId = op.OrderProductId,
                        Quantity = op.Quantity,
                        ProductId = op.ProductId,
                        Product = new ProductDto // Assuming UserDto is your DTO for User
                        {
                            ProductId = op.Product.ProductId,
                            ProductName = op.Product.ProductName,
                            ProductSlug = op.Product.ProductSlug,
                            ProductDescription = op.Product.ProductDescription,
                            ProductPrice = op.Product.ProductPrice,
                            ProductImage = op.Product.ProductImage,
                            ProductQuantityInStock = op.Product.ProductQuantityInStock,
                            CreatedAt = op.Product.CreatedAt,
                            CategoryId = op.Product.CategoryId



                        }

                    }).ToList()
                });
                return orderDtos;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while creating Order.", ex);
            }
        }

        public async Task<Order> CreateOrderService(Order newOrder)
        {
            try
            {

                // Add the new order asynchronously
                _dbContext.Orders.Add(newOrder);

                // Save changes asynchronously
                await _dbContext.SaveChangesAsync();

                // Return the newly created order
                return newOrder;

            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while creating Order.", ex);
            }
        }

        public async Task<Order?> GetOrderByIdService(int orderId)
        {

            try
            {
                return await _dbContext.Orders.FindAsync(orderId);
            }
            catch (Exception ex)
            {
                // Handle exception or log error
                throw new ApplicationException($"An error occurred while retrieving order with ID {orderId}.", ex);
            }
        }

        public async Task<Order?> UpdateOrderService(int orderId, Order updateOrder)
        {
            try
            {
                // Retrieve the existing order from the database
                var existingOrder = await _dbContext.Orders.FindAsync(orderId);

                if (existingOrder != null)
                {
                    existingOrder.OrderStatus = updateOrder.OrderStatus;
                    existingOrder.Payment = updateOrder.Payment; // Directly assign JsonElement value

                    // Save changes to the database
                    await _dbContext.SaveChangesAsync();

                    return existingOrder;
                }
                else
                {
                    return null; // Order with the given ID does not exist
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while updating Order with ID {orderId}.", ex);
            }
        }

        public async Task<bool> DeleteOrderService(int orderId)
        {
            try
            {
                var deleteOrder = await _dbContext.Orders.FirstOrDefaultAsync(order => order.OrderId == orderId);

                if (deleteOrder != null)
                {
                    _dbContext.Orders.Remove(deleteOrder);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while deleting order with ID {orderId}.", ex);
            }
        }
    }
}



