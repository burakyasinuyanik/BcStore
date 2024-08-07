﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repositories.EfCore;

namespace WebApi.ContextFactory
{
    public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
    {
        public RepositoryContext CreateDbContext(string[] args)
        {
          //configure
          var configuration =new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            //DbcontextBuilder
            var buildir = new DbContextOptionsBuilder<RepositoryContext>()
                  .UseSqlServer(configuration.GetConnectionString("sqlConnection"), prj=>prj.MigrationsAssembly("WebApi"));
            return new RepositoryContext(buildir.Options);
        }
    }
}
