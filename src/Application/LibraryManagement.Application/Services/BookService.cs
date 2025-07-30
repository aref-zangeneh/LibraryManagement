using AutoMapper;
using FluentValidation;
using LibraryManagement.Application.Commands;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Exceptions;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Services;
public class BookService : IBookService
{

    #region Fields

    private readonly IGenericRepository<Book> _bookRepository;
    private readonly IValidator<CreateBookCommand> _validator;
    private readonly IMapper _mapper;

    #endregion

    #region Constructor

    public BookService(IGenericRepository<Book> bookRepository,
        IMapper mapper,
        IValidator<CreateBookCommand> validator)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
        _validator = validator;
    }

    #endregion

    #region Methods

    public virtual async Task<IEnumerable<BookDto>> GetAllBooksAsync()
    {
        var books = await _bookRepository.GetAllAsync();

        return _mapper.Map<IEnumerable<BookDto>>(books);

    }

    public virtual async Task<BookDto?> GetBookByIdAsync(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);

        if (book == null) throw new NotFoundException("Book not found.");

        return _mapper.Map<BookDto>(book);
    }

    public virtual async Task CreateBookAsync(CreateBookCommand command)
    {
        await _validator.ValidateAndThrowAsync(command);

        var book = _mapper.Map<Book>(command);

        await _bookRepository.AddAsync(book);
    }

    public virtual async Task UpdateBookAsync(int id, CreateBookCommand command)
    {
        var book = await _bookRepository.GetByIdAsync(id);

        if (book == null) throw new NotFoundException("Book not found");

        _mapper.Map(command, book);

        await _bookRepository.UpdateAsync(book);
    }

    public virtual async Task SoftDeleteBookAsync(int id)
    {
        var book = await GetBookByIdAsync(id);

        if (book == null) throw new NotFoundException("Book not found");

        await _bookRepository.SoftDeleteAsync(id);
    }

    public virtual async Task HardDeleteBookAsync(int id)
    {
        var book = await GetBookByIdAsync(id);

        if (book == null) throw new NotFoundException("Book not found");

        await _bookRepository.HardDeleteAsync(id);
    }

    #endregion


}

