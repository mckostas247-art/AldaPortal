# PortalBase

## About this solution

PortalBase is a clean, lightweight ASP.NET Core 8 MVC solution with a micro-CMS feature. It serves as a base template for portal building businesses, designed for easy deployment and customization.

### Features

- **Micro-CMS**: Dynamic page management with slug-based routing
- **Identity System**: Standard ASP.NET Core Identity with custom views (not RCL)
- **Tailwind CSS**: Integrated build pipeline with pixel-perfect design system
- **MySQL Database**: Using Pomelo.EntityFrameworkCore.MySql
- **Docker Ready**: Optimized multi-stage Dockerfile for Coolify/Hetzner deployment

### Pre-requirements

* [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet)
* [Node.js v18 or 20](https://nodejs.org/en) (for Tailwind CSS build)
* MySQL Server

### Project Structure

```
PortalBase/
├── PortalBase.Core/          # Class Library - Entities
│   └── Entities/
│       └── Page.cs
└── PortalBase.Web/           # ASP.NET Core MVC Application
    ├── Controllers/
    ├── Views/
    ├── Data/
    ├── Services/
    └── wwwroot/
```

### Configuration

1. **Database Connection**: Update `appsettings.json` with your MySQL connection string:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=PortalBase;User=root;Password=;"
     }
   }
   ```

2. **Brand Colors**: Update `tailwind.config.js` with your brand colors:
   ```javascript
   colors: {
     'alda-dark': '#1C1C1C',
     'alda-gold': '#E1AD01',
     // ... your colors
   }
   ```

### Running the Application

1. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

2. **Build Tailwind CSS** (automatically runs during build):
   ```bash
   cd PortalBase.Web
   npm install
   npm run build:css
   ```

3. **Create database migration**:
   ```bash
   dotnet ef migrations add InitialCreate --project PortalBase.Web
   ```

4. **Run the application**:
   ```bash
   dotnet run --project PortalBase.Web
   ```

The application will automatically:
- Create the database if it doesn't exist
- Run pending migrations
- Seed an admin user: `admin@portal.com` / `Password123!`
- Create a sample "Home" page

### Default Admin Credentials

- **Email**: `admin@portal.com`
- **Password**: `Password123!`

**⚠️ Important**: Change these credentials in production!

### Docker Deployment

The solution includes an optimized Dockerfile for deployment on Coolify/Hetzner:

```bash
docker build -t portalbase -f PortalBase.Web/Dockerfile .
docker run -p 8080:8080 -e ConnectionStrings__DefaultConnection="your-connection-string" portalbase
```

### Key Features

- **Dynamic Routing**: Pages are accessible via `/{slug}` URLs
- **Admin Panel**: Full CRUD operations for pages at `/Admin`
- **TinyMCE Integration**: Rich text editor for page content
- **Responsive Design**: Mobile-friendly with Tailwind CSS
- **Memory Optimized**: Configured for low RAM deployment (4GB server)

### Quick Server Update

If you are on the server and want to update the site with one command:

1. Ensure `deploy.sh` is on the server (it's in the root of the repo).
2. Run this once to make it executable: `chmod +x deploy.sh`
3. Every time you want to update: `./deploy.sh`

### Technology Stack

- ASP.NET Core 8.0 MVC
- Entity Framework Core with MySQL (Pomelo)
- ASP.NET Core Identity
- Tailwind CSS 3.4
- TinyMCE (CDN)
