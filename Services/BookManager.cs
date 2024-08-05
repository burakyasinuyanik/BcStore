using AutoMapper;
using Entities.DataTransferObject;
using Entities.Exceptions;
using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    internal class BookManager : IBookService
    {
        private readonly IRepositoryManager manager;
        private readonly ILogerService logerService;
        private readonly IMapper _mapper;

        public BookManager(IRepositoryManager manager, ILogerService logerService, IMapper mapper)
        {
            this.manager = manager;
            this.logerService = logerService;
            _mapper = mapper;
        }

        public Book CreateOneBook(Book book)
        {

            manager.Book.CreateOneBook(book);
            manager.Save();
            return book;
        }

        public void DeleteOneBook(int id, bool trackChanges)
        {
            var entity = manager.Book.GetOnBookById(id, trackChanges);
            if (entity is null)
            {
                throw new BookNotFoundException(id);
            }

            manager.Book.DeleteOneBook(entity);
            manager.Save();
        }

        public IEnumerable<Book> GetAllBooks(bool trackChanges)
        {
           
            return manager.Book.GetAllBooks(trackChanges);
        }

        public Book GetOneBook(int id, bool trackChanges)
        {
            var book = manager.Book.GetOnBookById(id, trackChanges);
            if(book is null)
            {
               
                throw new BookNotFoundException(id);
            }
            return book;
        }

        public Book UpdateOneBook(int id, BookDtoForUpdate bookDto, bool trackChanges)
        {
            var entity = manager.Book.GetOnBookById(id, trackChanges);

            entity = _mapper.Map<Book>(bookDto);


            //entity.Title = book.Title;
            //entity.Price = book.Price;

            manager.Book.Update(entity);
            manager.Save();
            return entity;
        }
    }
}
