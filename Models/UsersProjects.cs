namespace EventPlannerAPI;

public class UsersProjects
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    public int UserId { get; set; }

    public RoleState Role { get; set; }
    
    public bool IsNotifiable { get; set; }

    public enum RoleState
    {
        Reader,
        Write
    }
}