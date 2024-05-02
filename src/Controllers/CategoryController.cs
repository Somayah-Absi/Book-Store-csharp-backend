using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        }

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            try
            {
                var categories = _categoryService.GetAllCategories();
                return Ok(new SuccessResponse<IEnumerable<Category>>
                {
                    Success = true,
                    Message = "Categories retrieved successfully",
                    Data = categories
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { Success = false, Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetCategory(int id)
        {
            try
            {
                var category = _categoryService.GetCategoryById(id);
                if (category != null)
                {
                    return Ok(new SuccessResponse<Category>
                    {
                        Message = "Category retrieved successfully",
                        Data = category
                    });
                }
                else
                {
                    return NotFound(new ErrorResponse { Message = "Category not found" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { Message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult CreateCategory(Category category)
        {
            try
            {
                category.CategorySlug = SlugGenerator.GenerateSlug(category.CategoryName);
                var createdCategory = _categoryService.CreateCategory(category);

                return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.CategoryId }, new SuccessResponse<Category>
                {
                    Message = "Category created successfully",
                    Data = createdCategory
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id, Category category)
        {
            try
            {
                category.CategorySlug = SlugGenerator.GenerateSlug(category.CategoryName);
                var updatedCategory = _categoryService.UpdateCategory(id, category);
                if (updatedCategory == null)
                {
                    return NotFound(new ErrorResponse { Message = "Category not found" });
                }
                else
                {
                    return Ok(new SuccessResponse<Category>
                    {
                        Message = "Category updated successfully",
                        Data = updatedCategory
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            try
            {
                var result = _categoryService.DeleteCategory(id);
                if (result)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound(new ErrorResponse { Message = "Category not found" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { Message = ex.Message });
            }
        }
    }
}
