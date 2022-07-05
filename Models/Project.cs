﻿namespace EventPlannerAPI.Models;

public class Project
{
    public int Id { get; set; }
        
    public string Name { get; set; }
    
    public string? Description { get; set; }
        
    public DateTime CreatedAt { get; set; }

    public virtual List<UserProject> UsersProject { get; set; }
    
    public virtual List<Task> Tasks { get; set; }
}