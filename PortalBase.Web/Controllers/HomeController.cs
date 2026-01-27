using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalBase.Core.Entities;
using PortalBase.Web.Data;
using PortalBase.Web.Models;

namespace PortalBase.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Get featured scholarships (featured first, then by deadline)
        var featuredScholarships = await _context.Scholarships
            .Where(s => s.IsActive && s.Deadline >= DateTime.UtcNow)
            .OrderByDescending(s => s.IsFeatured)
            .ThenBy(s => s.Deadline)
            .Take(6)
            .ToListAsync();

        ViewBag.FeaturedScholarships = featuredScholarships;
        return View();
    }

    public async Task<IActionResult> Page(string slug)
    {
        if (string.IsNullOrEmpty(slug))
        {
            return NotFound();
        }

        var page = await _context.Pages
            .FirstOrDefaultAsync(p => p.Slug == slug && p.IsPublished);

        if (page == null)
        {
            return NotFound();
        }

        return View(page);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
