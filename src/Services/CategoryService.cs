using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Backend.Dtos;
using Backend.Helpers;

namespace Backend.Services
{
    // Service class for managing categories and related operations.
    public class CategoryService
    {
        private readonly EcommerceSdaContext _dbContext;

        // Constructor for initializing the CategoryService with the provided database context.
        public CategoryService(EcommerceSdaContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        // Converts fetched database data (categories and their products) into DTOs by selecting/mapping specific properties and creating new objects.
        public async Task<PaginationResult<GetCategoryWithProductDto>> GetAllCategories(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                // Calculate the total count of categories before pagination
                var totalCount = await _dbContext.Categories.CountAsync();

                // Apply pagination to the categories query
                var categories = await _dbContext.Categories
                    .Include(c => c.Products)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                // Map categories to DTOs
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
                }).ToList();

                // Calculate the total number of pages
                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                // Return paginated result
                return new PaginationResult<GetCategoryWithProductDto>
                {
                    Items = categoryDtos,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving categories.", ex);
            }
        }

        public async Task<Category?> GetCategoryById(int categoryId)
        {
            try
            {
                return await _dbContext.Categories.FindAsync(categoryId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while retrieving category with ID {categoryId}.", ex);
            }
        }

        public async Task<Category> CreateCategory(Category category)
        {
            try
            {
                _dbContext.Categories.Add(category);
                await _dbContext.SaveChangesAsync();
                return category;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while creating category.", ex);
            }
        }

        public async Task<Category?> UpdateCategory(int categoryId, UpdateCategoryDto categoryDto)
        {
            try
            {
                var existingCategory = await _dbContext.Categories.FindAsync(categoryId);

                if (existingCategory != null)
                {
                    // Update category properties with values from the DTO.
                    existingCategory.CategoryName = categoryDto.CategoryName;
                    existingCategory.CategorySlug = SlugGenerator.GenerateSlug(categoryDto.CategoryName);
                    existingCategory.CategoryDescription = categoryDto.CategoryDescription;

                    // Save changes to the database.
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
                throw new ApplicationException($"An error occurred while updating category with ID {categoryId}.", ex);
            }
        }

        public async Task<bool> DeleteCategory(int categoryId)
        {
            try
            {
                // Retrieve the category entity with the specified ID from the database.
                var categoryToDelete = await _dbContext.Categories.FindAsync(categoryId);

                if (categoryToDelete != null)
                {
                    // Remove the category from the database and save changes.
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
                throw new ApplicationException($"An error occurred while deleting category with ID {categoryId}.", ex);
            }
        }
    }
}
