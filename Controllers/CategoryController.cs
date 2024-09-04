using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserCategorySystem.Context;
using UserCategorySystem.Models;
#nullable disable

namespace UserCategorySystem.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoryController : ControllerBase
{
    private readonly UserCategorySystemContext _context;

    public CategoryController(UserCategorySystemContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Category>>> GetAllCategories()
    {
        return await _context.Categories.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategory(int id)
    {
        return await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
    }

    [HttpPost]
    public async Task<ActionResult<Category>> CreateCategory(Category category)
    {
        var isCategoryExist = _context.Categories.Any(x => x.Name.ToLower() == category.Name.ToLower());

        if(isCategoryExist)
            return Ok("Category already exist");
        
        _context.Categories.Add(category);
        var result = await _context.SaveChangesAsync();

        if(result <= 0)
            throw new Exception("Something went wrong");
        else
            return category;
    }

    [HttpPut]
    public async Task<ActionResult> UpdateCategory(Category category)
    {
        var isCategoryExist = _context.Categories.Any(x => x.Id == category.Id);

        if(!isCategoryExist)
            return NotFound("Category not found");
        
        _context.Categories.Update(category);
        var result = await _context.SaveChangesAsync();

        if(result <= 0)
            throw new Exception("Something went wrong");
        else
            return NoContent();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteCategory(int id)
    {
        var existingCategory = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

        if(existingCategory == null)
            return NotFound("Category not found");
        
        _context.Categories.Remove(existingCategory);
        var result = await _context.SaveChangesAsync();

        if(result <= 0)
            throw new Exception("Something went wrong");
        else
            return NoContent();
    }
}