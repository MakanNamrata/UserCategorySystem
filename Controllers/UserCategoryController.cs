using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserCategorySystem.Context;
using UserCategorySystem.Models;
#nullable disable

namespace UserCategorySystem.Controllers;

[ApiController]
[Route("[controller]")]
public class UserCategoryController : ControllerBase
{
    private readonly UserCategorySystemContext _context;

    public UserCategoryController(UserCategorySystemContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserCategory>>> GetAllUserCategories()
    {
        return await _context.UserCategories.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserCategory>> GetUserCategory(int id)
    {
        return await _context.UserCategories.FirstOrDefaultAsync(x => x.Id == id);
    }    

    [HttpPost]
    public async Task<ActionResult<UserCategory>> CreateUserCategory(UserCategory userCategory)
    {
        var isUserCategoryExist = _context.UserCategories.Any(x => x.UserId == userCategory.UserId && x.CategoryId == userCategory.CategoryId);

        if(isUserCategoryExist)
            return Ok("UserCategory already exist");
        
        _context.UserCategories.Add(userCategory);
        var result = await _context.SaveChangesAsync();

        if(result <= 0)
            throw new Exception("Something went wrong");
        else
            return userCategory;
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUserCategory(UserCategory userCategory)
    {
        var isUserCategoryExist = _context.UserCategories.Any(x => x.Id == userCategory.Id);

        if(!isUserCategoryExist)
            return NotFound("UserCategory not found");
        
        _context.UserCategories.Update(userCategory);
        var result = await _context.SaveChangesAsync();

        if(result <= 0)
            throw new Exception("Something went wrong");
        else
            return NoContent();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteUserCategory(int id)
    {
        var existingUserCategory = await _context.UserCategories.FirstOrDefaultAsync(x => x.Id == id);

        if(existingUserCategory == null)
            return NotFound("UserCategory not found");
        
        _context.UserCategories.Remove(existingUserCategory);
        var result = await _context.SaveChangesAsync();

        if(result <= 0)
            throw new Exception("Something went wrong");
        else
            return NoContent();
    }
}