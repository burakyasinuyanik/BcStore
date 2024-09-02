using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


 namespace Repositories.EfCore.Config
{
    public class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasData(new Book { Id = 1, Price=150,Title= "Hacivat ve Karagöz",CategoryId= 1 },
                new Book { Id = 2, Price = 233, Title = "istanbul",CategoryId = 2 },
                new Book { Id = 3, Price = 150, Title = "Mesneviden Hikayeler",CategoryId=1 }
            );
        }
    }
}
