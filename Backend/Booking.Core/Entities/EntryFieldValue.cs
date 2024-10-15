using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Booking.Core.Interfaces;

namespace Booking.Core.Entities;

public class EntryFieldValue : IHasId
{
    [Key]   
    public required Guid Id { get; set; }
    
    public required Guid EventSignupEntryId { get; set; }
    [ForeignKey("EventSignupEntryId")]
    public EventSignupEntry EventSignupEntry { get; set; } = null!;
    
    public required Guid DynamicFieldId { get; set; }
    [ForeignKey("DynamicFieldId")] 
    public FormDynamicField DynamicField { get; set; } = null!;
    
    public string Value { get; set; } = null!;
}