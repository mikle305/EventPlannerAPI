using Microsoft.AspNetCore.Mvc;
using EventPlannerAPI.Data;
using EventPlannerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace EventPlannerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectController : Controller
{
    private readonly DataContext _db;

    public ProjectController(DataContext db)
    {
        _db = db;
    }
    
    [HttpPost("SetNewProject")]
    public async Task<IActionResult> SetNewProject(int userId, string projectName)
    {
        var project = new Project(projectName, DateTime.Now);
        await _db.Projects.AddAsync(project);
        await _db.SaveChangesAsync();
        await _db.UsersProjects.AddAsync(
            new UserProject(project.Id, userId, UserProjectRole.Owner, false)
            );
        await _db.SaveChangesAsync();
        
        return Ok(new
        {
            Success = true,
            ProjectId = project.Id
        });
    }

    [HttpPost("SetUserRole")]
    public async Task<IActionResult> SetUserRole(int roleAssignerId, int roleReceiverId, int projectId, UserProjectRole role)
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

    [HttpPost("UpdateProject")]
    public async Task<IActionResult> UpdateProject(int id, string name, string? description)
    {
        var project = (await _db.Projects.ToListAsync()).FirstOrDefault(p => p.Id == id);
        if (project == null)
        {
            return Ok(new
            {
                Success = false,
                Error = "This project id doesn't exist"
            });
        }

        project.Name = name;
        project.Description = description;
        await _db.SaveChangesAsync();
        return Ok(new
        {
            Success = true,
            Project = new
            {
                project.Id,
                project.Name,
                project.Description,
                project.CreatedAt
            }
        });
    }

    [HttpGet("GetAllProjectsOfUser")]
    public async Task<IActionResult> GetAllProjectsOfUser(int userId)
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
            Success = true,
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