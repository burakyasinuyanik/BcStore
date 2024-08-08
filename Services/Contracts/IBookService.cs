using Entities.DataTransferObject;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IBookService
    {
        IEnumerable<BookDto> GetAllBooks(bool trackChanges);
        BookDto GetOneBook(int id, bool trackChanges);
        BookDto CreateOneBook(BookDtoForInsertion book);
        Book UpdateOneBook(int id,BookDtoForUpdate book, bool trackChanges);
        void DeleteOneBook(int id,bool trackChanges);
        (BookDtoForUpdate bookDtoForUpdate, Book book) GetOneBookForPatch(int id, bool trackChanges);
        void SaveChangesForPatch(BookDtoForUpdate bookDtoForUpdate, Book book);

    }
}
