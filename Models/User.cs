namespace EventPlannerAPI.Models;

public class User
{
    public int Id { get; set; }
    
    public string Username { get; set; }
    
    public string Password { get; set; }
    
    public string Email { get; set; }

    public string? TelegramId { get; set; }

    public virtual List<UserProject> UserProjects { get; set; }

    public User(string username, string password, string email)
    {
        Username = username;
        Password = password;
        Email = email;
    }
}