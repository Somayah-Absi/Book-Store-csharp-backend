using AutoMapper;
using Backend.Dtos;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class OrderService
    {

        private EcommerceSdaContext _dbContext;
        private readonly IMapper _mapper;

        public OrderService(EcommerceSdaContext appDbContext, IMapper mapper)
        {
            _dbContext = appDbContext;
            _mapper = mapper;
        }
        public async Task<IEnumerable<OrderDto>> GetAllOrdersService()
        {
            try
            {
                var orderEntities = await _dbContext.Orders
                    .Include(o => o.User) // Include the User information
                    .ToListAsync();

                // Use AutoMapper to map Order entities to OrderDto
                var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(orderEntities);

                return orderDtos;
            }
            catch (IOException ex)
            {
                throw new ApplicationException("An error occurred while retrieving orders from the database.", ex);
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
            //simulate an asynchronous operation without delay
            try
            {
                var exitingOrder = await _dbContext.Orders.FirstOrDefaultAsync(order => order.OrderId == orderId);
                if (exitingOrder != null)
                {
                    exitingOrder.OrderStatus = updateOrder.OrderStatus ?? exitingOrder.OrderStatus;
                    exitingOrder.Payment = updateOrder.Payment ?? exitingOrder.Payment;
                    await _dbContext.SaveChangesAsync();
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



