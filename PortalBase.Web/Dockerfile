# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Install Node.js and npm
RUN apt-get update && apt-get install -y nodejs npm

# Copy package files and Tailwind config for CSS build
COPY ["PortalBase.Web/package.json", "PortalBase.Web/postcss.config.js", "PortalBase.Web/tailwind.config.js", "PortalBase.Web/"]
# Copy CSS input file needed for Tailwind build
COPY ["PortalBase.Web/wwwroot/css/input.css", "PortalBase.Web/wwwroot/css/"]
WORKDIR /src/PortalBase.Web

# Install npm dependencies and build CSS
# npm install works with or without package-lock.json
RUN npm install && npm run build:css

# Copy solution and project files
WORKDIR /src
COPY ["PortalBase.Core/PortalBase.Core.csproj", "PortalBase.Core/"]
COPY ["PortalBase.Web/PortalBase.Web.csproj", "PortalBase.Web/"]
COPY ["PortalBase.sln", "./"]

# Restore dependencies
RUN dotnet restore "PortalBase.sln"

# Copy all source files
COPY . .

# Build and publish
WORKDIR /src/PortalBase.Web
RUN dotnet build "PortalBase.Web.csproj" -c Release -o /app/build
RUN dotnet publish "PortalBase.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Install curl for healthchecks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Critical environment variables for low RAM deployment
ENV DOTNET_gcServer=0
ENV ASPNETCORE_HTTP_PORTS=8080
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1

# Copy published output from build stage
COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "PortalBase.Web.dll"]


