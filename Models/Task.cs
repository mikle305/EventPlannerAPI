namespace EventPlannerAPI.Models;

public class Task
{
    public int Id { get; set; }
        
    public int ProjectId { get; set; }

    public string Name { get; set; }
        
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }
        
    public DateTime Deadline { get; set; }

    public TaskStatus Status { get; set; }
        
    public TaskIteration? IterationFrequency { get; set; }
    
    public virtual Project Project { get; set; }

    public Task(string name, int projectId, DateTime deadline, DateTime createdAt, TaskStatus status)
    {
        Deadline = deadline;
        CreatedAt = createdAt;
        Name = name;
        ProjectId = projectId;
        Status = status;
    }
}

public enum TaskStatus
{
    InProcess = 0,
    Ended = 1,
    Failed = 2
}

public enum TaskIteration
{
     Daily = 0,
     Weekly = 1,
     Monthly = 2
}
