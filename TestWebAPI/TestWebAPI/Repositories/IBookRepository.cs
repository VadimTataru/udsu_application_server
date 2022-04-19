using System.Collections.Generic;
using System.Threading.Tasks;
using TestWebAPI.Models;

namespace TestWebAPI.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> Get();
        Task<IEnumerable<Book>> Get(string title);
        Task<Book> Get(int id);
        Task<Book> Create(Book book);
        Task Update(Book book);
        Task Delete(int id);
    }
}
