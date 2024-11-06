using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObject
{
    public abstract record BookDtoForManipulation
    {
        //manipule etmek için kullanacağız ve herhangi bir bilginin eksik olmasında kullanıcı tarafına ileteceğimiz mesajları ileteceğiz.
        [Required(ErrorMessage ="Title is a required field.")]
        [MinLength(2,ErrorMessage ="Title must vonsist of at least 2 characters")]
        [MaxLength(50)]
        public string Title { get; init; }

        [Required(ErrorMessage = "Price is a required field.")]
        [Range(10, 1000)]
        public decimal Price { get; init; }
    }
}
