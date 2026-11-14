using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    internal interface IAuthorDAO
    {
        Author Add(Author author);
        Author? Update(Author author);
        Author? Remove(int Id);
        Author? GetById(int Id);
        List<Author> GetAll();
    }
}
