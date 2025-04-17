

using System.ComponentModel.DataAnnotations;

public class RegisterRequest
{
    [Required]
    [MaxLength(20)]
    public string UserName { get; set; }

    [Required]
    [MaxLength(30)]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(8)]
    [MaxLength(20)]
    public string Password { get; set; }
}

public class RegisterResponse
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
}