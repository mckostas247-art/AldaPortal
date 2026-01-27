using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalBase.Core.Entities;
using PortalBase.Web.Data;

namespace PortalBase.Web.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AdminController> _logger;

    public AdminController(ApplicationDbContext context, ILogger<AdminController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: Admin
    public async Task<IActionResult> Index()
    {
        var pages = await _context.Pages.OrderByDescending(p => p.LastUpdated).ToListAsync();
        return View(pages);
    }

    // GET: Admin/Settings
    public IActionResult Settings()
    {
        return View();
    }

    // GET: Admin/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Admin/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Page page)
    {
        if (ModelState.IsValid)
        {
            page.CreatedAt = DateTime.UtcNow;
            page.LastUpdated = DateTime.UtcNow;
            
            _context.Add(page);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(page);
    }

    // GET: Admin/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var page = await _context.Pages.FindAsync(id);
        if (page == null)
        {
            return NotFound();
        }
        return View(page);
    }

    // POST: Admin/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Page page)
    {
        if (id != page.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                page.LastUpdated = DateTime.UtcNow;
                _context.Update(page);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PageExists(page.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(page);
    }

    // GET: Admin/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var page = await _context.Pages
            .FirstOrDefaultAsync(m => m.Id == id);
        if (page == null)
        {
            return NotFound();
        }

        return View(page);
    }

    // POST: Admin/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var page = await _context.Pages.FindAsync(id);
        if (page != null)
        {
            _context.Pages.Remove(page);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool PageExists(int id)
    {
        return _context.Pages.Any(e => e.Id == id);
    }

    // ========== Scholarship Management ==========

    // GET: Admin/Scholarships
    public async Task<IActionResult> Scholarships()
    {
        var scholarships = await _context.Scholarships.OrderByDescending(s => s.LastUpdated).ToListAsync();
        return View(scholarships);
    }

    // GET: Admin/Scholarships/Create
    public IActionResult CreateScholarship()
    {
        return View();
    }

    // POST: Admin/Scholarships/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateScholarship(Scholarship scholarship)
    {
        if (ModelState.IsValid)
        {
            scholarship.CreatedAt = DateTime.UtcNow;
            scholarship.LastUpdated = DateTime.UtcNow;
            
            _context.Add(scholarship);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Scholarships));
        }
        return View(scholarship);
    }

    // GET: Admin/Scholarships/Edit/5
    public async Task<IActionResult> EditScholarship(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var scholarship = await _context.Scholarships.FindAsync(id);
        if (scholarship == null)
        {
            return NotFound();
        }
        return View(scholarship);
    }

    // POST: Admin/Scholarships/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditScholarship(int id, Scholarship scholarship)
    {
        if (id != scholarship.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                scholarship.LastUpdated = DateTime.UtcNow;
                _context.Update(scholarship);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScholarshipExists(scholarship.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Scholarships));
        }
        return View(scholarship);
    }

    // GET: Admin/Scholarships/Delete/5
    public async Task<IActionResult> DeleteScholarship(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var scholarship = await _context.Scholarships
            .FirstOrDefaultAsync(m => m.Id == id);
        if (scholarship == null)
        {
            return NotFound();
        }

        return View(scholarship);
    }

    // POST: Admin/Scholarships/Delete/5
    [HttpPost, ActionName("DeleteScholarship")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteScholarshipConfirmed(int id)
    {
        var scholarship = await _context.Scholarships.FindAsync(id);
        if (scholarship != null)
        {
            _context.Scholarships.Remove(scholarship);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Scholarships));
    }

    private bool ScholarshipExists(int id)
    {
        return _context.Scholarships.Any(e => e.Id == id);
    }

    // ========== Contact Inquiries Management ==========

    // GET: Admin/Inquiries
    public async Task<IActionResult> Inquiries()
    {
        var inquiries = await _context.ContactInquiries
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();
        return View(inquiries);
    }

    // GET: Admin/Inquiries/Details/5
    public async Task<IActionResult> InquiryDetails(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var inquiry = await _context.ContactInquiries.FindAsync(id);
        if (inquiry == null)
        {
            return NotFound();
        }

        // Mark as read when viewing
        if (!inquiry.IsRead)
        {
            inquiry.IsRead = true;
            inquiry.ReadDate = DateTime.UtcNow;
            inquiry.LastUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        return View(inquiry);
    }

    // POST: Admin/Inquiries/MarkAsRead/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var inquiry = await _context.ContactInquiries.FindAsync(id);
        if (inquiry != null)
        {
            inquiry.IsRead = true;
            inquiry.ReadDate = DateTime.UtcNow;
            inquiry.LastUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Inquiries));
    }

    // POST: Admin/Inquiries/Archive/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Archive(int id)
    {
        var inquiry = await _context.ContactInquiries.FindAsync(id);
        if (inquiry != null)
        {
            inquiry.IsArchived = true;
            inquiry.LastUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Inquiries));
    }

    // POST: Admin/Inquiries/Unarchive/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Unarchive(int id)
    {
        var inquiry = await _context.ContactInquiries.FindAsync(id);
        if (inquiry != null)
        {
            inquiry.IsArchived = false;
            inquiry.LastUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Inquiries));
    }

    // POST: Admin/Scholarships/UpdateNotes/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateScholarshipNotes(int id, string adminNotes)
    {
        var scholarship = await _context.Scholarships.FindAsync(id);
        if (scholarship != null)
        {
            scholarship.AdminNotes = adminNotes;
            scholarship.LastUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(EditScholarship), new { id });
    }

    // POST: Admin/Inquiries/UpdateNotes/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateNotes(int id, string adminNotes)
    {
        var inquiry = await _context.ContactInquiries.FindAsync(id);
        if (inquiry != null)
        {
            inquiry.AdminNotes = adminNotes;
            inquiry.LastUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(InquiryDetails), new { id });
    }
}


