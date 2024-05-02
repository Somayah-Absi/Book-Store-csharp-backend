using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));;
        }

        // help to make get request
        [HttpGet]

        //IActionResult give all status code like :ok,not found.....
       
        public IActionResult GetAllOrder()
        {
            try
            {
                var orders = _orderService.GetAllOrdersService();
                return Ok(orders);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }


       [HttpGet("{id}")]
        public IActionResult GetOrder(int orderId)
        {
            try
            {
                var order = _orderService.GetOrderByIdService(orderId);
                if (order != null)
                {
                    return Ok(order);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost]

        public IActionResult CreateOrder(Order newOrder)
        {
            try
            {
                _orderService.CreateOrderService(newOrder);

                return Ok("order created successfully");

            }
            catch (Exception ex)
            {
                Console.WriteLine("there is error with creating order");
                return StatusCode(500, new ErrorResponse
                {
                    Success = false,
                    Message = ex.Message
                });

            }


        }




        [HttpPut("{orderId}")]

        public IActionResult UpdateOrder(int orderId, Order updateOrder)
        {
            try
            {
                var updatedOrder = _orderService.UpdateOrderService(orderId, updateOrder);
                if (updatedOrder == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(updatedOrder);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

       

            [HttpDelete("{orderId}")]

            public IActionResult DeleteOrder(int orderId)
            {
            try
            {
                var result = _orderService.DeleteOrderService(orderId);
                if (!result)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            }


    }
}

