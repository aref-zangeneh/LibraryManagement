using LibraryManagement.Application.Commands;
using LibraryManagement.Application.DTOs;

namespace LibraryManagement.Application.Interfaces;

public interface IBookService
{
    /// <summary>
    /// get all books
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<BookDto>> GetAllBooksAsync();

    /// <summary>
    /// get book by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<BookDto?> GetBookByIdAsync(int id);

    /// <summary>
    /// create a new book
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    Task CreateBookAsync(CreateBookCommand command);

    /// <summary>
    /// update a book
    /// </summary>
    /// <param name="id"></param>
    /// <param name="command"></param>
    /// <returns></returns>
    Task UpdateBookAsync(int id, CreateBookCommand command);

    /// <summary>
    ///soft delete a book
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task SoftDeleteBookAsync(int id);

    /// <summary>
    /// hard delete a book
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task HardDeleteBookAsync(int id);
}

