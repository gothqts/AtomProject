using System.ComponentModel.DataAnnotations;

namespace Booking.Core.Entities;

public class RevokedAccessToken
{
    [Key]
    public Guid Jti { get; set; }
    
    public DateTime ExpirationTime { get; set; }
}