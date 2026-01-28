#!/bin/bash

# --- Alda Portal Quick Update Script ---
# This script automates the pull, cleanup, and rebuild process.

echo "🚀 Starting Alda Portal Update..."

# 1. Navigate to the project directory
cd ~/alda || { echo "❌ Directory ~/alda not found!"; exit 1; }

# 2. Pull the latest code from GitHub
echo "📥 Pulling latest changes from Git..."
git pull origin main

# 3. Stop and Remove old manual containers (silencing errors if they don't exist)
echo "🧹 Cleaning up old containers..."
docker stop alda-app alda-db 2>/dev/null
docker rm alda-app alda-db 2>/dev/null

# 4. Spin up the new environment using Docker Compose
echo "🏗️  Rebuilding and Starting services..."
# Using 'docker compose' (V2) or 'docker-compose' (V1)
if command -v docker-compose &> /dev/null
then
    docker-compose up -d --build
else
    docker compose up -d --build
fi

echo "✅ Update Complete! Your site is now running the latest code."
echo "📺 Use 'docker compose logs -f web' to see real-time output."
