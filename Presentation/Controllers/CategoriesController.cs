using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/categories")]
    [ApiExplorerSettings(GroupName = "v1")]
    [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 80)]
    [Authorize]

    public class CategoriesController:ControllerBase
    {
        private readonly IServiceManager serviceManager;

        public CategoriesController(IServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            return Ok(await serviceManager.CategoryService.GetCategoriesAsync(false));
        }

        [HttpGet("{id:int}")]
       
        public async Task<IActionResult> GetAllCategories([FromRoute] int id)
        {
            return Ok(await serviceManager.CategoryService.GetOneCategoryByIdAsync(id, false));
        }
    }
}
