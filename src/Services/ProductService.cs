using Backend.Dtos;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Backend.Helpers;

namespace Backend.Services
{
    public class ProductService
    {
        private readonly EcommerceSdaContext _dbContext;

        public ProductService(EcommerceSdaContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<PaginationResult<Product>> GetAllProductsAsync(
            string? productName = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            DateTime? createDate = null,
            string sortBy = "id",
            bool ascending = true,
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var query = _dbContext.Products.AsQueryable();

                // Apply filters
                if (!string.IsNullOrEmpty(productName))
                {
                    var productNameLower = productName.ToLower();
                    query = query.Where(p => p.ProductName.ToLower().Contains(productNameLower));
                }

                if (minPrice.HasValue)
                    query = query.Where(p => p.ProductPrice >= minPrice);

                if (maxPrice.HasValue)
                    query = query.Where(p => p.ProductPrice <= maxPrice);

                if (createDate.HasValue)
                    query = query.Where(p => p.CreatedAt >= createDate);

                // Apply sorting
                query = sortBy.ToLower() switch
                {
                    "price" => ascending ? query.OrderBy(p => p.ProductPrice) : query.OrderByDescending(p => p.ProductPrice),
                    "product name" => ascending ? query.OrderBy(p => p.ProductName) : query.OrderByDescending(p => p.ProductName),
                    "create date" => ascending ? query.OrderBy(p => p.CreatedAt) : query.OrderByDescending(p => p.CreatedAt),
                    _ => ascending ? query.OrderBy(p => p.ProductId) : query.OrderByDescending(p => p.ProductId),
                };

                // Count total items before pagination
                var totalCount = await query.CountAsync();

                // Apply pagination
                query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

                var items = await query.ToListAsync();
                return new PaginationResult<Product>
                {
                    Items = items,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                // Handle exception or log error
                throw new ApplicationException("An error occurred while retrieving products.", ex);
            }
        }

        public async Task<Product?> GetProductByIdAsync(int ProductId)
        {
            try
            {
                return await _dbContext.Products.FindAsync(ProductId);
            }
            catch (Exception ex)
            {
                // Handle exception or log error
                throw new ApplicationException($"An error occurred while retrieving product with ID {ProductId}.", ex);
            }
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            try
            {
                product.ProductId = await IdGenerator.GenerateIdAsync<Product>(_dbContext);
                _dbContext.Products.Add(product);
                await _dbContext.SaveChangesAsync();
                return product;
            }
            catch (Exception ex)
            {
                // Handle exception or log error
                throw new ApplicationException("An error occurred while creating product.", ex);
            }
        }

        public async Task<Product?> UpdateProductAsync(int ProductId, Product product)
        {
            try
            {
                var existingProduct = await _dbContext.Products.FindAsync(ProductId);
                if (existingProduct != null)
                {
                    existingProduct.ProductName = product.ProductName;
                    existingProduct.ProductSlug = SlugGenerator.GenerateSlug(product.ProductName);
                    existingProduct.ProductDescription = product.ProductDescription;
                    existingProduct.ProductPrice = product.ProductPrice;
                    existingProduct.ProductImage = product.ProductImage;
                    existingProduct.ProductQuantityInStock = product.ProductQuantityInStock;
                    existingProduct.CreatedAt = product.CreatedAt;
                    existingProduct.CategoryId = product.CategoryId;

                    await _dbContext.SaveChangesAsync();
                    return existingProduct;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Handle exception or log error
                throw new ApplicationException($"An error occurred while updating product with ID {ProductId}.", ex);
            }
        }

        public async Task<bool> DeleteProductAsync(int ProductId)
        {
            try
            {
                var productToDelete = await _dbContext.Products.FindAsync(ProductId);
                if (productToDelete != null)
                {
                    _dbContext.Products.Remove(productToDelete);
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
                // Handle exception or log error
                throw new ApplicationException($"An error occurred while deleting product with ID {ProductId}.", ex);
            }
        }
        public async Task<IEnumerable<Product>> SearchProductsAsync(string keyword)
        {
            try
            {
                if (string.IsNullOrEmpty(keyword))
                {
                    // If no keyword provided, return all products
                    return await _dbContext.Products.ToListAsync();
                }
                else
                {
                    // Search products based on keyword in ProductName or ProductDescription
                    return await _dbContext.Products
                        .Where(p => p.ProductName.Contains(keyword) || p.ProductDescription.Contains(keyword))
                        .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                // Handle exception or log error
                throw new ApplicationException("An error occurred while searching products.", ex);
            }
        }
    }
}
