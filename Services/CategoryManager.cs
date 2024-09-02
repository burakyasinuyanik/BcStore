using Entities.Exceptions;
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
          var category= await manager.Category.GetOneCategoryByIdAsync(id,trackChanges);
            if (category is null)
                throw new CategoryNotFoundException(id);

            return category;
        }
    }
}
