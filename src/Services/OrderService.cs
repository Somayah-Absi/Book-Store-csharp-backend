using Microsoft.AspNetCore.Http.HttpResults;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class OrderService
    {

        private EcommerceSdaContext _dbContext;
        public OrderService(EcommerceSdaContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersService()
        {
            try
            {
                // Retrieve orders asynchronously
                var orderEntities = await _dbContext.Orders.ToListAsync();

                // Map Order entities to Order domain objects
                var orders = orderEntities.Select(row => new Order
                {
                    OrderId = row.OrderId,
                    OrderDate = row.OrderDate,
                    OrderStatus = row.OrderStatus,
                    UserId = row.UserId,
                    Payment = row.Payment,
                });

                return orders;
            }
            catch (IOException ex)
            {
                // Handle database-specific exceptions
                throw new ApplicationException("An error occurred while retrieving orders from the database.", ex);
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                throw new ApplicationException("An error occurred while creating Order.", ex);
            }
        }

        public async Task<Order> CreateOrderService(Order newOrder)
        {
            try
            {
                // Add the new order asynchronously
                await _dbContext.Orders.AddAsync(newOrder);

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
            await Task.CompletedTask;
            try
            {
                return _dbContext.Orders.Find(orderId);
            }
            catch (Exception ex)
            {
                // Handle exception or log error
                throw new ApplicationException($"An error occurred while retrieving order with ID {orderId}.", ex);
            }
        }

        public async Task<Order?> UpdateOrderService(int orderId, Order updateOrder)
        {
            await Task.CompletedTask;
            //simulate an asynchronous operation without delay
            try
            {
                var exitingOrder = _dbContext.Orders.FirstOrDefault(order => order.OrderId == orderId);
                if (exitingOrder != null)
                {
                    exitingOrder.OrderStatus = updateOrder.OrderStatus ?? exitingOrder.OrderStatus;
                    exitingOrder.Payment = updateOrder.Payment ?? exitingOrder.Payment;
                    _dbContext.Orders.Update(exitingOrder);
                    _dbContext.SaveChanges();
                    return exitingOrder;
                }
                else
                {
                    return null;
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



