using nCubed.MVCCore.Sample.dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nCubed.MVCCore.Sample.Services
{
    public interface IAuthorService
    {
        IEnumerable<Author> GetAuthorsFew();

        IEnumerable<Author> GetAuthorsEmpty();

        IEnumerable<Author> GetAuthorsBig();
    }
}
