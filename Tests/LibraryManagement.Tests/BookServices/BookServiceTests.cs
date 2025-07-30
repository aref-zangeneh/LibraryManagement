using AutoMapper;
using FluentValidation;
using LibraryManagement.Application.Commands;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Exceptions;
using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Application.Services;
using LibraryManagement.Application.Validators;
using LibraryManagement.Domain.Entities;
using Moq;

namespace LibraryManagement.Tests.BookServices
{
    public class BookServiceTests
    {
        #region Fields

        private readonly Mock<IGenericRepository<Book>> _mockBookRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly IValidator<CreateBookCommand> _validator;
        private readonly BookService _bookService;

        #endregion

        #region Constructor

        public BookServiceTests()
        {
            _mockBookRepository = new Mock<IGenericRepository<Book>>();
            _mockMapper = new Mock<IMapper>();
            _validator = new CreateBookCommandValidator();
            _bookService = new BookService(_mockBookRepository.Object, _mockMapper.Object, _validator);
        }

        #endregion

        #region Test Methods

        #region GetAllBooksAsync Tests

        [Fact]
        public async Task GetAllBooksAsync_ShouldReturnAllBooks_WhenBooksExist()
        {
            // arrange level
            var books = new List<Book>
            {
                new Book {
                    Id = 1,
                    Title = "Test Book 1",
                    Author = "Test Author 1",
                    PublishedYear = 2020,
                    Genre = "Test Genre1"
                },
                new Book
                {
                    Id = 2,
                    Title = "Test Book 2",
                    Author = "Test Author 2",
                    PublishedYear = 2021,
                    Genre = "Test Genre2"
                }
            };

            var bookDtos = new List<BookDto>
            {
                new BookDto
                {
                    Id = 1, Title = "Test Book 1",
                    Author = "Test Author 1",
                    PublishedYear = 2020,
                    Genre = "Test Genre1"
                },
                new BookDto { Id = 2,
                    Title = "Test Book 2",
                    Author = "Test Author 2",
                    PublishedYear = 2021,
                    Genre = "Test Genre2" }
            };

            _mockBookRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(books);
            _mockMapper.Setup(x => x.Map<IEnumerable<BookDto>>(books)).Returns(bookDtos);

            // act
            var result = await _bookService.GetAllBooksAsync();
            // assert level
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockBookRepository.Verify(x => x.GetAllAsync(), Times.Once);
            _mockMapper.Verify(x => x.Map<IEnumerable<BookDto>>(books), Times.Once);
        }

        [Fact]
        public async Task GetAllBooksAsync_ShouldReturnEmptyList_WhenNoBooksExist()
        {
            // arrange level
            var books = new List<Book>();
            var bookDtos = new List<BookDto>();
            _mockBookRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(books);
            _mockMapper.Setup(x => x.Map<IEnumerable<BookDto>>(books)).Returns(bookDtos);

            // act
            var result = await _bookService.GetAllBooksAsync();

            // assert level
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockBookRepository.Verify(x => x.GetAllAsync(), Times.Once);
        }

        #endregion

        #region GetBookByIdAsync Tests

        [Fact]
        public async Task GetBookByIdAsync_ShouldReturnBook_WhenBookExists()
        {
            // arrange level
            var bookId = 1;
            var book = new Book
            {
                Id = bookId,
                Title = "Test Book",
                Author = "Test Author",
                PublishedYear = 2020,
                Genre = "Test Genre"

            };
            var bookDto = new BookDto
            {
                Id = bookId,
                Title = "Test Book",
                Author = "Test Author",
                PublishedYear = 2020,
                Genre = "Test Genre"
            };

            _mockBookRepository.Setup(x => x.GetByIdAsync(bookId)).ReturnsAsync(book);
            _mockMapper.Setup(x => x.Map<BookDto>(book)).Returns(bookDto);

            // act
            var result = await _bookService.GetBookByIdAsync(bookId);

            // assert level
            Assert.NotNull(result);
            Assert.Equal(bookId, result.Id);
            Assert.Equal(book.Title, result.Title);
            _mockBookRepository.Verify(x => x.GetByIdAsync(bookId), Times.Once);
            _mockMapper.Verify(x => x.Map<BookDto>(book), Times.Once);
        }

