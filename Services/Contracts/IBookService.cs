﻿using Entities.DataTransferObject;
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
        Book GetOneBook(int id, bool trackChanges);
        Book CreateOneBook(Book book);
        Book UpdateOneBook(int id,BookDtoForUpdate book, bool trackChanges);
        void DeleteOneBook(int id,bool trackChanges);

    }
}
