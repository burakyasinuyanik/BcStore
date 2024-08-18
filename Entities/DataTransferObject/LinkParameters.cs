using Entities.RequestFeatures;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObject
{
    public  record LinkParameters
    {
       public BookParameters bookParameters { get; init; }
        public HttpContext httpContext  { get; init; }
    }
}
