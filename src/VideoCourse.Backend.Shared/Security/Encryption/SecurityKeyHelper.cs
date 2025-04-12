using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace VideoCourse.Backend.Shared.Security.Encryption;

/// <summary>
/// Creates a new instance of <see cref="SecurityKey"/> using the provided security key string.
/// </summary>
/// <param name="securityKey">The security key string.</param>
/// <returns>A new instance of <see cref="SecurityKey"/>.</returns>
public static class SecurityKeyHelper
{
    public static SecurityKey CreateSecurityKey(string securityKey) => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
}