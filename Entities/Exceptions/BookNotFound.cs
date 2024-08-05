namespace Entities.Exceptions
{
    public sealed class BookNotFound : NotFound
    {
        public BookNotFound(int id) : base($"The Book with id:{id} could not found.")
        {
        }
    }
}
