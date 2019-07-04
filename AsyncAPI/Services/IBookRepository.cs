using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncAPI.Entities;

namespace AsyncAPI.Services
{
    public interface IBookRepository
    {
        IEnumerable<Entities.Book> GetBooks();

        Entities.Book GetBook(int id);

        Task<IEnumerable<Entities.Book>> GetBooksAsync();

        Task<IEnumerable<Entities.Book>> GetBooksAsync(IEnumerable<int> bookIds);

        Task<Entities.Book> GetBookAsync(int id);

        void AddBook(Entities.Book bookToAdd);

        Task<bool> SaveChangesAsync();
    }
}