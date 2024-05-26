using Backend.Dtos;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Backend.Helpers;
using System.IdentityModel.Tokens.Jwt;
using Backend.Middlewares;


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
        public async Task<IActionResult> GetAllCategories(
     [FromQuery] int pageNumber = 1, // Optional query parameter for pagination - page number
     [FromQuery] int pageSize = 100// Optional query parameter for pagination - page size

 )
        {
            try
            {

                // Check if the sortBy value is valid, if not, return bad request


                var categories = await _categoryService.GetAllCategories(
                    pageNumber,
                    pageSize

                ); // Get categories from service

                return ApiResponse.Success(categories, "Successfully returned all categories."); // Return success response with categories
            }
            catch (Exception ex) // Catching any exceptions
            {
                throw new InternalServerException(ex.Message); // Return server error with exception message
            }
        }

        // Endpoint to retrieve a specific category by ID
        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetCategory(int categoryId)
        {
            try
            {
                // Call service to get category by ID
                var category = await _categoryService.GetCategoryById(categoryId);
                if (category != null)
                {
                    return ApiResponse.Created(category);
                }
                else
                {
                    throw new NotFoundException("Category was not found");
                }
            }
            catch (Exception ex)
            {
                throw new InternalServerException(ex.Message);
            }
        }

        // Endpoint to create a new category
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            try
            {

                var category = new Category
                {
                    CategoryName = createCategoryDto.CategoryName,
                    CategorySlug = SlugGenerator.GenerateSlug(createCategoryDto.CategoryName),
                    CategoryDescription = createCategoryDto.CategoryDescription
                };

                var createdCategory = await _categoryService.CreateCategory(category);
                var test = CreatedAtAction(nameof(GetCategory), new { categoryId = createdCategory.CategoryId }, createdCategory) ?? throw new NotFoundException("Failed to create a category");
                return ApiResponse.Created(createCategoryDto, "Category created successfully");


            }
            catch (Exception ex)
            {
                throw new InternalServerException(ex.Message);
            }
        }

        // Endpoint to update an existing category
        [HttpPut("{categoryId}")]
        public async Task<IActionResult> UpdateCategory(int categoryId, UpdateCategoryDto categoryDto)
        {
            try
            {


                //"Admin access granted"
                // Call service to update category
                var updatedCategory = await _categoryService.UpdateCategory(categoryId, categoryDto);
                if (updatedCategory == null)
                {
                    throw new NotFoundException("Category was not found");
                }
                else
                {
                    return ApiResponse.Success(updatedCategory, "Update Category successfully");
                }



            }
            catch (Exception ex)
            {
                throw new InternalServerException(ex.Message);
            }
        }

        // Endpoint to delete a category by ID
        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            try
            {
                // Fetch the existing category from the database
                var existingCategory = await _categoryService.GetCategoryById(categoryId)
                                        ?? throw new NotFoundException($"Category with ID {categoryId} was not found");


                // Admin can delete the category
                var result = await _categoryService.DeleteCategory(categoryId);
                if (result)
                {
                    return ApiResponse.Deleted(existingCategory, $"Category with ID {categoryId} successfully deleted.");
                }
                else
                {
                    throw new InternalServerException($"Failed to delete category with ID {categoryId}");
                }

            }
            catch (Exception ex)
            {
                throw new InternalServerException(ex.Message);
            }
        }

    }
}
