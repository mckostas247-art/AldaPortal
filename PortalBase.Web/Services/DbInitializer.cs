using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PortalBase.Core.Entities;
using PortalBase.Web.Data;

namespace PortalBase.Web.Services;

public class DbInitializer : IDbInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public DbInitializer(
        ApplicationDbContext context,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitializeAsync()
    {
        try
        {
            // Run migrations
            // We use a timeout to handle potential lock issues in Docker
            _context.Database.SetCommandTimeout(60);
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            // Log the error but don't crash the whole app startup if possible
            Console.WriteLine($"Migration error: {ex.Message}");
            
            if (ex.Message.Contains("already exists", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Tables already exist. Attempting to proceed to seeding...");
            }
            else 
            {
                // For other errors, we might still want to try seeding if the tables happen to be there
                Console.WriteLine("Unknown migration error. Checking if tables are accessible...");
            }
        }

        // Wrap the entire seeding in a try-catch to prevent app crash if tables are missing
        try 
        {
            // Create Admin role if it doesn't exist
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

        // Create default admin user if it doesn't exist
        var adminEmail = "admin@portal.com";
        var adminUser = await _userManager.FindByEmailAsync(adminEmail);
        
        if (adminUser == null)
        {
            adminUser = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(adminUser, "Password123!");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        // Create sample "Home" page if it doesn't exist
        if (!await _context.Pages.AnyAsync(p => p.Slug == "home"))
        {
            var homePage = new Page
            {
                Slug = "home",
                Title = "Welcome Home",
                ContentHtml = "<h1>Welcome to PortalBase</h1><p>This is your home page. You can edit this content from the admin panel.</p>",
                IsPublished = true,
                SeoDescription = "Welcome to PortalBase - Your portal solution",
                CreatedAt = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow
            };

            _context.Pages.Add(homePage);
        }

        // Create FAQ page
        if (!await _context.Pages.AnyAsync(p => p.Slug == "faq"))
        {
            _context.Pages.Add(new Page
            {
                Slug = "faq",
                Title = "Frequently Asked Questions",
                ContentHtml = @"
                    <div class='space-y-6'>
                        <div>
                            <h3 class='text-xl font-bold mb-2'>How do I apply for a scholarship?</h3>
                            <p>You can browse our scholarships list and click 'Apply Now' on any scholarship that interests you. Each scholarship has its own application process.</p>
                        </div>
                        <div>
                            <h3 class='text-xl font-bold mb-2'>What services do you offer?</h3>
                            <p>We offer study abroad admissions, visa support, accommodation advisory, and relocation assistance.</p>
                        </div>
                    </div>",
                IsPublished = true,
                CreatedAt = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow
            });
        }

        // Create Privacy Policy page
        if (!await _context.Pages.AnyAsync(p => p.Slug == "privacy-policy"))
        {
            _context.Pages.Add(new Page
            {
                Slug = "privacy-policy",
                Title = "Privacy Policy",
                ContentHtml = "<h1>Privacy Policy</h1><p>Testing privacy policy content for AldaTraveled...</p>",
                IsPublished = true,
                CreatedAt = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow
            });
        }

        // Create Terms of Service page
        if (!await _context.Pages.AnyAsync(p => p.Slug == "terms-of-service"))
        {
            _context.Pages.Add(new Page
            {
                Slug = "terms-of-service",
                Title = "Terms of Service",
                ContentHtml = "<h1>Terms of Service</h1><p>Testing terms of service content for AldaTraveled...</p>",
                IsPublished = true,
                CreatedAt = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow
            });
        }

        await _context.SaveChangesAsync();

        // Create sample scholarships if none exist
        if (!await _context.Scholarships.AnyAsync())
        {
            var scholarships = new List<Scholarship>
            {
                new Scholarship
                {
                    Title = "Fulbright Foreign Student Program",
                    Description = "The Fulbright Program is the flagship international educational exchange program sponsored by the U.S. government and is designed to increase mutual understanding between the people of the United States and the people of other countries.",
                    Country = "United States",
                    FieldOfStudy = "All Fields",
                    DegreeLevel = "Master",
                    Amount = 50000,
                    Currency = "USD",
                    Deadline = DateTime.UtcNow.AddMonths(6),
                    Eligibility = "Open to international students from eligible countries. Applicants must have completed undergraduate education and demonstrate academic excellence.",
                    RequiredDocuments = "1. Completed application form\n2. Academic transcripts\n3. Letters of recommendation\n4. Statement of purpose\n5. English proficiency test scores",
                    ApplicationUrl = "https://fulbright.state.gov",
                    OfficialWebsite = "https://fulbright.state.gov",
                    IsActive = true,
                    IsFeatured = true,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdated = DateTime.UtcNow
                },
                new Scholarship
                {
                    Title = "Chevening Scholarships",
                    Description = "Chevening Scholarships are the UK government's global scholarship programme, funded by the Foreign, Commonwealth & Development Office and partner organisations.",
                    Country = "United Kingdom",
                    FieldOfStudy = "All Fields",
                    DegreeLevel = "Master",
                    Amount = 35000,
                    Currency = "GBP",
                    Deadline = DateTime.UtcNow.AddMonths(4),
                    Eligibility = "Citizens of Chevening-eligible countries. Must have an undergraduate degree and at least 2 years of work experience.",
                    RequiredDocuments = "1. Online application\n2. Academic transcripts\n3. Two references\n4. English language test results",
                    ApplicationUrl = "https://www.chevening.org",
                    OfficialWebsite = "https://www.chevening.org",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdated = DateTime.UtcNow
                },
                new Scholarship
                {
                    Title = "Erasmus Mundus Joint Master Degrees",
                    Description = "Erasmus Mundus Joint Master Degrees are high-level integrated study programmes at master level, designed and delivered by an international partnership of higher education institutions.",
                    Country = "Multiple Countries",
                    FieldOfStudy = "Various Fields",
                    DegreeLevel = "Master",
                    Amount = 25000,
                    Currency = "EUR",
                    Deadline = DateTime.UtcNow.AddMonths(5),
                    Eligibility = "Open to students worldwide. Must hold a first higher education degree or demonstrate equivalent level of learning.",
                    RequiredDocuments = "1. Application form\n2. Academic transcripts\n3. CV\n4. Motivation letter\n5. Language certificates",
                    ApplicationUrl = "https://www.eacea.ec.europa.eu",
                    OfficialWebsite = "https://www.eacea.ec.europa.eu",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdated = DateTime.UtcNow
                }
            };

            _context.Scholarships.AddRange(scholarships);
            await _context.SaveChangesAsync();
        }
    }
    catch (Exception ex)
        {
            Console.WriteLine($"Seeding skipped or failed: {ex.Message}");
            // We don't throw here so the application can still start
        }
    }
}


