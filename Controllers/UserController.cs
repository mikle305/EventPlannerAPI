using EventPlannerAPI.Data;
using EventPlannerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task = EventPlannerAPI.Models.Task;

namespace EventPlannerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly DataContext _db;
    
    public UserController(DataContext db)
    {
        _db = db;
    }
    
    [HttpPost("SetNewUser")]
    public async Task<IActionResult> SetNewUser(string email, string username, string password)
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

        var user = new User(username, password, email);
        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();
        return Ok(new
        {
            Success = true,
            user.Id
        });
    }

    [HttpPost("UpdateUser")]
    public async Task<IActionResult> UpdateUser(int id, string username, string password, string email, string? telegramId)
    {
        List<User> users = await _db.Users.ToListAsync();
        var user = users.FirstOrDefault(user => user.Id == id);
        if (user == null) 
        {
            return Ok(new
            {
                Success = false,
                Error = "This user id doesn't exist"
            });
        }

        user.Username = username;
        user.Password = password;
        user.Email = email;
        user.TelegramId = telegramId;
        await _db.SaveChangesAsync();
        
        return Ok(new
        {
            Success = true,
            User = new
            {
                user.Id,
                user.Username,
                user.Email,
                user.TelegramId
            }
        });
    }

    [HttpPost("GetUserByData")]
    public async Task<IActionResult> GetUserByData(string email, string password)
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
            User = new
            {
                user.Id,
                user.Username,
                user.Email,
                user.TelegramId
            }
        });
    }
}
