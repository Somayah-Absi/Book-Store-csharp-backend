using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;


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
                return ApiResponse
                .Success(
                    orderProducts,
                    "OrderProduct retrieved successfully"
                       );
            }
            catch (Exception ex)
            {
                return ApiResponse.ServerError(ex.Message);
            }
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderProductById(int orderId)
        {
            try
            {
                var orderProduct = await _orderProductService.GetOrderProductByIdAsync(orderId);
                if (orderProduct.Count() != 0)
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

                    return ApiResponse
                   .Success(
                       response,
                       $"Order product By ID: {orderId} retrieved successfully"
                          );
                }
                else
                {
                    return ApiResponse.NotFound("Order not found");
                }
            }
            catch (Exception ex)
            {
                return ApiResponse.ServerError(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(OrderProduct product)
        {
            try
            {
                if (product == null)
                {
                    return ApiResponse.BadRequest("Order Product data is null");
                }

                var createdOrderProduct = await _orderProductService.AddOrUpdateProductQuantity(product);
                return CreatedAtAction(nameof(GetOrderProductById), new { orderId = createdOrderProduct.ProductId },
                 ApiResponse
                 .Created(
                    createdOrderProduct,
                    "OrderProduct created successfully"
                    ));
            }
            catch (Exception ex)
            {
                return ApiResponse.ServerError(ex.Message);
            }
        }

        [HttpPut("{OrderId}")]
        public async Task<IActionResult> UpdateProduct(int OrderId, int ProductId, int numberQuantity)
        {
            try
            {
                var updatedOrderProduct = await _orderProductService.UpdateProductQuantity(OrderId, ProductId, numberQuantity);                
                if (updatedOrderProduct.Count() == 0)
                {
                    return ApiResponse.NotFound("Order not found");
                }
                return ApiResponse
                       .Success(
                         updatedOrderProduct,
                         "OrderProduct updated successfully"
                             );
            }
            catch (Exception ex)
            {
                return ApiResponse.ServerError(ex.Message);
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
                      return ApiResponse
                             .Success(
                               "OrderProduct Deleted successfully"
                                  );
                }
                else
                {
                     return ApiResponse.NotFound("Order Product not found");
                }
            }
            catch (Exception ex)
            {
                return ApiResponse.ServerError(ex.Message);
            }
        }
    }
}