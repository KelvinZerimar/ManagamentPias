namespace ManagamentPias.App.Wrappers;

public record SignInData
{
    public MySignInResult Result { get; init; }
    public TokenModel Token { get; init; } = null!;
    public string Username { get; init; } = null!;
    public string Email { get; init; } = null!;
}

public record TokenModel
{
    public string TokenType { get; }
    public string AccessToken { get; }
    public DateTime ExpiresAt { get; }

    public TokenModel(string tokenType, string accessToken, DateTime expiresAt)
        => (TokenType, AccessToken, ExpiresAt) = (tokenType, accessToken, expiresAt);

    public int GetRemainingLifetimeSeconds()
        => Math.Max(0, (int)(ExpiresAt - DateTime.Now).TotalSeconds);
}