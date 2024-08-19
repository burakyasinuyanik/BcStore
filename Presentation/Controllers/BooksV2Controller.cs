using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
   // [ApiVersion("2.0",Deprecated =true)]
    [ApiController]
    [Route("api/booksV2")]
    public class BooksV2Controller:ControllerBase
    {
        private readonly IServiceManager manager;

        public BooksV2Controller(IServiceManager manager)
        {
            this.manager = manager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooksAsync()
        {

            var books = await manager.BookService.GetAllBooksAsync(false);
            
            return Ok(books);
        }
    }
}
