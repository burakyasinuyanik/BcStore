namespace Entities.DataTransferObject
{
   
    public record BookDto
    {
        //record sınıflar değiştirilemez 
        public int Id { get; init; }
        public string Title { get; init; }
        public decimal Price { get; init; }
    }
    
}
