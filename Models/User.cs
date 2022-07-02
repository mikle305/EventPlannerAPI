namespace EventPlannerAPI.Models;

public class User
{
    public int Id { get; set; }
    
    public string Username { get; set; }
    
    public string Password { get; set; }
    
    public string Email { get; set; }

    public string? TelegramId { get; set; }

    public List<UserProject> UserProjects { get; set; }
}