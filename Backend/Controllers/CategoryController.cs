using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Backend.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public CategoryController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCategories()
        {
            if (await _dataContext.Categories.ToListAsync() == null)
            {
                return NotFound("No categories in the database");
            }

            var results = await _dataContext.Categories.ToListAsync();
            return Ok(results);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Roles = "Admin"]
        public async Task<IActionResult> CreateCategory([FromBody] Category category)
        {
            if (category == null || string.IsNullOrEmpty(category.CategoryName))
            {
                return BadRequest("Category name is required.");
            }

            if (CategoryExists(category.CategoryName))
            {
                return BadRequest("Category already exists");
            }

            _dataContext.Categories.Add(category);
            await _dataContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategories), new { id = category.CategoryId }, category);
        }


        private bool CategoryExists(string categoryName)
        {
            return _dataContext.Categories.Any(c => c.CategoryName == categoryName);
        }


        [HttpPut("categoryId")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody] Category updatedCategory)
        {
            if (updatedCategory == null || string.IsNullOrEmpty(updatedCategory.CategoryName))
            {
                return BadRequest("Category name is required.");
            }

            var category = await _dataContext.Categories.FindAsync(categoryId);
            if (category == null)
            {
                return NotFound("No category with that ID found.");
            }

            if (CategoryExists(updatedCategory.CategoryName) && updatedCategory.CategoryName != category.CategoryName)
            {
                return BadRequest("Category with that name already exists");             
            }

            category.CategoryName = updatedCategory.CategoryName;
            await _dataContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategories), new { id = category.CategoryId }, category);
        }



        [HttpDelete("categoryId")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            var category = await _dataContext.Categories.FindAsync(categoryId);
            if (category == null)
            {
                return NotFound();
            }

            _dataContext.Categories.Remove(category);
            await _dataContext.SaveChangesAsync();

            return Ok("Category deleted.");
        }
    }
}