using AutoMapper;
using Entities.DataTransferObject;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IBookService> _bookService;
        private readonly Lazy<ICategoryService> _categoryService;

        private readonly Lazy<IAuthenticationService> _authenticationService;


        public ServiceManager(IRepositoryManager repositoryManager,
            ILogerService logerService,
            IMapper mapper,
            IBookLinks bookLinks,
            UserManager<User> userManager,
            IConfiguration configuration
            )
        {
            _bookService = new Lazy<IBookService>(() => new BookManager(repositoryManager, logerService, mapper, bookLinks));

            _categoryService = new Lazy<ICategoryService>(() => new CategoryManager(repositoryManager));

            _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationManager(logerService, mapper, userManager, configuration));
        }
        public IBookService BookService => _bookService.Value;
        public ICategoryService CategoryService => _categoryService.Value;

        public IAuthenticationService AuthenticationService => _authenticationService.Value;

    }
}