        [Fact]
        public async Task GetBookByIdAsync_ShouldThrowNotFoundException_WhenBookDoesNotExist()
        {
            // arrange level
            var bookId = 999;
            _mockBookRepository.Setup(x => x.GetByIdAsync(bookId)).ReturnsAsync((Book?)null);

            // act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _bookService.GetBookByIdAsync(bookId));
            Assert.Equal("Book not found.", exception.Message);
            _mockBookRepository.Verify(x => x.GetByIdAsync(bookId), Times.Once);
        }

        #endregion

        #region CreateBookAsync Tests

        [Fact]
        public async Task CreateBookAsync_ShouldCreateBook_WhenValidCommandProvided()
        {
            // arrange level
            var command = new CreateBookCommand
            {
                Title = "New Book",
                Author = "New Author",
                PublishedYear = 2023,
                Genre = "Test Genre"
            };

            var book = new Book
            {
                Title = command.Title,
                Author = command.Author,
                PublishedYear = command.PublishedYear,
                Genre = command.Genre
            };

            _mockMapper.Setup(x => x.Map<Book>(command)).Returns(book);
            _mockBookRepository.Setup(x => x.AddAsync(book)).Returns(Task.CompletedTask);

            // act
            await _bookService.CreateBookAsync(command);

            // assert level
            _mockMapper.Verify(x => x.Map<Book>(command), Times.Once);
            _mockBookRepository.Verify(x => x.AddAsync(book), Times.Once);
        }

        [Theory]
        [InlineData("", "Author", 2023, "Genre", "Title must be filled out.")]
        [InlineData("Title", "", 2023, "Genre", "Author must be filled out.")]
        [InlineData("Title", "Author", -1, "Genre", "PublishedYear must be a positive number")]
        [InlineData("Title", "Author", 2030, "Genre", "PublishedYear cannot be greater than")]
        [InlineData("Title", "Author", 2023, "", "Genre must be filled out")]
        public async Task CreateBookAsync_ShouldThrowValidationException_WhenInvalidCommandProvided(
            string title, string author, int publishedYear, string genre, string expectedError)
        {
            // arrange level
            var command = new CreateBookCommand
            {
                Title = title,
                Author = author,
                PublishedYear = publishedYear,
                Genre = genre
            };

            // act and assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _bookService.CreateBookAsync(command));
            Assert.Contains(expectedError, exception.Message);
        }

        #endregion

        #region UpdateBookAsync Tests

        [Fact]
        public async Task UpdateBookAsync_ShouldUpdateBook_WhenBookExists()
        {
            // arrange level
            var bookId = 1;
            var existingBook = new Book
            {
                Id = bookId,
                Title = "Test Old Title",
                Author = "Test Old Author",
                PublishedYear = 2020,
                Genre = "Test Old Genre"
            };
            var command = new CreateBookCommand
            {
                Title = "Updated Title",
                Author = "Updated Author",
                PublishedYear = 2023,
                Genre = "Updated Genre"
            };

            _mockBookRepository.Setup(x => x.GetByIdAsync(bookId)).ReturnsAsync(existingBook);
            _mockMapper.Setup(x => x.Map(command, existingBook)).Returns(existingBook);
            _mockBookRepository.Setup(x => x.UpdateAsync(existingBook)).Returns(Task.CompletedTask);

            // act
            await _bookService.UpdateBookAsync(bookId, command);

            //assert level
            _mockBookRepository.Verify(x => x.GetByIdAsync(bookId), Times.Once);
            _mockMapper.Verify(x => x.Map(command, existingBook), Times.Once);
            _mockBookRepository.Verify(x => x.UpdateAsync(existingBook), Times.Once);
        }


