namespace Application.Models.Requests;

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class TokenResponseModel
{
    public string Token { get; set; }
    public string Type { get; set; } = "Bearer";
    public int ExpiresIn { get; set; }
}
