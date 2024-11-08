﻿using Entities.DataTransferObject;
using Entities.LinkModels;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public class BookLinks : IBookLinks
    {
        private readonly LinkGenerator linkGenerator;
        private readonly IDataShaper<BookDto> dataShaper;

        public BookLinks(IDataShaper<BookDto> dataShaper, LinkGenerator linkGenerator)
        {
            this.dataShaper = dataShaper;
            this.linkGenerator = linkGenerator;
        }

        public LinkResponse TryGenerateLinks(IEnumerable<BookDto> bookDto,
            string fields,
            HttpContext httpContext)
        {
            var shapedBooks = ShapeDta(bookDto, fields);
            if (ShouldGenerateLinks(httpContext))
                return ReturnLinkedBooks(bookDto, fields, httpContext, shapedBooks);

            return ReturnShapedBooks(shapedBooks);
        }

        private LinkResponse ReturnLinkedBooks(IEnumerable<BookDto> bookDto, string fields, HttpContext httpContext, List<Entity> shapedBooks)
        {
            var bookDtoList = bookDto.ToList();
            for (int index = 0; index < bookDtoList.Count(); index++)
            {
                var bookLinks = CreateForBook(httpContext, bookDtoList[index], fields);
                shapedBooks[index].Add("Links", bookLinks);
            }
            var bookCollection = new LinkCollectionWrapper<Entity>(shapedBooks);
            CreateForBooks(httpContext, bookCollection);
            return new LinkResponse { HasLink = true,LinkedEntities= bookCollection };
        }

        private LinkCollectionWrapper<Entity> CreateForBooks(HttpContext httpContext,LinkCollectionWrapper<Entity> bookCollectionWrapper)
        {
            bookCollectionWrapper.Links.Add(new Link()
            {
                Href = $"/api/{httpContext.GetRouteData().Values["controller"].ToString().ToLower()}",             
                Rel = "self",
                Method = "GET"

            });
            return bookCollectionWrapper;

        }
        private List<Link> CreateForBook(HttpContext httpContext, BookDto bookDto, string fields)
        {
            var links = new List<Link>
         {
             new Link()
             {
                 Href=$"/api/{httpContext.GetRouteData().Values["controller"].ToString().ToLower()}"
                 +$"/{bookDto.Id}",
                 Rel="self",
                 Method="GET"
             },
             new Link()
             {
                 Href=$"/api/{httpContext.GetRouteData().Values["controller"].ToString().ToLower()}"
                 ,
                 Rel="create",
                 Method="POST"
             },

         };
            return links;
        }

        private LinkResponse ReturnShapedBooks(List<Entity> shapedBooks)
        {
            return new LinkResponse()
            {
                ShapedEntity = shapedBooks,
            };
        }

        private bool ShouldGenerateLinks(HttpContext httpContext)
        {
            var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];
            return mediaType
                .SubTypeWithoutSuffix
                .EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);
        }

        private List<Entity> ShapeDta(IEnumerable<BookDto> bookDto, string fields)
        {
            return dataShaper.ShapeData(bookDto, fields).Select(b => b.Entity).ToList();
        }
    }
}
