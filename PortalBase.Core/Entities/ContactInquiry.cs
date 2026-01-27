using System.ComponentModel.DataAnnotations;

namespace PortalBase.Core.Entities;

public class ContactInquiry
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string EmailAddress { get; set; } = string.Empty;
    
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Subject { get; set; } = string.Empty;
    
    [Required]
    public string Message { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string InquiryType { get; set; } = "General"; // General, Scholarship, Travel, Education
    
    public bool IsRead { get; set; } = false;
    
    public bool IsArchived { get; set; } = false;
    
    public DateTime? ReadDate { get; set; }
    
    [MaxLength(500)]
    public string? AdminNotes { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime LastUpdated { get; set; }
}


