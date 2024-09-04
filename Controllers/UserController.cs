using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserCategorySystem.Context;
using UserCategorySystem.Models;
#nullable disable

namespace UserCategorySystem.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly UserCategorySystemContext _context;

    public UserController(UserCategorySystemContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<User>>> GetAllUsers()
    {
        return await _context.Users.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
    }

    [HttpGet("GetMyCategory")]
    public async Task<ActionResult> GetMyCategory(int userId)
    {
        var userCategories = await _context.UserCategories.Where(x => x.UserId == userId)
                                            .Include(x => x.User)
                                            .Include(x => x.Category)
                                                .ThenInclude(x => x.SubCategories)
                                            .Select(x => new
                                            {
                                                x.User.Name,
                                                categoryName = x.Category.Name,
                                                x.Category.SubCategories
                                            })
                                            .ToListAsync();
        return Ok(userCategories);
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(User user)
    {
        var isUserExist = _context.Users.Any(x => x.Email.ToLower() == user.Email.ToLower());

        // Hash the user's password using MD5
        using (MD5 md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(user.Password);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert hashed bytes to a hexadecimal string
            user.Password = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        if(isUserExist)
            return Ok("User already exist");
        
        _context.Users.Add(user);
        var result = await _context.SaveChangesAsync();

        if(result <= 0)
            throw new Exception("Something went wrong");
        else
            return user;
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(User user)
    {
        var isUserExist = _context.Users.Any(x => x.Id == user.Id);

        if(!isUserExist)
            return NotFound("User not found");

        using (MD5 md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(user.Password);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert hashed bytes to a hexadecimal string
            user.Password = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        _context.Users.Update(user);
        var result = await _context.SaveChangesAsync();

        if(result <= 0)
            throw new Exception("Something went wrong");
        else
            return NoContent();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteUser(int id)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

        if(existingUser == null)
            return NotFound("User not found");
        
        _context.Users.Remove(existingUser);
        var result = await _context.SaveChangesAsync();

        if(result <= 0)
            throw new Exception("Something went wrong");
        else
            return NoContent();
    }

    [HttpPost("login")]
    public async Task<ActionResult<User>> Login(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());

        if (user == null)
            return NotFound("User not found");

        // Hash the input password using MD5
        string hashedInputPassword;
        using (MD5 md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            hashedInputPassword = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        // Compare the hashed input password with the actual password
        if (user.Password != hashedInputPassword)
            return Unauthorized("Invalid password");
        else
            return Ok("Login successful");
    }
}