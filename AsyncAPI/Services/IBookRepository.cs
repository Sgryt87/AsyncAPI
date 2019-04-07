using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsyncAPI.Services
{
    public interface IBookRepository
    {
//        IEnumerable<Entities.Book> GetBooks();
//
//        Entities.Book GetBook(int id);

        Task<IEnumerable<Entities.Book>> GetBooksAsync();

        Task<Entities.Book> GetBookAsync(int id);
    }
}