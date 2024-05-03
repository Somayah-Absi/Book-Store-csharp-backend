using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services
{
    public class ProductService
    {
        private readonly EcommerceSdaContext _dbContext;

        public ProductService(EcommerceSdaContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync(int pageNumber, int pageSize)
        {
            try
            {
                return await _dbContext.Products
                             .Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize)
                             .ToListAsync();
            }
            catch (Exception ex)
            {
                // Handle exception or log error
                throw new ApplicationException("An error occurred while retrieving products.", ex);
            }
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            try
            {
                return await _dbContext.Products.FindAsync(id);
            }
            catch (Exception ex)
            {
                // Handle exception or log error
                throw new ApplicationException($"An error occurred while retrieving product with ID {id}.", ex);
            }
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            try
            {
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

        public async Task<Product?> UpdateProductAsync(int id, Product product)
        {
            try
            {
                var existingProduct = await _dbContext.Products.FindAsync(id);
                if (existingProduct != null)
                {
                    existingProduct.ProductName = product.ProductName;
                    existingProduct.ProductSlug = product.ProductSlug;
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
                throw new ApplicationException($"An error occurred while updating product with ID {id}.", ex);
            }
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            try
            {
                var productToDelete = await _dbContext.Products.FindAsync(id);
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
                throw new ApplicationException($"An error occurred while deleting product with ID {id}.", ex);
            }
        }
    }
}
