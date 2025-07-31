# Library Management System

## Architecture 

This project follows *Clean Architecture* with clear separation of concerns:


### Features
-  **RESTful API**: Standard HTTP methods and status codes
-  **Swagger Documentation**: Interactive API documentation
-  **Entity Framework Core**: Modern ORM with SQLite database
-  **AutoMapper**: Efficient object-to-object mappimg
-  **Dependency Injection**: Built-in .NET DI container
-  **Unit Testing**: Comprehensive test coverage with xUnit and Moq

### Tests
- **xUnit**: Testing framework
- **Moq**: Mocking framework
- **FluentValidation.TestHelper**: Validation testing utilities


### Prerequisites
- .NET 9 SDK
- Visual Studio 2022 or VS Code
- Git

### Installation

1. *Clone the repository*
   ```bash
   git clone <repository-url>
   cd LibraryManagement
   ```

2. *Restore dependencies*
   ```bash
   dotnet restore
   ```
3. *Run database migrations*
   ```bash
   cd src/API/LibraryManagement.Api
   dotnet ef database update
   ```

4. *Run the application*
   ```bash
   dotnet run
   ```
5. *Access the API*
     - Swagger UI: `https://localhost:7291/swagger/index.html`


### Run All Tests
```bash
dotnet test
```

##  Design Patterns

### 1. **Clean Architecture**
- Clear separation of concerns
- Dependency inversion
- Testable and maintainable code

### 2. **Repository Pattern**
- Abstracts data access logic
- Enables easy testing with mocks
- Supports different data sources

### 3. **Dependency Injection**
- Loose coupling between components
- EaSy testing and mocking
- Configurtion-based dependency resolution

### 4. **Command Pattern**
- Encapsulates request data
- Supports validation
- Clear input contracts


### Database 
The application uses SQLite by default


## Deployment

### Development
```bash
dotnet run --project src/API/LibraryManagement.Api
```

