using Azure;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;
using Repositories.EfCore;


namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IRepositoryManager repositoryManager;

        public BooksController(IRepositoryManager repositoryManager)
        {
            this.repositoryManager = repositoryManager;
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var books = repositoryManager.Book.GetAllBooks(false);
            return Ok(books);
        }
        [HttpGet("{id:int}")]
        public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
        {
            try
            {
                var book = repositoryManager.Book.GetOnBookById(id, false);
                if (book is null)
                    return NotFound();
                return Ok(book);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult CreateOneBook([FromBody] Book book)
        {
            try
            {
                if (book is null)
                    return BadRequest();
                repositoryManager.Book.Create(book);
                repositoryManager.Save();
                return StatusCode(201, book);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] Book book)
        {
            try
            {
                var entity = repositoryManager.Book.GetOnBookById(id, true);
                if (entity is null)
                    return NotFound();
                if (id != entity.Id)
                    return BadRequest();
                entity.Title = book.Title;
                entity.Price = book.Price;
                repositoryManager.Save();
                return Ok(book);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute(Name = "id")] int id)
        {
            try
            {
                var entity = repositoryManager.Book.GetOnBookById(id, false);
                if (entity is null)
                    return NotFound(new
                    {
                        statusCode = 404,
                        message = $"Book with id:{id} could not found."
                    });
                repositoryManager.Book.Delete(entity);
                repositoryManager.Save();
                return Ok(entity);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            try
            {
                var entity = repositoryManager.Book.GetOnBookById(id, true);
                if (entity is null)
                    return NotFound();
                bookPatch.ApplyTo(entity);
                repositoryManager.Book.Update(entity);
                repositoryManager.Save();
                return NoContent();
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
