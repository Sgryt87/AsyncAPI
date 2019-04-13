using System;
using AsyncAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace AsyncAPI.Controllers
{
    [Route("api/syncbooks")]
    [ApiController]
    public class SyncBooksController : ControllerBase
    {
        private IBookRepository _bookRepository;

        public SyncBooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository ??
                              throw new ArgumentNullException(nameof(bookRepository));
        }

        [HttpGet]
        public IActionResult GetBooks()
        {
            var bookEntities = _bookRepository.GetBooks();
            return Ok(bookEntities);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetBook(int id)
        {
            var bookEntity = _bookRepository.GetBook(id);

            if (bookEntity == null)
            {
                return NotFound();
            }

            return Ok(bookEntity);
        }
    }
}