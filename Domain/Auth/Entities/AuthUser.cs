using System.ComponentModel.DataAnnotations;

public class AuthUser
{
    [Key]
    public int Id { get; private set; }

    [Required]
    public string Username { get; protected set; }

    [MaxLength(100)]
    public string Password { get; protected set; }

    [Required]
    public string Email { get; protected set; }

    [Required]
    public DateTime CreatedAt { get; protected set; }

    [Required]
    public string Role { get; protected set; }

    protected AuthUser() { }

    public static AuthUser CreateWithLocal(string username, string email, string password)
        => new AuthUser(username, email, password);

    private AuthUser(string username, string email, string password, string role = "User")
    {
        Username = username;
        Email = email;
        Password = password;
        CreatedAt = DateTime.UtcNow;
        Role = role;
    }
}


