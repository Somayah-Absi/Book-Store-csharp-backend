using Backend.Controllers;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;


namespace api.Controllers
{
    [ApiController]
    [Route("/api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;
        // to initialize the services we will create constructor 
        public OrderController(OrderService orderService)
        {
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService)); ;
        }

        // help to make get request
        [HttpGet]
        //IActionResult give all status code like :ok,not found.....
        public async Task<IActionResult> GetAllOrder()
        {
            try
            {
                var orders = await _orderService.GetAllOrdersService();
                return ApiResponse.Success(orders, "All Orders returned successfully");
            }
            catch (Exception e)
            {
                return ApiResponse.BadRequest(e.Message);
            }
        }

        [HttpGet("{orderId:int}")]
        public async Task<IActionResult> GetOrder( int orderId)
        {
            try
            {
                // Check if the orderId is non-negative
                if (orderId <= 0)
                {
                    return ApiResponse.BadRequest("Invalid order ID.");
                }
                var order = await _orderService.GetOrderByIdService(orderId);
                if (order != null)
                {
                    return ApiResponse.Success(order, "Order returned successfully");
                }
                else
                {
                    return ApiResponse.NotFound();
                }
            }
            catch (Exception ex)
            {
                return ApiResponse.ServerError(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order newOrder)
        {
            try
            {
                // Await the order creation operation
                var createdOrder = await _orderService.CreateOrderService(newOrder);

                // Return a 201 Created status with the created order
                return CreatedAtAction(nameof(GetOrder), new { id = createdOrder.OrderId }, createdOrder);

            }
            catch (Exception ex)
            {
                Console.WriteLine("there is error with creating order");
                return ApiResponse.ServerError(ex.Message);
            }
        }

        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateOrder(int orderId, Order updateOrder)
        {
            try
            {
                // Validate orderId
                if (orderId <= 0)
                {
                    return ApiResponse.BadRequest("Invalid order ID.");
                }

                // Validate updateOrder object
                if (updateOrder == null)
                {
                    return ApiResponse.BadRequest("Update order data is null.");
                }

                // Check if the order with orderId exists
                var existingOrder = await _orderService.GetOrderByIdService(orderId);
                if (existingOrder == null)
                {
                    return ApiResponse.NotFound("Order not found.");
                }
                var updatedOrder = await _orderService.UpdateOrderService(orderId, updateOrder);
                if (updatedOrder == null)
                {
                    return ApiResponse.NotFound();
                }
                else
                {
                    return ApiResponse.Success(updatedOrder, "Order updated successfully");
                }
            }
            catch (Exception ex)
            {
                return ApiResponse.ServerError(ex.Message);
            }
        }

        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            try
            {  // Validate orderId
                if (orderId <= 0)
                {
                    return ApiResponse.BadRequest("Invalid order ID.");
                }

                // Check if the order with orderId exists
                var existingOrder = await _orderService.GetOrderByIdService(orderId);
                if (existingOrder == null)
                {
                    return NotFound("Order not found.");
                }
                var result = await _orderService.DeleteOrderService(orderId);
                if (!result)
                {
                    return ApiResponse.NotFound();
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return ApiResponse.ServerError(ex.Message);
            }
        }
    }
}

