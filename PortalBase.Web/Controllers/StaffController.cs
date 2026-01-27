using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalBase.Web.Data;

namespace PortalBase.Web.Controllers;

[Authorize(Roles = "Admin")]
public class StaffController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<StaffController> _logger;

    public StaffController(ApplicationDbContext context, ILogger<StaffController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IActionResult> Dashboard()
    {
        var totalScholarships = await _context.Scholarships.CountAsync();
        var activeScholarships = await _context.Scholarships.CountAsync(s => s.IsActive && s.Deadline >= DateTime.UtcNow);
        var totalPages = await _context.Pages.CountAsync();
        var publishedPages = await _context.Pages.CountAsync(p => p.IsPublished);
        var totalInquiries = await _context.ContactInquiries.CountAsync();
        var unreadInquiries = await _context.ContactInquiries.CountAsync(i => !i.IsRead && !i.IsArchived);

        ViewBag.TotalScholarships = totalScholarships;
        ViewBag.ActiveScholarships = activeScholarships;
        ViewBag.TotalPages = totalPages;
        ViewBag.PublishedPages = publishedPages;
        ViewBag.TotalInquiries = totalInquiries;
        ViewBag.UnreadInquiries = unreadInquiries;

        // Recent inquiries
        ViewBag.RecentInquiries = await _context.ContactInquiries
            .OrderByDescending(i => i.CreatedAt)
            .Take(5)
            .ToListAsync();

        // Recent scholarships
        ViewBag.RecentScholarships = await _context.Scholarships
            .OrderByDescending(s => s.CreatedAt)
            .Take(5)
            .ToListAsync();

        return View();
    }
}


