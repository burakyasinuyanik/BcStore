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

        public BookManager(IRepositoryManager manager, ILogerService logerService)
        {
            this.manager = manager;
            this.logerService = logerService;
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
                string message = $"Book with {id} could not fount";
                logerService.LogInfo(message);
                throw new Exception(message);
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
                string message = $"Book with {id} could not fount";
                logerService.LogInfo(message);
                throw new Exception(message);
            }
            return manager.Book.GetOnBookById(id, trackChanges);
        }

        public Book UpdateOneBook(int id, Book book, bool trackChanges)
        {
            var entity = manager.Book.GetOnBookById(id, trackChanges);
            if (entity is null)
            {
                string message = $"Book with {id} could not fount";
                logerService.LogInfo(message);
                throw new Exception(message);
            }
            if (book is null)
                throw new ArgumentException(nameof(book));
            entity.Title = book.Title;
            entity.Price = book.Price;

            manager.Book.UpdateOneBook(entity);
            manager.Save();
            return entity;
        }
    }
}
