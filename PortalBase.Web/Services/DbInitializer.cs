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
                ContentHtml = @"
                    <div class='prose max-w-none text-gray-600'>
                        <h2 class='text-2xl font-bold text-alda-dark mb-4'>1. Introduction</h2>
                        <p class='mb-6'>At ALDA TRAVELED LIMITED, we respect your privacy and are committed to protecting your personal data. This privacy policy will inform you as to how we look after your personal data when you visit our website and tell you about your privacy rights and how the law protects you.</p>

                        <h2 class='text-2xl font-bold text-alda-dark mb-4'>2. The Data We Collect</h2>
                        <p class='mb-4'>We may collect, use, store and transfer different kinds of personal data about you which we have grouped together as follows:</p>
                        <ul class='list-disc pl-6 mb-6 space-y-2'>
                            <li><strong>Identity Data:</strong> includes first name, last name, username or similar identifier.</li>
                            <li><strong>Contact Data:</strong> includes email address and telephone numbers.</li>
                            <li><strong>Academic Data:</strong> includes educational history, transcripts, and qualifications for scholarship and admission purposes.</li>
                            <li><strong>Technical Data:</strong> includes internet protocol (IP) address, browser type and version, and location.</li>
                        </ul>

                        <h2 class='text-2xl font-bold text-alda-dark mb-4'>3. How We Use Your Data</h2>
                        <p class='mb-6'>We will only use your personal data when the law allows us to. Most commonly, we will use your personal data to provide consultancy services, process visa support inquiries, and facilitate university admissions.</p>

                        <h2 class='text-2xl font-bold text-alda-dark mb-4'>4. Data Security</h2>
                        <p class='mb-6'>We have put in place appropriate security measures to prevent your personal data from being accidentally lost, used or accessed in an unauthorised way, altered or disclosed.</p>

                        <h2 class='text-2xl font-bold text-alda-dark mb-4'>5. Contact Us</h2>
                        <p class='mb-6'>If you have any questions about this privacy policy or our privacy practices, please contact us at <a href='mailto:info@aldatraveled.com' class='text-alda-gold font-bold transition-colors'>info@aldatraveled.com</a>.</p>
                    </div>",
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
                ContentHtml = @"
                    <div class='prose max-w-none text-gray-600'>
                        <h2 class='text-2xl font-bold text-alda-dark mb-4'>1. Terms of Use</h2>
                        <p class='mb-6'>By accessing this website, you are agreeing to be bound by these website Terms of Service, all applicable laws and regulations, and agree that you are responsible for compliance with any applicable local laws.</p>

                        <h2 class='text-2xl font-bold text-alda-dark mb-4'>2. Use License</h2>
                        <p class='mb-6'>Permission is granted to temporarily view the materials on ALDA TRAVELED LIMITED's website for personal, non-commercial transitory viewing only. This is the grant of a license, not a transfer of title.</p>

                        <h2 class='text-2xl font-bold text-alda-dark mb-4'>3. Disclaimer of Services</h2>
                        <p class='mb-6'>The information provided by ALDA TRAVELED LIMITED is for general consultancy purposes. While we strive for accuracy in admissions and visa guidance, we do not guarantee the outcome of any application, as final decisions rest with the respective institutions and government authorities.</p>

                        <h2 class='text-2xl font-bold text-alda-dark mb-4'>4. Limitations</h2>
                        <p class='mb-6'>In no event shall ALDA TRAVELED LIMITED or its suppliers be liable for any damages arising out of the use or inability to use the materials on our website.</p>

                        <h2 class='text-2xl font-bold text-alda-dark mb-4'>5. Governing Law</h2>
                        <p class='mb-6'>These terms and conditions are governed by and construed in accordance with the laws of Nigeria and the United Kingdom, and you irrevocably submit to the exclusive jurisdiction of the courts in those locations.</p>
                    </div>",
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


