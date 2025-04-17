using System.ComponentModel.DataAnnotations;

public class LoginRequest
{
    [Required]
    [MaxLength(20)]
    public string Username { get; set; }

    [Required]
    [MinLength(8)]
    [MaxLength(20)]
    public string Password { get; set; }
}

public class LoginResponse
{
    public string Token { get; set; }
}