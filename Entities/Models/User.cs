using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class User:IdentityUser
    {
        //identity userkatılıldığı için onun özelliklerini taşır ve bu sayede ayrıca özellikler ekleyebiliyoruz.
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryToken { get; set; }
    }
}
