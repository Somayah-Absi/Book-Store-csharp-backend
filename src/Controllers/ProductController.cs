using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using api.Helpers;

namespace api.Controllers
{
    [ApiController]
    [Route("/api/products")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _productService.GetAllProducts();

                if (products.Count() == 0)
                {
                    return NotFound(new ErrorResponse
                    {
                        Success = false,
                        Message = "No products found"
                    });
                }

                return Ok(new SuccessResponse<IEnumerable<Product>>
                {
                    Success = true,
                    Message = "All products retrieved successfully",
                    Data = products
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while fetching products: {ex.Message}");
                return StatusCode(500, new ErrorResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductById(Guid productId)
        {
            try
            {
                var product = await _productService.GetProductById(productId);

                if (product == null)
                {
                    return NotFound(new ErrorResponse
                    {
                        Success = false,
                        Message = "Product not found"
                    });
                }

                return Ok(new SuccessResponse<Product>
                {
                    Success = true,
                    Message = "Product retrieved successfully",
                    Data = product
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while fetching product: {ex.Message}");
                return StatusCode(500, new ErrorResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
        
        [HttpPost]
public async Task<IActionResult> CreateProduct(Product newProduct)
{
    try
    {
        var createdProduct = await _productService.CreateProduct(newProduct);

        if (createdProduct == null)
        {
            return StatusCode(500, new ErrorResponse
            {
                Success = false,
                Message = "Failed to create product"
            });
        }

        return CreatedAtAction(nameof(GetProductById), new { productId = createdProduct.ProductId }, new SuccessResponse<Product>
        {
            Success = true,
            Message = "Product created successfully",
            Data = createdProduct
        });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error occurred while creating product: {ex.Message}");
        return StatusCode(500, new ErrorResponse
        {
            Success = false,
            Message = ex.Message
        });
    }
}

        [HttpPut("{productId}")]
        public async Task<IActionResult> UpdateProduct(Guid productId, Product updateProduct)
        {
            try
            {
                var updatedProduct = await _productService.UpdateProduct(productId, updateProduct);

                if (updatedProduct == null)
                {
                    return NotFound(new ErrorResponse
                    {
                        Success = false,
                        Message = "Product not found"
                    });
                }

                return Ok(new SuccessResponse<Product>
                {
                    Success = true,
                    Message = "Product updated successfully",
                    Data = updatedProduct
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while updating product: {ex.Message}");
                return StatusCode(500, new ErrorResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct(Guid productId)
        {
            try
            {
                var result = await _productService.DeleteProduct(productId);

                if (!result)
                {
                    return NotFound(new ErrorResponse
                    {
                        Success = false,
                        Message = "Product not found"
                    });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while deleting product: {ex.Message}");
                return StatusCode(500, new ErrorResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
    }
}