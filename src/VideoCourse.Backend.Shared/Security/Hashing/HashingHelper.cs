using System.Security.Cryptography;
using System.Text;

namespace VideoCourse.Backend.Shared.Security.Hashing;
/// <summary>
/// Provides methods for creating and verifying password hashes, and generating SHA-256 hashes.
/// </summary>

public static class HashingHelper
{
    /// <summary>
    /// Provides methods for creating and verifying password hashes, and generating SHA-256 hashes.
    /// </summary>
    public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using HMACSHA512 hmac = new();

        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    /// <summary>
    /// Verifies the password hash against the provided password and salt.
    /// </summary>
    public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using HMACSHA512 hmac = new(passwordSalt);

        byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        return computedHash.SequenceEqual(passwordHash);
    }

    public static string Sha256Hash(string value)
    {
        var messageBytes = Encoding.UTF8.GetBytes(value);
        var hashedBytes = SHA256.HashData(messageBytes);
        var hashedKey = BitConverter.ToString(hashedBytes).Replace("-", string.Empty);
        return hashedKey.ToLower();
    }
}