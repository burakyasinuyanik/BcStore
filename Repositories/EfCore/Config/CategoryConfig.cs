﻿using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EfCore.Config
{
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {

        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.CategoryId);//pk
            builder.Property(c => c.CategoryName).IsRequired();
            builder.HasData(
                new Category()
                {
                    CategoryId = 1,
                    CategoryName = "Computer Science"
                },
                  new Category()
                  {
                      CategoryId = 2,
                      CategoryName = "Network"
                  },
                    new Category()
                    {
                        CategoryId = 3,
                        CategoryName = "DataBase Magement Systems"
                    }
                );

        }
    }
}

