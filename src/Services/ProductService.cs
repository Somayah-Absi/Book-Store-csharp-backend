using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ProductService
{
    public static List<Product> Products = new List<Product>()
    {
        new Product
        {
            ProductId = Guid.Parse("abcdeabc-deab-cdea-bcad-abcdefabcdef"),
            ProductName = "Sample Product",
            ProductSlug = "sample-product",
            ProductDescription = "This is a sample product",
            ProductPrice = 99.99m,
            ProductImage = "sample.jpg",
            ProductQuantityInStock = 10,
            CategoryId = Guid.NewGuid(),
            CreatedAt = DateTime.Now
        },
        // Add more sample products here if needed
    };

    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        await Task.CompletedTask;
        return Products.AsEnumerable();
    }

    public async Task<Product?> GetProductById(Guid productId)
    {
        await Task.CompletedTask;
        return Products.Find(product => product.ProductId == productId);
    }

    public async Task<Product?> CreateProduct(Product newProduct)
    {
        await Task.CompletedTask;
        newProduct.ProductId = Guid.NewGuid();
        newProduct.CreatedAt = DateTime.Now;
        Products.Add(newProduct);
        return newProduct;
    }

    public async Task<Product?> UpdateProduct(Guid productId, Product updateProduct)
    {
        await Task.CompletedTask;
        var existingProduct = Products.FirstOrDefault(product => product.ProductId == productId);
        if (existingProduct != null)
        {
            existingProduct.ProductName = updateProduct.ProductName ?? existingProduct.ProductName;
            existingProduct.ProductSlug = updateProduct.ProductSlug ?? existingProduct.ProductSlug;
            existingProduct.ProductDescription = updateProduct.ProductDescription ?? existingProduct.ProductDescription;
            existingProduct.ProductPrice = updateProduct.ProductPrice;
            existingProduct.ProductImage = updateProduct.ProductImage ?? existingProduct.ProductImage;
            existingProduct.ProductQuantityInStock = updateProduct.ProductQuantityInStock;
            existingProduct.CategoryId = updateProduct.CategoryId;
        }
        return existingProduct;
    }

    public async Task<bool> DeleteProduct(Guid productId)
    {
        await Task.CompletedTask;
        var productToRemove = Products.FirstOrDefault(product => product.ProductId == productId);
        if (productToRemove != null)
        {
            Products.Remove(productToRemove);
            return true;
        }
        return false;
    }
}