using System;
using System.Threading.Tasks;
using AsyncAPI.Entities;
using AsyncAPI.Filters;
using AsyncAPI.Models;
using AsyncAPI.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AsyncAPI.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public BooksController(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository ?? throw new
                                  ArgumentNullException(nameof(bookRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [BooksResultFilter]
        public async Task<IActionResult> GetBooks()
        {
            var bookEntities = await _bookRepository.GetBooksAsync();
            return Ok(bookEntities);
        }

        [HttpGet]
        [BookResultFilter]
        [Route("{id}", Name = "GetBook")]
        public async Task<IActionResult> GetBook(int id)
        {
            var bookEntity = await _bookRepository.GetBookAsync(id);
            if (bookEntity == null)
            {
                return NotFound();
            }

            return Ok(bookEntity);
        }

        [HttpPost]
        [BookResultFilter]
        public async Task<IActionResult> CreateBook([FromBody] BookForCreation book)
        {
            // input validation, check if the Author exists, etc...

            var bookEntity = _mapper.Map<Entities.Book>(book);
            _bookRepository.AddBook(bookEntity);

            await _bookRepository.SaveChangesAsync();

            // Fetch(refresh) the book from the data store, including the author
            await _bookRepository.GetBookAsync(bookEntity.Id);

            return CreatedAtRoute("GetBook",
                new
                {
                    id = bookEntity.Id
                },
                bookEntity);
        }
    }
}