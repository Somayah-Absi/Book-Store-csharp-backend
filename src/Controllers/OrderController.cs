using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Backend.Helpers;
using Backend.Dtos;

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
        // //IActionResult give all status code like :ok,not found.....
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

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder(int orderId)
        {
            try
            {

                var order = await _orderService.GetOrderByIdService(orderId);
                if (order != null)
                {
                    return ApiResponse.Success(order, "Order returned successfully");
                }
                else
                {
                    return ApiResponse.NotFound("order not found");
                }
            }
            catch (Exception ex)
            {
                return ApiResponse.ServerError(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreationDto orderCreationDto)
        {
            try
            {
                var createdOrder = await _orderService.CreateOrderService(orderCreationDto.NewOrder, orderCreationDto.Products);
                var test = CreatedAtAction(nameof(GetOrder), new { id = createdOrder.OrderId }, createdOrder);
                if (test == null)
                {
                    return NotFound("value is null");
                }
                return Ok("created successfully");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{OrderId}")]
        public async Task<IActionResult> UpdateOrder(int OrderId, Order order)
        {
            try
            {
                var updatedOrder = await _orderService.UpdateOrderService(OrderId, order);
                if (updatedOrder == null)
                {
                    return NotFound("Order was not found");

                }
                else
                {
                    return Ok("Update Order successfully");
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
            {
                // Check if the order with orderId exists
                var existingOrder = await _orderService.GetOrderByIdService(orderId);
                if (existingOrder == null)
                {
                    return NotFound("Order not found.");
                }
                var result = await _orderService.DeleteOrderService(orderId);
                if (!result)
                {
                    return ApiResponse.NotFound($"Failed to delete order with ID {orderId}.");
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

