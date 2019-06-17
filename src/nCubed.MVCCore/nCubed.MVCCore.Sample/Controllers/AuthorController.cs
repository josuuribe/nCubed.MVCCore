using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using nCubed.MVCCore.Attributes;
using nCubed.MVCCore.Extensions;
using nCubed.MVCCore.Helpers;
using nCubed.MVCCore.Sample.Services;
using System.Linq;

namespace nCubed.MVCCore.Sample.Controllers
{
    [Route("/api/author")]
    public class AuthorController : Controller
    {
        private readonly IAuthorService authorService;
        private readonly IUrlHelper urlHelper;
        private readonly LinkGenerator _linkGenerator;

        public AuthorController(
            IAuthorService authorService,
            IUrlHelper urlHelper,
            LinkGenerator linkGenerator)
        {
            this._linkGenerator = linkGenerator;
            this.urlHelper = urlHelper;
            this.authorService = authorService;
        }

        [EnablePaginationHeader]
        [Route("few", Name = "GetAuthorsFew")]
        public IActionResult GetAuthorsFew()
        {
            var authors = authorService.GetAuthorsFew();
            return Ok(authors);
        }

        [EnablePaginationHeader]
        [Route("big", Name = "GetAuthorsBig")]
        public IActionResult GetAuthorsBig()
        {
            var authors = authorService.GetAuthorsBig();
            return Ok(authors);
        }

        [EnablePaginationHeader]
        [Route("empty", Name = "GetAuthorsEmpty")]
        public IActionResult GetAuthorsEmpty()
        {
            var authors = authorService.GetAuthorsEmpty();
            return Ok(authors);
        }

        [Route("empty", Name = "GetAuthorsEmpty")]
        public IActionResult GetAuthors()
        {
            var authors = authorService.GetAuthorsBig();
           
            return Ok(authors);
        }


        public IActionResult GetHATEOAS([FromHeader(Name = "Accept")] string mediaType)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                  new LinkDto(urlHelper.Link("GetAuthor", new { id = id }),
                  "self",
                  "GET"));
            }
            else
            {
                links.Add(
                  new LinkDto(urlHelper.Link("GetAuthor", new { id = id, fields = fields }),
                  "self",
                  "GET"));
            }

            links.Add(
              new LinkDto(urlHelper.Link("DeleteAuthor", new { id = id }),
              "delete_author",
              "DELETE"));

            links.Add(
              new LinkDto(urlHelper.Link("CreateBookForAuthor", new { authorId = id }),
              "create_book_for_author",
              "POST"));

            links.Add(
               new LinkDto(urlHelper.Link("GetBooksForAuthor", new { authorId = id }),
               "books",
               "GET"));

            return links;
        }
    }
}