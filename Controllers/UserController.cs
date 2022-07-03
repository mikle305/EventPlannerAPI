using EventPlannerAPI.Data;
using EventPlannerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventPlannerAPI.Controllers;

[ApiController]
[Route("api/I[controller]")]
public class UserController : ControllerBase
{
    private readonly DataContext _db;
    
    public UserController(DataContext db)
    {
        _db = db;
    }
    
    [HttpPost("Register")]
    public async Task<IActionResult> Register(string email, string username, string password)
    {
        List<User> users = await _db.Users.ToListAsync();
        bool isEmailTaken = users.FirstOrDefault(user => user.Email == email) != null;
        bool isNameTaken = users.FirstOrDefault(user => user.Username == username) != null;
        
        if (isEmailTaken || isNameTaken)
        {
            return Ok(new
            {
                Success = false,
                Error = "Any data has already been taken by another user(s)",
                IsEmailTaken = isEmailTaken,
                IsNameTaken = isNameTaken
            });
        }
        var user = new User()
        {
            Email = email,
            Username = username,
            Password = password
        };
        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();
        return Ok(new
        {
            Success = true,
            user.Id
        });
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(string email, string password)
    {
        List<User> users = await _db.Users.ToListAsync();
        var user = users.FirstOrDefault(user => user.Email == email && user.Password == password);
        
        if (user == null) 
        {
            return Ok(new
            {
                Success = false,
                Error = "Invalid user data"
            });
        }
        return Ok(new
        {
            Success = true, 
            user.Id, 
            user.Username
        });
    }
}
