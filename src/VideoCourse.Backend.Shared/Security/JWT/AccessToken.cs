namespace VideoCourse.Backend.Shared.Security.JWT;

/// <summary>
/// Represents an access token with its value and expiration date.
/// </summary>
public class AccessToken
{
    /// <summary>
    /// Gets or sets the token string.
    /// </summary>
    public string Token { get; set; }
    /// <summary>
    /// Gets or sets the expiration date of the token.
    /// </summary>
    public DateTime Expiration { get; set; }

    public AccessToken()
    {
        Token = string.Empty;
    }

    public AccessToken(string token, DateTime expiration)
    {
        Token = token;
        Expiration = expiration;
    }
}