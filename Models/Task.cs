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
}

public enum TaskStatus
{
    Ended,
    Failed,
    InProcess
}

public enum TaskIteration
{
     Daily,
     Weekly,
     Monthly
}
