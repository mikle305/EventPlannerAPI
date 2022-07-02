namespace EventPlannerAPI.Models;

public class UserProject
{
    public int ProjectId { get; set; }

    public int UserId { get; set; }

    public ProjectUserRole Role { get; set; }
    
    public bool IsNotifiable { get; set; }

    public Project Project { get; set; }
    
    public User User { get; set; }
}

public enum ProjectUserRole
{
    Owner,
    Administrator,
    Writer,
    Reader
}