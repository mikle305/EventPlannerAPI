namespace EventPlannerAPI.Models;

public class Event
{
    public int Id { get; set; }
        
    public int ProjectId { get; set; }

    public string Name { get; set; }
        
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }
        
    public DateTime Deadline { get; set; }

    public EventStatus Status { get; set; }
        
    public EventIteration? IterationFrequency { get; set; }
}

public enum EventStatus
{
    Ended,
    Failed,
    InProcess
}

public enum EventIteration
{
     Daily,
     Weekly,
     Monthly
}
