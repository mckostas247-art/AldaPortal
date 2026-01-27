# Docker Deployment Guide

This guide explains how to deploy PortalBase using Docker Compose.

## Prerequisites

- Docker Engine 20.10+
- Docker Compose 2.0+

## Quick Start

1. **Copy environment file** (optional, for custom configuration):
   ```bash
   cp .env.example .env
   ```
   Edit `.env` to customize settings.

2. **Build and start services**:
   ```bash
   docker-compose up -d
   ```

3. **View logs**:
   ```bash
   docker-compose logs -f web
   ```

4. **Stop services**:
   ```bash
   docker-compose down
   ```

5. **Stop and remove volumes** (⚠️ deletes database):
   ```bash
   docker-compose down -v
   ```

## Services

### MySQL Database
- **Container**: `portalbase-mysql`
- **Port**: `3306` (configurable via `MYSQL_PORT`)
- **Database**: `PortalBase` (configurable via `MYSQL_DATABASE`)
- **Root Password**: Set via `MYSQL_ROOT_PASSWORD`
- **Application User**: Set via `MYSQL_USER` / `MYSQL_PASSWORD`
- **Data Persistence**: Stored in `portalbase_mysql_data` volume

### Web Application
- **Container**: `portalbase-web`
- **Port**: `8080` (configurable via `WEB_PORT`)
- **Health Check**: HTTP GET on port 8080
- **Auto-initialization**: Database migrations and seeding run automatically on startup

## Environment Variables

Create a `.env` file in the project root to customize:

```env
# MySQL Configuration
MYSQL_ROOT_PASSWORD=YourSecurePassword123!
MYSQL_DATABASE=PortalBase
MYSQL_USER=portalbase
MYSQL_PASSWORD=YourSecurePassword123!
MYSQL_PORT=3306

# Web Application
WEB_PORT=8080
ASPNETCORE_ENVIRONMENT=Production
```

## Production Deployment

### Security Checklist

1. ✅ **Change default passwords** in `.env` file
2. ✅ **Use strong passwords** (min 16 characters, mixed case, numbers, symbols)
3. ✅ **Set `ASPNETCORE_ENVIRONMENT=Production`**
4. ✅ **Configure reverse proxy** (nginx/traefik) for HTTPS
5. ✅ **Change default admin credentials** (`admin@portal.com` / `Password123!`)
6. ✅ **Restrict MySQL port** exposure (remove port mapping in production)
7. ✅ **Set up regular backups** for MySQL volume
8. ✅ **Configure firewall** rules

### Production docker-compose.yml Override

For production, create `docker-compose.prod.yml`:

```yaml
version: '3.8'

services:
  mysql:
    ports: []  # Don't expose MySQL port externally
    environment:
      MYSQL_ROOT_PASSWORD: ${MYSQL_ROOT_PASSWORD}
      MYSQL_PASSWORD: ${MYSQL_PASSWORD}

  web:
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
```

Run with:
```bash
docker-compose -f docker-compose.yml -f docker-compose.prod.yml up -d
```

## Database Management

### Access MySQL CLI
```bash
docker-compose exec mysql mysql -u root -p
```

### Backup Database
```bash
docker-compose exec mysql mysqldump -u root -p${MYSQL_ROOT_PASSWORD} PortalBase > backup.sql
```

### Restore Database
```bash
docker-compose exec -T mysql mysql -u root -p${MYSQL_ROOT_PASSWORD} PortalBase < backup.sql
```

### View Database Logs
```bash
docker-compose logs -f mysql
```

## Troubleshooting

### Web container won't start
- Check MySQL is healthy: `docker-compose ps`
- View web logs: `docker-compose logs web`
- Verify connection string matches MySQL credentials

### Database connection errors
- Ensure MySQL container is healthy: `docker-compose ps mysql`
- Check MySQL logs: `docker-compose logs mysql`
- Verify environment variables match between services

### Port conflicts
- Change ports in `.env` file
- Or modify `docker-compose.yml` directly

### Reset everything
```bash
docker-compose down -v
docker-compose up -d
```

## Default Admin Credentials

⚠️ **IMPORTANT**: Change these in production!

- **Email**: `admin@portal.com`
- **Password**: `Password123!`

Change via admin panel after first login or modify `DbInitializer.cs`.

## Monitoring

### Check service health
```bash
docker-compose ps
```

### View resource usage
```bash
docker stats portalbase-web portalbase-mysql
```

### Application logs
```bash
docker-compose logs -f --tail=100 web
```

## Updates

To update the application:

```bash
# Pull latest code
git pull

# Rebuild and restart
docker-compose up -d --build

# Or rebuild specific service
docker-compose build web
docker-compose up -d web
```



