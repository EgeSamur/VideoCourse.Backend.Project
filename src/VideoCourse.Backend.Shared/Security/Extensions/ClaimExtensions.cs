using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace VideoCourse.Backend.Shared.Security.Extensions;

/// <summary>
/// Provides extension methods for adding claims to a collection.
/// </summary>
public static class ClaimExtensions
{
    public static void AddEmail(this ICollection<Claim> claims, string email) =>
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, email));

    public static void AddName(this ICollection<Claim> claims, string name) => claims.Add(new Claim(ClaimTypes.Name, name));

    public static void AddNameIdentifier(this ICollection<Claim> claims, string nameIdentifier) =>
        claims.Add(new Claim(ClaimTypes.NameIdentifier, nameIdentifier));

    public static void AddRoles(this ICollection<Claim> claims, string[] roles) =>
        roles.ToList().ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));
    
    public static void AddPermissions(this ICollection<Claim> claims, IEnumerable<string> permissions)
    {
        foreach (var permission in permissions)
        {
            claims.Add(new Claim("Permission", permission));
        }
    }
}