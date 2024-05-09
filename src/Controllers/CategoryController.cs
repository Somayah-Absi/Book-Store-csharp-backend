using Backend.Dtos;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Backend.Helpers;


namespace Backend.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        // Constructor to initialize CategoryService
        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        }

        // Endpoint to retrieve all categories
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                // Call service to get all categories
                var categories = await _categoryService.GetAllCategories();
                return ApiResponse.Success(categories, "all categories retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse.ServerError(ex.Message);
            }
        }

        // Endpoint to retrieve a specific category by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            try
            {
                // Call service to get category by ID
                var category = await _categoryService.GetCategoryById(id);
                if (category != null)
                {
                    return ApiResponse.Created(category);
                }
                else
                {
                    return ApiResponse.NotFound("Category was not found");
                }
            }
            catch (Exception ex)
            {
                return ApiResponse.ServerError(ex.Message);
            }
        }

        // Endpoint to create a new category
        [HttpPost]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            try
            {
                // Generate slug and create new category
                category.CategorySlug = SlugGenerator.GenerateSlug(category.CategoryName);
                var createdCategory = await _categoryService.CreateCategory(category);
                return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.CategoryId }, createdCategory);
            }
            catch (Exception ex)
            {
                return ApiResponse.ServerError(ex.Message);
            }
        }

        // Endpoint to update an existing category
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryDto categoryDto)
        {
            try
            {
                // Call service to update category
                var updatedCategory = await _categoryService.UpdateCategory(id, categoryDto);
                if (updatedCategory == null)
                {
                    return ApiResponse.NotFound("Category was not found");
                }
                else
                {
                    return ApiResponse.Success(updatedCategory, "Update Category successfully");
                }
            }
            catch (Exception ex)
            {
                return ApiResponse.ServerError(ex.Message);
            }
        }

        // Endpoint to delete a category by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                // Call service to delete category
                var result = await _categoryService.DeleteCategory(id);
                if (result)
                {
                    return NoContent();
                }
                else
                {
                    return ApiResponse.NotFound("Category was not found");
                }
            }
            catch (Exception ex)
            {
                return ApiResponse.ServerError(ex.Message);
            }
        }
    }
}
