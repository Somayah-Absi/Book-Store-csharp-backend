using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class CategoryService
    {
        private readonly EcommerceSdaContext _dbContext;

        public CategoryService(EcommerceSdaContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            try
            {
                return await _dbContext.Categories.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving categories.", ex);
            }
        }
        public async Task <Category?> GetCategoryById(int id)
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

        public async Task <Category> CreateCategory(Category category)
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

        public async Task<Category?> UpdateCategory(int id, Category category)
        {
            try
            {
                var existingCategory = _dbContext.Categories.Find(id);
                if (existingCategory != null)
                {
                    existingCategory.CategoryName = category.CategoryName;
                    existingCategory.CategorySlug = SlugGenerator.GenerateSlug(category.CategoryName);
                    existingCategory.CategoryDescription = category.CategoryDescription;
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
