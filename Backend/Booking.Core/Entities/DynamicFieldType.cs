using System.ComponentModel.DataAnnotations;
using Booking.Core.Interfaces;

namespace Booking.Core.Entities;

public class DynamicFieldType : IHasId
{
    [Key]
    public required Guid Id { get; set; }

    public required string Title { get; set; }
}