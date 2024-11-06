using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObject
{
    public record class TokenDto
    {
        //authentication devamlılığının devamlılığı için kullanacağımız sınıf
        public string AccessToken { get; init; }
        public string RefreshToken { get; init; }
    }
}
