using Microsoft.AspNetCore.Mvc;
using EventPlannerAPI.Data;
using EventPlannerAPI.Models;
using Microsoft.EntityFrameworkCore;

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
                Role = UserProjectRole.Owner,
                IsNotifiable = false,
            });
        await _db.SaveChangesAsync();
        return Ok(new
        {
            Success = true,
            ProjectId = project.Id
        });
    }

    [HttpPost("SetRole")]
    public async Task<IActionResult> SetRole(int roleAssignerId, int roleReceiverId, int projectId, UserProjectRole role)
    {
        if (roleAssignerId == roleReceiverId)
        {
            return Ok(new
            {
                Success = false,
                Error = "User can't edit role of himself"
            });
        }
        
        List<UserProject> relations = await _db.UsersProjects.ToListAsync();
        UserProject? assignerRelation = null;
        UserProject? receiverRelation = null;
        foreach (var relation in relations.Where(r => r.ProjectId == projectId))
        {
            if (relation.UserId == roleAssignerId)
                assignerRelation = relation;
            else if (relation.UserId == roleReceiverId)
                receiverRelation = relation;
        }
        
        if (receiverRelation == null || assignerRelation == null)
        {
            return Ok(new
            { 
                Success = false,
                Error = "User(s) is not member of this project",
                IsReceiverOnProject = receiverRelation != null,
                IsAssignerOnProject = assignerRelation != null
            });
        }

        if (assignerRelation.Role < UserProjectRole.Administrator)
        {
            return Ok(new
            {
                Success = false,
                Error = "Role assigner must have role greater than or equal to 'Administrator'"
            });
        }

        if ((int)assignerRelation.Role - (int)role < 1)
        {
            return Ok(new 
            {
                Success = false,
                Error = "Role assigner must have role greater than role which he gives"
            });
        }

        return Ok(new
        {
            Success = true
        });
    }

    [HttpGet("GetAllByUser")]
    public async Task<IActionResult> GetAllByUser(int userId)
    {
        var relations = (await _db.UsersProjects.ToListAsync()).Where(r => r.UserId == userId);
        var projects = new List<ProjectInfo>();
        foreach (var relation in relations)
        {
            projects.Add(new ProjectInfo
            {
                Id = relation.Project.Id,
                Name = relation.Project.Name,
                Role = relation.Role
            });
        }
        return Ok(new
        {
            Projects = projects
        });
    }

    private class ProjectInfo
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public UserProjectRole Role { get; set; }
    }
}