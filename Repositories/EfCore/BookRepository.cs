using Entities.DataTransferObject;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Repositories.EfCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EfCore
{
    public sealed class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        
        public BookRepository(RepositoryContext context) : base(context)
        {
        }

        public void CreateOneBook(Book book) => Create(book);
       

        public void DeleteOneBook(Book book)=>Delete(book);


        public async Task<PagedList<Book>> GetAllBooksAsync(BookParameters bookParamaters, bool trackChanges) {
           var books= await FindAll(trackChanges)
            .FilterBooks(bookParamaters.MinPrice,bookParamaters.MaxPrice)
            .Search(bookParamaters.SearchTerm)
            .Sort(bookParamaters.OrderBy)    
            .ToListAsync();

            return PagedList<Book>.ToPagedList(books,bookParamaters.PageNumber,bookParamaters.PageSize);
        }

        public async Task<List<Book>> GetAllBooksAsync(bool v)
        {
            
            return await FindAll(v)
                .OrderBy(b => b.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetAllBooksWithDetailsAsync(bool trackChanges)
        {
            return await _context.Books.Include(b => b.Category)
                  .OrderBy(b => b.Id)
                  .ToArrayAsync();
        }

        public async Task<Book> GetOnBookByIdAsync(int id, bool trackChanges)=>await FindByCondition(b=>b.Id==id, trackChanges).SingleOrDefaultAsync();
       

        public void UpdateOneBook(Book book)=>Update(book);
        
    }
}
