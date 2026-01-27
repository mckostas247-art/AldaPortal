using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalBase.Core.Entities;

public class Page
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Slug { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(500)]
    public string Title { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? HeroImageUrl { get; set; }
    
    [Required]
    public string ContentHtml { get; set; } = string.Empty;
    
    public bool IsPublished { get; set; }
    
    [MaxLength(500)]
    public string? SeoDescription { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime LastUpdated { get; set; }
}


