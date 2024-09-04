using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserCategorySystem.Context;
using UserCategorySystem.Models;
#nullable disable

namespace UserCategorySystem.Controllers;

[ApiController]
[Route("[controller]")]
public class SubCategoryController : ControllerBase
{
    private readonly UserCategorySystemContext _context;

    public SubCategoryController(UserCategorySystemContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<SubCategory>>> GetAllSubCategories()
    {
        return await _context.SubCategories.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SubCategory>> GetSubCategory(int id)
    {
        return await _context.SubCategories.FirstOrDefaultAsync(x => x.Id == id);
    }

    [HttpPost]
    public async Task<ActionResult<SubCategory>> CreateSubCategory(SubCategory subCategory)
    {
        var isSubCategoryExist = _context.SubCategories.Any(x => x.Name.ToLower() == subCategory.Name.ToLower() && x.CategoryId == subCategory.CategoryId);

        if(isSubCategoryExist)
            return Ok("SubCategory already exist");
        
        _context.SubCategories.Add(subCategory);
        var result = await _context.SaveChangesAsync();

        if(result <= 0)
            throw new Exception("Something went wrong");
        else
            return subCategory;
    }

    [HttpPut]
    public async Task<ActionResult> UpdateSubCategory(SubCategory subCategory)
    {
        var isSubCategoryExist = _context.SubCategories.Any(x => x.Id == subCategory.Id);

        if(!isSubCategoryExist)
            return NotFound("SubCategory not found");
        
        _context.SubCategories.Update(subCategory);
        var result = await _context.SaveChangesAsync();

        if(result <= 0)
            throw new Exception("Something went wrong");
        else
            return NoContent();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteSubCategory(int id)
    {
        var existingSubCategory = await _context.SubCategories.FirstOrDefaultAsync(x => x.Id == id);

        if(existingSubCategory == null)
            return NotFound("SubCategory not found");
        
        _context.SubCategories.Remove(existingSubCategory);
        var result = await _context.SaveChangesAsync();

        if(result <= 0)
            throw new Exception("Something went wrong");
        else
            return NoContent();
    }
}