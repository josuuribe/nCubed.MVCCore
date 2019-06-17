using nCubed.MVCCore.Sample.dtos;
using System;
using System.Collections.Generic;

namespace nCubed.MVCCore.Sample.Services
{
    public class AuthorService : IAuthorService
    {
        public AuthorService()
        {

        }

        public IEnumerable<Author> GetAuthorsFew()
        {
            var authors = new List<Author>()
            {
                new Author(){ Guid=Guid.NewGuid(), Name="Author 1" },
                new Author(){ Guid=Guid.NewGuid(), Name="Author 2" },
            };

            return authors;
        }

        public IEnumerable<Author> GetAuthorsBig()
        {
            var authors = new List<Author>();
            for (int i = 1; i < 100; i++)
            {
                authors.Add(new Author() { Guid = new Guid(), Name = $"Author_{i}" });
            }
            return authors;
        }

        public IEnumerable<Author> GetAuthorsEmpty()
        {
            var authors = new List<Author>();
            return authors;
        }
    }
}
