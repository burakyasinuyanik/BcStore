using AutoMapper;
using Entities.DataTransferObject;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

        public BookDto CreateOneBook(BookDtoForInsertion bookDto)
        {

            manager.Book.CreateOneBook(_mapper.Map<Book>(bookDto));
            manager.Save();
           
            return _mapper.Map<BookDto>(bookDto);
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

        public IEnumerable<BookDto> GetAllBooks(bool trackChanges)
        {
            var books = manager.Book.GetAllBooks(trackChanges);
          var booksDto=  _mapper.Map<IEnumerable <BookDto>>(books);
            return booksDto;
        }

        public BookDto GetOneBook(int id, bool trackChanges)
        {
            var book = manager.Book.GetOnBookById(id, trackChanges);
            if(book is null)
            {
               
                throw new BookNotFoundException(id);
            }

            return _mapper.Map<BookDto>(book);
        }

        public (BookDtoForUpdate bookDtoForUpdate, Book book) GetOneBookForPatch(int id, bool trackChanges)
        {
            var book = manager.Book.GetOnBookById(id,trackChanges);
            if (book is null)
                throw new BookNotFoundException(id);
            var bookDtoForUpdate=_mapper.Map<BookDtoForUpdate>(book);
            return (bookDtoForUpdate, book);
        }

        public void SaveChangesForPatch(BookDtoForUpdate bookDtoForUpdate, Book book)
        {
            _mapper.Map(bookDtoForUpdate, book);
            manager.Save();
        }

        public Book UpdateOneBook(int id, BookDtoForUpdate bookDto, bool trackChanges)
        {
            var entity = manager.Book.GetOnBookById(id, trackChanges);

            entity = _mapper.Map<Book>(bookDto);



            manager.Book.Update(entity);
            manager.Save();
            return entity;
        }
    }
}
