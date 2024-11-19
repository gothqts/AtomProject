using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Booking.Application.Utility;

public static class PasswordHelper
{
    private const int Pbkdf2IterCount = 1000;
    private const int Pbkdf2SubkeyLength = 256 / 8; // 256 bits
    private const int SaltSize = 128 / 8; // 128 bits
    private const KeyDerivationPrf Pbkdf2Prf = KeyDerivationPrf.HMACSHA1;
    
    public static HashAlgorithmName HashAlgorithm { get; } = HashAlgorithmName.SHA512;
    
    public static string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hashBytes = KeyDerivation.Pbkdf2(password, salt, Pbkdf2Prf, Pbkdf2IterCount, Pbkdf2SubkeyLength);

        var outputBytes = new byte[SaltSize + Pbkdf2SubkeyLength];
        Buffer.BlockCopy(salt, 0, outputBytes, 0, SaltSize);
        Buffer.BlockCopy(hashBytes, 0, outputBytes, SaltSize, Pbkdf2SubkeyLength);
        return Convert.ToHexString(outputBytes);
    }
    
    public static bool VerifyPassword(string hashedPassword, string password)
    {
        var hashBytes = Convert.FromHexString(hashedPassword);
        if (hashBytes.Length != SaltSize + Pbkdf2SubkeyLength)
        {
            return false; // bad size
        }
        var salt = new byte[SaltSize];
        
        Buffer.BlockCopy(hashBytes, 0, salt, 0, salt.Length);
        
        var expectedSubkey = new byte[Pbkdf2SubkeyLength];
        Buffer.BlockCopy(hashBytes, salt.Length, expectedSubkey, 0, expectedSubkey.Length);
        
        var actualSubkey = KeyDerivation.Pbkdf2(password, salt, Pbkdf2Prf, Pbkdf2IterCount, Pbkdf2SubkeyLength);
        return CryptographicOperations.FixedTimeEquals(actualSubkey, expectedSubkey);
    }
    
    public static string GenerateRandomPassword(int length)
    {
        return GenerateRandomString(length);
    }
    
    private static string GenerateRandomString(int length)
    {
        var builder = new StringBuilder();
        for(var i = 0; i < length; i++)
        {
            var ch = PossibleCharsForGenerating[Random.Shared.Next(0, PossibleCharsForGenerating.Length)];
            builder.Append(ch);
        }

        return builder.ToString();
    }
    
    private const string PossibleCharsForGenerating = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
}