using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Booking.Core;

public static class AuthOptions
{
    public const string Issuer = "UnbeatableBookingServer"; // издатель токена
    public const string Audience = "UnbeatableBookingClient"; // потребитель токена
    private const string SecretKey = "RsI2VwEQoMUXyUVDIegYF7jg6XqJQdqCdD88Uxof";   // ключ для шифрации
    public static SymmetricSecurityKey GetSymmetricSecurityKey() => 
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));

    public const string RefreshTokenCookieName = "refreshToken";
    public const string ClaimTypeUserId = JwtRegisteredClaimNames.UniqueName;
    public const string ClaimTypeRole = ClaimTypes.Role;
    public const string ClaimTypeJti = JwtRegisteredClaimNames.Jti;
    public const string ClaimTypeExpireTime = JwtRegisteredClaimNames.Exp;
}