using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalBase.Core.Entities;
using PortalBase.Web.Data;

namespace PortalBase.Web.Controllers;

public class ScholarshipsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ScholarshipsController> _logger;

    public ScholarshipsController(ApplicationDbContext context, ILogger<ScholarshipsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: Scholarships
    public async Task<IActionResult> Index(string? search, string? country, string? fieldOfStudy, string? degreeLevel, string? sort, int page = 1)
    {
        var pageSize = 12;
        var query = _context.Scholarships.Where(s => s.IsActive && s.Deadline >= DateTime.UtcNow);

        // Apply filters
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(s => 
                s.Title.Contains(search) || 
                s.Description.Contains(search) ||
                s.Country.Contains(search) ||
                s.FieldOfStudy.Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(country))
        {
            query = query.Where(s => s.Country == country);
        }

        if (!string.IsNullOrWhiteSpace(fieldOfStudy))
        {
            query = query.Where(s => s.FieldOfStudy == fieldOfStudy);
        }

        if (!string.IsNullOrWhiteSpace(degreeLevel))
        {
            query = query.Where(s => s.DegreeLevel == degreeLevel);
        }

        // Apply sorting
        query = sort switch
        {
            "amount_desc" => query.OrderByDescending(s => s.Amount),
            "amount_asc" => query.OrderBy(s => s.Amount),
            "deadline_desc" => query.OrderByDescending(s => s.Deadline),
            "country_asc" => query.OrderBy(s => s.Country),
            _ => query.OrderBy(s => s.Deadline) // Default: Deadline (Soonest)
        };

        var totalCount = await query.CountAsync();
        var scholarships = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        ViewBag.Search = search;
        ViewBag.Country = country;
        ViewBag.FieldOfStudy = fieldOfStudy;
        ViewBag.DegreeLevel = degreeLevel;
        ViewBag.Sort = sort;
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        ViewBag.TotalCount = totalCount;

        // Get filter options
        ViewBag.Countries = new List<string> { "UNITED KINGDOM", "IRELAND", "GERMANY", "AUSTRALIA", "USA", "CANADA" };

        ViewBag.FieldsOfStudy = new List<string> 
        { 
            "ARTS", 
            "BUSINESS, MANAGEMENT AND ECONOMICS", 
            "ENGINEERING AND TECHNOLOGY", 
            "HEALTH SCIENCES, MEDICINE, NURSING, PARAMEDIC AND KINESIOLOGY", 
            "LAW, POLITICS, SOCIAL, and SCIENCES" 
        };

        ViewBag.DegreeLevels = new List<string> 
        { 
            "4-YEAR BACHELOR'S DEGREE", 
            "TOP-UP DEGREE", 
            "2-YEAR UNDERGRADUATE DIPLOMA", 
            "INTEGRATED MASTERS",
            "MASTER'S DEGREE", 
            "DOCTORAL/PHD", 
            "POSTGRADUATE DIPLOMA", 
            "POSTGRADUATE CERTIFICATE" 
        };

        return View(scholarships);
    }

    // GET: Scholarships/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var scholarship = await _context.Scholarships
            .FirstOrDefaultAsync(m => m.Id == id && m.IsActive);

        if (scholarship == null)
        {
            return NotFound();
        }

        // Get related scholarships
        ViewBag.RelatedScholarships = await _context.Scholarships
            .Where(s => s.IsActive && 
                       s.Id != id && 
                       (s.Country == scholarship.Country || s.FieldOfStudy == scholarship.FieldOfStudy))
            .OrderBy(s => s.Deadline)
            .Take(3)
            .ToListAsync();

        return View(scholarship);
    }
}



