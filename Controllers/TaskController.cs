using EventPlannerAPI.Data;
using EventPlannerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task = EventPlannerAPI.Models.Task;
using TaskStatus = EventPlannerAPI.Models.TaskStatus;

namespace EventPlannerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController : Controller
{
    private readonly DataContext _db;

    public TaskController(DataContext db)
    {
        _db = db;
    }
    
    [HttpPost("SetNewTask")]
    public async Task<IActionResult> SetNewTask(int projectId, string name, DateTime deadline)
    {
        var task = new Task(name, projectId, deadline, DateTime.Now, TaskStatus.InProcess);
        await _db.Tasks.AddAsync(task);
        await _db.SaveChangesAsync();
        
        return Ok(new
        {
            Success = true,
            task.Id
        });
    }
    
    [HttpPost("UpdateTask")]
    public async Task<IActionResult> UpdateTask(
        int id, string name, string? description, TaskStatus status,
        DateTime deadline, TaskIteration? iterationFrequency)
    {
        var task = (await _db.Tasks.ToListAsync()).FirstOrDefault(t => t.Id == id);
        if (task == null)
        {
            return Ok(new 
            {
                Success = false,
                Error = "This task id doesn't exist"
            });
        }

        task.Name = name;
        task.Description = description;
        task.Status = status;
        task.Deadline = deadline;
        task.IterationFrequency = iterationFrequency;
        await _db.SaveChangesAsync();
        
        return Ok(new
        {
            Success = true,
            Task = new
            {
                task.Id,
                task.ProjectId,
                task.Name,
                task.Description,
                task.CreatedAt,
                task.Deadline,
                task.Status,
                task.IterationFrequency
            }
        });
    }

    [HttpGet("GetAllTasksByMonth")]
    public async Task<IActionResult> GetAllTasksByMonth(int userId, int projectId, int year, int month)
    {
        
        var tasks = 
            (await _db.Tasks.ToListAsync())
            .Where(t =>
                t.Project.UsersProject.FirstOrDefault(
                    r => r.UserId == userId && r.ProjectId == projectId) 
                != null &&
                t.Deadline.Year == year && 
                t.Deadline.Month == month);
        Dictionary<int, TaskInfoByDay> tasksByDay = new Dictionary<int, TaskInfoByDay>();
        foreach (var task in tasks)
        {
            if (!tasksByDay.ContainsKey(task.Deadline.Day))
                tasksByDay[task.Deadline.Day] = new TaskInfoByDay 
                { 
                    Weekday = task.Deadline.DayOfWeek,
                    IsToday = task.Deadline.Date == DateTime.Today,
                    Tasks = new List<Task>()
                };
            tasksByDay[task.Deadline.Day].Tasks.Add(task);
        }
        
        return Ok(new
        {
            Success = true,
            TasksOfMonth = tasksByDay
        });
    }

    [HttpGet("GetTaskInfoById")]
    public async Task<IActionResult> GetTaskInfoById(int id)
    {
        var task = (await _db.Tasks.ToListAsync()).FirstOrDefault(t => t.Id == id);
        if (task == null)
        {
            return Ok(new 
            {
                Success = false,
                Error = "This task id doesn't exist"
            });
        }

        return Ok(new
        {
            Success = true,
            Task = new
            {
                task.Id,
                task.ProjectId,
                task.Name,
                task.Description,
                task.CreatedAt,
                task.Deadline,
                task.Status,
                task.IterationFrequency
            }
        });
    }

    [HttpPost("DeleteTaskById")]
    public async Task<IActionResult> DeleteTaskById(int id)
    {
        var task = (await _db.Tasks.ToListAsync()).FirstOrDefault(t => t.Id == id);
        if (task == null)
        {
            return Ok(new 
            {
                Success = false,
                Error = "This task id doesn't exist"
            });
        }
        
        _db.Tasks.Remove(task);
        await _db.SaveChangesAsync();
        return Ok(new
        {
            Success = true
        });
    }

    private class TaskInfoByDay
    {
        public DayOfWeek Weekday { get; set; }

        public bool IsToday { get; set; }

        public List<Task> Tasks { get; set; }
    }
}