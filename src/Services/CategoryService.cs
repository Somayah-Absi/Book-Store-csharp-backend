using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Backend.Dtos;
using Backend.Helpers;

namespace Backend.Services
{
    public class CategoryService
    {
        private readonly EcommerceSdaContext _dbContext;

        public CategoryService(EcommerceSdaContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        // Converts fetched database data (categories and their products) into DTOs by selecting/mapping specific properties and creating new objects.
        public async Task<IEnumerable<GetCategoryWithProductDto>> GetAllCategories()
        {
            try
            {
                var categories = await _dbContext.Categories.Include(c => c.Products).ToListAsync();
                var categoryDtos = categories.Select(c => new GetCategoryWithProductDto
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    CategorySlug = c.CategorySlug,
                    CategoryDescription = c.CategoryDescription,

                    Products = c.Products.Select(p => new GetProductWithCategoryDto
                    {
                        ProductId = p.ProductId,
                        ProductName = p.ProductName,
                        ProductSlug = p.ProductSlug,
                        ProductPrice = p.ProductPrice,
                        ProductDescription = p.ProductDescription,
                        ProductImage = p.ProductImage,
                        ProductQuantityInStock = p.ProductQuantityInStock
                    }).ToList()
                });

                return categoryDtos;

            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving categories.", ex);
            }
        }
        public async Task<Category?> GetCategoryById(int id)
        {
            try
            {
                return await _dbContext.Categories.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while retrieving category with ID {id}.", ex);
            }
        }

        public async Task<Category> CreateCategory(Category category)
        {
            try
            {
                category.CategoryId = await IdGenerator.GenerateIdAsync<Category>(_dbContext);
                _dbContext.Categories.Add(category);
                await _dbContext.SaveChangesAsync();
                return category;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while creating category.", ex);
            }
        }

        public async Task<Category?> UpdateCategory(int id, UpdateCategoryDto categoryDto)
        {
            try
            {
                var existingCategory = await _dbContext.Categories.FindAsync(id);

                if (existingCategory != null)
                {
                    existingCategory.CategoryName = categoryDto.CategoryName;
                    existingCategory.CategorySlug = SlugGenerator.GenerateSlug(categoryDto.CategoryName);
                    existingCategory.CategoryDescription = categoryDto.CategoryDescription;
                    await _dbContext.SaveChangesAsync();
                    return existingCategory;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while updating category with ID {id}.", ex);
            }
        }

        public async Task<bool> DeleteCategory(int id)
        {
            try
            {
                var categoryToDelete = _dbContext.Categories.Find(id);
                if (categoryToDelete != null)
                {
                    _dbContext.Categories.Remove(categoryToDelete);
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
                throw new ApplicationException($"An error occurred while deleting category with ID {id}.", ex);
            }
        }
    }
}
