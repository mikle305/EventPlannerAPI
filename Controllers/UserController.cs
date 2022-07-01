using EventPlannerAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventPlannerAPI.Controllers;

[ApiController]
[Route("I[controller]")]
public class UserController : ControllerBase
{
    private DataContext _db;
    
    public UserController(DataContext db)
    {
        _db = db;
    }
    
    [HttpPost("Register")]
    public async Task<IActionResult> Register(string email, string username, string password)
    {
        List<User> users = await _db.Users.ToListAsync();
        bool emailTaken = users.FirstOrDefault(user => user.Email == email) != null;
        bool nameTaken = users.FirstOrDefault(user => user.Username == username) != null;
        if (emailTaken || nameTaken)
        {
            return BadRequest(new
            {
                Success = false,
                EmailTaken = emailTaken,
                NameTaken = nameTaken
            });
        }
        await _db.Users.AddAsync(new User()
        {
            Email = email,
            Username = username,
            Password = password
        });
        await _db.SaveChangesAsync();
        users = await _db.Users.ToListAsync();
        return Ok(new
        {
            Success = true,
            users.First(user => user.Email == email).Id
        });
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(string email, string password)
    {
        List<User> users = await _db.Users.ToListAsync();
        var user = users.FirstOrDefault(user => user.Email == email && user.Password == password);
        if (user == null) 
        {
            return BadRequest(new
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
