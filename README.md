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

Update your `appsettings.json` with your email and Google OAuth Client credentials:

   ```json
   {
     "Authentication": {
       "Google": {
         "ClientId": "your-google-client-id",
         "ClientSecret": "your-google-client-secret"
       }
     },
     "EmailSettings": {
       "ActualFrom": "your-email@example.com",
       "SmtpPassword": "your-app-password"
     }
   }
   ```
Then, run the container:

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
- **Framework**: React 19
- **Language**: TypeScript 5
- **Routing**: TanStack Router v1
- **State Management**: TanStack React Query v5
- **Styling**: Tailwind CSS 4 with DaisyUI 5
- **Build Tool**: Vite 7

### 🏗️ Infrastructure & DevOps
- **CI/CD**: GitHub Actions
- **Cloud**: Azure
- **IAC**: Terraform

## 🏁 Getting Started

### Prerequisites
- .NET 9.0 SDK
- Node.js (with npm/pnpm)
- Docker (optional)
- pnpm

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
   cd src/evently.client && pnpm install
   ```

3. **Development Setup**
   ```bash
   # In one terminal
   make dev
   
   # In another terminal
   cd src/evently.client && pnpm run dev
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
cd src/evently.client && pnpm test
```


## 🔧 Development
### Code Quality
The project maintains high code quality standards with:
- **ESLint**: JavaScript/TypeScript linting
- **Prettier**: Code formatting
- **EditorConfig**: Consistent coding styles
- **TypeScript**: Strong typing for frontend

### Build Tools
- **Vite**: Fast development server and build tool
- **Makefile**: Standardized build commands
- **Docker Compose**: Development environment orchestration

### 📁 Project Structure
The project follows a **Feature Folder Structure** or **Vertical Slice Architecture** pattern, 
organizing code by business features rather than technical layers. This approach encourages modularity and separation of concerns.

``` 
evently/
├── src/                          # Source code
│   ├── evently.client/           # React frontend application
│   │   └── src/
│   │       ├── routes/           # Route-based feature organization
│   │       │   ├── login/        # Authentication features
│   │       │   ├── bookings/     # Booking management features
│   │       │   ├── gatherings/   # Event/gathering management features
│   │       │   ├── healthcheck/  # System health monitoring
│   │       │   └── ...           
│   │       └── lib/              # Shared utilities and components
│   └── Evently.Server/           # .NET backend application
│       └── Common/               # Shared utilities and infrastructure
│       └── Features/             # Feature-based organization
│           ├── Accounts/         # User authentication & authorization
│           ├── Bookings/         # Booking system features
│           ├── Gatherings/       # Event management features
│           ├── Files/            # Blob Storage features
│           └── ...           
├── tests/                        # Test projects
│   └── Evently.Server.Test/      # Backend unit tests
├── deploy/                       # Infrastructure and deployment
│   └── Terraform/                # Terraform infrastructure code
├── .github/                      # GitHub Actions workflows
│   └── workflows/
│       ├── build.yml            # CI pipeline
│       └── deploy.yml           # Deployment pipeline
├── docker-compose.yml           # Docker services configuration
└── Makefile                # Build automation
```