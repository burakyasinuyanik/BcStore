using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObject
{
    public record BookDtoForInsertion : BookDtoForManipulation
    {
        //bir kitap eklemek istediğimiz kullanılacak sınıf category ıd bu sınıf sayesinde zorunlu kılınıyor
        [Required(ErrorMessage ="CategoryId is required.")]
        public int CategoryId { get; init; }
    }
}
