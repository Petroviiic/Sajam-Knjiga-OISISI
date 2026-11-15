using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    internal interface IBookDAO
    {
        Book? Remove(int Id);

        Book? GetById(int Id);

        List<Book> GetAll();
        List<Book> GetAll(int page, int pageSize, string sortCriteria, SortDirection sortDirection);

        bool BookExists(int id);
    }
}
