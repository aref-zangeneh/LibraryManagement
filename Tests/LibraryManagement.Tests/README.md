
### BookService Tests (`BookServices/BookServiceTests.cs`)
Comprehensuve unit tests for the `BookService` class covering all CRUD operations:

#### Coverage:
- **GetAllBooksAsync**: Tests for retrieving all books (with data and empty scenarios)
- **GetBookByIdAsync**: Tests for retrieving a book by ID (success and not found scenarios)
- **CreateBookAsync**: Tests for creating new books (valid and invalid validation scenarios)
- **UpdateBookAsync**: Tests for updating existing books (success and not found scenarios)
- **SoftDeleteBookAsync**: Tests for soft deleting books (success and not found scenarios)
- **HardDeleteBookAsync**: Tests for hard deleting books (success and not found scenarios)

#### Features:
- Uses **Moq** for mocking dependencies (`IGenericRepository<Book>`, `IMapper`)
- Tests both success and failure scenarios
- Validates exception handling (NotFoundException, ValidationException)
- Verifies that repoditory methods are called with correct parameters
- Tests validation logic integration





To run
```bash
dotnet test Tests/LibraryManagement.Tests/LibraryManagement.Tests.csproj
```
