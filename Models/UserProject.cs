namespace EventPlannerAPI.Models;

public class UserProject
{
    public int ProjectId { get; set; }

    public int UserId { get; set; }

    public UserProjectRole Role { get; set; }
    
    public bool IsNotifiable { get; set; }

    public virtual Project Project { get; set; }
    
    public virtual User User { get; set; }
}

public enum UserProjectRole
{
    Owner = 4,
    Administrator = 3,
    Writer = 2,
    Reader = 1
}