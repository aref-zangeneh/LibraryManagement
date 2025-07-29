using LibraryManagement.Application.Commands;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Interfaces;

namespace LibraryManagement.Application.Services;
public class BookService : IBookService
{
    public virtual async Task<IEnumerable<BookDto>> GetAllBooksAsync()
    {
        throw new NotImplementedException();
    }

    public virtual async Task<BookDto?> GetBookByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public virtual async Task CreateBookAsync(CreateBookCommand command)
    {
        throw new NotImplementedException();
    }

    public virtual async Task UpdateBookAsync(int id, CreateBookCommand command)
    {
        throw new NotImplementedException();
    }

    public virtual async Task SoftDeleteBookAsync(int id)
    {
        throw new NotImplementedException();
    }

    public virtual async Task HardDeleteBookAsync(int id)
    {
        throw new NotImplementedException();
    }
}

