using System.ComponentModel.DataAnnotations;


/***************************

          AuthUser

***************************/
// Description
// : 회원 인증 엔티티이며, 회원가입 및 로그인에 사용됩니다.
//   회원가입 시에는 Username, Email, Password를 사용하여 생성됩니다.       
//
// Author : ChoiHyunSan
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

    public void Verify(string password)
    {
        if(!PasswordUtils.VerifyPassword(password, Password))
        {
            throw new ApiException(AuthErrorCodes.InvalidCredentials);
        }
    }

    private AuthUser(string username, string email, string password, string role = "User")
    {
        Username = username;
        Email = email;
        Password = password;
        CreatedAt = DateTime.UtcNow;
        Role = role;
    }
}


