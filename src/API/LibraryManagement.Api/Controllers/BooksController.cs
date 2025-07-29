using LibraryManagement.Application.Commands;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        #region Fields

        private readonly IBookService _bookService;

        #endregion

        #region Constructor

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        #endregion

        #region Api endpoints
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetAll()
        {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetById(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null) return NotFound();
            return Ok(book);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateBookCommand command)
        {
            await _bookService.CreateBookAsync(command);
            return Created();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] CreateBookCommand command)
        {
            await _bookService.UpdateBookAsync(id, command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> SoftDelete(int id)
        {
            await _bookService.SoftDeleteBookAsync(id);
            return NoContent();
        }

        [HttpDelete("{id}/hard")]
        public async Task<ActionResult> HardDelete(int id)
        {
            await _bookService.HardDeleteBookAsync(id);
            return NoContent();
        }
        #endregion
    }
}
