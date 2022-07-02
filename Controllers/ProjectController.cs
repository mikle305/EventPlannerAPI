using Microsoft.AspNetCore.Mvc;
using EventPlannerAPI.Data;
using EventPlannerAPI.Models;

namespace EventPlannerAPI.Controllers;

[ApiController]
[Route("api/I[controller]")]
public class ProjectController : Controller
{
    private readonly DataContext _db;

    public ProjectController(DataContext db)
    {
        _db = db;
    }
    
    [HttpPost("Create")]
    public async Task<IActionResult> Create(int userId, string projectName)
    {
        var project = new Project()
        {
            Name = projectName,
            CreatedAt = DateTime.Now
        };
        await _db.Projects.AddAsync(project);
        await _db.SaveChangesAsync();
        await _db.UsersProjects.AddAsync(
            new UserProject()
            {
                ProjectId = project.Id,
                UserId = userId,
                Role = ProjectUserRole.Owner,
                IsNotifiable = false,
            });
        await _db.SaveChangesAsync();
        
        return Ok(new
        {
            Success = true,
            ProjectId = project.Id
        });
    }
}