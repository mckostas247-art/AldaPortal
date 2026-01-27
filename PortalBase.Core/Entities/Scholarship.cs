using System.ComponentModel.DataAnnotations;

namespace PortalBase.Core.Entities;

public class Scholarship
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    public string Description { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string Country { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string FieldOfStudy { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(50)]
    public string DegreeLevel { get; set; } = string.Empty; // Bachelor, Master, PhD, etc.
    
    [Required]
    public DateTime Deadline { get; set; }
    
    [Required]
    public decimal Amount { get; set; }
    
    [MaxLength(10)]
    public string Currency { get; set; } = "USD";
    
    [Required]
    public string Eligibility { get; set; } = string.Empty;
    
    [Required]
    public string RequiredDocuments { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? ApplicationUrl { get; set; }
    
    [MaxLength(500)]
    public string? OfficialWebsite { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public bool IsFeatured { get; set; } = false;
    
    [MaxLength(1000)]
    public string? AdditionalInfo { get; set; }

    [MaxLength(1000)]
    public string? AdminNotes { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime LastUpdated { get; set; }
}



