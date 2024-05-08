using System.Text.Json;
using AutoMapper;
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
                    OrderProducts = o.OrderProducts.Select(op => new OrderProduct
                    {
                        OrderProductId = op.OrderProductId,
                        Quantity=op.Quantity,
                        ProductId=op.ProductId

                }).ToList()
                });
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
                    // Validate the updateOrder parameter (e.g., ensure non-null values, perform business logic checks)

                    // Update order properties based on the updateOrder parameter
                    existingOrder.OrderStatus = updateOrder.OrderStatus;
                    existingOrder.Payment = updateOrder.Payment; // Directly assign JsonElement value

                    // Save changes to the database within a transaction
                    using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            await _dbContext.SaveChangesAsync();
                            await transaction.CommitAsync();
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            throw new ApplicationException($"Failed to update order with ID {orderId}. Transaction rolled back.", ex);
                        }
                    }

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



