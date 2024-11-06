using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObject
{
    public record BookDtoForUpdate : BookDtoForManipulation
    {
        //herhangi bir güncelleme yapacağımız zaman efcore bizden id isteyecektir. bu sebeple bu sınıfta bunu zorunlu kılıyoruz.
        [Required]
        public int Id { get; set; }

    }
}
