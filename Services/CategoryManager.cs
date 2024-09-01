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
    public class CategoryManager : ICategoryService
    {
        private readonly IRepositoryManager manager;

        public CategoryManager(IRepositoryManager manager)
        {
            this.manager = manager;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync(bool trackChanges)
        {
         return  await  manager.Category.GetAllCategoriesAsync(trackChanges);
        }

        public async Task<Category> GetOneCategoryByIdAsync(int id, bool trackChanges)
        {
          return await manager.Category.GetOneCategoryByIdAsync(id,trackChanges);
        }
    }
}
