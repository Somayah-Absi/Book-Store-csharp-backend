using Microsoft.AspNetCore.Http.HttpResults;
using Backend.Models;

public class OrderService
{

    private EcommerceSdaContext _dbContext;

    public OrderService(EcommerceSdaContext appDbContext)
    {
        _dbContext = appDbContext;
    }



    public IEnumerable<Order>  GetAllOrdersService()
    {

        List<Order> orders = new List<Order>();
        var OrderList =  _dbContext.Orders.ToList();
        OrderList.ForEach(row => orders.Add(new Order
        {

            OrderId = row.OrderId,
            OrderDate = row.OrderDate,
            OrderStatus = row.OrderStatus,
            UserId = row.UserId,
            Payment = row.Payment,




        })); return orders;



    }




    public Order CreateOrderService(Order newOrder)
    {
          
        try
        {
            _dbContext.Orders.Add(newOrder);


            _dbContext.SaveChanges();
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
    public bool  DeleteOrderService(int orderId)
    {
       
        try
        {
            var DeleteOrder = _dbContext.Orders.FirstOrDefault(order => order.OrderId == orderId);
            if (DeleteOrder != null)
            {
                _dbContext.Orders.Remove(DeleteOrder);
                _dbContext.SaveChanges();
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

