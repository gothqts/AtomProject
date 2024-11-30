using System.ComponentModel.DataAnnotations;

namespace Booking.Core.Interfaces;

public interface IHasId
{
    [Key]
    public Guid Id { get; set; }
}