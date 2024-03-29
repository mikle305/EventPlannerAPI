﻿namespace EventPlannerAPI.Models;

public class Task
{
    public int Id { get; set; }
        
    public int ProjectId { get; set; }

    public string Name { get; set; }
        
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }
        
    public DateTime Deadline { get; set; }

    public TaskType Type { get; set; }

    public TaskStatus Status { get; set; }
        
    public TaskIteration? IterationFrequency { get; set; }
    
    public virtual Project Project { get; set; }

    public Task(string name, int projectId, string? description, 
        DateTime deadline, DateTime createdAt, TaskType type, TaskStatus status)
    {
        Deadline = deadline;
        CreatedAt = createdAt;
        Name = name;
        ProjectId = projectId;
        Type = type;
        Description = description;
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

public enum TaskType
{
    Task = 0,
    Event = 1,
    Meeting = 2
}
