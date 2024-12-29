using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Booking.Core.Interfaces;

namespace Booking.Core.Entities;

public class FormDynamicField : IHasId
{
    [Key]
    public required Guid Id { get; set; }
    
    public required string Title { get; set; }
    
    public required bool IsRequired { get; set; }
    
    public Guid? FieldTypeId { get; set; }
    [ForeignKey("FieldTypeId")] 
    public DynamicFieldType? FieldType { get; set; } = null!;

    public required int MaxSymbols { get; set; }

    public string? MinValue { get; set; }
    
    public string? MaxValue { get; set; }
    
    public required Guid EventFormId { get; set; }
    [ForeignKey("EventFormId")] 
    public EventSignupForm SignupForm { get; set; } = null!;
}