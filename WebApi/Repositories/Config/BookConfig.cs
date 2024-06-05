using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Models;

namespace WebApi.Repositories.Config
{
    public class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasData(new Book { Id = 1, Price=150,Title= "Hacivat ve Karagöz" },
                new Book { Id = 2, Price = 233, Title = "istanbul" },
                new Book { Id = 3, Price = 150, Title = "Mesneviden Hikayeler" }
            );
        }
    }
}
