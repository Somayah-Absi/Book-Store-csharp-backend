using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/order-products")]
    public class OrderProductController : ControllerBase
    {
        private readonly OrderProductService _orderProductService;

        public OrderProductController(OrderProductService orderProductService)
        {
            _orderProductService = orderProductService ?? throw new ArgumentNullException(nameof(orderProductService));
        }


        [HttpGet]
        public async Task<IActionResult> GetAllOrderProducts()
        {
            try
            {
                var orderProducts = await _orderProductService.GetAllOrderProductAsync();
                return Ok(orderProducts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderProductById(int orderId)
        {
            try
            {
                var orderProduct = await _orderProductService.GetOrderProductByIdAsync(orderId);
                if (orderProduct != null)
                {
                    var totalOrderPrice = 0.0m;
                    foreach (int order in orderProduct.Select(op => op.OrderId).Distinct())
                    {
                        totalOrderPrice = await _orderProductService.GetTotalOrderPriceAsync(order);
                    }
                    var response = new
                    {
                        orderProduct,
                        totalOrderPrice
                    };
                    return Ok(response);
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
        public async Task<IActionResult> CreateProduct(OrderProduct product)
        {
            try
            {
                if (product == null)
                {
                    return BadRequest("Product data is null");
                }

                var createdOrderProduct = await _orderProductService.AddOrUpdateProductQuantity(product);
                return CreatedAtAction(nameof(GetOrderProductById), new { orderId = createdOrderProduct.ProductId }, createdOrderProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{OrderId}")]
        public async Task<IActionResult> UpdateProduct(int OrderId, int ProductId, int numberQuantity)
        {
            try
            {
                // if (orderProduct == null || orderProduct.ProductId != OrderProductId)
                // {
                //     return BadRequest("Invalid Order product data");
                // }

                var updatedOrderProduct = await _orderProductService.UpdateProductQuantity(OrderId,ProductId,numberQuantity);
                if (updatedOrderProduct == null)
                {
                    return NotFound();
                }

                return Ok(updatedOrderProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{OrderProductId}")]
        public async Task<IActionResult> DeleteProduct(int OrderProductId)
        {
            try
            {
                var result = await _orderProductService.DeleteOrderProduct(OrderProductId);
                if (result)
                {
                    return NotFound();
                }
                else
                {
                    return Ok("Deleted Successful");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}