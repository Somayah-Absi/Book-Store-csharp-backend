using System.Text.Json;
using Backend.Dtos;
using Backend.Helpers;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class OrderService
    {

        private readonly EcommerceSdaContext _dbContext;

        // Constructor for OrderService class

        public OrderService(EcommerceSdaContext appDbContext)
        {
            _dbContext = appDbContext;

        }
        // Method to get all orders asynchronously
        public async Task<IEnumerable<OrderDto>> GetAllOrdersService()
        {
            try
            {
                // Retrieve order entities from the database including related entities(user/orderProduct/product)

                var orderEntities = await _dbContext.Orders.Include(o => o.User)
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .ToListAsync();
                // Map order entities to order DTOs
                var orderDtos = orderEntities.Select(o => new OrderDto
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate,
                    OrderStatus = o.OrderStatus,
                    Payment = o.Payment.ValueKind == JsonValueKind.String ? o.Payment.GetString() : null,
                    UserId = o.UserId,
                    User = new UserDto
                    {
                        UserId = o.User.UserId,
                        FirstName = o.User.FirstName,
                        LastName = o.User.LastName,
                        Email = o.User.Email,
                        Mobile = o.User.Mobile,
                        CreatedAt = o.User.CreatedAt
                    },
                    OrderProducts = o.OrderProducts.Select(op => new OrderProductDto
                    {

                        Quantity = op.Quantity,

                        Product = new ProductDto
                        {
                            ProductId = op.Product.ProductId,
                            ProductName = op.Product.ProductName,
                            ProductPrice = op.Product.ProductPrice,
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
        // Method to create a new order asynchronously
        public async Task<Order> CreateOrderService(Order newOrder, List<OrderedProductDto> products)
        {
            try
            {
                // Generate ID for the new order
                // newOrder.OrderId = await IdGenerator.GenerateIdAsync<Order>(_dbContext);

                // Map ProductDto objects to OrderProduct entities
                var orderProducts = products.Select(p => new OrderProduct
                {
                    Order = newOrder,
                    ProductId = p.ProductId,
                    Quantity = p.Quantity
                });

                // Add orderProducts to newOrder
                newOrder.OrderProducts = orderProducts.ToList();

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

        // Method to get an order by ID asynchronously

        public async Task<Order?> GetOrderByIdService(int orderId)
        {

            try
            {
                // Retrieve order by ID from the database
                return await _dbContext.Orders.FindAsync(orderId);
            }
            catch (Exception ex)
            {
                // Handle exception or log error
                throw new ApplicationException($"An error occurred while retrieving order with ID {orderId}.", ex);
            }
        }
        // Method to update an existing order asynchronously

        public async Task<Order?> UpdateOrderService(int orderId, Order updateOrder)
        {
            try
            {
                //1- Retrieve the existing order from the database
                var existingOrder = await _dbContext.Orders.FindAsync(orderId);
                //2- Update properties of the existing order
                if (existingOrder != null)
                {
                    existingOrder.OrderStatus = updateOrder.OrderStatus;
                    existingOrder.Payment = updateOrder.Payment; //3- Directly assign JsonElement value

                    // 4-Save changes to the database
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
        // Method to delete an order asynchronously

        public async Task<bool> DeleteOrderService(int orderId)
        {
            try
            {
                // Find order to delete from the database
                var deleteOrder = await _dbContext.Orders.FirstOrDefaultAsync(order => order.OrderId == orderId);

                // Delete the order if found
                if (deleteOrder != null)
                {
                    // Delete associated order products
                    var orderProducts = _dbContext.OrderProducts.Where(op => op.OrderId == orderId);
                    _dbContext.OrderProducts.RemoveRange(orderProducts);

                    _dbContext.Orders.Remove(deleteOrder);
                    await _dbContext.SaveChangesAsync();
                    return true;// Deletion successful
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



