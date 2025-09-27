# Evently - Event Management Application

A modern, full-stack event management platform built with .NET and React, designed to streamline event organization and management processes.

## 📋 Features
- 🎫 **Event Creation & Management** - Create and manage events with detailed information
- 👥 **User Authentication** - Google OAuth integration
- 📱 **QR Code Support** - Generate and scan QR codes for events
- 🖼️ **Image Management** - Upload and compress event images
- 📊 **Data Export** - Export event data to CSV
- 📧 **Email Notifications** - Automated email system
- 📱 **Progressive Web App** - Mobile-friendly experience
- 🔍 **Advanced Search & Filtering** - Find events easily


## 🚀 Quick Start

### 🌐 Live Demo
Experience Evently in action: [Website](https://ca-evently-prod-sea.graybush-4e3751bc.southeastasia.azurecontainerapps.io/)

### 🐳 Docker (Recommended)
Get up and running in minutes with Docker:

```bash
# Build and run with Docker Compose
docker-compose up --build

# Access the application
# Website: http://localhost:4000
```


## 🛠 Tech Stack
### ⚙️ Backend
- **Framework**: .NET 9.0 with ASP.NET Core
- **Language**: C# 13.0
- **UI Framework**: Blazor Server components
- **Architecture**: Web API with MVC pattern

### 🎨 Frontend
- **Framework**: React 19.1.1
- **Language**: TypeScript 5.8.3
- **Routing**: TanStack Router v1.131.7
- **State Management**: TanStack React Query v5.84.2
- **Styling**: Tailwind CSS 4.1.11 with DaisyUI 5.0.50
- **Build Tool**: Vite 7.1.0

### 🏗️ Infrastructure & DevOps
- **CI/CD**: GitHub Actions
- **Cloud**: Azure
- **IAC**: Terraform

## 🚀 Getting Started

### Prerequisites
- .NET 9.0 SDK
- Node.js (with npm/pnpm)
- Docker (optional)

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd evently

2. **Install dependencies**
   ```bash
   # Backend dependencies (if needed)
   dotnet restore

   # Frontend dependencies
   npm install
   # or
   pnpm install
   ```

3. **Development Setup**
   ```bash
   # Use the Makefile for common tasks
   make build
   make run
   ```

## 🧪 Testing
The project includes a comprehensive testing setup:
- **Testing Framework**: Vitest 3.2.4
- **React Testing**: React Testing Library 16.3.0
- **DOM Testing**: Testing Library DOM 10.4.1
- **User Interaction Testing**: User Event 14.6.1

Run tests:
``` bash
# Backend tests
dotnet test tests/Evently.Server.Test/

# Frontend tests
npm test
# or
pnpm test
```


## 🔧 Development
### Code Quality
The project maintains high code quality standards with:
- **ESLint 9.32.0**: JavaScript/TypeScript linting
- **Prettier 3.6.2**: Code formatting
- **EditorConfig**: Consistent coding styles
- **TypeScript**: Strong typing for frontend

### Build Tools
- **Vite**: Fast development server and build tool
- **Makefile**: Standardized build commands
- **Docker Compose**: Development environment orchestration

### 📁 Project Structure
The project follows a **Feature Folder Structure** or **Vertical Slice Architecture** pattern, organizing code by business features rather than technical layers. This approach promotes better maintainability, team collaboration, and feature isolation.

``` 
evently/
├── src/                          # Source code
│   ├── evently.client/           # React frontend application
│   └── Evently.Server/           # .NET backend application
├── tests/                        # Test projects
│   └── Evently.Server.Test/      # Backend unit tests
├── deploy/                       # Infrastructure and deployment
│   └── Terraform/                # Terraform infrastructure code
├── .github/                      # GitHub Actions workflows
│   └── workflows/
│       ├── build.yml            # CI pipeline
│       └── deploy.yml           # Deployment pipeline
├── docker-compose.yml           # Docker services configuration
├── Makefile                     # Build automation
└── package.json                # Frontend dependencies
```