using System.ComponentModel.DataAnnotations;

/***************************

      RegisterRequest

***************************/
// Description : 회원가입 요청 DTO
// Author : ChoiHyunSan
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

/***************************

      RegisterResponse

***************************/
// Description : 회원가입 요청 DTO
// Author : ChoiHyunSan
public class RegisterResponse
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
}