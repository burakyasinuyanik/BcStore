using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EfCore
{
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(RepositoryContext context) : base(context)
        {
        }

        public void CreateOneBook(Book book) => Create(book);
       

        public void DeleteOneBook(Book book)=>Delete(book);


        public async Task<PagedList<Book>> GetAllBooksAsync(BookParameters bookParamaters, bool trackChanges) {
           var books= await FindAll(trackChanges)
            .OrderBy(b => b.Id)      
            .ToListAsync();

            return PagedList<Book>.ToPagedList(books,bookParamaters.PageNumber,bookParamaters.PageSize);
        } 
       

        public async Task<Book> GetOnBookByIdAsync(int id, bool trackChanges)=>await FindByCondition(b=>b.Id==id, trackChanges).SingleOrDefaultAsync();
       

        public void UpdateOneBook(Book book)=>Update(book);
        
    }
}
