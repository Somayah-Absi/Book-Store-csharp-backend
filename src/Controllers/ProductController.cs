using System.IdentityModel.Tokens.Jwt; // Importing namespace for JWT token handling
using Backend.Dtos;
using Backend.Helpers; // Importing helpers namespace for utility functions
using Backend.Middlewares;
using Backend.Models; // Importing models namespace for Product model
using Backend.Services; // Importing services namespace for ProductService
using Microsoft.AspNetCore.Mvc; // Importing ASP.NET Core MVC namespace

namespace Backend.Controllers // Defining namespace for controller
{
    [ApiController] // Attribute to mark the class as an API controller
    [Route("api/products")] // Attribute to define the base route for API endpoints
    public class ProductController : ControllerBase // Controller class inheriting from ControllerBase
    {
        private readonly ProductService _productService; // ProductService instance for handling product-related operations

        public ProductController(ProductService productService) // Constructor for injecting ProductService dependency
        {
            _productService =
                productService ?? throw new ArgumentNullException(nameof(productService)); // Null check for productService
        }

        // Action method for getting all products
        [HttpGet] // HTTP GET method attribute
        public async Task<IActionResult> GetAllProducts(
            [FromQuery] string? productName = null, // Optional query parameter for filtering by product name
            [FromQuery] decimal? minPrice = null, // Optional query parameter for filtering by minimum price
            [FromQuery] decimal? maxPrice = null, // Optional query parameter for filtering by maximum price
            [FromQuery] DateTime? createDate = null, // Optional query parameter for filtering by creation date
            [FromQuery] int pageNumber = 1, // Optional query parameter for pagination - page number
            [FromQuery] int pageSize = 10, // Optional query parameter for pagination - page size
            [FromQuery] string sortBy = "id", // Optional query parameter for sorting
            [FromQuery] bool ascending = true
        ) // Optional query parameter for sorting order
        {
            try
            {
                var sortOptions = new List<string>
                {
                    "id",
                    "price",
                    "title",
                    "product name",
                    "create date"
                }; // Valid sort options

                // Check if the sortBy value is valid, if not, return bad request
                if (!sortOptions.Contains(sortBy.ToLower()))
                {
                    throw new BadRequestException("Invalid value for sortBy. Valid options are: id, price, productName, createDate (Make sure to the lower and capital letters)");
                }

                var products = await _productService.GetAllProductsAsync(
                    productName,
                    minPrice,
                    maxPrice,
                    createDate,
                    sortBy,
                    ascending,
                    pageNumber,
                    pageSize
                ); // Get products from service
                return ApiResponse.Success(products); // Return success response with products
            }
            catch (Exception ex) // Catching any exceptions
            {
                throw new InternalServerException(ex.Message); // Return server error with exception message
            }
        }

