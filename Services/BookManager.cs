using AutoMapper;
using Entities.DataTransferObject;
using Entities.Exceptions;
using Entities.Models;
using Entities.RequestFeatures;
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

        public async Task< BookDto> CreateOneBookAsync(BookDtoForInsertion bookDto)
        {

            manager.Book.CreateOneBook(_mapper.Map<Book>(bookDto));
           await manager.SaveAsync();
           
            return _mapper.Map<BookDto>(bookDto);
        }

        public async Task DeleteOneBookAsync(int id, bool trackChanges)
        {
            var entity = await GetOneBookByIdCheckExists(id, trackChanges);

            manager.Book.DeleteOneBook(entity);
           await manager.SaveAsync();
        }

        public async Task<(IEnumerable<BookDto> books,MetaData metaData)> GetAllBooksAsync(BookParameters bookParameters, bool trackChanges)
        {
            if (!bookParameters.ValidPriceRange)
                throw new PriceOutofRangeBadRequestException();
            var booksWithMetaData = await manager
                .Book
                .GetAllBooksAsync(bookParameters,trackChanges);
            
            var booksDto=  _mapper.Map<IEnumerable <BookDto>>(booksWithMetaData);
            return (booksDto, booksWithMetaData.MetaData);
        }

        public async Task<BookDto> GetOneBookAsync(int id, bool trackChanges)
        {
            var entity = await GetOneBookByIdCheckExists(id, trackChanges);


            return _mapper.Map<BookDto>(entity);
        }

        public async Task<(BookDtoForUpdate bookDtoForUpdate, Book book)> GetOneBookForPatchAsync(int id, bool trackChanges)
        {
            var entity = await GetOneBookByIdCheckExists(id, trackChanges);
            var bookDtoForUpdate=_mapper.Map<BookDtoForUpdate>(entity);
            return (bookDtoForUpdate, entity);
        }

        public async Task SaveChangesForPatchAsync(BookDtoForUpdate bookDtoForUpdate, Book book)
        {
            _mapper.Map(bookDtoForUpdate, book);
            manager.SaveAsync();
        }

        public async Task<Book> UpdateOneBookAsync(int id, BookDtoForUpdate bookDto, bool trackChanges)
        {
            var entity = await manager.Book.GetOnBookByIdAsync(id, trackChanges);

            entity = _mapper.Map<Book>(bookDto);



            manager.Book.Update(entity);
            await manager.SaveAsync();
            return entity;
        }
        private async Task<Book> GetOneBookByIdCheckExists(int id,bool trackChanges)
        {
            var entity = await manager.Book.GetOnBookByIdAsync(id, trackChanges);
            if(entity is null)
                throw new BookNotFoundException(id);
            return entity;
        }
    }
}
