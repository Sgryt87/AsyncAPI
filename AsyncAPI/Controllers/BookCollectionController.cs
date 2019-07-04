using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AsyncAPI.Filters;
using AsyncAPI.Models;
using AsyncAPI.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Book = AsyncAPI.Entities.Book;

namespace AsyncAPI.Controllers
{
    [Route("api/bookcollections")]
    [ApiController]
    [BooksResultFilter]
    public class BookCollectionController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public BookCollectionController(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository ??
                              throw new ArgumentNullException(nameof(bookRepository));
            _mapper = mapper ??
                      throw new ArgumentNullException(nameof(mapper));
        }

        // api/bookcollections/(id1, id2... etc ...)
        [HttpGet("{bookIds}", Name = "GetBookCollection")]
        public async Task<IActionResult> GetBookCollection(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))]
            IEnumerable<int> bookIds)
        {
            var enumerable = bookIds as int[] ?? bookIds.ToArray();
            var bookEntities = await _bookRepository.GetBooksAsync(enumerable);

            if (enumerable.Length != bookEntities.Count())
            {
                return NotFound();
            }

            return Ok(bookEntities);
        }


        [HttpPost]
        public async Task<IActionResult> CreateBookCollection([FromBody] IEnumerable<BookForCreation> bookCollection)
        {
            var bookEntities = _mapper.Map<IEnumerable<Entities.Book>>(bookCollection);

            foreach (var bookEntity in bookEntities)
            {
                _bookRepository.AddBook(bookEntity);
            }

            await _bookRepository.SaveChangesAsync();

            var booksToReturn = await _bookRepository.GetBooksAsync(bookEntities.Select(b => b.Id).ToList());

            var bookIds = string.Join(",", booksToReturn.Select(a => a.Id));


            return CreatedAtRoute("GetBookCollection",
                new {bookIds = bookIds}, // param
                booksToReturn); // to return
        }
    }
}