        // Action method for getting a product by ID
        [HttpGet("{ProductId}")] // HTTP GET method attribute
        public async Task<IActionResult> GetProduct(int ProductId) // Action method to retrieve a product by ID
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(ProductId); // Get product by ID from service
                if (product != null) // Check if product exists
                {
                    return ApiResponse.Success(product); // Return success response with product
                }
                else
                {
                    throw new NotFoundException("Product was not found"); // Return not found response if product does not exist
                }
            }
            catch (Exception ex) // Catching any exceptions
            {
                throw new InternalServerException(ex.Message); // Return server error with exception message
            }
        }

        // Action method for creating a new product
        [HttpPost] // HTTP POST method attribute
        public async Task<IActionResult> CreateProduct(CreateProductDto productDto) // Action method for creating a new product
        {
            try
            {
                if (!Request.Cookies.ContainsKey("jwt")) // Check if JWT token exists in request cookies
                {
                    throw new UnauthorizedAccessExceptions("You are not logged in ❗");// Return unauthorized response if token is missing
                }
                else
                {
                    var jwt = Request.Cookies["jwt"]; // Get JWT token from request cookies
                    // Validate and decode JWT token to extract claims
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.ReadJwtToken(jwt);

                    var isAdminClaim = token.Claims.FirstOrDefault(c =>
                        c.Type == "role" && c.Value == "Admin"
                    ); // Check if user is admin

                    bool isAdmin = isAdminClaim != null; // Boolean flag for admin role

                    if (isAdmin) // Check if user is admin
                    {
                        //"Admin access granted"
                        if (productDto == null) // Check if product data is null
                        {
                            throw new BadRequestException("Product data is null"); // Return bad request response
                        }

                        var product = new Product
                        {
                            ProductName = productDto.ProductName,
                            ProductDescription = productDto.ProductDescription,
                            ProductPrice = productDto.ProductPrice,
                            ProductImage = productDto.ProductImage,
                            ProductQuantityInStock = productDto.ProductQuantityInStock,
                            CategoryId = productDto.CategoryId,
                            // Other properties as needed
                        };

                        product.ProductSlug = SlugGenerator.GenerateSlug(product.ProductName); // Generate slug for product
                        var createdProduct = await _productService.CreateProductAsync(product); // Create product using service
                        var test = CreatedAtAction(
                            nameof(GetProduct),
                            new { productId = createdProduct.ProductId },
                            createdProduct
                        ) ?? throw new NotFoundException("Failed to create a product"); // Check if product creation was successful
                        return ApiResponse.Created(productDto, "Product created successfully"); // Return success response with created product
                    }
                    else
                    {
                        throw new UnauthorizedAccessExceptions("You don't have permission to access this endpoint"); // Return unauthorized response if user is not admin
                    }
                }
            }
            catch (Exception ex) // Catching any exceptions
            {
                throw new InternalServerException(ex.Message); // Return server error with exception message
            }
        }

        // Action method for updating a product
        [HttpPut("{ProductId}")] // HTTP PUT method attribute
        public async Task<IActionResult> UpdateProduct(int ProductId, Product product) // Action method for updating a product
        {
            try
            {
                if (!Request.Cookies.ContainsKey("jwt")) // Check if JWT token exists in request cookies
                {
                    throw new UnauthorizedAccessExceptions("You are not logged in ❗"); // Return unauthorized response if token is missing
                }
                else
                {
                    var jwt = Request.Cookies["jwt"]; // Get JWT token from request cookies
                    // Validate and decode JWT token to extract claims
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.ReadJwtToken(jwt);

                    var isAdminClaim = token.Claims.FirstOrDefault(c =>
                        c.Type == "role" && c.Value == "Admin"
                    ); // Check if user is admin

                    bool isAdmin = isAdminClaim != null; // Boolean flag for admin role

                    if (isAdmin) // Check if user is admin
                    {
                        //"Admin access granted"
                        if (product == null || product.ProductId != ProductId) // Check if product data is null or product ID is not matching
                        {
                            throw new BadRequestException("Invalid product data"); // Return bad request response
                        }

                        product.ProductSlug = SlugGenerator.GenerateSlug(product.ProductName); // Generate slug for product
                        var updatedProduct = await _productService.UpdateProductAsync(
                            ProductId,
                            product
                        ) ?? throw new NotFoundException("Product was not found"); // Update product using service

                        return ApiResponse.Success(updatedProduct, "Update product successfully"); // Return success response with updated product
                    }
                    else
                    {
                        throw new UnauthorizedAccessExceptions("You don't have permission to access this endpoint"); // Return unauthorized response if user is not admin
                    }
                }
            }
            catch (Exception ex) // Catching any exceptions
            {
                throw new InternalServerException(ex.Message); // Return server error with exception message
            }
        }

        // Action method for deleting a product
        [HttpDelete("{ProductId}")] // HTTP DELETE method attribute
        public async Task<IActionResult> DeleteProduct(int ProductId) // Action method for deleting a product
        {
            try
            {
                if (!Request.Cookies.ContainsKey("jwt")) // Check if JWT token exists in request cookies
                {
                    throw new UnauthorizedAccessExceptions("You are not logged in ❗"); // Return unauthorized response if token is missing
                }
                else
                {
                    var jwt = Request.Cookies["jwt"]; // Get JWT token from request cookies
                    // Validate and decode JWT token to extract claims
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.ReadJwtToken(jwt);

                    var isAdminClaim = token.Claims.FirstOrDefault(c =>
                        c.Type == "role" && c.Value == "Admin"
                    ); // Check if user is admin

                    bool isAdmin = isAdminClaim != null; // Boolean flag for admin role

                    if (isAdmin) // Check if user is admin
                    {
                        // Check if the product with productId exists
                        var existingProduct = await _productService.GetProductByIdAsync(ProductId) ?? throw new NotFoundException("Product was not found.");

                        //"Admin access granted"
                        var result = await _productService.DeleteProductAsync(ProductId); // Delete product using service
                        if (result) // Check if product was deleted successfully
                        {
                            return ApiResponse.Deleted(existingProduct, $"Product with ID {ProductId} successfully deleted."); // Return no content response
                        }
                        else
                        {
                            throw new NotFoundException($"Product with ID {ProductId} was not found"); // Return not found response if product was not found
                        }
                    }
                    else
                    {
                        throw new UnauthorizedAccessExceptions("You don't have permission to access this endpoint"); // Return unauthorized response if user is not admin
                    }
                }
            }
            catch (Exception ex) // Catching any exceptions
            {
                throw new InternalServerException(ex.Message); // Return server error with exception message
            }
        }

        // Action method for searching products by keyword
        [HttpGet("search")] // HTTP GET method attribute
        public async Task<IActionResult> SearchProducts(string keyword) // Action method for searching products
        {
            try
            {
                var products = await _productService.SearchProductsAsync(keyword); // Search products using service
                return ApiResponse.Success(products, "Products are returned successfully"); // Return success response with search results
            }
            catch (Exception ex) // Catching any exceptions
            {
                throw new InternalServerException(ex.Message); // Return server error with exception message
            }
        }
    }
}
