using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalBase.Core.Entities;
using PortalBase.Web.Data;

namespace PortalBase.Web.Controllers;

public class ContactController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ContactController> _logger;

    public ContactController(ApplicationDbContext context, ILogger<ContactController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit(ContactInquiry inquiry)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", inquiry);
        }

        try
        {
            inquiry.CreatedAt = DateTime.UtcNow;
            inquiry.LastUpdated = DateTime.UtcNow;
            inquiry.IsRead = false;
            inquiry.IsArchived = false;

            _context.ContactInquiries.Add(inquiry);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Thank you for your message! We'll get back to you soon.";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting contact inquiry");
            ModelState.AddModelError("", "An error occurred while sending your message. Please try again.");
            return View("Index", inquiry);
        }
    }
}