        [Fact]
        public async Task UpdateBookAsync_ShouldThrowNotFoundException_WhenBookDoesNotExist()
        {
            //arrange level
            var bookId = 999;
            var command = new CreateBookCommand
            {
                Title = "Updated Title",
                Author = "Updated Author",
                PublishedYear = 2023,
                Genre = "Updated Genre"
            };

            _mockBookRepository.Setup(x => x.GetByIdAsync(bookId)).ReturnsAsync((Book?)null);
            //act and assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _bookService.UpdateBookAsync(bookId, command));
            Assert.Equal("Book not found", exception.Message);
            _mockBookRepository.Verify(x => x.GetByIdAsync(bookId), Times.Once);
            _mockBookRepository.Verify(x => x.UpdateAsync(It.IsAny<Book>()), Times.Never);
        }

        #endregion

        #region SoftDeleteBookAsync Tests
        [Fact]
        public async Task SoftDeleteBookAsync_ShouldSoftDeleteBook_WhenBookExists()
        {
            //arrange level
            var bookId = 1;
            var book = new Book
            {
                Id = bookId,
                Title = "Test Book",
                Author = "Test Author",
                PublishedYear = 2020,
                Genre = "Test Genre"
            };
            var bookDto = new BookDto
            {
                Id = bookId,
                Title = "Test Book",
                Author = "Test Author",
                PublishedYear = 2020,
                Genre = "Test Genre"
            };

            _mockBookRepository.Setup(x => x.GetByIdAsync(bookId)).ReturnsAsync(book);
            _mockMapper.Setup(x => x.Map<BookDto>(book)).Returns(bookDto);
            _mockBookRepository.Setup(x => x.SoftDeleteAsync(bookId)).Returns(Task.CompletedTask);

            // act
            await _bookService.SoftDeleteBookAsync(bookId);
            //assert level
            _mockBookRepository.Verify(x => x.GetByIdAsync(bookId), Times.Once);
            _mockBookRepository.Verify(x => x.SoftDeleteAsync(bookId), Times.Once);
        }



        [Fact]
        public async Task SoftDeleteBookAsync_ShouldThrowNotFoundException_WhenBookDoesNotExist()
        {
            // arrange level
            var bookId = 999;
            _mockBookRepository.Setup(x => x.GetByIdAsync(bookId)).ReturnsAsync((Book?)null);
            // act and assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _bookService.SoftDeleteBookAsync(bookId));
            Assert.Equal("Book not found.", exception.Message);
            _mockBookRepository.Verify(x => x.GetByIdAsync(bookId), Times.Once);
            _mockBookRepository.Verify(x => x.SoftDeleteAsync(bookId), Times.Never);
        }

        #endregion

        #region HardDeleteBookAsync Tests

        [Fact]
        public async Task HardDeleteBookAsync_ShouldHardDeleteBook_WhenBookExists()
        {
            // arrange level
            var bookId = 1;
            var book = new Book
            {
                Id = bookId,
                Title = "Test Book",
                Author = "Test Author",
                PublishedYear = 2020,
                Genre = "Test Genre"
            };
            var bookDto = new BookDto
            {
                Id = bookId,
                Title = "Test Book",
                Author = "Test Author",
                PublishedYear = 2020,
                Genre = "Test Genre"
            };

            _mockBookRepository.Setup(x => x.GetByIdAsync(bookId)).ReturnsAsync(book);
            _mockMapper.Setup(x => x.Map<BookDto>(book)).Returns(bookDto);
            _mockBookRepository.Setup(x => x.HardDeleteAsync(bookId)).Returns(Task.CompletedTask);

            // act
            await _bookService.HardDeleteBookAsync(bookId);
            // assert level
            _mockBookRepository.Verify(x => x.GetByIdAsync(bookId), Times.Once);
            _mockBookRepository.Verify(x => x.HardDeleteAsync(bookId), Times.Once);
        }


        [Fact]
        public async Task HardDeleteBookAsync_ShouldThrowNotFoundException_WhenBookDoesNotExist()
        {
            // arrange level
            var bookId = 999;
            _mockBookRepository.Setup(x => x.GetByIdAsync(bookId)).ReturnsAsync((Book?)null);

            // act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _bookService.HardDeleteBookAsync(bookId));
            Assert.Equal("Book not found.", exception.Message);
            _mockBookRepository.Verify(x => x.GetByIdAsync(bookId), Times.Once);
            _mockBookRepository.Verify(x => x.HardDeleteAsync(bookId), Times.Never);
        }

        #endregion

        #endregion
    }
}
