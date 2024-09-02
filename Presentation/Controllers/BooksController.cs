using Entities.DataTransferObject;
using Entities.Exceptions;
using Entities.Models;
using Entities.RequestFeatures;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
   // [ApiVersion("1.0")]

    //[ServiceFilter(typeof(LogFilterAttribute))]

    [ApiController]
    [Route("api/books")]
    [ApiExplorerSettings(GroupName = "v1")]

    // [ResponseCache(CacheProfileName ="5mins")]
    [HttpCacheExpiration(CacheLocation =CacheLocation.Public,MaxAge =80)]
    public class BooksController : ControllerBase
    {
        private readonly IServiceManager _manager;

        public BooksController(IServiceManager manager)
        {
            _manager = manager;
        }
        [Authorize]
        [HttpHead]
        [HttpGet(Name = "GetAllBooksAsync")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        [ResponseCache(Duration =60)]
        public async Task<IActionResult> GetAllBooksAsync([FromQuery]BookParameters bookParameters)
        {
            var linkParameters = new LinkParameters()
            {
                bookParameters = bookParameters,
                httpContext = HttpContext
            };

            var result = await _manager.BookService.GetAllBooksAsync(linkParameters,false);
            Response.Headers.Add("X-Pagination",JsonSerializer.Serialize(result.metaData));

            return result.linkResponse.HasLink ?
                Ok(result.linkResponse.LinkedEntities) :
                Ok(result.linkResponse.ShapedEntity);



        }
        [Authorize(Roles = "Editor")]

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOneBook([FromRoute(Name = "id")] int id)
        {

            var book = await _manager.BookService.GetOneBookAsync(id, false);
          
            return Ok(book);


        }
        [HttpPost(Name = "CreateOneBookAsync")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> CreateOneBookAsync([FromBody] BookDtoForInsertion bookDto)
        {
         
           await _manager.BookService.CreateOneBookAsync(bookDto);

            return StatusCode(201, bookDto);



        }
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] BookDtoForUpdate book)
        {      
           await _manager.BookService.UpdateOneBookAsync(id, book, false);
            return NoContent();
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOneBook([FromRoute(Name = "id")] int id)
        {


            await _manager.BookService.DeleteOneBookAsync(id, false);
            return NoContent();


        }
        [HttpPatch("{id:int}")]
        public async Task< IActionResult> PartiallyUpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<BookDtoForUpdate> bookPatch)
        {
            if (bookPatch is null)
                return BadRequest();

            var result = await _manager.BookService.GetOneBookForPatchAsync(id, false);
           
            bookPatch.ApplyTo(result.bookDtoForUpdate,ModelState);
             TryValidateModel(result.bookDtoForUpdate);
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);
            await _manager.BookService.SaveChangesForPatchAsync(result.bookDtoForUpdate, result.book);

            return NoContent();


        }

        [HttpOptions]
        public IActionResult GetBooksOption()
        {
            Response.Headers.Add("Allow", "GET,PUT,POST,DELETE,PATCH,HEAD,OPTİONS");
            return Ok();
        }
        [HttpGet("details")]
        public async Task<IActionResult> GetAllBooksWithDetailsAsync()
        {
            return Ok(await _manager.BookService.GetAllBooksWithDetailsAsync(false));
        }

    }

}
