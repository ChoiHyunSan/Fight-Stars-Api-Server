public class RefreshRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}

public class RefreshResponse
{
    public string accessToken { get; set; } = string.Empty;
}