using EventPlannerAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task = EventPlannerAPI.Models.Task;

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

    [HttpGet("GetAllTasksOfMonth")]
    public async Task<IActionResult> GetAllTasksOfMonth(int userId, int projectId, int year, int month)
    {
        
        var tasks = 
            (await _db.Tasks.ToListAsync())
            .Where(t => t.Deadline.Year == year && t.Deadline.Month == month);
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
        return Ok(tasksByDay);
    }

    private class TaskInfoByDay
    {
        public DayOfWeek Weekday { get; set; }

        public bool IsToday { get; set; }

        public List<Task> Tasks { get; set; }
    }
}