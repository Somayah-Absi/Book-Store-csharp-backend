using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Services
{
    public class CategoryService
    {
        private readonly EcommerceSdaContext _dbContext;

        public CategoryService(EcommerceSdaContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public IEnumerable<Category> GetAllCategories()
        {
            try
            {
                return _dbContext.Categories.ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving categories.", ex);
            }
        }
        public Category? GetCategoryById(int id)
        {
            try
            {
                var category = _dbContext.Categories.Find(id);
                return category;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while retrieving category with ID {id}.", ex);
            }
        }

        public Category CreateCategory(Category category)
        {
            try
            {
                _dbContext.Categories.Add(category);
                _dbContext.SaveChanges();
                return category;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while creating category.", ex);
            }
        }

        public Category? UpdateCategory(int id, Category category)
        {
            try
            {
                var existingCategory = _dbContext.Categories.Find(id);
                if (existingCategory != null)
                {
                    existingCategory.CategoryName = category.CategoryName;
                    existingCategory.CategorySlug = SlugGenerator.GenerateSlug(category.CategoryName);
                    existingCategory.CategoryDescription = category.CategoryDescription;
                    _dbContext.SaveChanges();
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

        public bool DeleteCategory(int id)
        {
            try
            {
                var categoryToDelete = _dbContext.Categories.Find(id);
                if (categoryToDelete != null)
                {
                    _dbContext.Categories.Remove(categoryToDelete);
                    _dbContext.SaveChanges();
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
