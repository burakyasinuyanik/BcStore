using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly RepositoryContext context;

        public BooksController(RepositoryContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var books=context.Books;
            return Ok(books);
        }
    }
}
