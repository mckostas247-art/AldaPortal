#!/bin/bash

# --- Alda Portal Quick Update Script ---
# This script automates the pull, cleanup, and rebuild process.

echo "🚀 Starting Alda Portal Update..."

# 1. Navigate to the project directory
cd ~/alda || { echo "❌ Directory ~/alda not found!"; exit 1; }

# 2. Pull the latest code from GitHub
echo "📥 Pulling latest changes from Git..."
git pull origin main

# 3. Stop and Remove old containers
echo "🧹 Cleaning up old containers..."
if command -v docker-compose &> /dev/null
then
    docker-compose down
else
    docker compose down
fi

# 4. Spin up the new environment using Docker Compose
echo "🏗️  Rebuilding and Starting services (Force Recreate)..."
if command -v docker-compose &> /dev/null
then
    docker-compose up -d --build --force-recreate
else
    docker compose up -d --build --force-recreate
fi

echo "✅ Update Complete! Your site is now running the latest code."
echo "📺 Use 'docker compose logs -f web' to see real-time output."
